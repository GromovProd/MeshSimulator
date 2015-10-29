using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class StationData
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private int hoc;

        public int Hoc
        {
            get { return hoc; }
            set { hoc = value; }
        }

        private TimeSpan firstHocTime;

        public TimeSpan FirstHocTime
        {
            get { return firstHocTime; }
            set { firstHocTime = value; }
        }

        private TimeSpan creationTime;

        public TimeSpan CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        public StationData(int id, int hoc, TimeSpan firstHocTime, TimeSpan creationTime)
        {
            Id = id;
            Hoc = hoc;
            FirstHocTime = firstHocTime;
            CreationTime = creationTime;
        }
    }
}
