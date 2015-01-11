using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkNode
{
    class Program
    {
        static void Main(string[] args)
        {

           
            Console.WriteLine("Choose file name:");
            string file_name = Console.ReadLine();
            
            
                        NetworkNode nnode = (NetworkNode)Serialization.DeSerializeFromFile(file_name, typeof(NetworkNode));
                        nnode.initializeNode();
                        
                     
                        nnode.Simulation();
                        
            
            Console.ReadLine();


        }
    }
}
