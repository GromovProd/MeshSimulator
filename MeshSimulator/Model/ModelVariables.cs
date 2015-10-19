using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class ModelVariables
    {
        private const int defaultMaxSpeed = 10;
        private const int defaultCountOfStations = 10;

        public static ModelVariables Default = new ModelVariables() { CountOfStations = defaultCountOfStations, MaxSpeed = defaultMaxSpeed };

        public int MaxSpeed = 10;
        public int CountOfStations = 10;
    }
}
