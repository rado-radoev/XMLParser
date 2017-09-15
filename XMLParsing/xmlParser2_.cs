using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLParsing
{
    class xmlParser2_
    {
        public void parseXML()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            using (XmlReader reader = XmlReader.Create("test.xml", settings))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Applications":
                                Console.WriteLine("Start " + reader.Name + " element.");
                                break;
                            case "Application":
                                Console.WriteLine("Start " + reader.Name + " element.");
                                string attribute = reader["Name"];
                                if (attribute != null)
                                {
                                    Console.WriteLine(" Has attribute Name: " + attribute);
                                }
                                if (reader.Read())
                                {
                                    if (reader.HasAttributes)
                                    {
                                        Console.WriteLine("Attribute: " + reader.ReadAttributeValue());
                                        int count = 0;
                                        while (reader.MoveToNextAttribute())
                                        {

                                            Console.WriteLine(reader.GetAttribute(count));
                                            count++;
                                        }
                                    }

                                }
                                break;
                            case "Enabled":
                                Boolean enabled = reader.ReadElementContentAsBoolean();
                                Console.WriteLine(enabled);
                                break;
                            case "LocalPaths":
                                break;
                            case "IncludeSubFolders":
                                break;
                            case "FileTypesToScan":
                                break;
                            case "ExcludeFileNames":
                                break;
                            case "RegKey":
                                break;
                            case "RegKeyValue":
                                break;
                            case "AdminNotifyDL":
                                break;
                            case "AdminNotifyIndividualUsers":
                                break;

                            default:
                                // implement some deafult action
                                break;
                        }
                    }
                    else if (!reader.IsStartElement())
                    {
                        if (reader.Name == "Applications")
                        {
                            Console.WriteLine("End " + reader.Name + " element.");
                        }
                    }
                }
                Console.ReadLine();
            }
        }
    }
}
