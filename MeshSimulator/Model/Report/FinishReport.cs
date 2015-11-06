using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.Report
{
    public class FinishReport : Report
    {
        private Dictionary<int, List<StationData>> stationsData = new Dictionary<int, List<StationData>>();

        public Dictionary<int, List<StationData>> StationsData
        {
            get { return stationsData; }
            set { stationsData = value; }
        }
    }
}
