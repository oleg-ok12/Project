using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkCallController
{
    class PC  //Packet Controller - ma służyć do odbierania i wysyłania wiadomości sygnalizacyjnych między komponentami
    {
        public SerializableDictionary<int, Port> ports; // porty
        public SerializableDictionary<String, int> name_id;  //potrzeba do zmiany nazwy elementu na odpowiedni port - element który coś chce wysłać podaje nazwę np. "NODE1" a PC znajduje odpowiedni port

        private Thread connectThread;


        private Queue data;
        private Queue sync_data;

        public PC()
        {
            ports = new SerializableDictionary<int, Port>();
            name_id = new SerializableDictionary<string, int>();
        }


        public void initializePC()
        {
            data = new Queue();
            sync_data = Queue.Synchronized(data);


            foreach (Port port in ports.Values)
            {

                connectThread = new Thread(new ParameterizedThreadStart(connectToNetwork));
                connectThread.Start(port);
            }

        }


        public Queue getData()
        {
            Queue received_messages = new Queue();

            foreach (Port port in ports.Values)
            {
                sync_data = port.getData();
            }

            foreach (String str in sync_data)
            {
                //CharacteristicInformation info = new CharacteristicInformation();
                Message msg = new Message();
                msg = (Message)Serialization.DeserializeObject(str, typeof(Message));
                received_messages.Enqueue(msg);
            }

            return received_messages;
        }

        public void sendData(String port_out, Message message) //należy podać nazwę, np. CC1, 
        {

            String serialized_message = Serialization.SerializeObject(message);

            int id_port_out = -1;
            foreach (String name in name_id.Keys)
            {
                if (name == port_out)
                {
                    id_port_out = name_id[name];
                }
            }

            try
            {
                ports[id_port_out].send(serialized_message);
            }
            catch (System.IndexOutOfRangeException e)
            {
                throw new System.ArgumentOutOfRangeException(
            "Parameter index is out of range. Port " + id_port_out + "does not exist.");
            }
        }



        private void connectToNetwork(object para)
        {
            Port tmp_port = (Port)para;
            tmp_port.connect();


        }



    }
}
