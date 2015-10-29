using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class SyncMessage : IMessage
    {
        private bool isNoise = true;
        public bool IsNoise
        {
            get { return isNoise; }
            set { isNoise = value; }
        }

        private int fromId;
        public int FromId
        {
            get { return fromId; }
            set { fromId = value; }
        }

        private int toId;
        public int ToId
        {
            get { return toId; }
            set { toId = value; }
        }

        private TimeSpan localTime;

        public TimeSpan LocalTime
        {
            get { return localTime; }
            set { localTime = value; }
        }

        private MessageType type;

        public MessageType Type
        {
            get { return type; }
            set { type = value; }
        }

        public SyncMessage(bool isNoise, int fromId, int toId, TimeSpan localTime)
        {
            IsNoise = isNoise;
            FromId = fromId;
            ToId = toId;
            LocalTime = localTime;

            Type = MessageType.Sync;
        }
    }
}
