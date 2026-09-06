using LabWork6.Tasks;

// получаем номер таска из профиля запуска VS (или запускаем 1-й по умолчанию)
var task = args.FirstOrDefault() ?? "1";

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
        Console.WriteLine($"Неизвестный таск: '{task}'. Варианты: 1, 2, 3, 4, 5");
        break;
}
