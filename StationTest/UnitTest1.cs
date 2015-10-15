using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MeshSimulator.Model;

namespace StationTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            var station = new Station(3, 50, new Coordinate() { X = 25, Y = 25 }, 3, 10, new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 0),
                new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 0, 0, 100), 0, 0, 0.1, 0);
            /*
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.None);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxUp);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxDown);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.RxUp);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.RxDown);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxUp);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxDown);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.RCU);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxUp);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxDown);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.RxUp);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.RxDown);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxUp);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.TxDown);

            station.LocalTime = station.LocalTime.Add(station.AwakeTime);
            station.Update();
            Assert.IsTrue(station.CurrentState == StationAction.RCU);
             * */
        }
    }
}
