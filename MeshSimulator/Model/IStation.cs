﻿using System;
using System.Collections.Generic;
namespace MeshSimulator.Model
{
    public interface IStation
    {
        TimeSpan AwakeTime { get; set; }
        double ConnectionRadius { get; }
        Coordinate Coordinate { get; set; }
        double TimeDeviation { get; set; }
        TimeSpan LocalTime { get; set; }
        StationAction CurrentState { get; set; }
        int Id { get; }
        bool IsReceive { get; set; }
        bool IsTransmit { get; set; }
        void Recieve(ChannelState channelState, Message message = null);
        double Speed { get; }
        int MaxSpeed { get; }
        double SpeedAngle { get; set; }
        Message Transmit(bool isNoise, int toId);
        void Update();
        void UpdatePosition(TimeSpan ts);
        void AddError(TimeSpan timeToSubstract);
        List<IStation> StationsToTransmit { get; set; }
        bool IsGotSpecialInfo { get; set; }
    }
}
