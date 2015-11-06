using MeshSimulator.Model.PositionHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public class ModelVariables
    {
        public static ModelVariables Default = new ModelVariables();

        public int CountOfStations = 100;
        public int CountOfReports = 10000;
        public TimeSpan EndTime = new TimeSpan(14, 0, 0, 0);

        public int Height = 600;
        public int Width = 600;

        public int ConnectionRadius = 50;
        public int MaxSpeed = 10;

        public int CyclesInSuperCycle = 3;
        public int SlotTimeMilliSeconds = 100;
        public int PacketTransmitTime = 80;

        public bool DoReports = false;
        public bool DoInfoExpandReports = false;

        public IPositionHelper PositionHelper = new RandomPositionHelper();
    }
}
