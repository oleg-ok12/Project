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
        List<int> client1 = new List<int>();
        List<int> client2 = new List<int>();
        List<int> pusta = new List<int>();
        public bool isAllow;

        public PolicyDirectory()
        {

            client1.Add(1);
            client1.Add(0);
            client1.Add(1);
            client2.Add(1);
            client2.Add(0);
            client2.Add(2);

            slownik_adresow.Add("CLIENT1", client1);
            slownik_adresow.Add("CLIENT2", client2);


            isAllow = true;


        }

        public List<int> askDirectory(String nazwa_klienta)
        {
            try
            {
                List<int> address = new List<int>();
                address = slownik_adresow[nazwa_klienta];

                return address;
            }

            catch (NullReferenceException e)
            {
                Console.WriteLine("Taki klient nie istnieje");
                return new List<int>(new int[] { 0, 0, 0 });
            }


        }


    }


}
