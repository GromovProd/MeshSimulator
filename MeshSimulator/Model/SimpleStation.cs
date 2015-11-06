using Log.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model.Station
{
    public class SimpleStation : IStation, INotifyPropertyChanged
    {
        #region Fields, Variables

        private StationAction currentState = StationAction.None;

        public StationAction CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                NotifyPropertyChanged();
            }
        }

        private StationAction nextState = StationAction.None;

        public StationAction NextState
        {
            get { return nextState; }
            set
            {
                nextState = value;
                NotifyPropertyChanged();
            }
        }

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
            set
            {
                coordinate = value;
                NotifyPropertyChanged();
            }
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

        private int maxSpeed;

        public int MaxSpeed
        {
            get { return maxSpeed; }
            private set { maxSpeed = value; }
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
            set { timeDeviation = value; }
        }

        private TimeSpan localTime;

        public TimeSpan LocalTime
        {
            get { return localTime; }
            set
            {
                localTime = value;
                NotifyPropertyChanged();
            }
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

        private TimeSpan guardReceiveTimeInterval;

        public TimeSpan GuardReceiveTimeInterval
        {
            get { return guardReceiveTimeInterval; }
            set { guardReceiveTimeInterval = value; }
        }

        private TimeSpan guardTransmitTimeInterval;

        public TimeSpan GuardTransmitTimeInterval
        {
            get { return guardTransmitTimeInterval; }
            set { guardTransmitTimeInterval = value; }
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
            set { isTransmit = value; NotifyPropertyChanged(); }
        }

        private TimeSpan startReceiveTime;

        public TimeSpan StartReceiveTime
        {
            get { return startReceiveTime; }
            set { startReceiveTime = value; }
        }

        private TimeSpan packetReceiveTime;

        public TimeSpan PacketReceiveTime
        {
            get { return packetReceiveTime; }
            set { packetReceiveTime = value; }
        }

        private bool isReceive;

        public bool IsReceive
        {
            get { return isReceive; }
            set { isReceive = value; NotifyPropertyChanged(); }
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
                awakeTime = value;
                NotifyPropertyChanged();
            }
        }

        private int fHeight = 0;

        public int FHeight
        {
            get { return fHeight; }
            set { fHeight = value; }
        }

        private int fWidth = 0;

        public int FWidth
        {
            get { return fWidth; }
            set { fWidth = value; }
        }

        private List<IStation> stationsToTransmit = new List<IStation>();

        public List<IStation> StationsToTransmit
        {
            get { return stationsToTransmit; }
            set { stationsToTransmit = value; }
        }

        private bool isGotSpecialInfo = false;

        public bool IsGotSpecialInfo
        {
            get { return isGotSpecialInfo; }
            set { isGotSpecialInfo = value; NotifyPropertyChanged(); }
        }

        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private List<StationData> data = new List<StationData>();

        public List<StationData> Data
        {
            get { return data; }
            set { data = value; NotifyPropertyChanged(); }
        }

        private int lastDataTransmited = -1;

        private Random rand = new Random();

        #endregion

        public SimpleStation(int id, double connectionRadius, Coordinate coord,
            int cyclesInSuperCycle, int slotsInCycle, TimeSpan slotTime, TimeSpan localTime,
            TimeSpan packetRecieveTime, TimeSpan packetTransmitTime, double speed, double speedAngle, double deviation, int rnd, int maxspeed, int fHeight, int fWidth)
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

            this.IsReceive = false;
            this.IsTransmit = false;
            this.LocalTime = localTime;
            this.PacketReceiveTime = packetRecieveTime;
            this.PacketTransmitTime = packetTransmitTime;
            this.Speed = speed;
            this.SpeedAngle = SpeedAngle;
            this.TimeDeviation = deviation;
            this.rand = new Random(Id);

            this.GuardReceiveTimeInterval = TimeSpan.FromMilliseconds((SlotTime.TotalMilliseconds - PacketReceiveTime.TotalMilliseconds) / 2);
            this.GuardTransmitTimeInterval = TimeSpan.FromMilliseconds((SlotTime.TotalMilliseconds - PacketTransmitTime.TotalMilliseconds) / 2);

            this.MaxSpeed = maxspeed;

            this.FHeight = fHeight;
            this.FWidth = fWidth;

            SpeedAngle = rand.Next(360);
            Speed = rand.Next(MaxSpeed);
            RxCycle = rand.Next(0, CyclesInSuperCycle);

            Logger.Instance.WriteInfo(Id + " initialized");
        }


        #region Methods

        public void Update()
        {
            UpdateState(NextState);

            NextState = GetNextState();

            //LocalTime.TotalMilliseconds.ToString();
            //AwakeTime.TotalMilliseconds.ToString();
            //CurrentState.ToString();

            Logger.Instance.WriteInfo(Id + " current state: " + CurrentState.ToString());
            Logger.Instance.WriteInfo(Id + " next state: " + NextState.ToString());
            Logger.Instance.WriteInfo(Id + " local time: " + LocalTime.ToString());
            Logger.Instance.WriteInfo(Id + " awake time: " + AwakeTime.ToString());

        }

        int bX = 1;
        int bY = 1;
        bool isChangedX = false;
        bool isChangedY = false;

        public void UpdatePosition(TimeSpan ts)
        {
            if (!isChangedX)
            {
                if (Coordinate.X < 10)
                {
                    Speed = rand.Next(MaxSpeed);
                    bX *= -1;
                    isChangedX = true;
                }
                if (Coordinate.X > FWidth - 10)
                {
                    Speed = rand.Next(MaxSpeed);
                    bX *= -1;
                    isChangedX = true;
                }
            }
            if (!isChangedY)
            {
                if (Coordinate.Y < 10)
                {
                    Speed = rand.Next(MaxSpeed);
                    bY *= -1;
                    isChangedY = true;
                }
                if (Coordinate.Y > FHeight - 10)
                {
                    Speed = rand.Next(MaxSpeed);
                    bY *= -1;
                    isChangedY = true;
                }
            }

            if (Coordinate.X > 10 && Coordinate.X < FWidth - 10)
            {
                isChangedX = false;
            }
            if (Coordinate.Y > 10 && Coordinate.Y < FHeight - 10)
            {
                isChangedY = false;
            }

            var newX = Coordinate.X + bX * Speed * Math.Sin(SpeedAngle * Math.PI / 180) * ts.TotalSeconds;
            var newY = Coordinate.Y + bY * Speed * Math.Cos(SpeedAngle * Math.PI / 180) * ts.TotalSeconds;

            Coordinate.X = newX;
            Coordinate.Y = newY;
        }

        public void AddError(TimeSpan timeToSubstract)
        {
            //Добавляем ошибку кварца устройству
            var error = TimeSpan.FromMilliseconds(TimeDeviation * timeToSubstract.TotalMilliseconds);
            LocalTime = LocalTime.Add(error);
        }

        private StationAction GetNextState()
        {
            var nextTxUp = GetTxUpAwakeTime();
            var nextTxDown = GetTxDownAwakeTime();
            var nextRxUp = GetRxUpAwakeTime();
            var nextRxDown = GetRxDownAwakeTime();
            var nextRxCycleUpdate = GetUpdateRxCycleAwakeTime();

            var minTime = TimeSpan.MaxValue;
            var state = StationAction.None;

            if ((minTime > nextTxUp) && (CurrentState != StationAction.TxUp))
            {
                minTime = nextTxUp;
                state = StationAction.TxUp;
            }

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

            if (minTime >= nextRxCycleUpdate && (CurrentState == StationAction.RxDown || (CurrentState == StationAction.TxDown)))
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
                        IsReceive = true;
                        IsTransmit = false;
                        Logger.Instance.WriteInfo(Id + " RxUp");
                        break;
                    }
                case StationAction.RxDown:
                    {
                        IsReceive = false;
                        IsTransmit = false;
                        Logger.Instance.WriteInfo(Id + " RxDown");
                        break;
                    }
                case StationAction.TxUp:
                    {
                        LastTxUpTime = LocalTime;
                        IsTransmit = true;
                        IsReceive = false;
                        Logger.Instance.WriteInfo(Id + " TxUp");
                        break;
                    }
                case StationAction.TxDown:
                    {
                        IsTransmit = false;
                        IsReceive = false;
                        Logger.Instance.WriteInfo(Id + " TxDown");
                        break;
                    }
                case StationAction.RCU:
                    {
                        IsTransmit = false;
                        IsReceive = false;

                        rand = new Random(Id);

                        RxCycle = rand.Next(0, CyclesInSuperCycle);

                        Logger.Instance.WriteInfo(Id + " RxCycle: " + RxCycle.ToString());
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
                time += GuardReceiveTimeInterval.TotalMilliseconds;
            }

            if (RxCycle == CurrentCycle)
            {
                //включаем сейчас!
                time = 0;
            }

            if (RxCycle > CurrentCycle)
            {
                time = (RxCycle - CurrentCycle - 1) * CycleTime.TotalMilliseconds + (SlotsInCycle - CurrentSlot) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                time += GuardReceiveTimeInterval.TotalMilliseconds;
            }

            return TimeSpan.FromMilliseconds(time);
        }

        private TimeSpan GetRxDownAwakeTime()
        {
            double time = 0;

            if (IsReceive)
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

            time -= GuardReceiveTimeInterval.TotalMilliseconds;

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
                        //костыль
                        //Код точно определяет начало слота
                        //работало нормально когда не было защитного интервала, попробуем сместить на него
                        time += GuardTransmitTimeInterval.TotalMilliseconds;
                    }
                    else
                    {
                        time = 2 * CycleTime.TotalMilliseconds - (CurrentSlot - Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                        //костыль
                        //Код точно определяет начало слота
                        //работало нормально когда не было защитного интервала, попробуем сместить на него
                        time += GuardTransmitTimeInterval.TotalMilliseconds;
                    }
                }

                if (Id > CurrentSlot)
                {
                    time = (Id - CurrentSlot) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                }

                if (Id == CurrentSlot)
                {
                    if (TimeFromSlotStart.TotalMilliseconds <= GuardTransmitTimeInterval.TotalMilliseconds)
                        //все что до отправки - имеет значение, после этого отдаем 0.
                        time += GuardTransmitTimeInterval.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                    else
                    {
                        if (CurrentCycle + 1 != RxCycle)
                        {
                            time = CycleTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds + GuardTransmitTimeInterval.TotalMilliseconds;
                        }
                        else
                        {
                            time = 2 * CycleTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds + GuardTransmitTimeInterval.TotalMilliseconds;
                        }

                    }
                }
            }
            else
            {
                if (Id < CurrentSlot)
                {

                    time = (SlotsInCycle - CurrentSlot + Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.TotalMilliseconds;
                    //костыль
                    //Код точно определяет начало слота
                    //работало нормально когда не было защитного интервала, попробуем сместить на него
                    time += GuardTransmitTimeInterval.TotalMilliseconds;
                }
                else
                {
                    time = CycleTime.TotalMilliseconds - (CurrentSlot - Id) * SlotTime.TotalMilliseconds - TimeFromSlotStart.Milliseconds;
                    //костыль
                    //Код точно определяет начало слота
                    //работало нормально когда не было защитного интервала, попробуем сместить на него
                    time += GuardTransmitTimeInterval.TotalMilliseconds;
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

            //костыль
            //Если мы находимся в этой точке, то по логике нам нужно значение 0, а не супер-цикла.
            if (time == SuperCycleTime.TotalMilliseconds) time = 0;

            return TimeSpan.FromMilliseconds(time);
        }

        public void Recieve(ChannelState channelState, IMessage message = null)
        {
            if (message != null)
            {
                if (!message.IsNoise)
                {
                    Logger.Instance.WriteInfo("Message recieved " + Id);

                    if (message.FromId < Id)
                    {
                        LocalTime = ((SyncMessage)message).LocalTime;
                    }

                    //Получили информацию о передавшем
                    if (!Data.Exists(i => i.Id == message.FromId))
                    {
                        Data.Add(new StationData(message.FromId, 1, LocalTime, LocalTime));
                    }
                    else
                    {
                        var data = Data.Single(i => i.Id == message.FromId);
                        {
                            data.Update(LocalTime, 1);
                        }
                    }

                    //Получили информацию о другом
                    if (message.Type == MessageType.InfoExpand)
                    {
                        var infoMessage = (InfoExpandMessage)message;
                        //Приняли информацию не о нас
                        if (infoMessage.AboutId != Id)
                        {
                            if (!Data.Exists(i => i.Id == infoMessage.AboutId))
                            {
                                Data.Add(new StationData(infoMessage.AboutId, infoMessage.Hoc, infoMessage.FirstHocTime, LocalTime));
                            }
                            else
                            {
                                //ПРОВЕРИТЬ
                                var data = Data.Single(i => i.Id == infoMessage.AboutId);
                                {
                                    data.Update(LocalTime, infoMessage.Hoc);
                                }
                            }
                        }
                    }

                    if (Data.Count == SlotsInCycle - 1)//-1 т.к. мы знаем информацию про себя
                    {
                        IsGotSpecialInfo = true;
                    }

                    NotifyPropertyChanged("Data");

                }
            }
        }

        public IMessage Transmit(bool isNoise, int toId)
        {
            if (Data.Count == 0)
            {
                var message = new InfoExpandMessage(isNoise, Id, toId, LocalTime, LocalTime, Id, 1);

                return message;
            }
            else
            {
                if (lastDataTransmited == Data.Count - 1)
                {
                    lastDataTransmited = 0;
                }
                else
                {
                    lastDataTransmited++;
                }
                var message = new InfoExpandMessage(isNoise, Id, toId, LocalTime, Data[lastDataTransmited].FirstHocTime, Data[lastDataTransmited].Id, Data[lastDataTransmited].FirstHoc + 1);

                return message;
            }

        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
