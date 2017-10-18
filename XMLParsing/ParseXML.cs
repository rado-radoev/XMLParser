using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;

namespace XMLParsing
{
    class ParseXML
    {
        private XmlReader reader;
        private XmlReader inner;
        private XMLUtility utility = new XMLUtility();
        public void parseXML()
        {
            
            // Custom XML settings to skip whitespaces and ignore comments. More can be added
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            // Start reading the xml file
            using (reader = XmlReader.Create(utility.GetXMLLocation(), settings))
            {
                // while there are still application nodes in the XML
                while (reader.Read())
                {

                    // Only look at start Applicaiton start nodes
                    if (reader.IsStartElement() && reader.Name.ToLower() == "application")
                    {

                        // TO DO:    THIS CAN BE PUT IN ITS OWN THREAD

                        // Create new EMAIL Object. -> each application updates will be sent in its own email
                        Email email = new Email();

                        // Create new MyRegistry object 
                        RegistryReader reg = new RegistryReader();
                        
                        // Create new File object
                        EnumerateFiles file = new EnumerateFiles();

                        // Creaete a child XmlReader object that contains all child elements for the Application node
                        string currentApp = reader["Name"];
                        Console.WriteLine("Processing: " + currentApp);
                        inner = reader.ReadSubtree();

                        // Quick check if Enabled is the first element. Otherwise throw an error to the fool that misconfigured the XML
                        // Start reading from the first element. If the first element is not Enabled the whole system goes DOWN
                        if (!inner.ReadToFollowing("Enabled")) 
                            throw new ArgumentException("Element \"Enabled\" must be the first element after the Application parent node");

                        // Control variable if the applicaiton should be skipped. default: false
                        Boolean skip = false;

                        // This is where the magic happens
                        do
                        {
                            // Go through each element unitl an end element with the name of Application is reached.
                            // We have read everything we wanted go to the next Application node
                            switch (inner.Name.ToLower())
                            {
                                // Checkign to see if application is enabled. if not. don't bother move to the next one
                                case "enabled":
                                    if (!inner.ReadElementContentAsBoolean())
                                    {
                                        inner.ReadToNextSibling("Application");
                                        skip = true;
                                    }
                                    break;
                                case "localpaths":
                                    file.Directories = inner.ReadElementContentAsString().ToLower();
                                    break;
                                case "includesubfolders":
                                    Boolean includeSubs = inner.ReadElementContentAsBoolean();
                                    file.SearchSubfolders = includeSubs ? 1 : 0;
                                    break;
                                case "filetypestoscan":
                                    file.FileExtensions = inner.ReadElementContentAsString().ToLower();
                                    break;
                                case "excludefilenames":
                                    file.ExcludedFiles = inner.ReadElementContentAsString().ToLower();
                                    break;
                                case "regkey":
                                    reg.Key = inner.ReadElementContentAsString().ToLower(); ;
                                    break;
                                case "regkeyvalue":
                                    reg.KeyValue = inner.ReadElementContentAsString().ToLower();
                                    break;
                                case "adminnotifydl":
                                    email.DistributionLists = inner.ReadElementContentAsString().ToLower();
                                    break;
                                case "adminnotifyindividualusers":
                                    email.IndiVidualUsers = inner.ReadElementContentAsString().ToLower();
                                    break;
                                default:
                                    break;
                            }
                        } while (inner.NodeType != XmlNodeType.EndElement && inner.Name.ToLower() != "application");


                        // Check for differences and send e-mail
                        if (!skip)
                        {
                            List<string> differences = utility.CompareFilesAndRegistry(file, reg);
                            // Send the e-mail
                            if (differences.Count > 0)
                            {
                                email.EmailBody = utility.FormatEmailBody(differences, currentApp);
                                email.EmailSubject = string.Format("{0} new executable update", currentApp);
                                email.SendMail();
                            }
                        }
                        skip = false;
                        Console.WriteLine();
                        inner.Close();
                    }
                }
            }
        }

    }
}

