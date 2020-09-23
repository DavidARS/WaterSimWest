using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColoradoRiverReservoirSystem;
using WaterSimDCDC;

namespace COtestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WaterSimDCDC.Generic.Model COS;
            COS = new WaterSimDCDC.Generic.Model(DataDirectoryName, TempDirectoryName);
        }

        #region Website directory faking
        private static string DataDirectoryName
        {
            get
            {
                return @"Data";
            }
        }

        private static string TempDirectoryName
        {
            set
            {
                string dir = value;
                string.Concat(@"Temp", dir);
            }
            get
            {
                // Make a common for testing
                return @"Temp";
                // Make the temp directory name unique for each access to avoid client clashes
                //return +System.Guid.NewGuid().ToString() + @"\";
            }
        }
        private static void CreateDirectory(string directoryName)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        #endregion
    }
}

