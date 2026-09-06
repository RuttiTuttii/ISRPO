Console.WriteLine("=== ЛР №6. Задание 5.4: Глобальный обработчик исключений ===\n");

// регистрируем глобальный обработчик неотловленных исключений для текущего домена приложения
AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

Console.WriteLine("глобальный перехватчик AppDomain.CurrentDomain.UnhandledException успешно подключен.");
Console.WriteLine("1 - штатная работа программы (без падений)");
Console.WriteLine("2 - спровоцировать фатальное необработанное исключение в фоновом потоке");
Console.Write("выберите вариант (1 или 2): ");
string? choice = Console.ReadLine();

if (choice?.Trim() == "2")
{
    Console.WriteLine("\nзапускаем фоновую задачу, которая выбросит необработанное исключение...");
    // запускаем таску, которая швырнет исключение мимо всех локальных try-catch
    _ = Task.Run(() =>
    {
        Thread.Sleep(300);
        // имитируем критический сбой
        throw new InvalidOperationException("Критический сбой подсистемы обработки платежей!");
    });

    // ждем падения потока
    Thread.Sleep(2000);
}
else
{
    Console.WriteLine("\nпрограмма отработала в штатном режиме без фатальных ошибок.");
}

Console.WriteLine("нажмите Enter для выхода...");
Console.ReadLine();

// глобальный метод-обработчик всех неотловленных исключений в процессе
static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
{
    // извлекаем объект пойманного исключения
    var ex = e.ExceptionObject as Exception;
    string errorDetails = $"[КРИТИЧЕСКИЙ СБОЙ {DateTime.Now}]\n" +
                          $"Тип: {ex?.GetType().FullName}\n" +
                          $"Сообщение: {ex?.Message}\n" +
                          $"IsTerminating: {e.IsTerminating}\n" +
                          $"Стек вызовов:\n{ex?.StackTrace}\n" +
                          new string('-', 50) + "\n";

    // сохраняем инфу о краше в файл crash.log по требованию п. 5.4.2
    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crash.log");
    File.AppendAllText(logPath, errorDetails);

    // выводим пользователю понятное сообщение о случившемся
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n" + new string('!', 60));
    Console.WriteLine("Произошла ошибка. Подробности в логах (crash.log).");
    Console.WriteLine($"Путь к файлу с логом: {logPath}");
    Console.WriteLine(new string('!', 60) + "\n");
    Console.ResetColor();
}
