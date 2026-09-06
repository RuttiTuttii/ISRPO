using LabWork5.Tasks;

// получаем номер таска из аргументов запуска VS или запрашиваем у пользователя
var task = args.FirstOrDefault();

if (string.IsNullOrEmpty(task))
{
    Console.WriteLine("=== Выбор задания для LabWork5 ===");
    Console.WriteLine("1 - Отладка и трассировка (Debug / Trace)");
    Console.WriteLine("3 - Использование Debug.Assert");
    Console.WriteLine("4 - Исследование стека вызовов (Call Stack)");
    Console.WriteLine("5 - Отладка JavaScript в браузере");
    Console.Write("\nВведите номер задания (1, 3, 4, 5) [по умолчанию 1]: ");

    var input = Console.ReadLine()?.Trim();
    task = string.IsNullOrEmpty(input) ? "1" : input;
    Console.WriteLine();
}

switch (task.ToLower())
{
    case "1":
    case "task1":
        Task1_2_DebugTrace.Run();
        break;

    case "3":
    case "task3":
        Task3_Assert.Run();
        break;

    case "4":
    case "task4":
        Task4_CallStack.Run();
        break;

    case "5":
    case "task5":
        Task5_JavaScriptLauncher.Run();
        break;

    default:
        Console.WriteLine($"Неизвестный таск: '{task}'. Доступные варианты: 1, 3, 4, 5");
        break;
}
