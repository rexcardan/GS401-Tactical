using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker.API.Models
{
    public class CheckResult
    {
        public bool IsPassed { get; set; }
        public string Message { get; set; }
        public string CheckName { get; set; }
    }
}
