using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Log.Support
{
    class DefaultLogTarget:ILogTarget
    {
        public void WriteInfo(string info, DateTime time, int thread, string module, string method)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(time.ToString("H:mm:ss:ff")).Append("|").Append(thread).Append("|");
            builder.Append(module).Append("/").Append(method);
            builder.Append(": ").Append(info);
            System.Diagnostics.Debug.WriteLine(builder.ToString());
        }

        public void WriteException(string info, Exception exception, DateTime time, string module, string method)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(time.ToString("H:mm:ss:ff")).Append(" ERROR "); 
            builder.Append(module).Append("/").Append(method);
            builder.Append("|").Append(info).Append(" ");
            builder.AppendLine(exception.GetType().ToString());
            builder.AppendLine(exception.StackTrace);
            System.Diagnostics.Debug.WriteLine(builder.ToString());
        }
    }
}
