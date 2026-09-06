using LabWork5.Tasks;

// проверяем переданный аргумент запуска из профиля Visual Studio
if (args.Length > 0 && !string.Equals(args[0], "menu", StringComparison.OrdinalIgnoreCase))
{
    switch (args[0].ToLowerInvariant())
    {
        case "task1":
        case "1":
            // сразу запускаем задание 5.1 и 5.2 по выбранному профилю
            Task1_2_DebugTrace.Run();
            return;
        case "task3":
        case "3":
            // сразу запускаем задание 5.3 с ассертами
            Task3_Assert.Run();
            return;
        case "task4":
        case "4":
            // сразу запускаем задание 5.4 со стеком вызовов
            Task4_CallStack.Run();
            return;
        case "task5":
        case "5":
            // сразу открываем страницу для отладки JS в браузере
            Task5_JavaScriptLauncher.Run();
            return;
    }
}

// если запущен общий профиль или аргументы не указаны — открываем меню
while (true)
{
    try { Console.Clear(); } catch { }
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("===============================================================");
    Console.WriteLine("   Лабораторная работа №5: Отладка модулей программного проекта");
    Console.WriteLine("===============================================================");
    Console.ResetColor();
    Console.WriteLine("1. Задания 5.1 и 5.2: Отладка и трассировка (Debug / Trace)");
    Console.WriteLine("2. Задание 5.3: Использование Debug.Assert (CalculateDiscount)");
    Console.WriteLine("3. Задание 5.4: Исследование стека вызовов (Call Stack & Log)");
    Console.WriteLine("4. Задание 5.5: Запуск фронтенда для отладки JavaScript в браузере");
    Console.WriteLine("0. Выход из программы");
    Console.WriteLine("---------------------------------------------------------------");
    Console.Write("выберите номер задания для запуска: ");

    string? choice = Console.ReadLine();
    Console.WriteLine();

    // маршрутизируем выбор пользователя на нужный статический метод таски
    switch (choice?.Trim())
    {
        case "1":
            Task1_2_DebugTrace.Run();
            break;
        case "2":
            Task3_Assert.Run();
            break;
        case "3":
            Task4_CallStack.Run();
            break;
        case "4":
            Task5_JavaScriptLauncher.Run();
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
