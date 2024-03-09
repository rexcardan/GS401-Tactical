using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;

namespace ContourChecker.API.Models
{
    public abstract class StructureCheck
    {
        public bool IsEnabled { get; set; } = true;
        public abstract string Name { get; }
        public abstract CheckResult IsPassing(Structure s, StructureSet ss);
    }
}
