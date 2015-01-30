using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientNode
{
    public class Message
    {
        public string source_component_name;
        public List<int> source_component_id;
        public string dest_component_name;
        public List<int> dest_component_id;
        public List<object> parameters;//jakies parametry pakietu

        public Message()
        {
            parameters = new List<object>();
            source_component_id = new List<int>();
            dest_component_id = new List<int>();
        }

        public Message(string scn, List<int> sc_id, string dcn, List<int> ds_id, List<object> param)
        {
            source_component_name = scn;
            source_component_id = sc_id;
            dest_component_name = dcn;
            dest_component_id = ds_id;

            parameters = param;

        }


        public Message(string scn, string dcn, List<Object> param)
        {
            source_component_name = scn;

            dest_component_name = dcn;


            parameters = param;

        }


    }
}
