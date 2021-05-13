using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Collections;


namespace CustomTools.XMLValidation
{
    /// <summary>
    /// Validates a collection of XML against a Schema. I'm positive this has been done before (and better) but I couldn't find anything
    /// that wasn't a web-based nightmare that didn't give any diagnostic information.
    /// </summary>
    class XMLValidation
    {
        public static int numErrors = 0;
        public static int numBadFiles = 0;
        public static ArrayList badFileNames = new ArrayList();
        public static string currentFile = "";
        public static string errorFile = "";
        public static StreamWriter fileWriter; 

        /// <summary>
        /// Provides main control flow and UI for the XML validator
        /// </summary>
        /// <returns>Nothing</returns>
        /// <creates>
        ///  A formatted diagnostic file with a list of files that violated the schema, along with detailed error messages including line #s
        /// </creates>
        static int Main()
        {
            Console.Write("Xml Folder Location (Complete directory): ");
            string xmlFolderLocation = Console.ReadLine();
            Console.Write("Schema Location (including extension): ");
            string schemaLocation = Console.ReadLine();
            Console.WriteLine("Enter Report File Name(Not including extension)");
            Console.Write("or press enter to use 'ValidationReport': ");
            string reportFile = Console.ReadLine();

            if (reportFile == "") reportFile = "ValidationReport.txt";
            else reportFile += ".txt";
            fileWriter = new StreamWriter(reportFile);
            
            XMLValidation xmlValidator = new XMLValidation();            
            DirectoryInfo d = new DirectoryInfo(xmlFolderLocation);
            try
            {
                int numFilesProcessed = 1;
                int numFiles = d.GetFiles("*.xml").Length;
                Console.WriteLine("Processing...");
                using (var progress = new ProgressBar())
                {
                    foreach (var file in d.GetFiles("*.xml"))
                    {
                        //if (numFilesProcessed % (numFiles / 10.0) == 0) //10% progress marker
                        //{
                        //    Console.Write(" -+- ");
                        //}
                        numFilesProcessed++;
                        progress.Report((double)numFilesProcessed / numFiles);
                        currentFile = file.Name;
                        xmlValidator.ValidateXML(file.FullName, schemaLocation);
                    }
                }
                Console.WriteLine("Done.");
                
                
                fileWriter.WriteLine(numErrors + "\n\n===+++ Validation Issues Found in " + numBadFiles + ".xml files :   +++===");
                Console.WriteLine(numErrors + "\n\n===+++ Validation Issues Found in " + numBadFiles + ".xml files :   +++===");
                for (int i = 0; i < badFileNames.Count; i++){ //Writes a list of affected files, max 3 per row.
                    fileWriter.Write(badFileNames[i]);
                    if (i < badFileNames.Count) fileWriter.Write(",    ");
                    if (i % 3 == 2) fileWriter.Write("\n");
                }
                Console.WriteLine("\nAnalysis Complete. For more information, please refer to validation report. Press enter to close.");
                fileWriter.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured(Did you enter the correct file locations?):\n " + e + "\n\nPress Enter to close");
                Console.ReadLine();
            }            
            return 0;
        }


        /// <summary>
        /// Validates individual xml files
        /// </summary>
        /// <param name="xmlLocation">The location of the xml file we are testing</param>
        /// <param name="schemaLocation">The schema file to test against</param>
        public void ValidateXML(string xmlLocation, string schemaLocation)
        {
            XmlDocument x = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CloseInput = true;
            settings.ValidationEventHandler += Handler;

            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, schemaLocation);//schema URI is location of the schema
            settings.ValidationFlags =
                XmlSchemaValidationFlags.ReportValidationWarnings |
                XmlSchemaValidationFlags.ProcessIdentityConstraints |
                XmlSchemaValidationFlags.ProcessInlineSchema |
                XmlSchemaValidationFlags.ProcessSchemaLocation;

            using (var r = new StreamReader(xmlLocation))
            using (XmlReader validatingReader = XmlReader.Create(r, settings))
            {
                while (validatingReader.Read()) { /* just loop through document */ }
            }
        }

        /// <summary>
        /// Handler takes all errors and writes them to report, in addition to keeping a list
        /// of offending files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Error reported by Validator Library</param>
        private static void Handler(object sender, ValidationEventArgs e)
        {
            numErrors ++;
            if (errorFile != currentFile)
            {
                errorFile = currentFile;
                badFileNames.Add(errorFile);
                numBadFiles++;
                fileWriter.WriteLine("\n==++In File:  " + currentFile + "    ++==");
            }
            if (e.Severity == XmlSeverityType.Error || e.Severity == XmlSeverityType.Warning)
                fileWriter.WriteLine(String.Format("\tLine: {0}, Position: {1} \"{2}\"",
                        e.Exception.LineNumber, e.Exception.LinePosition,
              e.Exception.Message));             
        }
    }
}
