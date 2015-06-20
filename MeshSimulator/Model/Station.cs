using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class Station
    {
        #region Values
        private int id;

        public int Id
        {
            get { return id; }
        }

        private Coordinate coordinate;

        public Coordinate Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }

        private double connectionRadius;

        public double ConnectionRadius
        {
            get { return connectionRadius; }
            private set { connectionRadius = value; }
        }

        private double speed;

        public double Speed
        {
            get { return speed; }
            private set { speed = value; }
        }

        private double timeDeviation;

        public double TimeDeviation
        {
            get { return timeDeviation; }
            private set { timeDeviation = value; }
        }

        private DateTime localTime;

        public DateTime LocalTime
        {
            get { return localTime; }
            private set { localTime = value; }
        }

        private int currentSuperCycle;

        public int CurrentSuperCycle
        {
            get { return currentSuperCycle; }
            private set { currentSuperCycle = value; }
        }

        private int currentCycle;

        public int CurrentCycle
        {
            get { return currentCycle; }
            private set { currentCycle = value; }
        }

        private int currentFrame;

        public int CurrentFrame
        {
            get { return currentFrame; }
            private set { currentFrame = value; }
        }

        private bool isWantTransmit;

        public bool IsWantTransmit
        {
            get { return isWantTransmit; }
            set { isWantTransmit = value; }
        }

        private TimeSpan startTransmitTime;

        public TimeSpan StartTransmitTime
        {
            get { return startTransmitTime; }
            set { startTransmitTime = value; }
        }

        private TimeSpan packetTransmitTime;

        public TimeSpan PacketTransmitTime
        {
            get { return packetTransmitTime; }
            set { packetTransmitTime = value; }
        }

        private bool isTransmit;

        public bool IsTransmit
        {
            get { return isTransmit; }
            set { isTransmit = value; }
        }

        private bool isWantRecieve;

        public bool IsWantRecieve
        {
            get { return isWantRecieve; }
            set { isWantRecieve = value; }
        }

        private TimeSpan startRecieveTime;

        public TimeSpan StartRecieveTime
        {
            get { return startRecieveTime; }
            set { startRecieveTime = value; }
        }

        private TimeSpan packetRecieveTime;

        public TimeSpan PacketRecieveTime
        {
            get { return packetRecieveTime; }
            set { packetRecieveTime = value; }
        }

        private bool isRecieve;

        public bool IsRecieve
        {
            get { return isRecieve; }
            set { isRecieve = value; }
        }

        private TimeSpan awakeTime;

        public TimeSpan AwakeTime
        {
            get { return awakeTime; }
            set
            {
                awakeTime = value + TimeSpan.FromMilliseconds(timeDeviation * value.TotalMilliseconds);

            }
        }

        private TimeSpan awakeAbsoluteTime;

        public TimeSpan AwakeAbsoluteTime
        {
            get { return awakeTime; }
            set { awakeTime = value; }
        }

        #endregion

        #region Methods

        public void Update();

        public void Recieve(ChannelState channelState, Message message = null);

        public Message Transmit();

        public void ChangeAwakeTime(TimeSpan newAwakeTime)
        {
            AwakeTime = newAwakeTime;
        }

        #endregion
    }
}
