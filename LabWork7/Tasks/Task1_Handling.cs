namespace LabWork7.Tasks;

/// <summary>
/// Задание 5.1: логирование исключений в файл log.txt через File.AppendAllText.
/// </summary>
public static class Task1_Handling
{
    private static readonly string LogPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

    public static void Run()
    {
        Console.WriteLine("--- Задание 5.1: Логирование исключений в файл ---");
        Console.WriteLine("Калькулятор: сложение, вычитание, умножение, деление.");
        Console.WriteLine("Введите 'exit' для выхода.\n");

        while (true)
        {
            Console.Write("Введите первое целое число: ");
            string? s1 = Console.ReadLine();
            if (s1 is null || IsExit(s1)) break;

            Console.Write("Введите второе целое число: ");
            string? s2 = Console.ReadLine();
            if (s2 is null || IsExit(s2)) break;

            try
            {
                // int.Parse провоцирует FormatException (текст) и
                // OverflowException (число вне диапазона Int32).
                int a = int.Parse(s1!);
                int b = int.Parse(s2!);

                // checked нужен, чтобы переполнение арифметики
                // (например, 2000000000 + 2000000000) бросало OverflowException,
                // а не молча заворачивалось.
                checked
                {
                    Console.WriteLine($"  {a} + {b} = {a + b}");
                    Console.WriteLine($"  {a} - {b} = {a - b}");
                    Console.WriteLine($"  {a} * {b} = {a * b}");
                    Console.WriteLine($"  {a} / {b} = {a / b}");
                }
            }
            catch (FormatException ex)
            {
                Report(ex, "Введен текст вместо числа.");
            }
            catch (OverflowException ex)
            {
                Report(ex, "Число вне диапазона Int32 или переполнение результата.");
            }
            catch (DivideByZeroException ex)
            {
                Report(ex, "Деление на ноль запрещено.");
            }
            catch (Exception ex)
            {
                // fallback для всего остального (ArgumentNullException и т.д.)
                Report(ex, "Непредвиденная ошибка.");
            }

            Console.WriteLine();
        }

        Console.WriteLine($"Лог исключений: {LogPath}");
    }

    private static bool IsExit(string? s) =>
        string.Equals(s?.Trim(), "exit", StringComparison.OrdinalIgnoreCase);

    private static void Report(Exception ex, string friendlyMessage)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[{ex.GetType().Name}] {friendlyMessage}");
        Console.WriteLine(ex.Message);
        Console.ResetColor();

        // Образец из методички:
        // [2026-09-10 14:23:11] FormatException: Input string was not in a correct format
        // + полный текст исключения ex.ToString()
        string entry =
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {ex.GetType().Name}: {ex.Message}{Environment.NewLine}" +
            $"{ex}{Environment.NewLine}" +
            $"---{Environment.NewLine}";

        File.AppendAllText(LogPath, entry);
    }
}
