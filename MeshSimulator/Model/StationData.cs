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

        private double totalHoc = 0;

        public double TotalHoc
        {
            get { return totalHoc; }
            set { totalHoc = value; }
        }

        private double averageHoc;

        public double AverageHoc
        {
            get { return TotalHoc / TimesRecieve; }
        }

        private int timesRecieve = 0;

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

        private TimeSpan lastUpdateTime = TimeSpan.Zero;

        public TimeSpan LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }

        private TimeSpan updateTime;

        public TimeSpan UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }


        private TimeSpan avarageUpdateTime;

        public TimeSpan AvarageUpdateTime
        {
            get { return TimeSpan.FromMilliseconds(UpdateTime.TotalMilliseconds / TimesRecieve); }
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
        }

        public void Update(TimeSpan currentTime, int currentHoc)
        {
            TimesRecieve++;
            UpdateTime = TimeSpan.FromMilliseconds((UpdateTime.TotalMilliseconds + currentTime.TotalMilliseconds - LastUpdateTime.TotalMilliseconds));
            LastUpdateTime = currentTime;
            TotalHoc += currentHoc;
        }
    }
}
