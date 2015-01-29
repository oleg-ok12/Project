using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet
{
    //klasa realizujaca packet do wysylania wiadomosci sygnalizacyjnych
    public class SendPacket
    {
        //pierwsza zmiana lol
        private string source;                  // 
        private string destination;
        private List<string> parameters;//jakies parametry pakietu

        public string getSource() { return this.source; }
        public void setSource(string s) { this.source = s; }
        public string getDestination() { return this.destination; }
        public void setDestination(string d) { this.destination = d; }
        public List<string> getParameters() { return this.parameters; }
        public void setParameters(List<string> LP) { this.parameters = LP; }



        public SendPacket()
        {
            source = "-";
            destination = "-";
            parameters = new List<string>();
        }

        public SendPacket(string source, string destination, List<string> parameters)
        {
            this.source = (string)source.Clone();           //zwraca sie referencja
            this.destination = (string)destination.Clone();
            this.parameters = new List<string>(parameters);

        }

        public SendPacket(string source, string destination, string parameters)
        {
            this.source = source;
            this.destination = destination;
            this.setParameters(parameters);
        }

        //tu musi byc jeszcze jakas serializacja i deserializacja...
        //
        //

        public void addParameters(string p)
        {
            if (p == null)
            {
                Console.WriteLine("Wskaznik na liste parametrow jest null");
                return;
            }
            else
            {
                parameters.Add(p);
                Console.WriteLine("Dodano parametr do listy parametrow");
            }

        }

        public void setParameters(string komenda)//kasuje poprzednia liste, rodziela stringa gdzie separatorem jest string ' ' i dodaje kolejne stringi po podziale do tablicy
        {
            parameters = new List<string>();
            String[] splitowane = komenda.Split(' ');
            for (int i = 0; i < splitowane.Length; i++)
            {
                parameters.Add(splitowane[i]);
            }
        }

        public void Zamiana(string parames)// zamienia source i dest i dodaje nowe parametry. moze jednak sie przyda...
        {
            string temp = this.source;
            this.source = this.destination;
            this.destination = temp;
            setParameters(parames);
        }

        /*
         //// wypisywanie SendPacket'u na ekran
        public override string ToString()                   
        {
            string result = "from: " + source + " to: " + destination + " params: ";
            for (int i = 0; i < parameters.Count; i++)
            {
                result += parameters.ElementAt[i] + " | ";
            }
            return result;
        }
        */

    }
}
