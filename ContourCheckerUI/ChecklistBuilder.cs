using ContourChecker.API.Models;
using ContourChecker.API.Models.Checks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourChecker
{
    public class ChecklistBuilder
    {
        public static StructureChecklist BuildChecklist()
        {
            var checklist = new StructureChecklist();
            checklist.AllChecks.Add(new Tg263Check());
            checklist.AllChecks.Add(new AllStructuresInsideBodyCheck());
            checklist.AllChecks.Add(new TargetWrappingCheck());
            return checklist;
        }
    }
}
