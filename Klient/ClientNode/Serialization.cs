using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace ClientNode
{
    static class Serialization
    {
        public static string SerializeObject(Object obj)
        {
            String xml_string = null;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            StringBuilder sb = new StringBuilder();
            StringWriter string_writer = new StringWriter(sb);
            serializer.Serialize(string_writer, obj);
            xml_string = sb.ToString();
            Console.WriteLine("Serialization done");
            return xml_string;
        }

        public static Object DeserializeObject(String xml_string, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            StringReader reader = new StringReader(xml_string);
            XmlTextReader xmlTextReader = new XmlTextReader(reader);
            return xs.Deserialize(xmlTextReader);
        }

        public static void SerializeToFile(Object obj, string fileName)
        {
            if (obj == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, obj);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();

                    Console.WriteLine("Serialization done.");
                }
            }
            catch (Exception ex)
            {
                //Log exception here
                Console.WriteLine(ex);
            }
        }

        public static Object DeSerializeFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(Object); }

            Object objectOut = default(Object);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(Object);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (Object)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
                Console.WriteLine(ex);
            }

            return objectOut;
        }


    }
}
