using System.Diagnostics;

namespace LabWork7.Tasks;

/// <summary>
/// Задание 5.3: TraceSource "Calculator" + TextWriterTraceListener (trace.log)
/// + ConsoleTraceListener. Уровни Verbose / Information / Error.
/// </summary>
public static class Task3_TraceSourceCalculator
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.3: TraceSource и TraceListener ---");

        Console.Write("Введите первое целое число: ");
        if (!int.TryParse(Console.ReadLine(), out int num1))
        {
            Console.WriteLine("Некорректное первое число.");
            return;
        }

        Console.Write("Введите второе целое число: ");
        if (!int.TryParse(Console.ReadLine(), out int num2))
        {
            Console.WriteLine("Некорректное второе число.");
            return;
        }

        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string tracePath = Path.Combine(baseDir, "trace.log");
        string verbosePath = Path.Combine(baseDir, "trace_verbose.log");

        // Проход 1: уровень Information — Verbose отсекается (п. 5.3.5).
        RunCalculatorPass(num1, num2, SourceLevels.Information, tracePath);

        // Проход 2: уровень Verbose — в лог попадает всё (п. 5.3.5).
        RunCalculatorPass(num1, num2, SourceLevels.Verbose, verbosePath);

        Console.WriteLine();
        Console.WriteLine($"Готово. Сравните файлы:{Environment.NewLine}  {tracePath}{Environment.NewLine}  {verbosePath}");
        Console.WriteLine("В trace.log строк Verbose нет, в trace_verbose.log — есть.");
    }

    private static void RunCalculatorPass(int num1, int num2, SourceLevels level, string logFile)
    {
        if (File.Exists(logFile))
            File.Delete(logFile);

        var ts = new TraceSource("Calculator")
        {
            Switch = new SourceSwitch("CalculatorSwitch") { Level = level }
        };

        ts.Listeners.Clear(); // убираем DefaultTraceListener, чтобы не дублировать в Output
        ts.Listeners.Add(new TextWriterTraceListener(logFile, "fileListener"));
        ts.Listeners.Add(new ConsoleTraceListener());
        Trace.AutoFlush = true; // в .NET Core свойство висит на Trace, а не на TraceSource

        Console.WriteLine($"\n[Уровень {level}] лог: {Path.GetFileName(logFile)}");

        // Входные параметры — Verbose (при Information в файл не попадут).
        ts.TraceEvent(TraceEventType.Verbose, 1, $"Входные параметры: num1 = {num1}, num2 = {num2}");

        try
        {
            checked
            {
                int sum = num1 + num2;
                ts.TraceInformation($"Выполнено сложение: {num1} + {num2} = {sum}");
                Console.WriteLine($"Сумма: {sum}");

                int diff = num1 - num2;
                ts.TraceInformation($"Выполнено вычитание: {num1} - {num2} = {diff}");
                Console.WriteLine($"Разность: {diff}");

                int mul = num1 * num2;
                ts.TraceInformation($"Выполнено умножение: {num1} * {num2} = {mul}");
                Console.WriteLine($"Произведение: {mul}");

                if (num2 == 0)
                {
                    const string msg = "Ошибка: деление на ноль.";
                    Console.WriteLine(msg);
                    ts.TraceEvent(TraceEventType.Error, 2, msg);
                }
                else
                {
                    int div = num1 / num2;
                    ts.TraceInformation($"Выполнено деление: {num1} / {num2} = {div}");
                    Console.WriteLine($"Частное: {div}");
                }
            }
        }
        catch (OverflowException ex)
        {
            ts.TraceEvent(TraceEventType.Error, 3, $"Переполнение: {ex.Message}");
            Console.WriteLine($"Переполнение: {ex.Message}");
        }
        catch (Exception ex)
        {
            ts.TraceEvent(TraceEventType.Error, 4, $"Непредвиденная ошибка: {ex.Message}");
            Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
        }
        finally
        {
            ts.Flush();
            ts.Close(); // после Close() повторная запись теряется — это ожидаемо

            Console.WriteLine($"Записано в {Path.GetFileName(logFile)}.");
        }
    }
}
