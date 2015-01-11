using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkCallController
{
    public partial class NCC : Form
    {
        public bool isRunning { get; set; }//treba? czy NNC chodzi czy nie.
        private string myAddress;
        private Dictionary<string, string> dns;//dns--> wiki. 1string-nazwa dla nas. 2string-adres ip
        private int maxConnections;


        public NCC()
        {

            InitializeComponent();
        }

        private void butClear_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox_Log.Clear();
            }
            catch
            {

            }
        }
    }


   
    }
}
