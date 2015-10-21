using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class ModelVariables
    {
        public static ModelVariables Default = new ModelVariables();

        public int MaxSpeed = 10;
        public int CountOfStations = 100;
        public int CountOfReports = 10000;
        public TimeSpan EndTime = new TimeSpan(14,0,0,0);
    }
}
