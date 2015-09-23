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

        private StationAction currentState = StationAction.None;

        public StationAction CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        private StationAction nextState = StationAction.None;

        public StationAction NextState
        {
            get { return nextState; }
            set { nextState = value; }
        }

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
            set { localTime = value; }
        }

        public TimeSpan TimeFromSlotStart
        {
            get
            {
                var totalslots = (LocalTime.TotalMilliseconds / SlotTime.TotalMilliseconds);
                var roundedslots = (int)(LocalTime.TotalMilliseconds / SlotTime.TotalMilliseconds);
                return TimeSpan.FromMilliseconds((totalslots - roundedslots) * SlotTime.TotalMilliseconds);
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

        private int slotsInCycle;

        public int SlotsInCycle
        {
            get { return slotsInCycle; }
            set { slotsInCycle = value; }
        }

        private int currentSlot;

        public int CurrentSlot
        {
            get
            {
                return (int)Math.Truncate((LocalTime.TotalMilliseconds - CurrentSuperCycle * SuperCycleTime.TotalMilliseconds - CurrentCycle * CycleTime.TotalMilliseconds) / SlotTime.TotalMilliseconds);
            }
        }

        private TimeSpan slotTime;

        public TimeSpan SlotTime
        {
            get { return slotTime; }
            set { slotTime = value; }
        }

        //private TimeSpan guardTimeInterval;

        //public TimeSpan GuardTimeInterval
        //{
        //    get { return guardTimeInterval; }
        //    set { guardTimeInterval = value; }
        //}

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

        private TimeSpan lastRxUpTime;

        public TimeSpan LastRxUpTime
        {
            get { return lastRxUpTime; }
            set { lastRxUpTime = value; }
        }

        private TimeSpan lastTxUpTime;

        public TimeSpan LastTxUpTime
        {
            get { return lastTxUpTime; }
            set { lastTxUpTime = value; }
        }

        private TimeSpan awakeTime;

        public TimeSpan AwakeTime
        {
            get { return awakeTime; }
            set
            {
                awakeTime = value + TimeSpan.FromMilliseconds(TimeDeviation * value.TotalMilliseconds);
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

        public Station(int id, double connectionRadius, Coordinate coord,
            int cyclesInSuperCycle, int slotsInCycle, TimeSpan slotTime, TimeSpan localTime,
            TimeSpan packetRecieveTime, TimeSpan packetTransmitTime, double speed, double speedAngle, double deviation, Random rand)
        {
            Id = id;
            this.AwakeTime = TimeSpan.FromMilliseconds(0);
            this.ConnectionRadius = connectionRadius;
            this.Coordinate = coord;
            this.SlotTime = slotTime;
            this.SlotsInCycle = slotsInCycle;
            this.CyclesInSuperCycle = cyclesInSuperCycle;
            this.CycleTime = new TimeSpan(0, 0, 0, 0, (int)(slotsInCycle * SlotTime.TotalMilliseconds));
            this.SuperCycleTime = new TimeSpan(0, 0, 0, 0, (int)(cyclesInSuperCycle * CycleTime.TotalMilliseconds));

            this.IsRecieve = false;
            this.IsTransmit = false;
            this.LocalTime = localTime;
            this.PacketRecieveTime = packetRecieveTime;
            this.PacketTransmitTime = packetTransmitTime;
            this.Speed = speed;
            this.SpeedAngle = SpeedAngle;
            this.TimeDeviation = deviation;
            this.rand = rand;

            RxCycle = rand.Next(CyclesInSuperCycle);

            //NextState = GetNextState();
            //UpdateState(nextAction);
        }


        #region Methods

        public void Update()
        {
            UpdateState(NextState);

            NextState = GetNextState();

            LocalTime.TotalMilliseconds.ToString();
            AwakeTime.TotalMilliseconds.ToString();
            CurrentState.ToString();
        }

        private StationAction GetNextState()
        {
            var nextTxUp = GetTxUpAwakeTime();
            var nextTxDown = GetTxDownAwakeTime();
            var nextRxUp = GetRxUpAwakeTime();
            var nextRxDown = GetRxDownAwakeTime();
            var nextRxCycleUpdate = GetUpdateRxCycleAwakeTime();

            var minTime = nextTxUp;
            var state = StationAction.TxUp;

            if ((minTime > nextTxDown || nextTxUp.TotalMilliseconds == 0) && (CurrentState == StationAction.TxUp))
            {
                minTime = nextTxDown;
                state = StationAction.TxDown;
            }

            if (minTime > nextRxUp)
            {
                minTime = nextRxUp;
                state = StationAction.RxUp;
            }

            if ((minTime > nextRxDown || nextRxUp.TotalMilliseconds == 0) && (CurrentState == StationAction.RxUp))
            {
                minTime = nextRxDown;
                state = StationAction.RxDown;
            }

            if (minTime >= nextRxCycleUpdate)
            {
                minTime = nextRxCycleUpdate;
                state = StationAction.RCU;
            }

            AwakeTime = minTime;

            return state;
        }

        private void UpdateState(StationAction state)
        {
            CurrentState = NextState;
            switch (CurrentState)
            {
                case StationAction.RxUp:
                    {
                        LastRxUpTime = LocalTime;
                        IsRecieve = true;
                        break;
                    }
                case StationAction.RxDown:
                    {
                        IsRecieve = false;
                        break;
                    }
                case StationAction.TxUp:
                    {
                        LastTxUpTime = LocalTime;
                        IsTransmit = true;
                        break;
                    }
                case StationAction.TxDown:
                    {
                        IsTransmit = false;
                        break;
                    }
                case StationAction.RCU:
                    {
                        IsTransmit = false;
                        IsRecieve = false;
                        //additional logic

                        RxCycle = rand.Next(CyclesInSuperCycle);
                        break;
                    }
            }
            NextState = state;
        }

        private TimeSpan GetRxUpAwakeTime()
        {
            double time = 0;

            if (RxCycle < CurrentCycle)
            {
                time = (CyclesInSuperCycle - 1 - CurrentCycle + RxCycle) * CycleTime.TotalMilliseconds + (SlotsInCycle - CurrentSlot) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
            }

            if (RxCycle == CurrentCycle)
            {
                //включаем сейчас!
                time = 0;// (CurrentSlot + 1) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
            }

            if (RxCycle > CurrentCycle)
            {
                time = (RxCycle - CurrentCycle - 1) * CycleTime.TotalMilliseconds + (SlotsInCycle - CurrentSlot) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
            }

            return TimeSpan.FromMilliseconds(time);
        }

        private TimeSpan GetRxDownAwakeTime()
        {
            double time = 0;

            if (IsRecieve)
            {
                //Берем последнее время когда начали передавать и к нему прибавляем и остаток длительности прослушки.
                //time = PacketRecieveTime.TotalMilliseconds - (LocalTime.TotalMilliseconds - LastRxUpTime.TotalMilliseconds); 
                //ПРАВИТЬ
                time = (SlotsInCycle - CurrentSlot) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
            }
            else
            {
                //находим ближайшую передачу и прибавляем время цикла
                var rxUpTime = GetRxUpAwakeTime();
                time = rxUpTime.TotalMilliseconds + CycleTime.TotalMilliseconds;
            }

            return TimeSpan.FromMilliseconds(time);
        }

        private TimeSpan GetTxUpAwakeTime()
        {
            double time = 0;

            if (CurrentCycle != RxCycle)
            {
                if (Id < CurrentSlot)
                {
                    if (CurrentCycle + 1 != RxCycle)
                    {
                        time = CycleTime.TotalMilliseconds - (CurrentSlot - Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                    }
                    else
                    {
                        time = 2 * CycleTime.TotalMilliseconds - (CurrentSlot - Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                    }
                }

                if (Id > CurrentSlot)
                {
                    time = (Id - CurrentSlot) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                }
            }
            else
            {
                if (Id < CurrentSlot)
                {

                    time = (SlotsInCycle - CurrentSlot + Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                }
                else
                {
                    time = CycleTime.TotalMilliseconds - (CurrentSlot - Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.Milliseconds;
                }
            }
            return TimeSpan.FromMilliseconds(time);
        }

        private TimeSpan GetTxDownAwakeTime()
        {
            double time = 0;

            if (IsTransmit)
            {
                //Берем последнее время когда начали передавать и к нему прибавляем и остаток длительности прослушки.
                time = PacketTransmitTime.TotalMilliseconds - (LocalTime.TotalMilliseconds - LastTxUpTime.TotalMilliseconds);
            }
            else
            {
                //находим ближайшую передачу и прибавляем время передачи
                var txUpTime = GetTxUpAwakeTime();
                time = txUpTime.TotalMilliseconds + PacketTransmitTime.TotalMilliseconds;
            }

            return TimeSpan.FromMilliseconds(time);
        }

        private TimeSpan GetUpdateRxCycleAwakeTime()
        {
            double time = 0;

            time = SuperCycleTime.TotalMilliseconds - CurrentCycle * CycleTime.TotalMilliseconds - CurrentSlot * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;

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
