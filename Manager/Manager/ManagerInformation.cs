using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ManagerInformation
    {
        public bool ifAdd;
        public int? inContainer;
        public int? outContainer;
        public int type;
       
        public static int current_id;
        public int inPort { get; set; }
        public int outPort { get; set; }

        public ManagerInformation()
        {
            this.inPort = 0;
            this.outPort = 0;
        }

        public ManagerInformation(int inNode, int outNode, int? inContainer, int? outContainer, int type, bool ifAdd)
        {
            this.ifAdd = ifAdd;
            this.inPort = inNode;
            this.outPort = outNode;
            this.inContainer = inContainer;
            this.outContainer = outContainer;
            this.type = type;
            
        }


        static ManagerInformation()
        {
            current_id = 0;
        }

    }
}