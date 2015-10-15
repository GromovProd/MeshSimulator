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

namespace MeshSimulator.Model
{
    public class Environment
    {
        #region Properties

        private List<Station> stations = new List<Station>();
        public List<Station> Stations
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

        private TimeSpan globalTime;

        public TimeSpan GlobalTime
        {
            get { return globalTime; }
            set { globalTime = value; NotifyPropertyChanged(); }
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
            set { isRealTime = value; }
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
            CountOfStations = 50;
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
                        var station = new Station(k, 50, new Coordinate() { X = 25 + 25 * i, Y = 25 + 25 * n }, cyclesInSuperCycle, CountOfStations, new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 0),
                                    new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 80), 0, 0, 0.0, Rand.Next());

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

            var isTimeAdded = false;

            DelayTime = new TimeSpan(0, 0, 1);

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

                    foreach (Station s in Stations)
                    {
                        s.LocalTime = s.LocalTime.Add(timeToSubstract);
                        s.AwakeTime = s.AwakeTime.Subtract(timeToSubstract);
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
                        Thread.Sleep((int)(DelayTime.TotalMilliseconds));
                    }
                }

                //вызываем событие
                OnTurn(this, EventArgs.Empty);
            }
        }

        public void Update(Station station)
        {

            station.Update();

            ProvideTransmition(station);

        }

        private void ProvideTransmition(Station station)
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
                foreach (Station rx in station.StationsToTransmit)
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

        private Station GetNextStation()
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
        public List<Station> GetNearRxStations(Station tx)
        {
            var rxStations = Stations.Where(i => i.IsReceive == true);

            var nearRxStations = new List<Station>();

            foreach (Station rx in rxStations)
            {
                if (CanListen(tx, tx))
                {
                    nearRxStations.Add(rx);
                }
            }

            return nearRxStations;
        }

        public List<Station> GetNearTxStations(Station rx)
        {
            var txStations = Stations.Where(i => i.CurrentState == StationAction.TxUp).ToList();

            if (txStations.IndexOf(rx) >= 0)
                txStations.Remove(rx);


            var nearTxStations = new List<Station>();

            foreach (Station tx in txStations)
            {
                if (CanListen(tx, rx))
                {
                    nearTxStations.Add(tx);
                }
            }

            return nearTxStations;
        }

        public ChannelState GetChannelState(Station rx)
        {
            var nearStations = GetNearTxStations(rx).Count;

            if (nearStations == 0) return Model.ChannelState.Empty;
            if (nearStations == 1) return Model.ChannelState.Filled;

            return Model.ChannelState.Сollision;
        }

        public bool CanListen(Station tx, Station rx)
        {
            double length = GetRange(tx, rx);

            if (length <= tx.ConnectionRadius) return true;

            return false;
        }

        public double GetRange(Station tx, Station rx)
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
