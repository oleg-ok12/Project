using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class Node
    {

        public int node_id;  //id wezla
        public int ports;   //ilość portów
        public string type;   // typ węzła- CLIENT, NETWORK
        public Dictionary<int, List<bool> > used_c3_containers_in; //lista portów + wykorzystywane kontenery wejściowe typu c3: numer portu, kolejne kontenery
        public Dictionary<int, List<bool>> used_c3_containers_out;//lista portów + wykorzystywane kontenery wyjściowe typu c3:numer portu, kolejne kontenery
        public Dictionary<int,bool> if_c4_in_used;     //jak jest true to wtedy nie można dodać żadnego c3 ani c4
        public Dictionary<int,bool> if_c4_out_used;    // to co wyżej

        public List<int> if_client; //numery węzłów gdzie następny węzeł to klient

        public Node()
        {
            used_c3_containers_in = new Dictionary<int, List<bool>>();
            used_c3_containers_out = new Dictionary<int, List<bool>>();
            if_c4_in_used = new Dictionary<int,bool>();
                        if_c4_out_used = new Dictionary<int,bool>();

        }

        public Node(int node_id, int ports, string type)
        {
            this.node_id = node_id;
            this.ports = ports;
            this.type = type;


            used_c3_containers_in = new Dictionary<int, List<bool>>();
            used_c3_containers_out = new Dictionary<int, List<bool>>();
            if_c4_in_used = new Dictionary<int, bool>();
            if_c4_out_used = new Dictionary<int, bool>();

            for (int i = 1; i < ports+1; i++)
            {
                if_c4_in_used.Add(i, false);
                if_c4_out_used.Add(i, false);

                List<bool> list_false = new List<bool>();
                list_false.Add(false);
                list_false.Add(false);
                list_false.Add(false);

                used_c3_containers_in.Add(i,list_false );
                used_c3_containers_out.Add(i, list_false);




            }


        }


    }
}
