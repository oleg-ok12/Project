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
using System.Runtime.InteropServices;

using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;


namespace NetworkCallController
{
    public partial class NCC : Form
    {
        private PC pc;
        private Thread ReceiveThread;//
        private PolicyDirectory pd;
        public int CallID;
        private bool canCall;

        public static SerializableDictionary<int, Port> int_ports;
        
        private Thread connectThread;
        string configure;

        public NCC()
        {
            InitializeComponent();


            Win32.AllocConsole();
            Console.WriteLine("Wybierz NCC (wpisz 1)");

            // Console.Write("Podaj nazwę pliku: ");
            string configname = Console.ReadLine();
            int_ports = new SerializableDictionary<int, Port>();
            try
            {
                System.IO.FileStream file = new System.IO.FileStream("NCC" + configname + ".config", System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.StreamReader conf = new System.IO.StreamReader("NCC" + configname + ".config");
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
                    Data typeofdata = Data.message;
                    Port p = new Port(Convert.ToInt32(interfacesTab[i + 1]), Convert.ToInt32(interfacesTab[i + 2]), typeofdata);
                    int_ports.Add(Convert.ToInt32(interfacesTab[i]), p);
                    i += 3;
                }
                //Console.WriteLine(ports[45].getPortID());
                //Console.WriteLine(ports[52].getNodeID());
                // textBox7.Text = interfacesTab[1];
            }
            catch
            {

                Console.WriteLine("Błąd wczytywania. Wciśnij dowolny klawisz aby zakończyć działanie programu...");
                Console.ReadKey();
                Console.Clear();
                Environment.Exit(0);


            }

            CallID = 0;
            pc = new PC();
            pc.initializePC();
            pd = new PolicyDirectory();

            inicjalizacja();

            refreshThread();
            /*                       
            (new Thread(new ThreadStart(() =>
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
                    /*
                     ReceiveThread = new Thread(new ThreadStart(ReceiveFunction));
                     ReceiveThread.IsBackground = true;
                     ReceiveThread.Start();*/
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
                
                /*while (true)
                {*/
                    
                    Queue messages = pc.getData();
                        

                    foreach (Message msg in messages)
                    {
                        
                        Message tempMessage = new Message();
                        tempMessage.source_component_name = "NCC";//
                        

                        try
                        {
                            switch ((String)msg.parameters[0])//zakladam ze w parames[0] bedzie przesylana nazwa message'a
                            {
                                    //w sumie tu dodac rozne mozliwosci, tylko nie wiem co jeszcze
                                case "CALL_REQUEST":
                                    CallID++;
                                    if (msg.source_component_name == "CLIENT1") 
                                    {
                                        setLogText("Dostalem "+(String)msg.parameters[0]+ "od " + msg.source_component_name + "\n");
                                        if (askForCall(pd))  // jakies sprawdzenie w PolicyDirectory
                                        {
                                            //tu cos sprawdza
                                            tempMessage.dest_component_name = "CC1";
                                            tempMessage.parameters.Add("CONNECTION_REQUEST");//parameters[0]
                                            tempMessage.parameters.Add(pd.askDirectory("CLIENT1"));  // adres wywolujacego
                                            tempMessage.parameters.Add(pd.askDirectory("CLIENT2"));   //adres wywolywanego 
                                            tempMessage.parameters.Add(msg.parameters[3]); //liczba kontenerow
                                            tempMessage.parameters.Add(CallID);

                                            
                                             pc.sendData("CC1", tempMessage);//
                                             setLogText("Wyslalem: " + tempMessage.parameters[0]+" do "+tempMessage.dest_component_name+ "\n");
                                            
                                        }
                                    }

                                    if (msg.source_component_name == "CLIENT2")
                                    {
                                        setLogText("Dostalem " + (String)msg.parameters[0] + "od " + msg.source_component_name + "\n");
                                        if (askForCall(pd))  // jakies sprawdzenie w PolicyDirectory
                                        {
                                            //tu cos sprawdza
                                            tempMessage.dest_component_name = "CC1";
                                            tempMessage.parameters.Add("CONNECTION_REQUEST");//parameters[0]
                                            tempMessage.parameters.Add(pd.askDirectory("CLIENT2"));  
                                            tempMessage.parameters.Add(pd.askDirectory("CLIENT1"));
                                            tempMessage.parameters.Add(msg.parameters[3]); //liczba kontenerow
                                            tempMessage.parameters.Add(CallID);  

                                                                            

                                             pc.sendData("CC1", tempMessage);
                                             setLogText("Wyslalem: " + tempMessage.parameters[0] + " do " + tempMessage.dest_component_name + "\n");
                                        }
                                    }
                                        
                                

                                    break;

                                case "ESTABLISHED":
                                    setLogText("Dostalem " + (String)msg.parameters[0] + "od " + msg.source_component_name + "\n");
                                    if (msg.source_component_name == "CC1")
                                    {
                                        setLogText("CC1 powiedzial ze polaczenie zostalo nawiazane\n");
                                        tempMessage.parameters.Add("OK");
                                        tempMessage.dest_component_name = "CLIENT1";
                                        pc.sendData("CLIENT1", tempMessage);
                                        tempMessage.dest_component_name = "CLIENT2";
                                        pc.sendData("CLIENT2", tempMessage);
                                    }   
                                    break;

                                case "CALL_TEARDOWN":    //rozlaczenie od clienta, nie wiem czy to zrobimy
                                    setLogText("Dostalem "+(String)msg.parameters[0]+ "od " + msg.source_component_name + "\n");   //przy Teardown musi cos sprawzdac u PD?
                                    tempMessage.parameters.Add("CONNECTION_TEARDOWN");
                                    tempMessage.parameters.Add(CallID);
                                    tempMessage.dest_component_name = "CC1";
                                    pc.sendData("CC1", tempMessage);
                                    setLogText("Wyslalem: " + tempMessage.parameters[0] + " do " + tempMessage.dest_component_name + "\n");
                                      break;

                                case "NO_ESTABLISHED":
                                      setLogText("Dostalem " + (String)msg.parameters[0] + "od " + msg.source_component_name + "\n");
                                      if (msg.source_component_name == "CC1")
                                      {
                                          setLogText("CC1 powiedzial ze polaczenie jest rozerwane\n");
                                          tempMessage.parameters.Add("NO_ESTABLISHED");
                                          tempMessage.dest_component_name = "CLIENT1";
                                          pc.sendData("CLIENT1", tempMessage);
                                          setLogText("Wyslalem: " + tempMessage.parameters[0] + " do " + tempMessage.dest_component_name + "\n");
                                          tempMessage.dest_component_name = "CLIENT2";
                                          pc.sendData("CLIENT2", tempMessage);
                                          setLogText("Wyslalem: " + tempMessage.parameters[0] + " do " + tempMessage.dest_component_name + "\n");
                                        
                                      }
                                      break;
                                default: break;
                            }
                        }
                        catch
                        {
                            setLogText("Problem przy odczytaniu przychadzacego message\n");
                        }
                    }
                //}
            }
            catch
            {
               setLogText("Problem  w Receive function");
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
                if (text.ElementAt(text.Length - 1) != '\n')    //jak jest enter na końcu tego tekstu to spoko, jak nie to dodaj
                    richTextBox_Log.AppendText(DateTime.Now.ToString(@"h\:mm\:ss tt") + ": " + text + "\n");
                else richTextBox_Log.AppendText(DateTime.Now.ToString(@"h\:mm\:ss tt") + ": " + text);
                richTextBox_Log.ScrollToCaret();  //przewiń boxa z tekstem do karetki
                
            }
        }

        delegate void refreshThreadCallBack();
        public void refreshThread()   //funkcja pokazująca tekst w okienku z logiem
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new refreshThreadCallBack(refreshThread), new object[] { });
            }
            else
            {
                Thread thread;
                thread = new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        ReceiveFunction();
                        //TU FUNKCJA ODŚWIEŻ
                        Thread.Sleep(500);
                    }
                }));
                thread.SetApartmentState(ApartmentState.MTA);
                thread.IsBackground = true;
                thread.Start();
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
                setLogText("PD: Pozwalam na zestawienie polaczenia\n");
                return true;
            }

            else
            {
                setLogText("PD: nie pozwalam na zestawienie polaczenia\n");
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReceiveFunction();
            
        }

        
    }

    class Win32
    {
        /// <summary>
        /// Allocates a new console for current process.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        /// <summary>
        /// Frees the console.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
    }

   
    
}
