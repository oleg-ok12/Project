using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Collections;

using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace NetworkNode
{
    //[Flags]
   // public enum Data { client, characteristic }

    [Serializable]
    public enum Data
    {
        [XmlEnum("client")]
        client = 1,
        [XmlEnum("characteristic")]
        characteristic = 2,
        [XmlEnum("manager")]
        manager=3
    }


    public class NetworkNode
    {
        public int id_node;
        public SerializableDictionary<int, Port> ports = new SerializableDictionary<int,Port>();
        private Thread connectThread;

        Queue data;
        Queue sync_data;

        private Switching switching = new Switching();

        public NetworkNode()
        {
        }

        public NetworkNode(int id, SerializableDictionary<int,Port> porty)
        {
            id_node = id;
            ports = porty;
            

        }

        public void initializeNode ()
        {
            data = new Queue();
            sync_data = Queue.Synchronized(data);


            foreach (Port port in ports.Values)
            {

                connectThread = new Thread(new ParameterizedThreadStart(connectToNetwork));
                connectThread.Start(port);
            }
          
        }


        public void Simulation()
        {

            while (true)
            {
                foreach (Port port in ports.Values)
                {

                    sync_data = port.getData(); //pobranie wszystkich nieodebranych danych, które przyszły
                    

                    //List<ClientInformation> client_info = new List<ClientInformation>();
                    List<ManagerInformation> manager_info = new List<ManagerInformation>();
                    List<STM1> stm = new List<STM1>();
            


                    //deserializacja tych danych na obiekty odpowiedniego typu
                    //if (port.type_of_receiving_data == Data.client)   //jak dane otrzymywane przez port są typu "client"
                    //{
                    //    foreach (String str in sync_data)
                    //    {
                    //        ClientInformation info = new ClientInformation();
                    //        info = (ClientInformation)Serialization.DeserializeObject(str, typeof(ClientInformation));
                    //        client_info.Add(info);
                    //    }
                    //}
                    if (port.type_of_receiving_data == Data.characteristic) //jak dane otrzymywane przez port są typu "characteristic"
                    {
                        foreach (String str in sync_data)
                        {
                            //CharacteristicInformation info = new CharacteristicInformation();
                            STM1 stm_frame = new STM1();
                            stm_frame = (STM1)Serialization.DeserializeObject(str, typeof(STM1));
                            stm.Add(stm_frame);
                        }
                    }
                    else if (port.type_of_receiving_data == Data.manager) //jak dane otrzymywane przez port są typu "manager"
                    {
                        foreach (String str in sync_data)
                        {
                            ManagerInformation info = new ManagerInformation();
                            info = (ManagerInformation)Serialization.DeserializeObject(str, typeof(ManagerInformation));
                            manager_info.Add(info);
                        }


                    }



                    if (stm.Count > 0)
                    {
                        CharacteristicInformation char_information;
                        ClientInformation client_information;
                        int? new_pointer;
                        int ID_port_out;

                        foreach (STM1 stm_frame in stm)
                        {
                            String port_container; //oznacza kombinację portu wejściowego i miejsca w kontenerze -> potrzebne do odnalezienia właściwego matrixa
                            if (stm_frame.AU_Pointer == null)
                            {
                                port_container = port.port_ID.ToString();  //jak przyjdzie VC 4 to wtedy AU-pointer jest równy null
                                Console.WriteLine("Wezel {0}: Dostalem STM-1 na port {1}. Zawiera VC-4", this.id_node, port_container);
                            }
                            else
                            {
                                port_container = (port.port_ID.ToString() + stm_frame.AU_Pointer.ToString());    //jak przyjdzie VC3 to trzeba sprawdzić na jakiej pozycji
                                Console.WriteLine("Wezel {0}: Dostalem STM-1 na port {1}. Zawiera VC-3 na pozycji {2}.", this.id_node, port.port_ID.ToString(), stm_frame.AU_Pointer.ToString());
                            }


                            ID_port_out = switching.CharacteristicSwitching(port_container,stm_frame.information , out char_information, out client_information, out new_pointer);

                            if (char_information != null)
                            {
                                STM1 frame_to_send = new STM1(char_information, new_pointer);   //Tworzenie nowej ramki STM, z danymi charakterystycznymi i  odpowiednim AU-Pointer
                                String serialized_frame = Serialization.SerializeObject(frame_to_send);

                                ports[ID_port_out].send(serialized_frame);     //wysłanie zserializowanych danych odpowiednim portem


                               
                                if (new_pointer == null)
                                {
                                    Console.WriteLine("Wezel {0}: Wysylam ramke STM-1 portem {1}. Zawiera VC-4.", this.id_node, ID_port_out);
                                }
                                else
                                {
                                    Console.WriteLine("Wezel {0}: Wysylam ramke STM-1 portem {1}. Zawiera VC-3 na pozycji {2}.", this.id_node, ID_port_out, new_pointer);
                                }
                               // Console.WriteLine("Została wysłana wiadomosc typu: {0} do portu o ID=={1}. Rozmiar wiadomosci {2}", char_information.type, ID_port_out, char_information.size);
                            }
                            //else if ((char_information == null) && (client_information != null))   //jak kolejny węzeł to klient to wysyłane są tylko kontenery typu C
                            //{

                            //    String serialized_c_info = Serialization.SerializeObject(client_information);
                            //    ports[ID_port_out].send(serialized_c_info);


                            //    //Zrobić dobre wyświetlanie bo to jest złe
                            //    Console.WriteLine("Wezel {0}: Wysylam client_information portem {1}. Typ {2}. Wiadomosc: '{3}'. Rozmiar wiadomosci: {4}. Kolejny to wezel kliencki ", this.id_node, ID_port_out, client_information.type, client_information.text, client_information.data_size);
                                
                            //    //Console.WriteLine("Została wysłana wiadomosc typu: {0} do portu o ID=={1}. Rozmiar wiadomosci {2}", client_information.type, ID_port_out, client_information.size);

                            //}

                        }

                    }

                    
                    if (manager_info.Count > 0)
                    {

                        foreach (ManagerInformation inf in manager_info)
                        {
                            if (inf != null && inf.ifAdd==true)
                            {
                                addMatrix(inf.inPort, inf.outPort, inf.inContainer, inf.outContainer, inf.type);
                            }
                            else if (inf.ifAdd == false)
                            {
                                clearMatrix(inf.inPort, inf.inContainer);
                            }
                            
                        }

                    }






                }

            }
        }


        public void addMatrix(int inPort, int outPort, int? inContainer, int? outContainer, int type)
        {
            switching.addMatrix(inPort, outPort, inContainer, outContainer, type);

        }

        public void clearMatrix(int inPort, int? inContainer)
        {
            switching.clearMatrix(inPort, inContainer);
        }


        private void connectToNetwork(object para)
        {
            Port tmp_port = (Port)para;
            tmp_port.connect();


        }

    }
}
