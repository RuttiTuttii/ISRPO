using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LabWork7.Tasks
{
    public static class Task4_GlobalHandler
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static void InitializeNLog()
        {
            var config = new LoggingConfiguration();

            string logFile = "${basedir}/crash.log";

            var fileTarget = new FileTarget("logfile")
            {
                FileName = logFile,
                Layout = "${longdate}|${level:uppercase=true}|${logger}|${message}${newline}${exception:format=tostring}"
            };

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

            _logger.Info("NLog успешно инициализирован. Путь к логу: {Path}", logFile);
        }

        private static void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = e.ExceptionObject as Exception;

                if (ex != null)
                {
                    _logger.Fatal(ex, "Глобальное необработанное исключение");
                }
                else
                {
                    _logger.Fatal("Событие UnhandledException сработало, но объект исключения отсутствует");
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Ошибка при попытке записать лог: {logEx.Message}");
            }
            finally
            {
                LogManager.Shutdown();

                Console.WriteLine("\nПроизошла ошибка. Подробности в логах (crash.log).");
            }
        }
        public static void Run()
        {
            InitializeNLog();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Console.WriteLine("Бросаем необработанное исключение в фоновом потоке...");

            _ = Task.Run(() =>
            {
                Thread.Sleep(300);
                throw new InvalidOperationException("Тестовый критический сбой подсистемы");
            });

            Thread.Sleep(2000);
        }
    }
}
