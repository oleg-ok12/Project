using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addresses
{
    class Address
    {
        public int network { get; set; }
        public int subnetwork { get; set; }
        public int host { get; set; }


        //konstruktor
        public Address(int network, int subnetwork, int host)
        {
            this.network = network;
            this.subnetwork = subnetwork;
            this.host = host;
     
        }


        //konstruktor bez parametrow , nadaje wszystkim zmiennym wartosci np. -1

        public Address()
        {
            this.network = -1;
            this.subnetwork = -1;
            this.host = -1;

        }

        //wypisywanie adresu do konsoli
        public override string ToString()
        {
            return network + "." + subnetwork + "." + host;
            
        }



        //rzutowanie string na int

        public static Address Parse(string str)
        {
            char[] rozdzielacz = { '.' };
            string[] split = str.Split(rozdzielacz);
            int _network = int.Parse(split[0]);
            int _subnetwork = int.Parse(split[1]);
            int _host = int.Parse(split[2]);
            Address addr = new Address(_network, _subnetwork, _host);
            return addr;

        }

        //to samo co standardowe TryParse, tylko
        public static bool TryParse(string str, out Address addr)
        {
            char[] rozdzielacz = { '.' };
            string[] split = str.Split(rozdzielacz);
            if (split.Length == 3)
            {
                try
                {
                    int _network = int.Parse(split[0]);
                    int _subnetwork = int.Parse(split[1]);
                    int _host = int.Parse(split[2]);
                    addr = new Address(_network, _subnetwork, _host);
                    return true;
                }
                catch
                {
                    addr = null;
                    return false;
                }
            }
            else
            {
                addr = null;
                return false;
            }
           
        }
    }
}
