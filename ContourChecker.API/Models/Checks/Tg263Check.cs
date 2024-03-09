using ContourChecker.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker.API.Models.Checks
{
    public class Tg263Check : StructureCheck
    {
        public override string Name => "TG 263 Check";

        public override CheckResult IsPassing(Structure s, StructureSet ss)
        {
            if(s == null)
            {
                return new CheckResult
                {
                    IsPassed = false,
                    Message = "Structure is null",
                    CheckName = this.Name
                };
            }

            var isValid = Tg263Helper.IsStructureIdValid(s.Id);
            return new CheckResult
            {
                IsPassed = isValid,
                Message = isValid? "Passed": "Not a valid TG 263 name",
                CheckName = this.Name
            };
        }
    }
}
