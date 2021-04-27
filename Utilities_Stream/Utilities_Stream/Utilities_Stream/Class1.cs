using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities_Stream
{
    public class SimpleWrite
    {
        string path = "C:\\Users\\dasamps1\\Source\\Repos\\WaterSimWest\\WaterSim West API 2020\\WaterSim_West\\WaterSim_West_v_2\bin\\Debug";
        string fid = "LCLU.txt";
        public SimpleWrite()
        { 
            StreamWriter MyStreamWriter = new StreamWriter(path + fid);      
        }
  
        
        public void myClose(StreamWriter sw)
        {
            sw.Flush();
            sw.Close();
        }
    }

}

