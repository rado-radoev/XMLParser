using System.Collections.Generic;

namespace XMLParsing
{
    interface IUtilityInterface
    {
        List<string> CompareFilesAndRegistry(EnumerateFiles file, RegistryReader registry);
        string GetXMLLocation();
        string FormatEmailBody(List<string> differences, string applicatioName, string regValue = "ProcessWhiteList");
    }
}
