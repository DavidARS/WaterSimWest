using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORiverModel;
// EDIT QUAY 9/10/20
// REMOVED REFERENCE TO WATERSIM NAMESPACE
// ADDED WaterSim Class Objects
//using WaterSimDCDC.Generic;
// END EDIT 9/10/20

namespace ColoradoRiverReservoirSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //WaterSimDCDC.Generic.Model COS;
            COriverModel COS;
            //COS = new WaterSimDCDC.Generic.Model(DataDirectoryName, TempDirectoryName);
            COS = new COriverModel(DataDirectoryName, TempDirectoryName);
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
