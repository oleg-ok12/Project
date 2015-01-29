using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCallController
{
    class PolicyDirectory
    {
        public SerializableDictionary<String, List<int>> slownik_adresow = new SerializableDictionary<string, List<int>>();
        List<int> client1;
        List<int> client2;
        List<int> pusta = new List<int>();
        public bool isAllow;

       public PolicyDirectory()
        {

            isAllow = true;
           List<int> client1 = new List<int>();
           client1.Add(1);
           client1.Add(0);
           client1.Add(1);
           List<int> client2 = new List<int>();
           client1.Add(1);
           client1.Add(0);
           client1.Add(2);
           slownik_adresow.Add("CLIENT1", client1);
           slownik_adresow.Add("CLIENT2", client2);

        }

       public List<int> askDirectory(String nazwa_klienta)
       {
           if (nazwa_klienta == "CLIENT1")
           {
               return client1;
           }
           else if (nazwa_klienta == "CLIENT2")
           {
               return client2;
           }
           return pusta;
       }
        

    }


}
