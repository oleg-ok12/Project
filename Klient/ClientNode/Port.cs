using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Collections;

namespace ClientNode
{

    public enum Data
    {
       cos,client, characteristic, control
    }


    public class Port
    {

        //public delegate void MediumConnected(object sender, EventArgs e);
        // public delegate void MediumDisconnected(object sender, EventArgs e);

        public int router_ID;
        public int port_ID;
        public static int SPECJAL_PORT = 99;
        private TcpClient tcp;
        private NetworkStream stm;


        //private List<String> received_data = new List<string>(); //dana które zostały odebrane, ale jeszcze nie przetworzone przez pole komutacyjne
        private Queue received_data = new Queue();

        public Data type_of_receiving_data;

        public static int port = 40000;
        public static string ip = "localhost";
        public static string username = "Client";

        private volatile int errorCounter = 0;
        private volatile bool medConnected = false;
        //public Thread threadMed = null;

        // public event MediumConnected ConnectedEv;
        //public event MediumDisconnected DisconnectedEv;
        public Port()
        {
        }

        public Port(int routerID, int portID, Data type_of_data)
        {
            router_ID = routerID;
            this.port_ID = portID;
            type_of_receiving_data = type_of_data;
            received_data = new Queue();

        }


        // public void performSignalConnected()
        //{
        //    ConnectedEv(this, null);
        //}
        // public void performSignalDisconnected()
        // {
        //     DisconnectedEv(this, null);
        // }


        public void connect()
        {
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    tcp = new TcpClient();
                    //parent_.netOutput.AppendText("Connecting to medium on port " + porty[i] + " ...\n");
                    Console.WriteLine("Connecting to medium Node" + router_ID + " on port:" + port_ID + " ...");
                    tcp.Connect(ip, port);
                    stm = tcp.GetStream();
                    //portAndStream.Add(Convert.ToInt32(porty[i]), i);
                }
                catch
                {
                    //parent_.netOutput.AppendText("Connected to medium on port " + porty[i] + " failed!\n");
                    Console.WriteLine("Connected to medium Node" + router_ID + " on port:" + port_ID + " failed!");
                    medConnected = false;
                    //parent_.connectMediumToolStripMenuItem.Enabled = true;
                    //parent_.connectMediumToolStripMenuItem.Text = "Connect";
                    break;
                }
                // parent_.netOutput.AppendText("Connected to medium on port " + porty[i] + " success!\n");
                //parent_.netOutput.AppendText("Starting writing thread on port " + porty[i] + " ...\n");

                Console.WriteLine("Connected to medium Node" + router_ID + " on port:" + port_ID + " success!");
                Console.WriteLine("Starting writing thread. Node" + router_ID + " on port:" + port_ID + " ...");

                Thread zapis = new Thread(new ParameterizedThreadStart(connectThreadWrite));
                zapis.IsBackground = true;
                //zapis.Start(new KeyValuePair<int, Stream>(Convert.ToInt32(porty[i]), stm[i]));
                zapis.Start(new KeyValuePair<int, Stream>(router_ID, stm));

                //parent_.netOutput.AppendText("Starting reading thread on port " + porty[i] + " ...\n");
                Console.WriteLine("Starting reading thread. Node" + router_ID + " on port:" + port_ID + " ...");
                Thread odczyt = new Thread(new ParameterizedThreadStart(connectThreadRead));
                odczyt.IsBackground = true;
                //odczyt.Start(new KeyValuePair<int, Stream>(Convert.ToInt32(porty[i]), stm[i]));
                odczyt.Start(new KeyValuePair<int, Stream>(router_ID, stm));
                medConnected = true;

            }

            //if (medConnected.Equals(true))          
            //    ConnectedEv(this, null);

        }


        private void connectThreadWrite(object para)
        {
            int nr = ((KeyValuePair<int, Stream>)para).Key;

            ASCIIEncoding asen = new ASCIIEncoding();

            byte[] ba = asen.GetBytes(username + "_" + nr + ":" + port_ID);
            ((KeyValuePair<int, Stream>)para).Value.Write(ba, 0, ba.Length);

        }
        private void connectThreadRead(object para)
        {
            int nr = ((KeyValuePair<int, Stream>)para).Key;

            ASCIIEncoding asen = new ASCIIEncoding();

            int bytesRead;
            int buffersize = 60000;
            byte[] buffer = new byte[buffersize];
            Stream tmp = ((KeyValuePair<int, Stream>)para).Value;



            //while (medConnected.Equals(true))
            while (true)
            {
                try
                {
                    bytesRead = tmp.Read(buffer, 0, buffersize);
                    if (bytesRead == -1)
                        Thread.Sleep(10);
                    else if (bytesRead == 0)
                    {
                        //parent_.netOutput.AppendText("Connection reject!\n");
                        //parent_.connectMediumToolStripMenuItem.PerformClick();

                        Console.WriteLine("Connection reject!");
                        medConnected = false;    // DOPISANE PRZEZE MNIE
                    }
                    else
                    {
                        //parent_.netOutput.AppendText("Recived message from port " + nr + ": \n");
                        //parent_.netOutput.AppendText(asen.GetString(buffer, 0, buffer.Length));
                        //parent_.netOutput.AppendText("\n");
                        StringBuilder sb = new StringBuilder();
                        String data = sb.Append(asen.GetString(buffer, 0, buffer.Length)).ToString();
                        String new_data = data.Remove(bytesRead);
                        if (new_data.Length > 0)
                        {
                            Console.WriteLine();
                            //Console.WriteLine(new_data + " to " + router_ID + port_ID);     // WYPISUJE NA EKRAN PLIK ZAWARTOSC XML I GDZIE WYSYLA
                        }
                        putData(new_data);

                        sb.Clear();
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                }
                catch
                {
                    errorCounter++;
                    //parent_.netOutput.AppendText("Reading byte from port " + nr + " failed!\n");
                    Console.WriteLine("Reading byte from port " + nr + " failed!");

                    if (errorCounter > 5)
                    {
                        //parent_.connectMediumToolStripMenuItem.PerformClick();       //CO TO ROBI?
                        errorCounter = 0;
                    }
                    //DisconnectedEv(this,null);
                }

            }
        }
        public bool send(string message)
        {
            try
            {
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] bb = asen.GetBytes(message);
                //stm[portAndStream[port]].Write(bb, 0, bb.Length);
                stm.Write(bb, 0, bb.Length);
                Console.WriteLine("Message sent from Node {0} (port {1}): ", router_ID, port_ID);
            }
            catch
            {
                //parent_.netOutput.AppendText("Failed to send message!\n");
                Console.WriteLine("Failed to send message!");
                return false;
            }
            return true;
        }
        public void disconnect()
        {
            if (medConnected.Equals(false))
                return;

            medConnected = false;
            //parent_.netOutput.AppendText("Closing connections...\n");
            Console.WriteLine("Closing connections...");
            try
            {
                //for (int i = 0; i < tcpC1.Length; i++)
                //{
                //tcpC1[i].GetStream().Flush();
                //tcpC1[i].Client.Close();
                //tcpC1[i].Close();
                tcp.GetStream().Flush();
                tcp.Client.Close();
                tcp.Close();
                //}
            }
            catch (InvalidOperationException) { }
        }


        //public bool getMedConnected()
        //{
        //    return medConnected;
        //}

        private void putData(String data)
        {
            received_data.Enqueue(data);

        }
        public Queue getData()
        {
            Queue tmp_queue = new Queue();

            if (received_data.Count > 0)
            {
                foreach (String info in received_data)
                {
                    tmp_queue.Enqueue(info);
                }
            }

            //int remove = Math.Max(0, received_data.Count);
            //received_data.RemoveRange(0, remove);

            received_data.Clear();

            return tmp_queue;

        }



    }
}
