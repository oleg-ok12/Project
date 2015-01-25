using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCallController
{
    //klasa reprezentuje pewne polaczenie NCC z kims
    public class Connection
    {
        public string source { get; private set; }
        public string destination { get; private set; }
        public int number { get; private set; }
        public bool status { get; private set; }

        public Connection()
        {
            source = null;
            destination = null;
            number = 0;
            status = false;
        }

        public Connection(string source, string destination, int number, bool status)
        {
            this.source = source;
            this.destination = destination;
            this.number = number;
            this.status = status;
        }
    }
}