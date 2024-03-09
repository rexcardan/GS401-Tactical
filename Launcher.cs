using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using VMS.CA.Scripting;

namespace VMS.IRS.Scripting
{

    public class Script
    {

        public Script()
        {
        }

        public void Execute(ScriptContext context /*, System.Windows.Window window*/)
        {
            try
            {
                if (context == null)
                {
                    MessageBox.Show("Context is null");
                    return;
                }

                if (context.Patient == null)
                {
                    MessageBox.Show("Patient is null");
                    return;
                }

                if (context.Structure == null)
                {
                    MessageBox.Show("Please highlight/select a structure before running this script");
                    return;
                }


                var appPath = @"C:\Code\Projects\ContourChecker\bin\Debug\ContourChecker.exe";
                var args = new Dictionary<string, string>();

                args.Add("-p", context.Patient.Id);
                args.Add("-s", context.Structure.StructureSet.UID);

                var argsString = string.Join(" ", args.Select(x => x.Key + " " + x.Value));

                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = appPath;
                process.StartInfo.Arguments = argsString;
                process.Start();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

    }

}