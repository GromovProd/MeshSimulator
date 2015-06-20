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
            private set { id = value; }
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

        private double speedAngle;

        public double SpeedAngle
        {
            get { return speedAngle; }
            set { speedAngle = value; }
        }

        private double timeDeviation;

        public double TimeDeviation
        {
            get { return timeDeviation; }
            private set { timeDeviation = value; }
        }

        private TimeSpan localTime;

        public TimeSpan LocalTime
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

        private TimeSpan superCycleTime;

        public TimeSpan SuperCycleTime
        {
            get { return superCycleTime; }
            set { superCycleTime = value; }
        }

        private int currentCycle;

        public int CurrentCycle
        {
            get { return currentCycle; }
            private set { currentCycle = value; }
        }

        private TimeSpan cycleTime;

        public TimeSpan CycleTime
        {
            get { return cycleTime; }
            set { cycleTime = value; }
        }

        private int currentFrame;

        public int CurrentFrame
        {
            get { return currentFrame; }
            private set { currentFrame = value; }
        }

        private TimeSpan frameTime;

        public TimeSpan FrameTime
        {
            get { return frameTime; }
            set { frameTime = value; }
        }

        private TimeSpan guardTimeInterval;

        public TimeSpan GuardTimeInterval
        {
            get { return guardTimeInterval; }
            set { guardTimeInterval = value; }
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

        public Station(int id, double connectionRadius, Coordinate coord, int currentSuperCycle, int currentCycle, int currentFrame,
            TimeSpan superCycleTime, TimeSpan cycleTime, TimeSpan frameTime, TimeSpan guardTimeInterval, TimeSpan localTime,
            TimeSpan packetRecieveTime, TimeSpan packetTransmitTime, double speed, double speedAngle, double deviation)
        {
            Id = id;
            this.AwakeTime = TimeSpan.FromMilliseconds(0);
            this.ConnectionRadius = connectionRadius;
            this.Coordinate = coord;
            this.CurrentSuperCycle = CurrentSuperCycle;
            this.CurrentCycle = currentCycle;
            this.CurrentFrame = CurrentFrame;
            this.SuperCycleTime = superCycleTime;
            this.CycleTime = cycleTime;
            this.FrameTime = FrameTime;
            this.GuardTimeInterval = guardTimeInterval;
            this.IsRecieve = false;
            this.IsTransmit = false;
            this.IsWantRecieve = false;
            this.IsWantTransmit = false;
            this.LocalTime = localTime;
            this.PacketRecieveTime = packetRecieveTime;
            this.PacketTransmitTime = packetTransmitTime;
            this.Speed = speed;
            this.SpeedAngle = SpeedAngle;
            this.TimeDeviation = deviation;
        }


        #region Methods

        public void Update()
        {
            IsWantRecieve = true;
            
        }

        public void Recieve(ChannelState channelState, Message message = null);

        public Message Transmit(bool isNoise) { return null; }

        public void ChangeAwakeTime(TimeSpan newAwakeTime)
        {
            AwakeTime = newAwakeTime;
        }

        #endregion
    }
}
