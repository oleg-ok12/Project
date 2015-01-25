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
        public bool isRunning { get; set; }//treba? czy NNC chodzi czy nie.
        private string myAddress;
        private Dictionary<string, string> dns;//dns--> wiki. 1string-nazwa dla nas. 2string-adres ip

        private PC pc;

        
        



        public NCC()
        {
            // myAddress = "1.1.1";

           /* (new Thread(new ThreadStart(() =>
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
                   

                }

                catch
                {
                    richTextBox_Log.Text = "cos jest zle";
                }
            }   
        }

        public void ReceiveFunction()
        {
            try
            {
                //Packet.SendPacket sPacket;
                //string ccAddress=
                //string pdAddress=

                

                //Hello!!
                 //send(myAddress, "1.1.1", "Hello");

                while (true)
                {

                    Queue messages = pc.getData();

                    foreach (Message msg in messages)
                    {
                        string src = msg.source_component_name;
                        string dst = msg.dest_component_name;
                        var parames = msg.parameters;

                        try
                        {
                            switch (parames[0])//zakladam ze w parames[0] bedzie przesylana nazwa akcji
                            {
                                    
                            }
                        }
                        catch
                        {
 
                        }

                    }



                }



            }
            catch
            {
               setLogText("Cos sie spieprzylo w Receive function");
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
