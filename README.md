# XMLParser
XML Parsing application to compare data of registry value to files in a folder


LANDesk Endpoint Security Application Control can white list applications(executables) that are allowed to run.

This appliation is XML driven. It reads an XML file located in the same folder as the executable and scans the folder provided in the XML for specific file types. Each file is then scanned against the White List registry key and any executables that are not white listed are being sent out in an e-mail to provided list of e-mails. There are options to either scan or not scan subfolders and exclude specific files from scanning. 

This is v1 so it will most likely be updated.

- **10/18/2017** - Renamed some of the classes, to they better reflect what the class is doing.
			 Updated method names, to comply with C# naming conventions.
			 Added more error checking to the E-mail class and the XMLUtility class.
			 Better structured the project.
