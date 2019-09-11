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
            //Create a string array of files in specific folder
            string[] fileEntries = Directory.GetFiles(@"C:\Users\ml\Desktop\PDF");

            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName);
            }

            Console.ReadLine();
        }

        public static void ProcessFile(string path)
        {
            string text = ExtractTextsFromAllPages(path);

            int pFrom = text.IndexOf("stk. 3-5).") + "stk. 3-5).".Length;
            int pTo = text.LastIndexOf("BILAG");

            string result = text.Substring(pFrom, pTo - pFrom).Trim()
                .Replace("§ 12.  Underskrift", "")
                .Replace("Dato: ", "")
                .Replace("Som udlejer", "")
                .Replace("Som lejer", "");

            Console.WriteLine(result);       
        }

        public static string ExtractTextsFromAllPages(string path)
        {
            var sb = new StringBuilder();
            using (var doc = new Doc())
            {
                doc.Read(path);
                for (var currentPageNumber = 1; currentPageNumber <= doc.PageCount; currentPageNumber++)
                {
                    doc.PageNumber = currentPageNumber;
                    sb.Append(doc.GetText("text").Replace("Side " + currentPageNumber + " af " + doc.PageCount, ""));
                }
            }
            return sb.ToString();
        }
    }
}