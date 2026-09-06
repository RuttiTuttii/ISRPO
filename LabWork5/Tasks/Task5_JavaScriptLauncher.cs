using System.Diagnostics;

namespace LabWork5.Tasks;

public static class Task5_JavaScriptLauncher
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.5: Отладка JavaScript в браузере ---\n");

        // вычисляем путь к html-файлу с фронтендом для отладки
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string htmlPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "Task5_JavaScript", "index.html"));

        // если файл найден — открываем его в браузере по умолчанию
        if (File.Exists(htmlPath))
        {
            Console.WriteLine($"открываем файл в браузере: {htmlPath}");
            Console.WriteLine("не забудьте нажать F12 и открыть вкладку Sources и Console!\n");

            try
            {
                // открываем через системный shell
                Process.Start(new ProcessStartInfo
                {
                    FileName = htmlPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"не удалось автоматически запустить браузер: {ex.Message}");
                Console.WriteLine($"вы можете открыть файл вручную: {htmlPath}");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"файл не найден по пути: {htmlPath}");
            Console.ResetColor();
        }
    }
}
