using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Log.Support
{
    public class Logger
    {
        private List<ILogTarget> targets = new List<ILogTarget>();

        private Logger()
        {
        }

        private static volatile Logger instance;

        public static Logger Instance
        {
            get 
            {
                if (instance==null)
                {
                    instance = new Logger();
                    instance.init();
                }
                return instance;
            }           
        }

        private void init()
        {
            this.targets.Add(new DefaultLogTarget());   
        }

        public void WriteInfo(string info, [CallerFilePath] string filePath = "", [CallerMemberName] string method = "")
        {
            foreach (var target in targets)
            {
                target.WriteInfo(
                      info
                    , DateTime.Now
                    , Environment.CurrentManagedThreadId
                    , Path.GetFileNameWithoutExtension(filePath)
                    , method
                    );
            }
        }

        public void WriteException(string info, Exception exception, [CallerFilePath] string filePath = "", [CallerMemberName] string method = "")
        {
            foreach (var target in targets)
            {
                target.WriteException(
                      info
                    , exception
                    , DateTime.Now
                    , Path.GetFileNameWithoutExtension(filePath)
                    , method
                    );
            }
        }

    }
}
