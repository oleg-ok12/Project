using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;


namespace NetworkCallController
{
    public partial class NCC : Form
    {
        /*public bool isRunning { get; set; }//treba? czy NNC chodzi czy nie.
        private string myAddress;
        private Dictionary<string, string> dns;//dns--> wiki. 1string-nazwa dla nas. 2string-adres ip
        */
        private PC pc;
        private Thread ReceiveThread;
        private PolicyDirectory pd;

        private bool canCall;

        public Dictionary<int, Port> ports = new Dictionary<int, Port>();
        private Thread connectThread;

        public NCC()
        {
            InitializeComponent();

            pc = new PC();
            pd = new PolicyDirectory();

            inicjalizacja();

            /*(new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(100);
                inicjalizacja();
            }))).Start();*/

           
            

        }


       delegate void inicjalizacjaCallback();
        private void inicjalizacja()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new inicjalizacjaCallback(inicjalizacja), new object[] { });
            }
            else
            {
                try
                {
                    foreach (Port port in ports.Values)
                    {

                        connectThread = new Thread(new ParameterizedThreadStart(connectToNetwork));
                        connectThread.Start(port);
                    }
                    //ReceiveThread = new Thread(new ThreadStart(ReceiveFunction));
                }

                catch
                {
                    richTextBox_Log.Text = "cos jest zle";
                }
            }   
        }


        private void connectToNetwork(object para)
        {
            Port tmp_port = (Port)para;
            tmp_port.connect();


        }

        //funkcja skanuje pakiety na wejsciu
        public void ReceiveFunction()
        {
            try
            {                            
                
                while (true)
                {

                    Queue messages = pc.getData();

                    foreach (Message msg in messages)
                    {
                        Message tempMessage = new Message();
                        tempMessage.source_component_name = "NCC";
                        tempMessage.parameters[1] = "NCC";

                        try
                        {
                            switch (msg.parameters[0])//zakladam ze w parames[0] bedzie przesylana nazwa message'a
                            {
                                    //w sumie tu dodac rozne mozliwosci, tylko nie wiem co jeszcze
                                case "CALL_REQUEST":
                                    if ((msg.source_component_name == "CLIENT1") ||(msg.source_component_name=="CLIENT2"))
                                    {
                                        setLogText("Dostalem zadanie polaczenia od" + msg.source_component_name);
                                        if (askForCall(pd))  // jakies sprawdzenie w PolicyDirectory
                                        {
                                            //tu otrzymuje cos od pd
                                            //tu cos sprawdza
                                            tempMessage.dest_component_name = "CC1";
                                            tempMessage.parameters[0] = "CONNECTION_REQUEST";


                                             pc.sendData("CC1", tempMessage);
                                        }
                                    }
                                                                        
                                    break;

                                case "ESTABLISHED":
                                    if (msg.source_component_name == "CC1")
                                    {
                                        setLogText("CC1 powiedzial ze polaczenie zostalo nawiazane");
                                        tempMessage.parameters[0] = "OK";
                                        tempMessage.dest_component_name = "CLIENT1";
                                        pc.sendData("CLIENT1", tempMessage);
                                        tempMessage.dest_component_name = "CLIENT2";
                                        pc.sendData("CLIENT2", tempMessage);
                                    }   
                                    break;

                                case "CALL_TEARDOWN":                   //rozlaczenie od clienta, nie wiem czy to zrobimy
                                    setLogText("Dostalem zadanie rozlaczenia od" + msg.source_component_name);    //przy Teardown musi cos sprawzdac u PD?
                                    tempMessage.parameters[0] = "CALL_TEARDOWN";
                                    tempMessage.dest_component_name = "CC1";
                                    pc.sendData("CC1", tempMessage);
                                      break;

                                case "NO_ESTABLISHED":
                                      if (msg.source_component_name == "CC1")
                                      {
                                          setLogText("CC1 powiedzial ze polaczenie jest rozerwane");
                                          tempMessage.parameters[0] = "NO_ESTABLISHED";
                                          tempMessage.dest_component_name = "CLIENT1";
                                          pc.sendData("CLIENT1", tempMessage);
                                          tempMessage.dest_component_name = "CLIENT2";
                                          pc.sendData("CLIENT2", tempMessage);
                                        
                                      }
                                      break;
                                default: break;
                            }
                        }
                        catch
                        {
                            setLogText("Cos jest zle przy odczytaniu przychadzacego message");
                        }
                    }
                }
            }
            catch
            {
               setLogText("Cos jest zle  w Receive function");
            }
        }



        //funcja ktora bedzie wyswietlala tekst w okienku loga
        delegate void setLogTextCallback(string text);
        public void setLogText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new setLogTextCallback(setLogText), new object[] { text });
            }
            else
            {
                richTextBox_Log.AppendText(text);
            }
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox_Log.Clear();
            }
            catch
            {

            }
        }


        private bool askForCall(PolicyDirectory pd)
        {
            if (pd.isAllow)
            {
                setLogText("PD: Pozwalam na zestawienie polaczenia");
                return true;
            }

            else
            {
                setLogText("PD: nie pozwalam na zestawienie polaczenia");
                return false;
            }
        }

       /* public void send(string src, string dst, string payload)
        {
            Packet.SendPacket sendPack = new Packet.SendPacket(src, dst, payload);
            sendBF.Serialize(networkStream, sendPack);

        }

        public void send(string src, string dst, List<string> payloadList)
        {
            Packet.SendPacket sendPack = new Packet.SendPacket(src, dst, payloadList);
            sendBF.Serialize(networkStream, sendPack);
        }*/
   
    }



   
    
}
