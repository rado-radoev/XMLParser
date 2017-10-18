using System;
using Microsoft.Win32;
using System.Security;
using System.IO;
using System.Collections.Generic;


namespace XMLParsing
{
    class RegistryReader
    {
        private string key; // the full path to the kye
        private string keyValue; // the key name

        public string Key { get => key; set => key = value; }
        public string KeyValue { get => keyValue; set => keyValue = value; }

        public RegistryReader() { }

        /// <summary>
        /// Constructor that accepts registry key and value and concatenates them to full key name
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyValue"></param>
        public RegistryReader(string key, string keyValue)
        {
            Key = key;
            KeyValue = keyValue;
        }

        /// <summary>
        /// Helper method to read registry key and return its data
        /// </summary>
        /// <param name="regKey">MyRegistry key path</param>
        /// <param name="regValue">MyRegistry value</param>
        /// <returns>String array of registry value</returns>
        private List<string> ReadRegKey(string regKey, string regValue)
        {
            // Instantiate a registry key object that will either point to HKLM or HKCU hive
            RegistryKey localmachine;

            if (regKey.StartsWith("HKEY_CURRENT_USER") || regKey.StartsWith("HKCU"))
            {
                localmachine = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            }
            else
            {
                localmachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            }

            // read the key, discarding anything before SOFTWARE.
            regKey = regKey.Substring(regKey.IndexOf("software"));
            
            using (RegistryKey rootKey = localmachine.OpenSubKey(regKey))
            {
                string[] keyData = null;
                try
                {
                    // Read the data of the registry key value
                    keyData = (string[])rootKey.GetValue(regValue);
                }
                catch (SecurityException)
                {
                    Console.WriteLine("The user does not have the permissions required to read from the registry key.");
                }
                catch (IOException)
                {
                    Console.WriteLine("The RegistryKey that contains the specified value has been marked for deletion.");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("keyName does not begin with a valid registry root.");
                }

                // Convert the data collected to a list and sort it
                List<string> output = new List<string>(keyData);
                output.Sort();
                return output;
            }
        }

        /// <summary>
        /// Read registry key and return its data
        /// </summary>
        /// <returns>Lisgt of strings of registry value</returns>
        public List<string> GetRegKeys()
        {
            return ReadRegKey(Key, KeyValue);
        }
    }
}
