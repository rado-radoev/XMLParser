using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;
using System.Diagnostics;

namespace XMLParsing
{
    class XMLUtility : IUtilityInterface
    {
        public XMLUtility() { }

        // this a method that compares the output from the registry and
        // the reg file scan and if there are any differences 
        // it will raise the flag that an e-mail needs to be sent out
        public List<string> CompareFilesAndRegistry(EnumerateFiles f, RegistryReader r)
        {
            List<string> regval = new List<string>(r.GetRegKeys());
            List<string> fileval = new List<string>(f.GetFileNames());
            List<string> differences = null;

            if (regval.Count > 0 && fileval.Count > 0)
            {
                differences = fileval.Except(regval).ToList();
                differences.Sort();
                return differences;
            }

            return differences = new List<string>();
        }


        /// <summary>
        /// Find the xml file located in curent directory
        /// </summary>
        /// <returns>String value of the full path to the XML file</returns>
        public string GetXMLLocation()
        {
            string currentDir = Directory.GetCurrentDirectory();
            // return only the 1st XML file. If mulitiple files are found, its not my fault
            string[] files = Directory.GetFiles(currentDir, "*.xml");
            string xmlFile = string.Empty;

            // check if any XML files are found
            if (files.Length > 0)
            {
                // return the 1st xml found
                xmlFile = files[0];
            }
            else
            {
                // if no xml files are found create a new EventLog for the current application
                // and quit application
                if (!EventLog.SourceExists(AppDomain.CurrentDomain.FriendlyName))
                {
                    EventLog.CreateEventSource(AppDomain.CurrentDomain.FriendlyName, "Application");
                }
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, "No XMl file present in " + currentDir, EventLogEntryType.Error, 666);
                Environment.Exit(666);
            }

            return xmlFile;
        }

        public string FormatEmailBody(List<string> differences, string applicatioName, string regValue = "ProcessWhiteList")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("Application: {0}\n", applicatioName));
            sb.Append(string.Format("Following files found to not exist in the registry value: {0}\n\n", regValue));

            differences.ForEach(s => sb.Append(string.Format("{0}, ", s)));


            return sb.ToString().TrimEnd(' ').TrimEnd(',');
        }
    }

}
