using CommandLine;
using ContourChecker.API.Models;
using NLog;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
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
            
            var checklist = ChecklistBuilder.BuildChecklist();
            var results = new List<CheckResult>();

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

                    AnsiConsole.Progress()
                         .Start(ctx =>
                         {
                             results = checklist.EvaluateSet(ss, ctx);
                         });
                }
            });

            //Render results
            var table = new Table();
            // Add some columns
            table.AddColumn("Passed").Centered();
            table.AddColumn("Constraint");
            table.AddColumn("Message");

            foreach (var res in results)
            {
                if (res.IsPassed)
                {
                    var renderable = new List<IRenderable>
                        {
                            new Panel("[green]O[/]"),
                            new Markup(res.CheckName),
                            new Markup(res.Message)
                        };

                    table.AddRow(renderable);
                }
                else
                {
                    var renderable = new List<IRenderable>
                       {
                            new Panel("[red]X[/]"),
                            new Markup(res.CheckName),
                            new Markup(res.Message)
                        };

                    table.AddRow(renderable);
                }

            }


            // Render the table to the console
            AnsiConsole.Write(table);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}