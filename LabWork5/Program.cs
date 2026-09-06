using LabWork5.Tasks;

// главный интерактивный цикл для запуска любого задания пятой лабораторной
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
