using CommandLine;
using System;

namespace ContourChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
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