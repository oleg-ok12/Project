using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;




namespace NetworkNode
{
    class Switching
    {

        //reprezentuje wejścia pola komutacyjnego
        private Dictionary<string, Matrix> matrixes = new Dictionary<string, Matrix>();// reprezentuje wejścia pola komutacyjnego (wyjść nie musi bo i tak porty są tylko w jedną stronę
        // key[0] = inPort, key[1] = inContainer, key[2] = outContainer, key[3] = outPort -> żeby na ten sam port mogły przyjść różne VC3(na różnych pozycjach) to matrix musi być identyfikowany w taki sposób
        private int VC4size;
        private int VC3size;
        private int matrixes_number;
        
        Matrix temp_matrix = new Matrix();





        //public void SetSwitching(int inPort, int outPort, int type, bool ifClient, int inContainer, int outContainer)       // MA SIE WYWOŁAć PO DOSTANIU INFO OD MANAGERA  
        //{
        //    matrixes.Add(inPort, new Matrix(inPort, outPort, inContainer, outContainer, type, ifClient));  //dodawanie musi być z kluczem oznaczającym port wejściowy, żeby później dało się znaleźć w Switching
        //    matrixes_number++;

        //    Console.WriteLine("Dodano matrix: portIn {0}, portOut {1}, inContainer {2}, outContainer{3}", inPort, outPort, inContainer, outContainer);
        //}

        //private void DeleteSwitching()
        //{
        //    matrixes.Clear();
        //    matrixes_number = 0;
        //}


        //public int ClientSwitching(string portID, ClientInformation info, out ClientInformation client_info, out CharacteristicInformation char_info, out int? out_container)
        //{
             
        //    //  temp_matrix = null; //wyczyszczenie pomocniczego matrixa, żeby nie było błędów typu dwa razy obsłużone to samo albo coś takiego                           
        //    temp_matrix = matrixes[portID];       //znalezienie matrixa odpowiadającego danemu portowi

        //    int port_out_ID;


        //    switch (temp_matrix.GetType())
        //    {
        //        case (1):
        //            {
        //                if (temp_matrix.GetIfClient()) //kolejny węzeł kliencki, więc wysyłane jest dokładnie to samo na odpowiedni port
        //                {
        //                    //ports_out[temp_matrix.GetOutPort()].SendObject(inf);  //wykonanie na porcie wyjściowym o numerze wyciągniętym z odpowiedniego Matrixa
        //                    // operacji wysyłania kontenera VC4 - 
        //                    port_out_ID = temp_matrix.GetOutPort();

        //                    client_info = info;
        //                    char_info = null;
        //                    out_container = null;


        //                    return port_out_ID;

        //                }
        //                else //kolejny węzeł jest sieciowy dlatego musimy C4 zapokować w VC4 
        //                {
        //                    CharacteristicInformation characteristic_information = new CharacteristicInformation(info, "POH");    //utworzenie nowego kontenera wyjściowego i zapakowanie naszej informacji na odpowiednie miejsce(tutaj nr 1 bo VC4)
        //                    port_out_ID = temp_matrix.GetOutPort();
        //                    char_info = characteristic_information;
        //                    client_info = null;
        //                    out_container = null;

        //                    return port_out_ID;
        //                }

        //                break;
        //            }

        //        case (0): //oznacza że przyszło C3
        //            {
        //                if (temp_matrix.GetIfClient())
        //                {


        //                    //ports_out[cont.outPort].SendObject(inf); //wysłanie informacji użytkownika portem wyjściowym zapisanym w kontenerze z Matrixa
        //                    port_out_ID = temp_matrix.GetOutPort();
        //                    client_info = info;
        //                    char_info = null;
        //                    out_container = null;


        //                    return port_out_ID;




        //                }
        //                else
        //                {

        //                    CharacteristicInformation characteristic_information = new CharacteristicInformation(info, "POH");    //utworzenie nowego kontenera wyjściowego i zapakowanie naszej informacji na odpowiednie miejsce
        //                    port_out_ID = temp_matrix.GetOutPort();
        //                    char_info = characteristic_information;
        //                    client_info = null;
        //                    out_container = temp_matrix.outContainer;

        //                    return port_out_ID;
        //                }

        //            }


        //            break;
        //    }





        //    client_info = null;
        //    char_info = null;
        //    out_container = null;

        //    return 0;
        //}

        public int CharacteristicSwitching(string portID, CharacteristicInformation info, out CharacteristicInformation char_info, out ClientInformation client_info, out int? out_container)
        {


            temp_matrix = null; //wyczyszczenie pomocniczego matrixa, żeby nie było błędów typu dwa razy obsłużone to samo albo coś takiego                           
            temp_matrix = matrixes[portID];       //znalezienie matrixa odpowiadającego danemu portowi
            int port_out_ID;


            switch (temp_matrix.GetType())
            {
                case (3):  //przychodzi VC4
                    {

                        //if (temp_matrix.GetIfClient())   ///jak kolejny port jest portem klienckim
                        //{
                        //    ClientInformation client_information = info.client_information;
                        //    //ports_out[temp_matrix.GetOutPort()].SendObject(client_information);  //wysłanie informacji klienckiej na port ustawiony w matrixie
                        //    port_out_ID = temp_matrix.GetOutPort();
                        //    char_info = null;
                        //    client_info = client_information;
                        //    out_container = null;

                        //    return port_out_ID;

                        //}
                        //else    //jak kolejny port jest portem sieciowym
                        //{

                            port_out_ID = temp_matrix.GetOutPort();
                            char_info = info;
                            client_info = null;
                            out_container = null;

                            return port_out_ID;

                        //}



                        //break;
                    }

                case (2):   //przychodzi VC3
                    {

                        //if (temp_matrix.GetIfClient())   //jak kolejny port jest portem klienckim
                        //{

                        //    ClientInformation client_information = info.client_information;

                        //    // ports_out[temp_matrix.GetOutPort()].SendObject(client_information);   //to wtedy C3 jest wysyłany prosto do klienta
                        //    port_out_ID = temp_matrix.GetOutPort();
                        //    client_info = client_information;
                        //    char_info = null;
                        //    out_container = null;

                        //    return port_out_ID;





                        //}
                        //else  //jak kolejny port jest portem sieciowym
                        //{

                            ClientInformation client_information = info.client_information;


                            CharacteristicInformation characteristic_information = new CharacteristicInformation(client_information, "POH");    //utworzenie nowego kontenera wyjściowego i zapakowanie naszej informacji na odpowiednie miejsce
                            //ports_out[cont.outPort].SendObject(characteristic_information); //wysłanie informacji charakterystycznej portem wyjściowym zapisanym w kontenerze z Matrixa
                            port_out_ID = temp_matrix.GetOutPort();
                            char_info = characteristic_information;
                            client_info = null;
                            out_container = temp_matrix.outContainer;

                            return port_out_ID;






                        //}

                        //break;
                    }
            }

            client_info = null;
            char_info = null;
            out_container = null;

            Console.WriteLine("Coś poszło nie tak");

            return 0;

        }


        public void addMatrix(int inPort, int outPort, int? inContainer, int? outContainer, int type)
        {
            Matrix new_m = new Matrix(inPort, outPort, inContainer, outContainer, type);
            //string key = inPort.ToString() + inContainer.ToString() +outContainer.ToString() +outPort.ToString();
            string key = inPort.ToString() + inContainer.ToString();

           
            matrixes.Add(key, new_m);
            Console.WriteLine("Matrix added: {0} {1} {2} {3} {4} ", inPort, outPort, inContainer, outContainer, type);

        }

        public void clearMatrix(int inPort, int? inContainer)
        {
            string key = inPort.ToString() + inContainer.ToString();
            matrixes.Remove(key);

            //matrixes.Clear();
            //Console.WriteLine("Wyczyszczono pole komutacyjne");
        }

    }

}
