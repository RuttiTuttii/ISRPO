namespace LabWork6.Tasks;

public static class Task3_FinallyUsing
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 5.3: Finally и Using ===");
        var path = "numbers.txt";

        // создаем тестовый файл если его еще нет
        if (!File.Exists(path))
        {
            File.WriteAllLines(path, ["12", "7", "44", "3", "18", "99", "100"]);
        }

        // 1. чтение через using
        Console.WriteLine("\n--- Чтение через using ---");
        try
        {
            // using автоматически закроет файл при выходе из блока
            using var reader = new StreamReader(path);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (int.TryParse(line, out var n) && n % 2 == 0)
                {
                    Console.WriteLine($"Четное: {n}");
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // 2. чтение через try-finally с явным закрытием
        Console.WriteLine("\n--- Чтение через try-finally ---");
        StreamReader? manualReader = null;
        try
        {
            manualReader = new StreamReader(path);
            string? line;
            while ((line = manualReader.ReadLine()) != null)
            {
                if (int.TryParse(line, out var n) && n % 2 == 0)
                {
                    Console.WriteLine($"Четное: {n}");
                }
            }
        }
        finally
        {
            // в блоке finally гарантированно закрываем ресурс
            manualReader?.Dispose();
            Console.WriteLine("Файл гарантированно закрыт в блоке finally");
        }
    }
}
