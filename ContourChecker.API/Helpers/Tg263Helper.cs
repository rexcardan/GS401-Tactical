using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourChecker.API.Helpers
{
    public class Tg263Helper
    {
        public static bool IsStructureIdValid(string structureId)
        {
            var result = ESAPIX.Constraints.TG263.IsCompliant(structureId, "Control", 0).GetAwaiter().GetResult();
            return result.nameCompliant;
        }
    }
}
