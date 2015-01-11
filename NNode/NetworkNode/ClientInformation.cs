using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkNode
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
                type = "C3";
                size = 774;
                id = getNextID();
            }

            if (data_type == 1)
            {
                this.text = text;
                data_size = text.Length;
                type = "C4";
                size = 2340;
                id = getNextID();
            }


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
