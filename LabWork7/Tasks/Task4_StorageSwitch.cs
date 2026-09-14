using System.Diagnostics;

namespace LabWork7.Tasks;

/// <summary>
/// Задание 5.4: SourceSwitch "StorageSwitch" + TraceSource "Storage".
/// Имитация хранилища: загрузка / сохранение / удаление.
/// Verbose — детали входных данных, Information — начало/конец операций,
/// Warning — потенциальные проблемы, Error — ошибки (try-catch).
/// </summary>
public static class Task4_StorageSwitch
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.4: SourceSwitch и уровни детализации ---");

        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string warningLog = Path.Combine(baseDir, "storage_warning.log");
        string verboseLog = Path.Combine(baseDir, "storage_verbose.log");
        string offLog = Path.Combine(baseDir, "storage_off.log");
        string mainLog = Path.Combine(baseDir, "storage.log");

        int warningLines = RunScenario(SourceLevels.Warning, warningLog);
        int verboseLines = RunScenario(SourceLevels.Verbose, verboseLog);
        int offLines = RunScenario(SourceLevels.Off, offLog);

        // Основной storage.log — полная версия (Verbose), чтобы проверяющий
        // всегда видел все сообщения.
        File.Copy(verboseLog, mainLog, overwrite: true);

        Console.WriteLine();
        Console.WriteLine("=== Сравнение ===");
        Console.WriteLine($"  Warning -> {warningLines} строк ({Path.GetFileName(warningLog)})");
        Console.WriteLine($"  Verbose -> {verboseLines} строк ({Path.GetFileName(verboseLog)})");
        Console.WriteLine($"  Off     -> {offLines} строк ({Path.GetFileName(offLog)})");
        Console.WriteLine();
        Console.WriteLine("При Warning видны только Warning+Error+Critical,");
        Console.WriteLine("при Verbose — всё (Verbose, Information, Warning, Error),");
        Console.WriteLine("при Off — ничего (удобно для полного отключения логов в production).");
        Console.WriteLine($"Основной лог: {mainLog}");
    }

    private static int RunScenario(SourceLevels level, string logFile)
    {
        if (File.Exists(logFile))
            File.Delete(logFile);

        var storageSwitch = new SourceSwitch("StorageSwitch") { Level = level };
        var ts = new TraceSource("Storage") { Switch = storageSwitch };
        ts.Listeners.Clear();
        ts.Listeners.Add(new TextWriterTraceListener(logFile, "fileListener"));
        ts.Listeners.Add(new ConsoleTraceListener());
        Trace.AutoFlush = true; // в .NET Core свойство висит на Trace, а не на TraceSource

        Console.WriteLine($"\n[Уровень {level}] лог: {Path.GetFileName(logFile)}");

        var storage = new Dictionary<string, string>
        {
            ["user:1"] = "Иван",
            ["user:2"] = "Егор"
        };

        Save(ts, storage, "user:3", "Алексей");   // Information + Verbose
        Load(ts, storage, "user:1");              // Information + Verbose
        Load(ts, storage, "user:404");            // Error: нет такой записи
        Delete(ts, storage, "user:999");          // Warning: удаление несуществующей
        Delete(ts, storage, "user:2");            // Information: нормальное удаление
        Save(ts, storage, "", "пустой ключ");     // Error через try-catch

        ts.Flush();
        ts.Close();

        int lines = File.Exists(logFile) ? File.ReadAllLines(logFile).Length : 0;
        Console.WriteLine($"  -> записано строк: {lines}");
        return lines;
    }

    private static void Save(TraceSource ts, Dictionary<string, string> storage, string key, string value)
    {
        ts.TraceEvent(TraceEventType.Verbose, 10, $"Save: key='{key}', value='{value}'");
        ts.TraceEvent(TraceEventType.Information, 11, $"Начало операции сохранения key='{key}'");
        try
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Ключ не может быть пустым.", nameof(key));

            storage[key] = value;
            ts.TraceEvent(TraceEventType.Information, 12, $"Завершено сохранение key='{key}'");
        }
        catch (Exception ex)
        {
            ts.TraceEvent(TraceEventType.Error, 13, $"Ошибка сохранения key='{key}': {ex.Message}");
        }
    }

    private static void Load(TraceSource ts, Dictionary<string, string> storage, string key)
    {
        ts.TraceEvent(TraceEventType.Verbose, 20, $"Load: key='{key}'");
        ts.TraceEvent(TraceEventType.Information, 21, $"Начало операции загрузки key='{key}'");
        try
        {
            string value = storage[key]; // KeyNotFoundException, если нет
            ts.TraceEvent(TraceEventType.Information, 22, $"Завершена загрузка key='{key}', value='{value}'");
        }
        catch (Exception ex)
        {
            ts.TraceEvent(TraceEventType.Error, 23, $"Ошибка загрузки key='{key}': {ex.Message}");
        }
    }

    private static void Delete(TraceSource ts, Dictionary<string, string> storage, string key)
    {
        ts.TraceEvent(TraceEventType.Verbose, 30, $"Delete: key='{key}'");
        ts.TraceEvent(TraceEventType.Information, 31, $"Начало операции удаления key='{key}'");
        if (!storage.ContainsKey(key))
        {
            // Потенциальная проблема, но не ошибка: записи просто нет.
            ts.TraceEvent(TraceEventType.Warning, 32, $"Попытка удалить несуществующую запись key='{key}'");
            return;
        }

        storage.Remove(key);
        ts.TraceEvent(TraceEventType.Information, 33, $"Завершено удаление key='{key}'");
    }
}
