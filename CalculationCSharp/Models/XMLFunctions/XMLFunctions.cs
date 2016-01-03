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

        public void MatchXML(String sourcePath, string actualPath)

        {

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

                                    Debug.Print(ID + " " + value);
                            }

                        }
                        
                    }

                }

            }
        }
    }
}




