using NLog;
using NLog.Config;
using NLog.Targets;

namespace LabWork6.Tasks;

public static class Task1_Handling
{
    public static void Run()
    {
        // настраиваем NLog программно прямо перед запуском таски
        var config = new LoggingConfiguration();
        string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log");
        var fileTarget = new FileTarget("logfile")
        {
            FileName = logFile,
            Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}"
        };
        config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, fileTarget);
        LogManager.Configuration = config;

        var logger = LogManager.GetCurrentClassLogger();

        Console.WriteLine("--- Задание 5.1: Обработка исключений и NLog ---");
        Console.WriteLine("введите 'exit' для возврата в меню.\n");

        while (true)
        {
            Console.Write("введите первое число (делимое): ");
            string? s1 = Console.ReadLine();
            if (string.Equals(s1?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

            Console.Write("введите второе число (делитель): ");
            string? s2 = Console.ReadLine();
            if (string.Equals(s2?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

            // перехватываем возможные исключения ввода и деления
            try
            {
                // парсим числа через int.Parse для провокации FormatException при тексте
                int num1 = int.Parse(s1!);
                int num2 = int.Parse(s2!);

                // выполняем целочисленное деление
                int res = num1 / num2;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"результат деления {num1} / {num2} = {res}\n");
                Console.ResetColor();

                logger.Info($"Успешный расчет {num1} / {num2} = {res}");
            }
            catch (FormatException ex)
            {
                // обрабатываем некорректный ввод букв
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[FormatException] ошибка: введен текст вместо числа!");
                Console.ResetColor();

                logger.Error(ex, "Пользователь ввел некорректный формат числа");
            }
            catch (DivideByZeroException ex)
            {
                // обрабатываем деление на ноль
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[DivideByZeroException] ошибка: деление на ноль запрещено!");
                Console.ResetColor();

                logger.Error(ex, "Попытка деления на ноль");
            }
            catch (Exception ex)
            {
                // ловим общее исключение со стек-трейсом
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"[Общее исключение]: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();

                logger.Fatal(ex, "Критическое общее исключение");
            }

            Console.WriteLine();
        }

        Console.WriteLine($"логи записаны в файл: {logFile}");
    }
}
