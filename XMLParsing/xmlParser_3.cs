using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLParsing
{
    class xmlParser_3
    {
        public void parseXML()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            using (XmlReader reader = XmlReader.Create("test.xml", settings))
            {
                // while there are still applications in the XML
                while (reader.Read())
                {
                    // If the parent node is an application node
                    if (reader.Name.ToLower() == "application")
                    {
                        Console.WriteLine("Looking at " + reader["Name"]);

                        // create a second XmlReader stream for all child elements of the parent node
                        XmlReader inner = reader.ReadSubtree();

                        // while there are child elements
                        while (inner.Read())
                        {
                            if (inner.IsStartElement())
                                switch (inner.Name.ToLower())
                                {
                                    case "enabled":
                                        //Boolean enabled = inner.ReadElementContentAsBoolean();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "localpaths":
                                        // string paths = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "includesubfolders":
                                        //Boolean includeSubs = inner.ReadElementContentAsBoolean();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "filetypestoscan":
                                        //string filesTypesToScan = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "excludefilenames":
                                        //string excludeFiles = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "regkey":
                                        //string regKey = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "regkeyvalue":
                                        //string regValue = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "adminnotifydl":
                                        //string emailDL = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    case "adminnotifyindividualusers":
                                        //string individualEmails = inner.ReadElementContentAsString().ToLower();
                                        Console.WriteLine(inner.Value);
                                        break;
                                    default:
                                        break;
                                }
                        }
                    }

                    Console.WriteLine("\n\n\n");
                }
            }
        }
    }
}
