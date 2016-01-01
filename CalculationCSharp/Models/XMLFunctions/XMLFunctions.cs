using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CalculationCSharp.Models.XMLFunctions
{
    public class XMLFunctions
    {
        public string XMLStringBuilder(object Object)
        {
            XmlSerializer serial = new XmlSerializer(Object.GetType());
            MemoryStream ms = new MemoryStream();
            serial.Serialize(ms, Object);
            ms.Position = 0;
            StreamReader r = new StreamReader(ms);
            string res = r.ReadToEnd();

            return res;
        } 

    }
}