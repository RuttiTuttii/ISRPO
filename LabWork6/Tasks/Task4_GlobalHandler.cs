namespace LabWork6.Tasks;

public static class Task4_GlobalHandler
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 5.4: Глобальный обработчик ===");

        // вешаем глобальный перехватчик на необработанные исключения домена
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            var ex = e.ExceptionObject as Exception;
            var text = $"[{DateTime.Now}] Критическая ошибка: {ex?.Message}\n{ex?.StackTrace}\n";

            // пишем в crash.log по заданию 5.4.2
            File.AppendAllText("crash.log", text);
            Console.WriteLine("\nПроизошла ошибка. Подробности в логах (crash.log).");
        };

        Console.WriteLine("Бросаем необработанное исключение в фоновом потоке...");
        // запускаем таску мимо всех try-catch блоков
        _ = Task.Run(() =>
        {
            Thread.Sleep(300);
            throw new InvalidOperationException("Тестовый критический сбой подсистемы");
        });

        Thread.Sleep(1500);
    }
}
