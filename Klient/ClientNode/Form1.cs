using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Collections;

namespace ClientNode
{
    public partial class Form1 : Form
    {
        //private string ip;
        public int portIn;
        //private string mynetnode;
        //private string myclientname;
        public int portOut;
        public int idPortIn;
        public int idPortOut;
        public int tcpPort;
        //private string destclientname;
        //private bool connect;
        Thread polacz;
        string configName;
        private int idNode;
        int data_type;
        int client_num;
        int liczba_kontenerow;
        int? container_number;

        /////////////////////////
        public Dictionary<int, Port> ports = new Dictionary<int, Port>();
        private Thread connectThread;
        string configure;



      //  public Form1();
        public Form1()
        {
            
            InitializeComponent();
            button1.Enabled = true;   //przycisk wyslij nieaktywny na poczatku
            //button3.Enabled = false; //przycisk uruchomienia portu wyjściowego nieaktywny póki wejściowy nieuruchomiony
           // rozłączToolStripMenuItem.Enabled = false;
            // Console.Write("Podaj nazwę pliku: ");
            // configName = Console.ReadLine();
            //string configName = "klient1.config";

            //////////////////

             Console.WriteLine("Choose client: 1,2");
           
           // Console.Write("Podaj nazwę pliku: ");
             string configname = Console.ReadLine();
            ports = new Dictionary<int, Port>();
            try
            {
                System.IO.FileStream file = new System.IO.FileStream("K"+configname+".config", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.StreamReader conf = new System.IO.StreamReader("K" + configname + ".config");
                configure = conf.ReadToEnd().ToString();
                conf.Close();
                file.Close();
            }
            catch
            {
            }

            try
            {
                string[] interfacesTab = configure.Split(new string[] { "-", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                int i = 0;
                while (i < interfacesTab.Length - 1)
                {
                    if ((Convert.ToInt32(interfacesTab[i + 3])) == 2)
                    {
                        Data typeofdata = Data.characteristic;
                        Port p = new Port(Convert.ToInt32(interfacesTab[i + 1]), Convert.ToInt32(interfacesTab[i + 2]), typeofdata);
                        ports.Add(Convert.ToInt32(interfacesTab[i]), p);
                        i += 4;
                    }
                    if ((Convert.ToInt32(interfacesTab[i + 3])) == 3)
                    {
                        Data typeofdata = Data.message;
                        Port p = new Port(Convert.ToInt32(interfacesTab[i + 1]), Convert.ToInt32(interfacesTab[i + 2]), typeofdata);
                        ports.Add(Convert.ToInt32(interfacesTab[i]), p);
                        i += 4;
                    }
                    
                }
                //Console.WriteLine(ports[45].getPortID());
                //Console.WriteLine(ports[52].getNodeID());
                textBox7.Text = interfacesTab[1];
            }
            catch
            {

                Console.WriteLine("Błąd wczytywania. Wciśnij dowolny klawisz aby zakończyć działanie programu...");
                Console.ReadKey();
                Console.Clear();
                Environment.Exit(0);


            }
            initializeNode();
            
        }       
/*
            try
            {
                //Ustawienie parametrów z pliku
                string[] text = System.IO.File.ReadAllLines(configName);
               
                
                textBox2.Text = text[0]; 
                textBox4.Text = text[1];
                textBox5.Text = text[2];
                textBox6.Text = text[3];
                textBox1.Text = text[4];
                textBox7.Text = text[5];

                idPortIn = Convert.ToInt32(text[0]);
                portIn = Convert.ToInt32(text[1]);
                idPortOut = Convert.ToInt32(text[2]);
                portOut = Convert.ToInt32(text[3]);   //jego port
                tcpPort = Convert.ToInt32(text[4]); //port z którym ma się połączyć
                idNode = Convert.ToInt32(text[5]);
                //destclientname = text[4];  
                //this.Text += " - " + myclientname;
                //connect = Convert.ToBoolean(text[5]);
                pIn = new PortIn(idPortIn, portIn, "client");
                pOut = new PortOut(idPortOut, portOut, tcpPort);
                     

            }
            catch { Console.WriteLine(idPortIn+"cos sie zle wczytuje"); }
            */

           
        private void initializeNode()
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

        
        // wysłanie wiadomości sygnalizacyjnej

        private void sendcontrolMessage(String port_out, Message message)
        {
            String serialized_message = Serialization.SerializeObject(message);

            //jakies sprawdzenie dla port_out
            richTextBox1.Text += "Wysłałem: CALL_REQUEST do NCC\n\n";
            if (port_out == "NCC")
            {
                ports[2].send(serialized_message);
            }
        }


        // wysłanie wiadomości
        private void sendMessage()
        {
            try
            {
                if (textBox3.Text == "") return;
                UnicodeEncoding uni = new UnicodeEncoding();
                ASCIIEncoding asci = new ASCIIEncoding();

            
                string text = textBox3.Text;
                int? con_number = null;

                switch(container_number)
                {
                    case 0: con_number = null; break;
                    case 1: con_number = 1; break;
                    case 2: con_number = 2; break;
                    case 3: con_number = 3; break;
                }


                if (data_type == 0)
                {
                    ClientInformation info = new ClientInformation(text, data_type);
                    CharacteristicInformation char_info = new CharacteristicInformation(info, "poh");
                    STM1 stm = new STM1(char_info, con_number);
                    String serialized_info = Serialization.SerializeObject(stm);
                  
                    ports[1].send(serialized_info);
                }
                if (data_type == 1)
                {
                    ClientInformation info = new ClientInformation(text, data_type);
                    CharacteristicInformation char_info = new CharacteristicInformation(info, "poh");
                    STM1 stm = new STM1(char_info, con_number);
                    String serialized_info = Serialization.SerializeObject(stm);
                 
                    ports[1].send(serialized_info);
                }
                


                richTextBox1.Text += "Wysłana Wiadomość:\n" + textBox3.Text + "\n\n";
                
                textBox3.Clear();
            }
            catch
            {
                richTextBox1.Text += "Błąd Podczas Wysyłania Wiadomości\n\n";
            }
        }

      

        //wprowadzenie poprawnej wartości do pola
        private void comboBox1_Leave_1(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = container;
        }        

        // wysłanie wiadomości gdy klikniemy enter
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //sprawdzenie czy kliknięto enter podczas wysyłania wiadomości
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {
            this.Close();
        }

        //automatyczne przewijanie tekstu
        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
        
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
         
        }

        private void label1_Click(object sender, EventArgs e)
        {
        
        }


        private void label3_Click(object sender, EventArgs e)
        {
        
        }

        //przycisk wyslij bedzie aktywny tylko gdy wczesniej polaczono z wezlem sieciowym
        private void button1_Click_1(object sender, EventArgs e)
        {
            //Thread send = new Thread(sendMessage);
            //send.Start();
            sendMessage();
        }

        private void label4_Click(object sender, EventArgs e)
        {
        
        }

        private void label5_Click(object sender, EventArgs e)
        {
        
        }

        private void label6_Click(object sender, EventArgs e)
        {
        
        }

        private void label7_Click(object sender, EventArgs e)
        {
        
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
           // textBox2.Text = ip;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //textBox4.Text = Convert.ToString(port);
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            //textBox5.Text = Convert.ToString(mynetnode);
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {
           // textBox5.Text = myclientname;
        }

        private void label8_Click(object sender, EventArgs e)
        {
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        
        }
        
     
        //polaczenie z wezlem sieciowym
        private void połączToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
            ////
        }

   /*     private void rozłączToolStripMenuItem_Click(object sender, EventArgs e)
        {
                    
            try
            {
                richTextBox1.Text += "Rozłaczono połączenie z węzłem sieciowym:\n\n";
                richTextBox1.Text += "Włącz port wyjściowy:\n\n";
            }
            catch { }
            textBox3.Enabled = true;
            
            button1.Enabled = false;   //przycisk wyślij
           // połączToolStripMenuItem.Enabled = true;
            rozłączToolStripMenuItem.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
           
           
            textBox3.Enabled = false;
          
            
            //this.Enabled = true;                    
           
            try
            {
                //trzeba zakończyć nasłuchiwanie tcp???????????!!!!!!!!!!!
               // pOut.ReleaseSocket();

                foreach (Port port in ports.Values)
                {
                    port.disconnect();
                }
            }
            catch { }
            try
            {
                polacz.Abort();
            }
            catch { }
            
            polacz = null;
        }
        */
        private void wyjścieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void routerToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
/*
        private void button2_Click_1(object sender, EventArgs e)
        {
            //polaczRouter();
           
            button3.Enabled = true;
            button2.Enabled = false;
            rozłączToolStripMenuItem.Enabled = true;
            button1.Enabled = true;   //przycisk wyslij aktywny
        }  */
        
        
        //odświeżanie odebrania danych
        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
               
                foreach (Port port in ports.Values)
                {
                    
                    Queue odbierane = port.getData();
                    Queue sync_data = Queue.Synchronized(odbierane);

                    Queue received_messages = new Queue();

                    List<STM1> stm = new List<STM1>();

                    if (sync_data.Count > 0)
                    {
                        //dla danych od NCC typu Message
                        if (port.type_of_receiving_data == Data.message)
                        {
                            foreach (String obj in sync_data)
                            {
                                Message msg = new Message();
                                msg = (Message)Serialization.DeserializeObject(obj, typeof(Message));
                                received_messages.Enqueue(msg);
                            }
                        }

                        // Dla danych typu STM1
                        else if (port.type_of_receiving_data == Data.characteristic)
                        {

                            foreach (String obj in sync_data)
                            {

                                STM1 stm_frame = new STM1();
                                stm_frame = (STM1)Serialization.DeserializeObject(obj, typeof(STM1));
                                stm.Add(stm_frame);
                                                                
                            }
                        }
                    }

                    if (stm.Count > 0)
                    {
                        foreach (STM1 odeb in stm)
                        {
                            // STM1 odeb = (STM1)Serialization.DeserializeObject(obj, typeof(STM1));

                            CharacteristicInformation char_info = odeb.information;
                            ClientInformation client_info = char_info.client_information;


                            richTextBox1.Text += "Odebrana Wiadomość:\n" + client_info.text + "\n\n";
                        }
                    }

                    if (received_messages.Count > 0)
                    {
                        foreach (Message msg in received_messages)
                        {
                            

                            if ((msg.dest_component_name == "CLIENT1") || (msg.dest_component_name == "CLIENT2") )
                            {
                                try
                                {
                                    switch ((String)msg.parameters[0])
                                    {
                                        case "OK":
                                            if (msg.source_component_name == "NCC")
                                            {
                                                richTextBox1.Text += "NCC powiedzial OK, moge wysylac\n\n";
                                                
                                                //button1.Enabled = true;
                                            }
                                            break;
                                        default: break;
                                    }

                                }
                                catch
                                {
                                    richTextBox1.Text += "Problem z odebraniem wiadomości: MESSAGE\n\n";
                                }

                            }
                                          
                        }
                    }


                }
           }
            catch
            {
                richTextBox1.Text += "Problem z odebraniem wiadomości:\n\n";
                //blad();
            }
                    

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //tu wybieramy w jaki typ kontenera pakujemy wiadomość
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            data_type = comboBox1.SelectedIndex;
            Console.WriteLine(data_type);
            //if (data_type == 0)
            //    data_type = 3;
            //if (data_type == 1)
            //    data_type = 4;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            container_number = comboBox2.SelectedIndex;
           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            client_num = comboBox3.SelectedIndex;
            Console.WriteLine(client_num);
            if (client_num == 0)
                client_num = 1;
            if (client_num == 1)
                client_num = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Message temp_msg = new Message();
            temp_msg.dest_component_name = "NCC";
            temp_msg.parameters.Add("CALL_REQUEST");    //params[0]       
            
            if (client_num == 1)
            {
                temp_msg.source_component_name = "CLIENT2";
                temp_msg.parameters.Add("CLIENT1");
                temp_msg.parameters.Add("CLIENT2");
            }
            if (client_num == 2)
            {
                temp_msg.source_component_name = "CLIENT1";
                temp_msg.parameters.Add("CLIENT2");
                temp_msg.parameters.Add("CLIENT1");
                
            }
            temp_msg.dest_component_name = "NCC";
            temp_msg.parameters.Add(liczba_kontenerow);                  

            sendcontrolMessage("NCC", temp_msg);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
             liczba_kontenerow = comboBox4.SelectedIndex;
            Console.WriteLine(liczba_kontenerow);
            if (liczba_kontenerow == 0)
                liczba_kontenerow = 1;
            if (liczba_kontenerow == 1)
                liczba_kontenerow = 3;
        }

     


    }

}