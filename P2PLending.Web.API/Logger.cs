﻿using log4net;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace P2PLending.Web.API
{
    public class Logger
    {
        private static readonly string LOG_CONFIG_FILE = @"log4net.config";

        private static readonly ILog _log = GetLogger(typeof(Logger));

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        public static void Debug(object message)
        {
            SetLog4NetConfiguration();
            _log.Debug(message);
        }
        public static void Error(object message)
        {
            SetLog4NetConfiguration();
            _log.Error(message);
        }
        public static void Info(object message)
        {
            SetLog4NetConfiguration();
            _log.Info(message);
        }
        private static void SetLog4NetConfiguration()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(LOG_CONFIG_FILE));

            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
