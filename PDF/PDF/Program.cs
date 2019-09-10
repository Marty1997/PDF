using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupergoo.ABCpdf11;

namespace PDF
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a string array of files in folder
            string[] fileEntries = Directory.GetFiles(@"C:\Users\ml\Desktop\PDF");

            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);
            }
        }

        public static void ProcessFile(string path)
        {
            int index;
            string fileName = path.Substring(path.LastIndexOf('\\') + 1);
            List<string> pages = ExtractTextsFromAllPages(path);

            //Removing undesired text from page 1
            index = pages[0].IndexOf("3-5).");
            pages[0] = pages[0].Substring(index + 5).Trim();

            //Removing undesired text from page 2
            index = pages[1].IndexOf("Som lejer");
            pages[1] = pages[1].Replace("Som udlejer", "").Substring(index + 9).Trim();
            index = pages[1].IndexOf("   \r\n \r\n");
            if(index != -1)
            {
                pages[1] = pages[1].Substring(index).Trim();
            }

            SavePDF(pages, fileName);
        }

        public static List<string> ExtractTextsFromAllPages(string path)
        {
            List<string> pages = new List<string>();
            using (var doc = new Doc())
            {
                doc.Read(path);
                for (var currentPageNumber = 5; currentPageNumber <= 6; currentPageNumber++)
                {
                    doc.PageNumber = currentPageNumber;
                    pages.Add(doc.GetText("text"));
                }
            }
            return pages;
        }

        public static void SavePDF(List<string> pages, string fileName)
        {
            using (Doc doc = new Doc())
            {
                doc.FontSize = 12;
                doc.AddText(pages[0]);
                doc.Page = doc.AddPage();
                doc.AddText(pages[1]);
                doc.Save(@"C:\Users\ml\Desktop\New\" + fileName);
            }
        }
    }
}
