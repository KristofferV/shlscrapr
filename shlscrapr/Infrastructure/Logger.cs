using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace shlscrapr.Infrastructure
{
    public static class Logger
    {
        public static ILogger Instance { get; private set; }

        public static void Init(string application)
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            const string layout = @"${longdate} ${message}";
            consoleTarget.Layout = layout;
            fileTarget.FileName = "C:/shlscrapr/log/file.txt";
            fileTarget.Layout = layout;

            // Step 4. Define rules
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            Instance = LogManager.GetLogger(application);
        }

        public static void Info(string message)
        {
            Instance.Info(message);
        }

        public static void Info(string message, object extra)
        {
            Instance.Info(message, extra);
        }

        public static void Debug(string message)
        {
            Instance.Debug(message);
        }

        public static void Debug(string message, object extra)
        {
            Instance.Debug(message, extra);
        }

        public static void Warning(string message)
        {
            Instance.Warn(message);
        }

        public static void Warning(string message, object extra)
        {
            Instance.Warn(message, extra);
        }

        public static void Warning(string message, Exception exception)
        {
            Instance.Warn(message, exception);
        }

        public static void Error(string message)
        {
            Instance.Error(message);
        }

        public static void Error(string message, object extra)
        {
            Instance.Error(message, extra);
        }

        public static void Error(string message, Exception exception)
        {
            Instance.Error(message, exception);
        }

        public static void Error(string message, Exception exception, object extra)
        {
            Instance.Error(message, exception, extra);
        }

        public static void Fatal(string message)
        {
            Instance.Fatal(message);
        }

        public static void Fatal(string message, object extra)
        {
            Instance.Fatal(message, extra);
        }

        public static void Fatal(string message, Exception exception)
        {
            Instance.Fatal(message, exception);
        }

        public static void Fatal(string message, Exception exception, object extra)
        {
            Instance.Fatal(message, exception, extra);
        }
    }
}