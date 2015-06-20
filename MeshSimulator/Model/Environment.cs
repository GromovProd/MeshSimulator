﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MeshSimulator.Model;

namespace MeshSimulator.Model
{
    public class Environment
    {
        private List<Station> stations;
        public List<Station> Stations
        {
            get { return stations; }
            set { stations = value; }
        }

        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public TimeSpan NowTime
        {
            get { return DateTime.Now - StartTime; }
        }

        public Station NextStation
        {
            get { return Stations.OrderBy(p => p.AwakeTime.TotalMilliseconds).First(); }
        }

        private bool isEmulate = false;

        public bool IsEmulate
        {
            get { return isEmulate; }
            set { isEmulate = value; }
        }

        private TimeSpan delayTime;

        public TimeSpan DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }

        private Random rand;

        public Random Rand
        {
            get { return rand; }
            set { rand = value; }
        }

        public void Emulate()
        {

            while (IsEmulate)
            {
                Update();
                DelayTime = NextStation.AwakeTime;
                Thread.Sleep((int)(DelayTime.TotalMilliseconds));
            }
        }

        public void LoadData()
        {

        }

        public void Update()
        {
            //обновляем устройство
            var station = NextStation;

            station.Update();

            //если передает
            if (station.IsTransmit)
            {
                var channelState = ChannelState(station);

                if (channelState == Model.ChannelState.Empty)
                {
                    var RxStations = GetNearRxStations(station);
                    foreach (Station rx in RxStations)
                    {
                        //проверяем слушателей
                        if (station.StartTransmitTime > rx.StartRecieveTime) //принимаем пакет от начала передачи, слушаем всегда раньше
                        {
                            //через это время передача завершится
                            var packetEndTime = station.PacketTransmitTime - (NowTime - station.StartTransmitTime);
                            rx.ChangeAwakeTime(packetEndTime);  //формируем событие пробуждение слушателям
                        }
                    }
                }

            }

            //если слушает
            if (station.IsRecieve)
            {
                var channelState = ChannelState(station);
                //проверяем что передача началась после прослушивания
                //принимаем

                if (channelState == Model.ChannelState.Filled)
                {
                    var TxStations = GetNearTxStations(station);

                    bool isNoise = CanDeliver(TxStations[0].ConnectionRadius, GetRange(TxStations[0], station));

                    var message = TxStations[0].Transmit(isNoise);
                    station.Recieve(channelState, message);
                }

                else
                {
                    station.Recieve(channelState);
                }

                //надо проснуться через время прослушивания и сказать что не слушаешь
            }

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

        public List<Station> GetNearRxStations(Station tx)
        {
            var rxStations = Stations.Where(i => i.IsRecieve == true);

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
            var txStations = Stations.Where(i => i.IsTransmit == true);

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

        public ChannelState ChannelState(Station rx)
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

    }
}
