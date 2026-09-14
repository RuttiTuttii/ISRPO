using LabWork6.Tasks;

// Номер задания из аргументов запуска VS или запрос у пользователя.
var task = args.FirstOrDefault();

if (string.IsNullOrEmpty(task))
{
    Console.WriteLine("=== LabWork6: Исключения и REST API ===");
    Console.WriteLine("1 - Обработка исключений и NLog");
    Console.WriteLine("2 - Пользовательское исключение (NegativeNumberException)");
    Console.WriteLine("3 - finally и using");
    Console.WriteLine("4 - Глобальный обработчик (AppDomain.UnhandledException)");
    Console.WriteLine("5 - REST API сервер (http://localhost:5000)");
    Console.Write("\nВведите номер задания (1-5) [по умолчанию 5]: ");

    var input = Console.ReadLine()?.Trim();
    task = string.IsNullOrEmpty(input) ? "5" : input;
    Console.WriteLine();
}

switch (task.ToLower())
{
    case "1":
        Console.WriteLine("5.1 — исключения и NLog, errors.log");
        Task1_Handling.Run();
        break;

    case "2":
        Console.WriteLine("5.2 — NegativeNumberException");
        Task2_CustomException.Run();
        break;

    case "3":
        Console.WriteLine("5.3 — finally и using");
        Task3_FinallyUsing.Run();
        break;

    case "4":
        Console.WriteLine("5.4 — глобальный обработчик, crash.log");
        Task4_GlobalHandler.Run();
        break;

    case "5":
        Console.WriteLine("5.5 — REST API, http://localhost:5000");
        Task5_RestApi.Run(args);
        break;

    default:
        Console.WriteLine($"Неизвестный таск: '{task}'. Доступные варианты: 1..5");
        break;
}
