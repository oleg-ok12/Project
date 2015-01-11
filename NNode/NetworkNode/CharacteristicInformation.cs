using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkNode
{
    public class CharacteristicInformation
    {
        public string poh;
        public ClientInformation client_information;
        public string type;
        public int size;

        public CharacteristicInformation()
        {

            poh = "POH";

            client_information = new ClientInformation() ;

        }

        public CharacteristicInformation(ClientInformation inf,  String poh)
        {
            this.poh = poh;
            client_information = inf;
            
            type = inf.getType();
            size = inf.size + 9;
        }

        public string getTypeOfContainer()
        {
            return type;
        }

    }
}
