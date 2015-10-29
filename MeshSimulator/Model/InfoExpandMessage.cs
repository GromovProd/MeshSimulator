using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class InfoExpandMessage : SyncMessage
    {
        private int aboutId;

        public int AboutId
        {
            get { return aboutId; }
            set { aboutId = value; }
        }

        private int hoc = 1;

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

        public InfoExpandMessage(bool isNoise, int fromId, int toId, TimeSpan localTime, TimeSpan firstHocTime, int aboutId, int hocs)
            : base(isNoise, fromId, toId, localTime)
        {
            FirstHocTime = firstHocTime;
            AboutId = aboutId;
            Hoc = hocs;

            Type = MessageType.InfoExpand;
        }

    }
}
