using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class Report
    {
        public int Id;

        public ModelVariables Variables { get; set; }

        public TimeSpan EmulationTime { get; set; }

        public TimeSpan GlobalTime { get; set; }

        public int InfoExpandedStations { get; set; }

        public int MessagesSended { get; set; }

        public int MessagesRecieved { get; set; }

        public int Efficiency { get; set; }


    }
}
