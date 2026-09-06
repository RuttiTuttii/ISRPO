using LabWork6.Tasks;

// получаем номер таска из аргументов запуска VS или запрашиваем у пользователя
var task = args.FirstOrDefault();

if (string.IsNullOrEmpty(task))
{
    Console.WriteLine("=== Выбор задания для LabWork6 ===");
    Console.WriteLine("1 - Обработка исключений и NLog");
    Console.WriteLine("2 - Пользовательское исключение (NegativeNumberException)");
    Console.WriteLine("3 - Использование finally и using");
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
    case "task1":
        Task1_Handling.Run();
        break;

    case "2":
    case "task2":
        Task2_CustomException.Run();
        break;

    case "3":
    case "task3":
        Task3_FinallyUsing.Run();
        break;

    case "4":
    case "task4":
        Task4_GlobalHandler.Run();
        break;

    case "5":
    case "task5":
        Task5_RestApi.Run(args);
        break;

    default:
        Console.WriteLine($"Неизвестный таск: '{task}'. Доступные варианты: 1..5");
        break;
}
