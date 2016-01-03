using Microsoft.XmlDiffPatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CalculationCSharp.Models.Calculation;
using System.Diagnostics;

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

        public string MatchXML(String sourcePath, string actualPath)

        {
            List<OutputCompare> List = new List<OutputCompare>();
            StringBuilder strbuilder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strbuilder);

            string sourceId, sourceValue;

            using (XmlReader xr = XmlReader.Create(new StringReader(sourcePath)))

            {
                while (!xr.EOF)
                {

                    xr.ReadToFollowing("ID");

                    sourceId = xr.ReadString();

                    xr.ReadToFollowing("Value");

                    sourceValue = xr.ReadString();


                    using (XmlReader xr1 = XmlReader.Create(new StringReader(actualPath)))

                    {

                        while (!xr1.EOF)

                        {

                            xr1.ReadToFollowing("ID");
                            string ID;
                            ID = xr1.ReadString();

                            if (ID == sourceId)

                            {                              
                                string value;

                                xr1.ReadToFollowing("Value");

                                value = xr1.ReadString();

                                if (sourceValue != value)
                                    List.Add(new OutputCompare { ID = sourceId, NewID = ID, Value = sourceValue, NewValue = value });

                            }

                        }
                        
                    }

                }
                if(List.Count >0 )
                {

                
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Compare");

                    foreach (OutputCompare OutputCompare in List)
                    {
                        writer.WriteStartElement("Output");

                        writer.WriteElementString("ID", OutputCompare.ID.ToString());
                        writer.WriteElementString("NewID", OutputCompare.NewID.ToString());
                        writer.WriteElementString("Value", OutputCompare.Value.ToString());
                        writer.WriteElementString("NewValue", OutputCompare.NewValue.ToString());

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();

                    return strbuilder.ToString();

                }
                return null;
            }
        }
    }

    public class OutputCompare
    {
        public string ID { get; set; }
        public string NewID { get; set; }
        public string Value { get; set; }
        public string NewValue { get; set; }
    }

}




