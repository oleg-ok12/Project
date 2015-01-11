using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientNode
{
    public class ClientInformation
    {
        public static int current_id;

        public int size { get; set; }
        public string type { get; set; }

        public int data_size { get; set; }
        public int id { get; set; }
        public string text;

        public ClientInformation()
        {
            data_size = 0;
            id = 0;
        }

        public ClientInformation(string text, int data_type)
        {
            if (data_type == 0)
            {
                this.text = text;
                data_size = text.Length;
                size = 774;
                id = getNextID();
                type = "C3";
            }

            if (data_type == 1)
            {
                this.text = text;
                data_size = text.Length;
                size = 2340;
                id = getNextID();
                type = "C4";
            }


        }

        public ClientInformation(int data_size)
        {
            if (data_size <= 2340 && data_size > 783)
            {
                this.size = 2340;
                type = "C4";
            }
            else { this.size = 783; type = "C3"; }

            this.data_size = data_size;
            id = getNextID();

        }

        static ClientInformation()
        {
            current_id = 0;
        }

        public string getType()
        {
            return type;
        }

        private static int getNextID()
        {

            return ++current_id;

        }

    }
}
