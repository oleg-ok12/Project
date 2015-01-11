using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class Program
    {
        static void Main(string[] args)
        {


            SerializableDictionary<int, Port> ports = new SerializableDictionary<int, Port>();
            ports.Add(1, new Port(1, 1, Data.manager));
            ports.Add(2, new Port(1, 2, Data.manager));
            ports.Add(3, new Port(1, 3, Data.manager));
            ports.Add(4, new Port(1, 4, Data.manager));
            ports.Add(5, new Port(1, 5, Data.manager));


            Manager man = new Manager(1, ports, "Network3.config");

            Serialization.SerializeToFile(man, "manager.xml");
             
            String file_name = "manager.xml";
            //Manager man = (Manager)Serialization.DeSerializeFromFile(file_name, typeof(Manager)); 
            man.initializeManager();
            //while (true)
            //{
            //    Console.ReadLine();
            //    Console.WriteLine("Podaj wezel: nr_wezla/port wejsciowy/port wyjsciowy/kontener wejsciowy/kontener wyjsciowy/typ/ifClient/ifAdd");
            //    string s = Console.ReadLine();
            //    string[] x = s.Split(new string[] { "/", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //    int? inContainer, outContainer;
            //    if (x[3] == "null") inContainer = null; else inContainer=Convert.ToInt32(x[3]);
            //    if (x[4] == "null") outContainer = null; else outContainer=Convert.ToInt32(x[4]);
            //    ManagerInformation info = new ManagerInformation(Convert.ToInt32(x[1]), Convert.ToInt32(x[2]), inContainer, outContainer, Convert.ToInt32(x[5]), Convert.ToBoolean(x[6]), Convert.ToBoolean(x[7]));
            //    String serialized_info = Serialization.SerializeObject(info);
            //    Console.ReadLine();
            //    man.ports[Convert.ToInt32(x[0])].send(serialized_info);
            //}
            Console.ReadLine();

            man.sendInformation(5, 2, 1, 1, 2, 1, true);
            man.sendInformation(1, 2, 3, 2, 1, 1, true);
            man.sendInformation(4, 1, 2, 1, 1, 1, true);
            man.sendInformation(3, 2, 3, 1, 2, 1, true);

            Console.ReadLine();

            man.sendInformation(5, 2, 1, 1, 2, 1, false);
        }
    }
}
