using System.Collections.Generic;

namespace XMLParsing
{
    interface IUtilityInterface
    {
        List<string> compareFilesAndRegistry(File file, MyRegistry registry);
        string getXMLLocation();
        string formatEmailBody(List<string> differences, string applicatioName, string regValue = "ProcessWhiteList");
    }
}
