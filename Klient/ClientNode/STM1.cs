using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientNode
{
    public class STM1
    {
        public int? AU_Pointer;
        public CharacteristicInformation information;

        public STM1()
        {
        }

        public STM1(CharacteristicInformation info, int? pointer)
        {
            information = info;
            AU_Pointer = pointer;
        }

    }
}
