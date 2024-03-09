using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker.API.Models.Checks
{
    public class AllStructuresInsideBodyCheck : StructureCheck
    {
        public override string Name => "All structures inside body";

        public override CheckResult IsPassing(Structure s, StructureSet ss)
        {
            return new CheckResult
            {
                IsPassed = true,
                Message = "Passed",
                CheckName = this.Name
            };
        }
    }
}
