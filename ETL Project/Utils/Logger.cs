using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL_Project.Utils
{
    /// <summary>
    /// Klasa do logowania zdarzeń
    /// </summary>
    public static class Logger
    {
        public static event EventHandler<string> NewLogAppeared;
        public static List<string> Logs = new List<string>();

        /// <summary>
        /// Zanotuj zdarzenie
        /// </summary>
        /// <param name="msg"></param>
        public static void Log(string msg)
        {
            Logs.Add(msg);
            NewLogAppeared?.Invoke(null, msg);
        }
    }
}
