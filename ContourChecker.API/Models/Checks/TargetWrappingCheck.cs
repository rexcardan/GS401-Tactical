using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker.API.Models.Checks
{
    public class TargetWrappingCheck : StructureCheck
    {
        public override string Name => "Targets wrapped";

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
