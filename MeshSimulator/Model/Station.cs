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

        public TimeSpan TimeFromFrameStart
        {
            get
            {
                return TimeSpan.FromMilliseconds((LocalTime.TotalMilliseconds / FrameTime.TotalMilliseconds - (int)LocalTime.TotalMilliseconds / FrameTime.TotalMilliseconds) * FrameTime.TotalMilliseconds);
            }
        }

        public int CurrentSuperCycle
        {
            get { return (int)Math.Truncate(LocalTime.TotalMilliseconds / SuperCycleTime.TotalMilliseconds); }
        }

        private TimeSpan superCycleTime;

        public TimeSpan SuperCycleTime
        {
            get { return superCycleTime; }
            set { superCycleTime = value; }
        }

        private int cyclesInSuperCycle;

        public int CyclesInSuperCycle
        {
            get { return cyclesInSuperCycle; }
            set { cyclesInSuperCycle = value; }
        }

        private int rxCycle;

        public int RxCycle
        {
            get { return rxCycle; }
            private set { rxCycle = value; }
        }

        public int CurrentCycle
        {
            get
            {
                return (int)Math.Truncate((LocalTime.TotalMilliseconds - CurrentSuperCycle * SuperCycleTime.TotalMilliseconds) / CycleTime.TotalMilliseconds);
            }
        }

        private TimeSpan cycleTime;

        public TimeSpan CycleTime
        {
            get { return cycleTime; }
            set { cycleTime = value; }
        }

        private int framesInCycle;

        public int FramesInCycle
        {
            get { return framesInCycle; }
            set { framesInCycle = value; }
        }

        private int currentFrame;

        public int CurrentFrame
        {
            get
            {
                return (int)Math.Truncate((LocalTime.TotalMilliseconds - CurrentSuperCycle * SuperCycleTime.TotalMilliseconds - CurrentCycle * CycleTime.TotalMilliseconds) / FrameTime.TotalMilliseconds);
            }
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

        private Random rand;

        #endregion

        public Station(int id, double connectionRadius, Coordinate coord, int currentSuperCycle, int currentCycle, int currentFrame,
            TimeSpan superCycleTime, TimeSpan cycleTime, TimeSpan frameTime, TimeSpan guardTimeInterval, TimeSpan localTime,
            TimeSpan packetRecieveTime, TimeSpan packetTransmitTime, double speed, double speedAngle, double deviation, Random rand)
        {
            Id = id;
            this.AwakeTime = TimeSpan.FromMilliseconds(0);
            this.ConnectionRadius = connectionRadius;
            this.Coordinate = coord;
            //this.CurrentSuperCycle = CurrentSuperCycle;
            this.RxCycle = currentCycle;
            //this.CurrentFrame = CurrentFrame;
            this.SuperCycleTime = superCycleTime;
            this.CycleTime = cycleTime;
            this.FrameTime = FrameTime;
            this.GuardTimeInterval = guardTimeInterval;
            this.IsRecieve = false;
            this.IsTransmit = false;
            this.IsWantRecieve = true;
            this.IsWantTransmit = false;
            this.LocalTime = localTime;
            this.PacketRecieveTime = packetRecieveTime;
            this.PacketTransmitTime = packetTransmitTime;
            this.Speed = speed;
            this.SpeedAngle = SpeedAngle;
            this.TimeDeviation = deviation;
            this.rand = rand;

            RxCycle = rand.Next(CyclesInSuperCycle);
        }


        #region Methods

        public void Update()
        {
            if (IsWantRecieve)
            {
                IsRecieve = true;
            }

            if (IsWantTransmit)
            {
                IsTransmit = true;
            }

            if (IsRecieve)
            {


            }

            if (IsTransmit && CurrentSuperCycle != 0)
            {



            }




            //CurrentCycle = (int)Math.Truncate(LocalTime.TotalMilliseconds / CycleTime.TotalMilliseconds);

        }

        private TimeSpan GetRxAwakeTime()
        {
            double time = 0;

            if (RxCycle < CurrentCycle)
            {
                time = (CyclesInSuperCycle - 1 - CurrentCycle + RxCycle) * CycleTime.TotalMilliseconds + (FramesInCycle - CurrentFrame) * FrameTime.TotalMilliseconds - TimeFromFrameStart.TotalMilliseconds;
            }

            if (RxCycle == CurrentCycle)
            {
                time = (CurrentFrame + 1) * FrameTime.TotalMilliseconds - TimeFromFrameStart.TotalMilliseconds;
            }

            if (RxCycle > CurrentCycle)
            {
                time = (RxCycle - CurrentCycle - 1) * CycleTime.TotalMilliseconds + (FramesInCycle - CurrentFrame) * FrameTime.TotalMilliseconds - TimeFromFrameStart.TotalMilliseconds;
            }

            return TimeSpan.FromMilliseconds(time);
        }

        private TimeSpan GetTxAwakeTime()
        {
            double time = 0;

            if (Id < CurrentFrame)
            {
                //next cycle
            }

            //if (Id == CurrentFrame)
            //{
            //    IsTransmit = true;
            //}

            if (Id > CurrentFrame)
            {
                time = (Id - CurrentFrame - 1) * FrameTime.TotalMilliseconds - TimeFromFrameStart.TotalMilliseconds;
            }

            return TimeSpan.FromMilliseconds(time);
        }

        public void Recieve(ChannelState channelState, Message message = null)
        {
            IsRecieve = false;
        }

        public Message Transmit(bool isNoise) { return null; }

        public void ChangeAwakeTime(TimeSpan newAwakeTime)
        {
            AwakeTime = newAwakeTime;
        }

        #endregion
    }
}
