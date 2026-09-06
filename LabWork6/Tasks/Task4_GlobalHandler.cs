namespace LabWork6.Tasks;

public static class Task4_GlobalHandler
{
    private static bool _handlerSubscribed = false;

    public static void Run()
    {
        Console.WriteLine("--- Задание 5.4: Глобальный обработчик исключений ---\n");

        // подписываемся на необработанные исключения домена
        if (!_handlerSubscribed)
        {
            AppDomain.CurrentDomain.UnhandledException += GlobalHandler;
            _handlerSubscribed = true;
            Console.WriteLine("[ИНФО] обработчик AppDomain.CurrentDomain.UnhandledException успешно зарегистрирован.");
        }

        Console.WriteLine("1 - штатная работа");
        Console.WriteLine("2 - спровоцировать фатальное необработанное исключение в фоновом потоке");
        Console.Write("выберите вариант (1 или 2): ");

        string? choice = Console.ReadLine();

        if (choice?.Trim() == "2")
        {
            Console.WriteLine("\nзапускаем фоновую задачу, которая выбросит необработанное исключение...");
            // запускаем фоновый поток, который бросит исключение мимо локальных try-catch
            _ = Task.Run(() =>
            {
                Thread.Sleep(300);
                throw new InvalidOperationException("Фатальный сбой обработки транзакции!");
            });

            Thread.Sleep(2000);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("программа отработала штатно без сбоев.");
            Console.ResetColor();
        }
    }

    // глобальный обработчик критических сбоев
    private static void GlobalHandler(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        string errorLog = $"[{DateTime.Now}] ФАТАЛЬНАЯ ОШИБКА:\nТип: {ex?.GetType().FullName}\nСообщение: {ex?.Message}\nСтек:\n{ex?.StackTrace}\n{new string('-', 50)}\n";

        // пишем детали в crash.log по заданию 5.4.2
        string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log");
        File.AppendAllText(logPath, errorLog);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n" + new string('!', 60));
        Console.WriteLine("Произошла ошибка. Подробности в логах (crash.log).");
        Console.WriteLine($"Файл с отчетом: {logPath}");
        Console.WriteLine(new string('!', 60) + "\n");
        Console.ResetColor();
    }
}
