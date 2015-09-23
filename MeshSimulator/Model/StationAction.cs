using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Model
{
    public enum StationAction
    {
        None = 0,
        TxUp = 1,
        TxDown = 2,
        RxUp = 3,
        RxDown = 4,
        RCU = 5 //EndOfCycleUpdate
    }
}
