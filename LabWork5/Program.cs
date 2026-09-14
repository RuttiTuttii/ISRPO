using LabWork5.Tasks;

// Номер задания из аргументов запуска VS или запрос у пользователя.
var task = args.FirstOrDefault();

if (string.IsNullOrEmpty(task))
{
    Console.WriteLine("=== LabWork5: Отладка ===");
    Console.WriteLine("1 - Debug / Trace");
    Console.WriteLine("3 - Debug.Assert");
    Console.WriteLine("4 - Стек вызовов (Call Stack)");
    Console.WriteLine("5 - Отладка JavaScript в браузере");
    Console.Write("\nВведите номер задания (1, 3, 4, 5) [по умолчанию 1]: ");

    var input = Console.ReadLine()?.Trim();
    task = string.IsNullOrEmpty(input) ? "1" : input;
    Console.WriteLine();
}

switch (task.ToLower())
{
    case "1":
        Console.WriteLine("5.1-5.2 — Debug / Trace");
        Task1_2_DebugTrace.Run();
        break;

    case "3":
        Console.WriteLine("5.3 — Debug.Assert");
        Task3_Assert.Run();
        break;

    case "4":
        Console.WriteLine("5.4 — Call Stack, stacktrace.txt");
        Task4_CallStack.Run();
        break;

    case "5":
        Console.WriteLine("5.5 — JavaScript в браузере");
        Task5_JavaScriptLauncher.Run();
        break;

    default:
        Console.WriteLine($"Неизвестный таск: '{task}'. Доступные варианты: 1, 3, 4, 5");
        break;
}
