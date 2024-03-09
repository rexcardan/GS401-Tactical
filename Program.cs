using CommandLine;
using System;

namespace ContourChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //For testing purposes
            args = new string[] { "-p", "Prostate003", "-s", "2.25.137385552897349939998230737929399514051" };

            Parser.Default.ParseArguments<EsapiState>(args)
            .WithParsed(state =>
            {
                Console.WriteLine($"PatientId: {state.PatientId}");
                Console.WriteLine($"StructureSetUID: {state.StructureSetUID}");
            });

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}