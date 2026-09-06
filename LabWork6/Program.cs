using LabWork6.Tasks;

// главный интерактивный цикл для запуска любого задания шестой лабораторной
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
