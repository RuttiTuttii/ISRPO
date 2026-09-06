using NLog;
using NLog.Config;
using NLog.Targets;

// конфигурируем NLog программно прямо в коде, чтобы писать ошибки в errors.log
var config = new LoggingConfiguration();
var fileTarget = new FileTarget("logfile")
{
    FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log"),
    Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}"
};
config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);
LogManager.Configuration = config;

// создаем логгер для фиксации ошибок
var logger = LogManager.GetCurrentClassLogger();

Console.WriteLine("=== ЛР №6. Задание 5.1: Обработка исключений и NLog ===\n");
Console.WriteLine("программа делит два числа и аккуратно ловит любые ошибки ввода\n");

// крутим цикл с вводом чисел, чтобы протестировать все виды исключений
while (true)
{
    Console.WriteLine("--- Новая попытка деления (введите 'exit' для выхода) ---");
    Console.Write("введите делимое (первое число): ");
    string? input1 = Console.ReadLine();
    if (string.Equals(input1?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

    Console.Write("введите делитель (второе число): ");
    string? input2 = Console.ReadLine();
    if (string.Equals(input2?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

    // заворачиваем парсинг и вычисление в try-catch для перехвата разных типов исключений
    try
    {
        // парсим строго через int.Parse, чтобы намеренно получить FormatException если введен текст
        int a = int.Parse(input1!);
        int b = int.Parse(input2!);

        // делим числа в лоб, чтобы при нуле выскочил честный DivideByZeroException
        int result = a / b;

        // выводим успешный ответ, если юзер ввел адекватные числа
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"результат деления {a} / {b} = {result}\n");
        Console.ResetColor();
        logger.Info($"Успешное деление {a} на {b} = {result}");
    }
    catch (FormatException ex)
    {
        // перехватываем кривой ввод пользователя (когда вбили буквы вместо цифр)
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[FormatException] ошибка: введен текст вместо числа!");
        Console.ResetColor();

        // логируем ошибку в errors.log через NLog, как требует задание 5.1.2
        logger.Error(ex, "Ошибка формата данных: пользователь ввел не число");
    }
    catch (DivideByZeroException ex)
    {
        // перехватываем классическое деление на ноль
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[DivideByZeroException] ошибка: делить на ноль нельзя!");
        Console.ResetColor();

        // фиксируем попытку деления на ноль в лог-файле
        logger.Error(ex, "Попытка деления на ноль");
    }
    catch (Exception ex)
    {
        // ловим вообще любое непредвиденное исключение верхнего уровня со стек-трейсом
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"[Общее исключение]: {ex.Message}");
        Console.WriteLine($"стек вызовов: {ex.StackTrace}");
        Console.ResetColor();

        // пишем фатальный лог со всеми деталями
        logger.Fatal(ex, "Непредвиденное общее исключение");
    }

    Console.WriteLine();
}

Console.WriteLine("логи записаны в errors.log. работа программы завершена.");
