using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkNode
{
    ////reprezentuje kontener VC3
    //struct Container
    //{
    //    //pozycja ktora zajmuje kontener VC3 w przychodzacej VC3, możliwe 1,2,3 jak przychodzi C3 to inContainer == 0
    //    public int inContainer;
    //    //pozycja ktora zajmuje kontener VC3 w wychodzacej VC4
    //    public int outContainer;
    //    //port wyjsciowy dla kontenera VC3
    //    public int outPort;
    //}

    //reprezentuje tak naprawde punkt komutacyjny a nie cale pole 
    class Matrix
    {
        //port wejściowy z którym jest połączony
        private int inPort;
        //port wyjsciowy z ktorym jest polaczony (gdy wyysylane VC4)
        private int outPort;
        //pozycja ktora zajmuje kontener VC3 w przychodzacej STM, możliwe 1,2,3 jak przychodzi C3 to inContainer == null
        public int? inContainer;
        //pozycja ktora zajmuje kontener VC3 w wychodzacej STM
        public int? outContainer;
        //typ: VC4-3 lub VC3-2, C4-1, C3-0, 
        private int type;


        public int GetType() { return type; }
     
        public int GetOutPort() { return outPort; }
        public int GetInPort() { return inPort; }

       
        public Matrix() { }
       
        public Matrix(int inPort, int outPort, int? inContainer, int? outContainer, int type)
        {
            
            this.inPort = inPort;
            this.outPort = outPort;
            this.inContainer = inContainer; ;
            this.outContainer = outContainer;
           
            this.type = type;
           
        }




    }

}
