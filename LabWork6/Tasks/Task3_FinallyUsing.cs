namespace LabWork6.Tasks;

public static class Task3_FinallyUsing
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.3: Использование finally и using ---\n");

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "numbers.txt");

        // создаем файл с тестовыми числами если его еще нет
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "12\n7\n44\n3\n18\n99\n100\n25\n8");
            Console.WriteLine($"[ИНФО] создан файл {filePath} с тестовыми данными\n");
        }

        Console.WriteLine("1 - чтение четных чисел через using (автоматический Dispose)");
        Console.WriteLine("2 - чтение четных чисел через try-finally (явное закрытие файла)");
        Console.WriteLine("3 - тест обработки FileNotFoundException");
        Console.Write("выберите вариант (1-3): ");

        string? choice = Console.ReadLine();

        // запускаем выбранный способ чтения
        switch (choice?.Trim())
        {
            case "1":
                ReadWithUsing(filePath);
                break;
            case "2":
                ReadWithFinally(filePath);
                break;
            case "3":
                ReadWithUsing("non_existent_file.txt");
                break;
            default:
                Console.WriteLine("неверный выбор");
                break;
        }
    }

    // считывание файла через конструкцию using
    private static void ReadWithUsing(string path)
    {
        Console.WriteLine($"\n[USING] открытие файла: {path}");

        try
        {
            // using гарантирует закрытие ресурса при любом исходе
            using (var reader = new StreamReader(path))
            {
                Console.WriteLine("найденные четные числа:");
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line.Trim(), out int num) && num % 2 == 0)
                    {
                        Console.WriteLine($"  -> четное: {num}");
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[USING] файл успешно прочитан и освобожден через using");
            Console.ResetColor();
        }
        catch (FileNotFoundException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[FileNotFoundException]: файл '{ex.FileName}' отсутствует на диске!");
            Console.ResetColor();
        }
    }

    // считывание файла через явный блок finally
    private static void ReadWithFinally(string path)
    {
        Console.WriteLine($"\n[FINALLY] открытие файла через StreamReader: {path}");
        StreamReader? reader = null;

        try
        {
            reader = new StreamReader(path);
            Console.WriteLine("найденные четные числа:");
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (int.TryParse(line.Trim(), out int num) && num % 2 == 0)
                {
                    Console.WriteLine($"  -> четное: {num}");
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ОШИБКА]: файл не найден: {ex.Message}");
            Console.ResetColor();
        }
        finally
        {
            // блок finally выполнится абсолютно всегда
            if (reader != null)
            {
                reader.Close();
                reader.Dispose();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[FINALLY] блок finally гарантированно закрыл файловый дескриптор!");
                Console.ResetColor();
            }
        }
    }
}
