using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MeshSimulator.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Log.Support;
using MeshSimulator.Model.Station;

namespace MeshSimulator.Model
{
    public class Environment
    {
        #region Properties

        private List<IStation> stations = new List<IStation>();
        public List<IStation> Stations
        {
            get { return stations; }
            set
            {
                stations = value;
                NotifyPropertyChanged();
            }
        }

        private int transmitredTotal = 0;

        public int TransmitredTotal
        {
            get { return transmitredTotal; }
            set { transmitredTotal = value; NotifyPropertyChanged(); }
        }

        private int recievedTotal = 0;

        public int RecievedTotal
        {
            get { return recievedTotal; }
            set { recievedTotal = value; NotifyPropertyChanged(); }
        }

        private double effisiensy = 0;

        public double Effisiensy
        {
            get { return effisiensy; }
            set { effisiensy = value; NotifyPropertyChanged(); }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private TimeSpan globalTime = new TimeSpan(0, 0, 0);

        public TimeSpan GlobalTime
        {
            get { return globalTime; }
            set
            {
                globalTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("NowTime");
            }
        }

        public TimeSpan NowTime
        {
            get { return DateTime.Now - StartTime; }
        }

        private bool isEmulate = false;

        public bool IsEmulate
        {
            get { return isEmulate; }
            set { isEmulate = value; }
        }

        private bool isRealTime = false;

        public bool IsRealTime
        {
            get { return isRealTime; }
            set
            {
                isRealTime = value;
            }
        }

        private TimeSpan delayTime;

        public TimeSpan DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        private int countOfStations = 0;

        public int CountOfStations
        {
            get { return countOfStations; }
            set { countOfStations = value; }
        }

        private TimeSpan endTime = new TimeSpan(14, 0, 0, 0, 0);

        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private Random rand;

        public Random Rand
        {
            get { return rand; }
            set { rand = value; }
        }

        #endregion

        public event EventHandler OnTurn;

        public event EventHandler OnFinish;

        public Environment()
        {
            Rand = new Random();
        }

        public void LoadData()
        {
            CountOfStations = 20;
            var cyclesInSuperCycle = 3;

            var columns = (int)Math.Sqrt(CountOfStations);
            var k = 0;
            var n = 0;

            while (k < CountOfStations)
            {
                for (int i = 0; i < columns; i++)
                {
                    if (k < CountOfStations)
                    {
                        k++;
                        //var station = new Station(k, 50, new Coordinate() { X = 25 + 25 * i, Y = 25 + 25 * n }, cyclesInSuperCycle, CountOfStations, new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 0),
                        //            new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 80), 0, 0, 0.0, Rand.Next());

                        var station = new SimpleStation(k, 50, new Coordinate() { X = 300, Y = 300 }, cyclesInSuperCycle, CountOfStations, new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 80), 0, 0, 0.0, Rand.Next());
                        Stations.Add(station);
                    }
                    else
                    {
                        break;
                    }
                }
                n++;
            }


        }

        public void Emulate()
        {
            StartTime = DateTime.Now;

            var isTimeAdded = false;

            while (IsEmulate)
            {

                if (TransmitredTotal != 0)
                    Effisiensy = (double)RecievedTotal / (double)TransmitredTotal;

                //обновляем устройство
                var station = GetNextStation();

                if (station.AwakeTime == TimeSpan.Zero || isTimeAdded)
                {
                    isTimeAdded = false;
                    Update(station);

                }
                else
                {
                    isTimeAdded = true;

                    var timeToSubstract = station.AwakeTime;

                    foreach (SimpleStation s in Stations)
                    {
                        s.LocalTime = s.LocalTime.Add(timeToSubstract);
                        s.AwakeTime = s.AwakeTime.Subtract(timeToSubstract);
                        s.UpdatePosition(timeToSubstract);
                    }

                    GlobalTime = GlobalTime.Add(timeToSubstract);

                    if (GlobalTime >= EndTime)
                    {
                        IsEmulate = false;
                        OnFinish(this, EventArgs.Empty);
                    }

                    if (IsRealTime)
                    {
                        //Реалтайм
                        Thread.Sleep(timeToSubstract);
                    }
                }

                //вызываем событие
                if (OnTurn != null)
                {
                    OnTurn(this, EventArgs.Empty);
                }
            }
        }

        public void Update(IStation station)
        {

            station.Update();

            ProvideTransmition(station);

        }

        private void ProvideTransmition(IStation station)
        {
            //если передает
            if (station.CurrentState == StationAction.TxUp)
            {
                var channelState = GetChannelState(station);

                if (channelState == Model.ChannelState.Empty)
                {
                    var RxStations = GetNearRxStations(station);

                    station.StationsToTransmit = RxStations;

                    //foreach (Station rx in RxStations)
                    //{
                    //    //проверяем слушателей
                    //    if (station.StartTransmitTime <= rx.StartRecieveTime) //принимаем пакет от начала передачи, слушаем всегда раньше
                    //    {
                    //        //надо запомнить все станции которые слушают

                    //        //через это время передача завершится
                    //        var packetEndTime = station.PacketTransmitTime - (NowTime - station.StartTransmitTime);
                    //        //rx.ChangeAwakeTime(packetEndTime);  //формируем событие пробуждение слушателям
                    //    }
                    //}


                }
            }

            if (station.CurrentState == StationAction.TxDown)
            {
                Logger.Instance.WriteInfo(station.Id + " trying to transmit");
                foreach (SimpleStation rx in station.StationsToTransmit)
                {
                    Logger.Instance.WriteInfo(station.Id + " transmit to:" + rx.Id);
                    TransmitredTotal++;
                    var channelState = GetChannelState(rx);
                    bool isNoise = !CanDeliver(station.ConnectionRadius, GetRange(station, rx));
                    var message = station.Transmit(isNoise, rx.Id);
                    rx.Recieve(channelState, message);
                    if (!isNoise)
                    {
                        RecievedTotal++;
                    }
                }

                station.StationsToTransmit.Clear();
            }



            ////если слушает
            //if (station.IsRecieve)
            //{
            //    var channelState = GetChannelState(station);
            //    //проверяем что передача началась после прослушивания
            //    //принимаем

            //    //мне не нравится этот код
            //    if (channelState != Model.ChannelState.Empty)
            //    {
            //        var TxStations = GetNearTxStations(station);

            //        bool isNoise = CanDeliver(TxStations[0].ConnectionRadius, GetRange(TxStations[0], station));

            //        var message = TxStations[0].Transmit(isNoise);

            //        Logger.Instance.WriteInfo("Can send " + TxStations[0].Id);

            //        if (TxStations[0].StartTransmitTime >= station.StartRecieveTime)
            //            station.Recieve(channelState, message);
            //        else
            //        {
            //            station.Recieve(channelState);
            //        }
            //    }
            //    else
            //    {
            //        station.Recieve(channelState);
            //    }

            //если никто не разбудил
            //надо проснуться через время прослушивания и сказать что не слушаешь

        }

        private IStation GetNextStation()
        {
            var list = Stations.OrderBy(p => p.AwakeTime.TotalMilliseconds).ToList();
            return list.First();
        }


        //Обеспечение приема заданной вероятности
        public bool CanDeliver(double maxRange, double range)
        {
            double p = (1 / (maxRange * maxRange)) * range * range - (2 / maxRange) * range + 1;

            var random = rand.NextDouble();

            if (random < p)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Станции, которые могут услышать
        /// </summary>
        /// <param name="tx"></param>
        /// <returns></returns>
        public List<IStation> GetNearRxStations(IStation tx)
        {
            var rxStations = Stations.Where(i => i.IsReceive == true).ToList();

            return GetCanConnectStations(tx, rxStations);
        }

        public List<IStation> GetNearTxStations(IStation rx)
        {
            var txStations = Stations.Where(i => i.CurrentState == StationAction.TxUp).ToList();

            if (txStations.IndexOf(rx) >= 0)
                txStations.Remove(rx);


            return GetCanConnectStations(rx, txStations);
        }

        private List<IStation> GetCanConnectStations(IStation s, List<IStation> otherStations)
        {
            var nearStations = new List<IStation>();

            foreach (IStation otherStation in otherStations)
            {
                if (CanListen(s, otherStation))
                {
                    nearStations.Add(otherStation);
                }
            }

            return nearStations;
        }

        public ChannelState GetChannelState(IStation rx)
        {
            var nearStations = GetNearTxStations(rx).Count;

            if (nearStations == 0) return Model.ChannelState.Empty;
            if (nearStations == 1) return Model.ChannelState.Filled;

            return Model.ChannelState.Сollision;
        }

        public bool CanListen(IStation tx, IStation rx)
        {
            double length = GetRange(tx, rx);

            if (length <= tx.ConnectionRadius) return true;

            return false;
        }

        public double GetRange(IStation tx, IStation rx)
        {
            return new Vector(tx.Coordinate.X - rx.Coordinate.X, tx.Coordinate.Y - rx.Coordinate.Y).Length;
        }

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
