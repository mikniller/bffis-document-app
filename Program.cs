using System;
using System.IO;

namespace bffisApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // string doc = @"C:\Users\Michael\Dropbox\BFFIS\2017\HoldOversigt-ver1.xls";
            string doc = "..\\HoldOversigt-ver1.xls";
            if (args.Length != 2)
            {
                Console.WriteLine("Usage is bffisApp.exe [fuld sti til excel fil] - e.g. bffisApp.exe c:\\dropbox\\bffis\\HoldOversigt-ver1.xls");
                Console.WriteLine("Defaulter til : " + doc);
            }
            else
            {
                doc = args[1];
            }
            var path = Path.GetDirectoryName(doc);
            var file = Path.GetFileName(doc);
            // split into path and filename
            Directory.SetCurrentDirectory(path);



            // figure out where the xls is.





            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            DocumentReader reader = new DocumentReader();
            var teams = reader.Read(file);
            foreach(var t in teams) {
                Console.Write(t.ToString());
            }

            string year = DateTime.Now.Year + " - " + DateTime.Now.AddYears(1).Year;

            DocumentBuilder builder = new DocumentBuilder();
            builder.Build(teams, "program.pdf", year);
            builder.Build(teams, "holdoversigt.pdf", year,true);

        }
    }
}
