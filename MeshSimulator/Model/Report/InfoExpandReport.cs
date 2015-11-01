using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.Report
{
    public class InfoExpandReport : Report
    {
        private List<StationData> data = new List<StationData>();

        public List<StationData> Data
        {
            get { return data; }
            set { data = value; }
        }

        private int stationId;

        public int StationId
        {
            get { return stationId; }
            set { stationId = value; }
        }
    }
}
