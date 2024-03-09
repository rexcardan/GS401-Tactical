using CommandLine;
using NLog;
using System;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace ContourChecker
{
    internal class Program
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main(string[] args)
        {
            LogHelper.SetupLogging();

            //For testing purposes
            args = new string[] { "-p", "Prostate003", "-s", "2.25.137385552897349939998230737929399514051" };

            Parser.Default.ParseArguments<EsapiState>(args)
            .WithParsed(state =>
            {
                using (var app = Application.CreateApplication())
                {
                    _logger.Info($"{app.CurrentUser.Id} operating on {state.PatientId}, structure set {state.StructureSetUID}");

                    var patient = app.OpenPatientById(state.PatientId);
                    if (patient == null)
                    {
                        Console.WriteLine("Patient not found");
                        return;
                    }
                    var ss = patient.StructureSets.FirstOrDefault(s => s.UID == state.StructureSetUID);
                    if (ss == null)
                    {
                        Console.WriteLine("Structure set not found");
                        return;
                    }

                    Console.WriteLine("Patient: {0}, Structure Set: {1}", patient.Name, ss.Id);
                }
            });

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}