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

        private int firstHoc;

        public int FirstHoc
        {
            get { return firstHoc; }
            set { firstHoc = value; }
        }

        private double averageHoc;

        public double AverageHoc
        {
            get { return averageHoc; }
            set { averageHoc = value; }
        }

        private int timesRecieve = 1;

        public int TimesRecieve
        {
            get { return timesRecieve; }
            set { timesRecieve = value; }
        }

        private TimeSpan firstHocTime;

        public TimeSpan FirstHocTime
        {
            get { return firstHocTime; }
            set { firstHocTime = value; }
        }

        private TimeSpan avarageUpdateTime;

        public TimeSpan AvarageUpdateTime
        {
            get { return avarageUpdateTime; }
            set { avarageUpdateTime = value; }
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
            FirstHoc = hoc;
            FirstHocTime = firstHocTime;
            CreationTime = creationTime;
            AvarageUpdateTime = CreationTime;
            AverageHoc = hoc;
        }

        public void Update(TimeSpan currentTime, int currentHoc)
        {
            TimesRecieve++;
            AvarageUpdateTime = TimeSpan.FromMilliseconds((AvarageUpdateTime.TotalMilliseconds + currentTime.TotalMilliseconds) / 2);
            AverageHoc = (AverageHoc + currentHoc) / 2;
        }
    }
}
