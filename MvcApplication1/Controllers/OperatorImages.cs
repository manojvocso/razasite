using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MvcApplication1.Controllers
{
    public class OperatorImages
    {

        public List<Operator> ListOfOperators { get; set; }
    }

    public class Operator
    {
        [XmlElement("countryid")]
        public int countryid { get; set; }
        [XmlElement("operatorname")]
        public string operatorname { get; set; }
        [XmlElement("operatorimage")]
        public string operatorimage { get; set; }

    }


    /// <summary>

    /// TODO: Update summary.

    /// </summary>

    public static class SerializationUtility<T> where T : class
    {

        public static string SerializeAnObject(T obj)
        {

            var doc = new XmlDocument();

            var serializer = new XmlSerializer(obj.GetType());

            var stream = new System.IO.MemoryStream();

            try
            {

                serializer.Serialize(stream, obj);

                stream.Position = 0;

                doc.Load(stream);

                return doc.InnerXml;



            }

            catch
            {

                throw;

            }

            finally
            {

                stream.Close();

            //    stream.Dispose();



            }



        }



        public static T DeserializeObject(String pXmlizedString)
        {

            

            XmlSerializer xs = new XmlSerializer(typeof (T));

            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));

            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            return (T) xs.Deserialize(memoryStream);

        }



        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {

            UTF8Encoding encoding = new UTF8Encoding();

            Byte[] byteArray = encoding.GetBytes(pXmlString);

            return byteArray;

        }



        private static string ByteArrayToString(byte[] byteArray)
        {

            UTF8Encoding encoding = new UTF8Encoding();

            return encoding.GetString(byteArray);

        }



    }

}