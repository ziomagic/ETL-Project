using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Project.Utils
{
    public static class Logger
    {
        public static event EventHandler<string> NewLogAppeared;
        public static List<string> Logs = new List<string>();

        public static void Log(string msg)
        {
            Logs.Add(msg);
            NewLogAppeared?.Invoke(null, msg);
        }
    }
}
