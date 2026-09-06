using LabWork6.Tasks;

// проверяем переданный аргумент запуска из профиля Visual Studio
if (args.Length > 0 && !string.Equals(args[0], "menu", StringComparison.OrdinalIgnoreCase))
{
    switch (args[0].ToLowerInvariant())
    {
        case "task1":
        case "1":
            // сразу запускаем задание 5.1 с NLog
            Task1_Handling.Run();
            return;
        case "task2":
        case "2":
            // сразу запускаем задание 5.2 с NegativeNumberException
            Task2_CustomException.Run();
            return;
        case "task3":
        case "3":
            // сразу запускаем задание 5.3 с using и finally
            Task3_FinallyUsing.Run();
            return;
        case "task4":
        case "4":
            // сразу запускаем задание 5.4 с глобальным обработчиком
            Task4_GlobalHandler.Run();
            return;
        case "task5":
        case "5":
            // сразу запускаем веб-сервер REST API на ASP.NET Core
            Task5_RestApi.Run(args);
            return;
    }
}

// если запущен общий профиль или аргументы не указаны — открываем меню
while (true)
{
    try { Console.Clear(); } catch { }
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("===============================================================");
    Console.WriteLine("   Лабораторная работа №6: Организация обработки исключений");
    Console.WriteLine("===============================================================");
    Console.ResetColor();
    Console.WriteLine("1. Задание 5.1: Обработка исключений и логирование (NLog в errors.log)");
    Console.WriteLine("2. Задание 5.2: Пользовательское исключение (NegativeNumberException)");
    Console.WriteLine("3. Задание 5.3: Использование finally и using (чтение четных чисел)");
    Console.WriteLine("4. Задание 5.4: Глобальный обработчик (AppDomain.UnhandledException)");
    Console.WriteLine("5. Задание 5.5: Запуск REST API сервера (ASP.NET Core Minimal API)");
    Console.WriteLine("0. Выход из программы");
    Console.WriteLine("---------------------------------------------------------------");
    Console.Write("выберите номер задания для запуска: ");

    string? choice = Console.ReadLine();
    Console.WriteLine();

    // выполняем выбранное подзадание
    switch (choice?.Trim())
    {
        case "1":
            Task1_Handling.Run();
            break;
        case "2":
            Task2_CustomException.Run();
            break;
        case "3":
            Task3_FinallyUsing.Run();
            break;
        case "4":
            Task4_GlobalHandler.Run();
            break;
        case "5":
            Task5_RestApi.Run(args);
            break;
        case "0":
            Console.WriteLine("выход из программы. до встречи!");
            return;
        default:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("пункт не распознан, попробуйте еще раз.");
            Console.ResetColor();
            break;
    }

    Console.WriteLine("\nнажмите Enter для возврата в главное меню...");
    Console.ReadLine();
}
