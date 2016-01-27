﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Log.Support;
using MeshSimulator.Model.Station;
using MeshSimulator.Data;

namespace MeshSimulator.Model
{
    public class Environment : INotifyPropertyChanged
    {
        #region Properties

        private ModelVariables variables;

        public ModelVariables Variables
        {
            get { return variables; }
        }

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

        private double efficiency = 0;

        public double Efficiency
        {
            get { return efficiency; }
            set { efficiency = value; NotifyPropertyChanged(); }
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
                NotifyPropertyChanged("EmulationTime");
            }
        }

        public TimeSpan EmulationTime
        {
            get { return DateTime.Now - StartTime; }
        }

        private volatile bool isEmulate = false;

        public bool IsEmulate
        {
            get { return isEmulate; }
            set { isEmulate = value; NotifyPropertyChanged(); }
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

        private bool isUIon = false;

        public bool IsUIon
        {
            get { return isUIon; }
            set
            {
                isUIon = value;
                foreach (IStation s in Stations)
                {
                    s.SetBinding(value);
                }
            }
        }

        private Random rand;

        public Random Rand
        {
            get { return rand; }
            set { rand = value; }
        }

        private int reportsDone = 0;

        public int ReportsDone
        {
            get { return reportsDone; }
            set { reportsDone = value; NotifyPropertyChanged(); }
        }

        private int reportMillisecondsInterval = 0;

        public int ReportMillisecondsInterval
        {
            get { return reportMillisecondsInterval; }
            set { reportMillisecondsInterval = value; NotifyPropertyChanged(); }
        }

        private int countOfInfoExpandedStations = 0;

        public int CountOfInfoExpandedStations
        {
            get { return countOfInfoExpandedStations; }
            set { countOfInfoExpandedStations = value; }
        }

        private bool isInfoExpanded = false;

        public bool IsInfoExpanded
        {
            get { return isInfoExpanded; }
            set { isInfoExpanded = value; }
        }

        #endregion

        public event EventHandler OnTurn;

        public event EventHandler OnFinish;

        public event EventHandler OnInfoExpanded;

        public Environment(ModelVariables v)
        {
            Rand = new Random();

            variables = v;

            ReportMillisecondsInterval = (int)(Variables.EndTime.TotalMilliseconds / Variables.CountOfReports);

            ReportWriter.Init();

            if (v.DoReports)
            {
                ReportWriter.GenerateReport(v);
            }

            if (v.DoInfoExpandReports)
            {
                ReportWriter.GenerateInfoExpandReport(v);
            }

            LoadData();
        }

        public void LoadData()
        {
            var posHelper = Variables.PositionHelper;
            var coords = posHelper.GetCoordinates(Variables.CountOfStations, Variables.ConnectionRadius, Variables.Height, Variables.Width);

            for (int i = 0; i < Variables.CountOfStations; i++)
            {
                var station = new SimpleStation(i, Variables.ConnectionRadius, coords[i], Variables.CyclesInSuperCycle, Variables.CountOfStations, new TimeSpan(0, 0, 0, 0, Variables.SlotTimeMilliSeconds), new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, Variables.SlotTimeMilliSeconds), new TimeSpan(0, 0, 0, 0, Variables.PacketTransmitTime), 0, 0, (10 * Rand.NextDouble() - 5) / 500, Rand.Next(), Variables.MaxSpeed, Variables.Height, Variables.Width);

                Stations.Add(station);
            }

        }

        private void SetStationWithSpecialInfo()
        {
            var idWithInfo = Rand.Next(Variables.CountOfStations);
            Stations[idWithInfo].IsGotSpecialInfo = true;
        }

        public void Emulate()
        {
            StartTime = DateTime.Now;

            var isTimeAdded = false;

            while (IsEmulate)
            {

                if (TransmitredTotal != 0)
                    Efficiency = (double)RecievedTotal / (double)TransmitredTotal;

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

                    foreach (IStation s in Stations)
                    {
                        s.LocalTime = s.LocalTime.Add(timeToSubstract);
                        s.AwakeTime = s.AwakeTime.Subtract(timeToSubstract);
                        s.UpdatePosition(timeToSubstract);
                    }

                    station.AddError(timeToSubstract);

                    GlobalTime = GlobalTime.Add(timeToSubstract);

                    CheckEvents();

                    if (IsUIon)
                    {
                        if (IsRealTime)
                        {
                            //Реалтайм
                            Thread.Sleep(timeToSubstract);
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                }

                //вызываем событие
                CallOnTurn();
            }
        }

        private void CheckEvents()
        {
            if (Variables.DoReports)
            {
                //Собираем аналитику каждые ReportMinutesInterval минут
                if (GlobalTime.TotalMilliseconds > ReportsDone * ReportMillisecondsInterval)
                {
                    ReportsDone++;

                    var report = new Report.Report() { Id = ReportsDone, EmulationTime = EmulationTime, GlobalTime = GlobalTime, MessagesSended = TransmitredTotal, MessagesRecieved = RecievedTotal, Efficiency = Efficiency };
                    ReportWriter.WriteReport(report);

                }
            }

            if (Variables.DoInfoExpandReports)
            {
                if (Stations.Where(i => i.IsGotSpecialInfo).Count() != CountOfInfoExpandedStations)
                {
                    CountOfInfoExpandedStations++;

                    var report = new Report.InfoExpandReport() { Id = CountOfInfoExpandedStations, EmulationTime = EmulationTime, GlobalTime = GlobalTime, MessagesSended = TransmitredTotal, MessagesRecieved = RecievedTotal, Efficiency = Efficiency };
                    ReportWriter.WriteReport(report);

                    CallOnInfoExpanded();
                }
            }

            if (GlobalTime >= Variables.EndTime)
            {
                IsEmulate = false;

                var dict = new Dictionary<int, List<StationData>>();

                for (int i = 0; i < Stations.Count; i++)
                {
                    dict.Add(Stations[i].Id, Stations[i].Data);
                }

                var report = new Report.FinishReport() { Id = ReportsDone, EmulationTime = EmulationTime, GlobalTime = GlobalTime, MessagesSended = TransmitredTotal, MessagesRecieved = RecievedTotal, Efficiency = Efficiency, StationsData = dict };
                ReportWriter.GenerateFinishReport(Variables, report);

                CallOnFinish();
            }
        }

        private void CallOnInfoExpanded()
        {
            if (OnInfoExpanded != null)
            {
                OnInfoExpanded(this, EventArgs.Empty);
            }
        }

        private void CallOnTurn()
        {
            if (OnTurn != null)
            {
                OnTurn(this, EventArgs.Empty);
            }
        }

        private void CallOnFinish()
        {
            if (OnFinish != null)
            {
                OnFinish(this, EventArgs.Empty);
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
                }
            }

            if (station.CurrentState == StationAction.TxDown)
            {
#if DEBUG
                Logger.Instance.WriteInfo(station.Id + " trying to transmit");
#endif

                foreach (SimpleStation rx in station.StationsToTransmit)
                {
#if DEBUG
                    Logger.Instance.WriteInfo(station.Id + " transmit to:" + rx.Id);
#endif

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
            if (IsUIon)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        #endregion

    }
}
