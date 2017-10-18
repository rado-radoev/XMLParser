using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XMLParsing
{
    /// <summary>
    /// class that reads files in folder (subfolders)
    /// </summary>
    class EnumerateFiles
    {
        string directories;
        string fileExtensions;
        string excludedFiles;
        int searchSubfolders;

        /// <summary>
        /// Overloaded File constructor
        /// </summary>
        /// <param name="directories">The directory(ies) to scan</param>
        /// <param name="fileExtensions">File extension(s) to scan for</param>
        /// <param name="excludeFiles">File(s) to exlcude from scan</param>
        /// <param name="searchSubfolders">Should subfolders be scanned</param>
        public EnumerateFiles(string directories, string fileExtensions, string excludeFiles, int searchSubfolders)
        {
            Directories = directories;
            FileExtensions = fileExtensions;
            ExcludedFiles = excludeFiles;
            SearchSubfolders = searchSubfolders;
        }

        /// <summary>
        /// Default cosnturctor
        /// </summary>
        public EnumerateFiles() { }

        // Getters and setters
        public int SearchSubfolders { get => searchSubfolders; set => searchSubfolders = value; }
        public string Directories { get => directories; set => directories = value; }
        public string FileExtensions { get => fileExtensions; set => fileExtensions = value; }
        public string ExcludedFiles { get => excludedFiles; set => excludedFiles = value; }



        /// <summary>
        /// <para>Scan specific folder for files with specific extensions.</para>
        /// <para>Different extensions should be separated with a comma
        /// and should be in the format of *.exe,*.pdf,*.doc</para>
        /// <para>Method also accepts exclusions so a file can be excluded from scanning. File should include filename and extension</para>
        /// </summary>
        /// <param name="directories">The directoris to search separated by comma</param>
        /// <param name="includeFileExtensions">The file extensions to search for</param>
        /// <param name="excludeFileNames">Files to be excluded</param>
        /// <param name="subfolderSearch">Should search subfolders or not</param>
        /// <returns></returns>
        public List<string> ScanFiles(string directories, string includeFileExtensions, string excludeFileNames = "", int subfolderSearch = 0)
        {
            // check if the correct value is provided for the suboflder search 
            if (subfolderSearch < 0 && subfolderSearch > 1)
                throw new ArgumentException("subFolderSearch must be 0 or 1");

            List<string> includes = ProcessElementValue(includeFileExtensions, "fileExtensions");
            List<string> excludes = ProcessElementValue(excludeFileNames, "fileNames");
            List<string> dirs = ProcessElementValue(directories, "directories");
            List<string> output = new List<string>();

            // go through each folder
            foreach (string dir in dirs)
            {
                // and then through each extension
                foreach (string ext in includes)
                {
                    /*
                     * now the fun begins ...
                     * grab all the files in the folder (and any subfolders)
                     * then check each file if it's file name (end of path the string)
                     * exists in the excludes List
                     * If it does that file is not added to the output list
                     */
                    try
                    {
                        output.AddRange(Directory.GetFiles(dir, ext, (SearchOption)subfolderSearch).Where(file => !excludes.Contains(file.Substring(file.LastIndexOf('\\') + 1))));
                    }
                    catch (ArgumentNullException)
                    {
                        Console.WriteLine("Path or searchpattern is null.");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("The caller does not have the required permission.");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.WriteLine("The specified path is not found or is invalid (for example, it is on an unmapped drive).");
                    }
                    catch (PathTooLongException)
                    {
                        Console.WriteLine("The specified path, file name, or both exceed the system-defined maximum length (248 characters)");
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("Path is a file name, or another error occured");
                    }
                }
            }

            // return a list of strings containing the full path to the files in the specified folders
            output.Sort();
            return output;
        }

        /// <summary>
        /// Normalize xml element value
        /// </summary>
        /// <param name="element">XML element value</param>
        /// <param name="elementType">What type of value is being processed</param>
        /// <returns>List of strings of normalized element values</returns>
        private List<string> ProcessElementValue(string element, string elementType)
        {
            string[] temp = element.Split(',');

            for (int i = 0; i < temp.Length; i++)
            {
                switch (elementType)
                {
                    case "fileExtensions":
                        if (!temp[i].StartsWith("*."))
                            temp[i] = "*." + (temp[i]);
                    break;
                    case "fileNames":
                    case "directories":
                        if (temp[i].StartsWith("\""))
                            temp[i] = temp[i].Substring(1, temp[i].Length - 2).Trim();
                    break;
                    default:
                        break;
                }
            }
            return temp.ToList();
        }

        /// <summary>
        /// Helper method that strips down files names from full path
        /// </summary>
        /// <param name="filesFullPath">List of strings containing full path to file</param>
        /// <returns>List of strings containing only the file name with extension</returns>
        private List<string> StripFileNames(List<string> filesFullPath)
        {
            List<string> output = new List<string>();

            foreach (string file in filesFullPath)
            {
                output.Add(file.Substring(file.LastIndexOf('\\') + 1));
            }

            output.Sort();
            return output;
        } 

        public List<string> GetFileNames()
        {
            return StripFileNames(ScanFiles(Directories, FileExtensions, ExcludedFiles, SearchSubfolders));
        }
    }
}
