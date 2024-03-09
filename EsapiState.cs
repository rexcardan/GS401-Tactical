using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ContourChecker
{
    public class EsapiState
    {
        [Option('p', "pat", Required = true, HelpText = "Patient Id")]
        public string PatientId { get; set; }

        [Option('s', "ssuid", Required = true, HelpText = "Structure Set UID")]
        public string StructureSetUID { get; set; }
    }
}
