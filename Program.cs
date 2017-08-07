using System;

namespace bffisApp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            DocumentReader reader = new DocumentReader();
            var teams = reader.Read("HoldOversigt-ver1.xls");
            foreach(var t in teams) {
                Console.Write(t.ToString());
            }

            DocumentBuilder builder = new DocumentBuilder();
            builder.Build(teams,"holdoversigt.pdf","2017 - 2018");

        }
    }
}
