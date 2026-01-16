using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace BSolution_.Util
{
    public class Slogger
    {
        public enum LogType
        {
            Info,
            Error,
            Debug
        }
        private static readonly ILog _log = LogManager.GetLogger(nameof(_log));

        public static event Action<string> LogUpdated;

        static Slogger()
        { }

        public static void Write(string message, LogType type = LogType.Info)
        {
            string logMessage = $"[{type}] {message}";

            switch (type)
            {
                case LogType.Error:
                    _log.Error(logMessage);
                    break;
                case LogType.Debug:
                    _log.Debug(logMessage);
                    break;
                default:
                    _log.Info(logMessage);
                    break;
            }

            logMessage = $"[{DateTime.Now:MM-dd HH:mm:ss}] {logMessage}";

            LogUpdated?.Invoke(logMessage);
        }
    }
}
