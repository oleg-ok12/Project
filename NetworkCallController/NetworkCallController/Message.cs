using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCallController
{
    public class Message
    {
        public string source_component_name;
        public List<int> source_component_id;
        public string dest_component_name;
        public List<int> dest_component_id;
        public List<string> parameters;//jakies parametry pakietu ZMIENILEM SOBIE TUTAJ NA STRINGA


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //moze cos w tym stylu
        //parameters[0] polecenie np"CALL_REQ", "DISCON" czy cos , parameters[1] od kogo parameters[2] do kogo
        //np Client1 wysyla do NCC zadanie polaczenia. to w Message bedzie tak
        //source="CLIENT1" dest="NCC" params[0]="CALL_REQUEST" params[1]="CLIENT1" params[2]="CLIENT2"
        //i wtedy NCC bedzie patrzyl w te params[2]

        public Message()
        {
            parameters = new List<string>();
            source_component_id = new List<int>();
            dest_component_id = new List<int>();
        }

        public Message(string scn, List<int> sc_id, string dcn, List<int> ds_id, List<string> param)
        {
            source_component_name = scn;
            source_component_id = sc_id;
            dest_component_name = dcn;
            dest_component_id = ds_id;

            parameters = param;

        }


    }
}
