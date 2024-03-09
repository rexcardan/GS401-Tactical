using ContourChecker.API.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker
{
    public class StructureChecklist
    {
        public List<StructureCheck> AllChecks { get; set; } = new List<StructureCheck>();
        public List<CheckResult> EvaluateSet(StructureSet ss, ProgressContext ctx)
        {
            var results = new List<CheckResult>();
            foreach (var s in ss.Structures)
            {
                var strTask = ctx.AddTask($"[green]Analyzing {s.Id}...[/]");
                var checkCount = AllChecks.Count(c => c.IsEnabled);

                for (int i = 0; i < AllChecks.Count; i++)
                {
                    var c = AllChecks[i];
                    strTask.Increment(i*100.0/checkCount);
                    if (c.IsEnabled)
                    {
                        results.Add(c.IsPassing(s, ss));
                    }
                }
            }
            return results;
        }
    }
}
