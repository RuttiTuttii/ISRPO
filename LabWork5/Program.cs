using LabWork5.Tasks;

// получаем номер таска из профиля запуска VS (или запускаем 1-й по умолчанию)
var task = args.FirstOrDefault() ?? "1";

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
        Console.WriteLine($"Неизвестный таск: '{task}'. Варианты: 1, 3, 4, 5");
        break;
}
