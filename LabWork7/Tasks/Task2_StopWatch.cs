using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace LabWork7.Tasks;

public static class Task2_StopWatch
{
    private static readonly HttpClient client = new();
    private static readonly string logPath = "timings.log";

    public static async Task Run()
    {
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

        Console.WriteLine("--- Задание 5.2: NLog с StopWatch ---");
        Console.WriteLine("введите 'exit' для возврата в меню.\n");

        var path = "numbers.txt";

        if (!File.Exists(path))
        {
            Console.WriteLine("Генерация большого файла для теста...");
            using var writer = new StreamWriter(path);
            for (int i = 0; i < 1_000_000; i++)
            {
                writer.WriteLine(i % 2 == 0 ? i.ToString() : $"Odd{i}");
            }
            Console.WriteLine("Файл сгенерирован.");
        }

        if (File.Exists(logPath))
        {
            File.Delete(logPath);
        }

        while (true)
        {
            Console.Write("введите 4 для запуска бенчмарка: ");
            string? s1 = Console.ReadLine();
            if (string.Equals(s1?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

            try
            {
                if (!int.TryParse(s1, out int case1))
                {
                    Console.WriteLine("Введите цифру 1, 2, 3 или 4.");
                    continue;
                }

                switch (case1)
                {
                    case 1:
                        Console.WriteLine("\n--- Чтение через try-finally ---");
                        StreamReader? manualReader = null;
                        try
                        {
                            manualReader = new StreamReader(path);
                            string? line;
                            int evenCount = 0;
                            while ((line = manualReader.ReadLine()) != null)
                            {
                                if (int.TryParse(line, out var n) && n % 2 == 0)
                                {
                                    evenCount++;
                                    Console.WriteLine($"Четное: {n}");
                                }
                            }
                            Console.WriteLine($"Всего четных найдено: {evenCount}");
                        }
                        finally
                        {
                            manualReader?.Dispose();
                            Console.WriteLine("Файл гарантированно закрыт в блоке finally");
                        }
                        break;

                    case 2:
                        try
                        {
                            string url = "https://yandex.ru/pogoda/ru/arhangelsk";
                            string response = await client.GetStringAsync(url);
                            Console.WriteLine($"Длина ответа: {response.Length}");
                            Console.WriteLine(response.Substring(0, Math.Min(response.Length, 200)));
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"Ошибка HTTP: {ex.Message}");
                            logger.Error(ex, "Ошибка запроса к погоде");
                        }
                        break;

                    case 3:
                        HeavyMath.StartMath();
                        break;

                    case 4:
                        await RunBenchmark(path, logger);
                        break;

                    default:
                        Console.WriteLine("Неверный выбор. Введите 1, 2, 3 или 4.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ввод должен быть числом!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                logger.Error(ex, "Ошибка в главном цикле");
            }
        }
    }

    private static string FormatElapsed(double microseconds)
    {
        if (microseconds < 1000)
            return $"{microseconds:F1} microseconds";
        return $"{microseconds / 1000:F3} miliseconds";
    }

    private static async Task RunBenchmark(string filePath, NLog.Logger logger)
    {
        Console.WriteLine("\n=== Запуск бенчмарка: 3 операции x 3 раза ===\n");

        var totalStopwatch = new Stopwatch();
        totalStopwatch.Start();

        var operations = new (string Name, Func<Task> Action)[]
        {
            ("ReadTxtFile", async () =>
            {
                int evenCount = 0;
                using var reader = new StreamReader(filePath);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line, out var n))
                    {
                        if (n % 2 == 0) evenCount++;
                    }
                }
                _ = evenCount;
            }),
            ("ApiRequest", async () =>
            {
                try
                {
                    string url = "https://yandex.ru/pogoda/ru/arhangelsk";
                    string response = await client.GetStringAsync(url);
                    _ = response.Length;
                }
                catch (HttpRequestException ex)
                {
                    logger.Error(ex, "Ошибка при бенчмарке API");
                }
            }),
            ("HeavyMath", async () =>
            {
                await Task.Run(() => HeavyMath.StartMath());
            })
        };

        var averages = new List<(string Name, double AvgUs)>();

        foreach (var (name, action) in operations)
        {
            double totalUs = 0;

            for (int i = 0; i < 3; i++)
            {
                var sw = Stopwatch.StartNew();
                await action();
                sw.Stop();
                double elapsedUs = sw.Elapsed.TotalMicroseconds;
                totalUs += elapsedUs;

                string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={name}, Elapsed={FormatElapsed(elapsedUs)}";
                Debug.WriteLine(entry);
                File.AppendAllText(logPath, entry + Environment.NewLine, Encoding.UTF8);
                Console.WriteLine(entry);
            }

            double avgUs = totalUs / 3.0;
            averages.Add((name, avgUs));
            string avgEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={name}_Average, Elapsed={FormatElapsed(avgUs)}";
            Debug.WriteLine(avgEntry);
            File.AppendAllText(logPath, avgEntry + Environment.NewLine, Encoding.UTF8);
            Console.WriteLine(avgEntry);
            Console.WriteLine();
        }

        totalStopwatch.Stop();
    }
}
