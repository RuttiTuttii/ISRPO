using LabWork7.Tasks;

// Номер задания из аргументов запуска VS или запрос у пользователя.
var task = args.FirstOrDefault();

if (string.IsNullOrEmpty(task))
{
    Console.WriteLine("=== LabWork7: Отладка и логирование ===");
    Console.WriteLine("1 - 5.1 Логирование исключений в log.txt");
    Console.WriteLine("2 - 5.2 Stopwatch и timings.log");
    Console.WriteLine("3 - 5.3 TraceSource Calculator (trace.log)");
    Console.WriteLine("4 - 5.4 SourceSwitch Storage (storage.log)");
    Console.Write("\nВведите номер задания (1-4) [по умолчанию 1]: ");

    var input = Console.ReadLine()?.Trim();
    task = string.IsNullOrEmpty(input) ? "1" : input;
    Console.WriteLine();
}

switch (task.ToLower())
{
    case "1":
        Console.WriteLine("5.1 — исключения в log.txt");
        Task1_Handling.Run();
        break;
    case "2":
        Console.WriteLine("5.2 — замеры в timings.log");
        await Task2_StopWatch.Run();
        break;
    case "3":
        Console.WriteLine("5.3 — TraceSource, trace.log");
        Task3_TraceSourceCalculator.Run();
        break;
    case "4":
        Console.WriteLine("5.4 — SourceSwitch, storage.log");
        Task4_StorageSwitch.Run();
        break;
    default:
        Console.WriteLine($"Неизвестный таск: '{task}'. Доступные варианты: 1..4");
        break;
}
