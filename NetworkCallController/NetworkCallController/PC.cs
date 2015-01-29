using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkCallController
{
   public class PC  //Packet Controller - ma służyć do odbierania i wysyłania wiadomości sygnalizacyjnych między komponentami
    {
        public SerializableDictionary<String, Port> string_ports; // porty
        public SerializableDictionary<String, int> name_id;  //potrzeba do zmiany nazwy elementu na odpowiedni port - element który coś chce wysłać podaje nazwę np. "NODE1" a PC znajduje odpowiedni port

        private Thread connectThread;


        private Queue data;
        private Queue sync_data;

        public PC()
        {
            string_ports = new SerializableDictionary<String, Port>();
            name_id = new SerializableDictionary<string, int>();

        }


        public void initializePC()
        {
            data = new Queue();
            sync_data = Queue.Synchronized(data);


            foreach (Port port in NCC.int_ports.Values)
            {

                connectThread = new Thread(new ParameterizedThreadStart(connectToNetwork));
                connectThread.Start(port);
            }

        }


        public Queue getData()
        {
            Queue received_messages = new Queue();
            Queue messages = new Queue();

            foreach (Port port in NCC.int_ports.Values)
            {
                sync_data = port.getData();


                if (sync_data != null)
                {
                    foreach (String str in sync_data)
                    {
                        //CharacteristicInformation info = new CharacteristicInformation();
                        Message msg = new Message();
                        msg = (Message)Serialization.DeserializeObject(str, typeof(Message));
                        received_messages.Enqueue(msg);
                    }
                }
                else
                {
                    Console.WriteLine("NCC nie otrzymal zadnych danych");
                }
            }

            return received_messages;
        }

        public void sendData(String port_out, Message message) //należy podać nazwę, np. CC1, 
        {

            String serialized_message = Serialization.SerializeObject(message);

            //int id_port_out=-1;
            //foreach (String name in name_id.Keys)
            //{
            //    if(name == port_out)
            //    {
            //        id_port_out = name_id[name];
            //    }
            //}

            try
            {
                switch (port_out)
                {
                    case "CLIENT1":
                        
                   NCC.int_ports[1].send(serialized_message);
                    break;
                default: break;
                }
                
            }
            catch (System.IndexOutOfRangeException  e)
            {
                throw new System.ArgumentOutOfRangeException(
            "Parameter index is out of range. Port " + port_out + "does not exist.");
            }
        }



        private void connectToNetwork(object para)
        {
            Port tmp_port = (Port)para;
            tmp_port.connect();


        }



    }
}
