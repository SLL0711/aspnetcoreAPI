using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Lib.Log
{
    public class LogHelper
    {
        private static ILog log = LogManager.GetLogger(typeof(LogHelper));
        static LogHelper()
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public static void Debug(string msg, params string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    log.Debug(msg);
                }
                else
                {
                    log.Debug(string.Format(msg, args));
                }
            }
            catch (FormatException ex)
            {
                log.Debug(ex);
            }
        }

        public static void Warn(string msg, params string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    log.Warn(msg);
                }
                else
                {
                    log.Warn(string.Format(msg, args));
                }
            }
            catch (FormatException ex)
            {
                log.Warn(ex);
            }
        }

        public static void Info(string msg, params string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    log.Info(msg);
                }
                else
                {
                    log.Info(string.Format(msg, args));
                }
            }
            catch (FormatException ex)
            {
                log.Info(ex);
            }
        }

        public static void Error(string msg, params string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    log.Error(msg);
                }
                else
                {
                    log.Error(string.Format(msg, args));
                }
            }
            catch (FormatException ex)
            {
                log.Error(ex);
            }
        }
    }
}
