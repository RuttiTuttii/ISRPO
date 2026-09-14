using System.Diagnostics;
using System.Text;

namespace LabWork7.Tasks;

/// <summary>
/// Задание 5.2: Stopwatch + Debug.WriteLine + timings.log.
/// Три операции (чтение файла, запрос по API, мат. расчеты),
/// каждая по 3 раза, среднее по каждой + общее время.
/// </summary>
public static class Task2_StopWatch
{
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(15) };
    private static readonly string LogPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "timings.log");

    public static async Task Run()
    {
        Console.WriteLine("--- Задание 5.2: Stopwatch и timings.log ---");

        // Готовим файл для операции чтения: numbers.txt из проекта слишком
        // маленький (7 строк), поэтому для честного замера генерируем
        // bench-файл на 200 тыс. строк и меряем его.
        string dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bench_numbers.txt");
        if (!File.Exists(dataFile))
        {
            Console.WriteLine("Генерация файла для теста чтения (200 000 строк)...");
            using var w = new StreamWriter(dataFile, false, Encoding.UTF8);
            for (int i = 0; i < 200_000; i++)
                await w.WriteLineAsync(i % 2 == 0 ? i.ToString() : $"Odd{i}");
        }

        // Чистим лог перед замером, чтобы средние считались по текущему запуску.
        if (File.Exists(LogPath))
            File.Delete(LogPath);

        var totalSw = Stopwatch.StartNew();

        var results = new List<(string Name, double AvgMs)>
        {
            ("ReadTxtFile", await MeasureAsync("ReadTxtFile", 3, () =>
            {
                int evenCount = 0;
                using var reader = new StreamReader(dataFile);
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line, out int n) && n % 2 == 0)
                        evenCount++;
                }
                return Task.FromResult(evenCount);
            })),
            ("ApiRequest", await MeasureAsync("ApiRequest", 3, async () =>
            {
                try
                {
                    // Легкий публичный API; при отсутствии сети фиксируем ошибку в лог.
                    string response = await Client.GetStringAsync("https://jsonplaceholder.typicode.com/posts/1");
                    return response.Length;
                }
                catch (Exception ex)
                {
                    string err = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation=ApiRequest, Error={ex.GetType().Name}: {ex.Message}";
                    Debug.WriteLine(err);
                    File.AppendAllText(LogPath, err + Environment.NewLine, Encoding.UTF8);
                    Console.WriteLine("  API недоступно: " + ex.Message);
                    return -1;
                }
            })),
            ("HeavyMath", await MeasureAsync("HeavyMath", 3, () =>
            {
                double r = HeavyMath.Compute(1_000_000);
                return Task.FromResult(r);
            })),
        };

        totalSw.Stop();

        string totalEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation=TOTAL, Elapsed={(long)totalSw.Elapsed.TotalMilliseconds} ms";
        Debug.WriteLine(totalEntry);
        File.AppendAllText(LogPath, totalEntry + Environment.NewLine, Encoding.UTF8);

        Console.WriteLine();
        Console.WriteLine("=== Итоги (среднее по 3 запускам) ===");
        foreach (var (name, avg) in results)
            Console.WriteLine($"  {name}: {(long)avg} ms");
        Console.WriteLine($"  TOTAL: {(long)totalSw.Elapsed.TotalMilliseconds} ms");
        Console.WriteLine();
        Console.WriteLine("Вывод: обычно дольше всего ApiRequest (сеть + ожидание ответа),");
        Console.WriteLine("затем HeavyMath (CPU), быстрее всего ReadTxtFile (локальный диск).");
        Console.WriteLine($"Лог: {LogPath}");
    }

    private static async Task<double> MeasureAsync<T>(string operation, int repeats, Func<Task<T>> action)
    {
        double totalMs = 0;

        for (int i = 0; i < repeats; i++)
        {
            var sw = Stopwatch.StartNew();
            _ = await action();
            sw.Stop();

            double ms = sw.Elapsed.TotalMilliseconds;
            totalMs += ms;

            // Формат строго по методичке: [дата] Operation=..., Elapsed=123 ms
            string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={operation}, Elapsed={(long)ms} ms";
            Debug.WriteLine(entry); // видно в окне Output -> Debug при отладке
            File.AppendAllText(LogPath, entry + Environment.NewLine, Encoding.UTF8);
            Console.WriteLine($"  {entry}");
        }

        double avg = totalMs / repeats;
        string avgEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Operation={operation}_Average, Elapsed={(long)avg} ms";
        Debug.WriteLine(avgEntry);
        File.AppendAllText(LogPath, avgEntry + Environment.NewLine, Encoding.UTF8);
        Console.WriteLine($"  {avgEntry}");

        return avg;
    }
}
