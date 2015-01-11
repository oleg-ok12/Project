using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Manager
{
    public enum Data
    {
        [XmlEnum("client")]
        client = 1,
        [XmlEnum("characteristic")]
        characteristic = 2,
        [XmlEnum("manager")]
        manager = 3
    }
    public class Manager
    {
        public int id_node;
        public SerializableDictionary<int, Port> ports = new SerializableDictionary<int, Port>();
        private Thread connectThread;



        private Dictionary<string, Node> nodes;

        private string configure;


        public Manager()
        {
        }

        public Manager(int id, SerializableDictionary<int, Port> porty, string network)
        {
            id_node = id;
            ports = porty;
            ReadConfig(network);
        }

        public void initializeManager()
        {
            foreach (Port port in ports.Values)
            {

                connectThread = new Thread(new ParameterizedThreadStart(connectToNetwork));
                connectThread.Start(port);
            }




        }

        private void connectToNetwork(object para)
        {
            Port tmp_port = (Port)para;
            tmp_port.connect();


        }


        public void sendInformation(int node_id, int port_in, int port_out, int? c_in, int? c_out, int type, bool ifAdd)
        {
            string key = "Node_"+node_id.ToString();
            int cont_in;
            int cont_out;
            int.TryParse(c_in.ToString(), out cont_in);
            int.TryParse(c_out.ToString(), out cont_out);


            Node node = nodes[key];

            if( node.if_c4_in_used[port_in] == false && node.if_c4_out_used[port_out] == false )
            {

                bool check = true;
                if (c_in != null & c_out != null)
                { 
                if (node.used_c3_containers_in[port_in][cont_in - 1] == false && node.used_c3_containers_in[port_in][cont_out - 1] == false) { check = false; }
                }

                if ((c_in == null && c_out == null) || check == false)
                {
                    ManagerInformation info1 = new ManagerInformation(port_in, port_out, c_in, c_out, type, ifAdd);  //ifClient i type będzie usunięte ale narazie musi być żeby działało robienie manager_information
                    String serialized_info1 = Serialization.SerializeObject(info1);


                    ManagerInformation info2 = new ManagerInformation(port_out, port_in, c_out, c_in, type, ifAdd);
                    String serialized_info2 = Serialization.SerializeObject(info2);


                    ports[node_id].send(serialized_info1);
                    ports[node_id].send(serialized_info2);

                    Console.WriteLine("Wysłano wiadomość do węzła.");

                    if (ifAdd == true)
                    {
                        if (c_in == null && c_out == null)  // zmiana paramtetrów Node
                        {
                            nodes[key].if_c4_in_used[port_in] = true;
                            nodes[key].if_c4_out_used[port_out] = true;


                        }
                        else if (node.used_c3_containers_in[port_in][cont_in - 1] == false && node.used_c3_containers_in[port_in][cont_out - 1] == false)
                        {
                            nodes[key].used_c3_containers_in[port_in][cont_in - 1] = true;
                            nodes[key].used_c3_containers_out[port_out][cont_out - 1] = true;

                            nodes[key].used_c3_containers_out[port_in][cont_in - 1] = true;
                            nodes[key].used_c3_containers_in[port_out][cont_out - 1] = true;


                        }
                    }
                    else if (ifAdd == false)
                    {
                        if (c_in == null && c_out == null)  // zmiana paramtetrów Node
                        {
                            nodes[key].if_c4_in_used[port_in] = false;
                            nodes[key].if_c4_out_used[port_out] = false ;


                        }
                        else if (node.used_c3_containers_in[port_in][cont_in - 1] == false && node.used_c3_containers_in[port_in][cont_out - 1] == false)
                        {
                            nodes[key].used_c3_containers_in[port_in][cont_in - 1] = false;
                            nodes[key].used_c3_containers_out[port_out][cont_out - 1] = false;
                            nodes[key].used_c3_containers_out[port_in][cont_in - 1] = false;
                            nodes[key].used_c3_containers_in[port_out][cont_out - 1] = false;


                        }


                    }



                }
            }
            else
            { 
                Console.WriteLine("Wybrane porty są zajęte.");
            }
        }



        private void ReadConfig(string src)
        {
            try
            {
                FileStream file = new FileStream(src, FileMode.Open, FileAccess.Read);
                StreamReader conf = new StreamReader(src);
                configure = conf.ReadToEnd().ToString();
                conf.Close();
                file.Close();
            }
            catch
            {
            }

            
            nodes = new Dictionary<string, Node>();
            try
            {
                string[] interfacesTab = configure.Split(new string[] { "-", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                int i = 0;
                while (i < interfacesTab.Length )
                {

                    int number_of_ports;
                    int index_num;
                    int id_node;

                    index_num = interfacesTab[i].IndexOf(":");
                    int.TryParse(interfacesTab[i].Remove(0, index_num + 1), out number_of_ports);

                    


                    if( interfacesTab[i].StartsWith("N"))
                    {
                       string key = interfacesTab[i].Remove(index_num);

                        int.TryParse(interfacesTab[i].Substring(5,index_num-5), out id_node);
                        Node node = new Node(id_node, number_of_ports, "NETWORK");
                        nodes.Add(key, node);


                    }
                    else if (interfacesTab[i].StartsWith("C"))
                    {
                        string key = interfacesTab[i].Remove(index_num);

                        int.TryParse(interfacesTab[i].Substring(7, index_num - 7), out id_node);
                        Node node = new Node(id_node, number_of_ports, "CLIENT");
                        nodes.Add(key, node);
                    }
                    i++;
                }

                Console.WriteLine("Wczytano konfigurację sieci");
            }
            catch
            {
                Console.WriteLine("Plik \"{0}\" zawiera złe dane lub nie istnieje...", src);
                Console.WriteLine("Wciśnij dowolny klawisz aby zakończyć działanie programu...");
                Console.ReadKey();
                Console.Clear();
                Environment.Exit(0);
            }

        }
    }
}
