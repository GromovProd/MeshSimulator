using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Support
{
    public interface ILogTarget
    {
        void WriteInfo(string info, DateTime time, int thread, string module, string method);
        void WriteException(string info, Exception exception, DateTime time, string module, string method);
    }
}
