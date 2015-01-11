using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientNode
{
    class Program
    {
        [STAThread]
        static void Main(String[] args)
        {
            //Form1 form = new Form1();

            //Serialization.SerializeToFile(form, "klient1.xml");
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
            
        }
    }
}
