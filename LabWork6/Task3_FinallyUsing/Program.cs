Console.WriteLine("=== ЛР №6. Задание 5.3: Использование finally и using ===\n");

string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "numbers.txt");

// создаем тестовый файл с числами, если его еще нет, чтобы было что читать
if (!File.Exists(filePath))
{
    File.WriteAllText(filePath, "12\n7\n44\n3\n18\n99\n100\n25\n8");
    Console.WriteLine($"[ИНФО] создан тестовый файл {filePath} с числами\n");
}

Console.WriteLine("1 - чтение четных чисел через using (гарантированное освобождение IDisposable)");
Console.WriteLine("2 - чтение четных чисел через try-finally (явное закрытие ресурса)");
Console.WriteLine("3 - спровоцировать FileNotFoundException (попытка открыть несуществующий файл)");
Console.Write("выберите вариант (1-3): ");
string? choice = Console.ReadLine();

// отрабатываем выбранный сценарий
switch (choice?.Trim())
{
    case "1":
        // читаем файл через современную конструкцию using
        ReadEvenNumbersWithUsing(filePath);
        break;
    case "2":
        // читаем файл через классический блок try-finally
        ReadEvenNumbersWithFinally(filePath);
        break;
    case "3":
        // намеренно передаем путь к фейковому файлу для проверки обработки FileNotFoundException
        ReadEvenNumbersWithUsing("non_existent_file_12345.txt");
        break;
    default:
        Console.WriteLine("неверный выбор");
        break;
}

// метод чтения файла с четными числами с помощью конструкции using
static void ReadEvenNumbersWithUsing(string path)
{
    Console.WriteLine($"\n[USING] открываем файл: {path}");

    // заворачиваем операцию в try-catch для обработки отсутствия файла
    try
    {
        // конструкция using гарантированно вызовет Dispose() и закроет файл даже при ошибке
        using (var reader = new StreamReader(path))
        {
            Console.WriteLine("четные числа из файла:");
            string? line;
            // читаем файл построчно до самого конца
            while ((line = reader.ReadLine()) != null)
            {
                // парсим строку в число и проверяем на четность
                if (int.TryParse(line.Trim(), out int num) && num % 2 == 0)
                {
                    Console.WriteLine($"  -> четное число: {num}");
                }
            }
        } // вот тут поток reader автоматически закрывается

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[USING] файл успешно прочитан и автоматически закрыт через using\n");
        Console.ResetColor();
    }
    catch (FileNotFoundException ex)
    {
        // ловим отсутствие файла по заданию 5.3.2
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ОШИБКА FileNotFoundException]: файл '{ex.FileName}' не найден на диске!\n");
        Console.ResetColor();
    }
}

// метод чтения файла с явным закрытием ресурса в блоке finally
static void ReadEvenNumbersWithFinally(string path)
{
    Console.WriteLine($"\n[FINALLY] открываем файл через FileStream / StreamReader: {path}");
    StreamReader? reader = null;

    try
    {
        // открываем поток вручную
        reader = new StreamReader(path);
        Console.WriteLine("четные числа из файла (блок try):");
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (int.TryParse(line.Trim(), out int num) && num % 2 == 0)
            {
                Console.WriteLine($"  -> четное число: {num}");
            }
        }
    }
    catch (FileNotFoundException ex)
    {
        // перехватываем ошибку отсутствия файла
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ОШИБКА]: файл не найден: {ex.Message}");
        Console.ResetColor();
    }
    finally
    {
        // блок finally выполнится абсолютно всегда, гарантируя закрытие файла
        if (reader != null)
        {
            reader.Close();
            reader.Dispose();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[FINALLY] блок finally сработал: файл гарантированно закрыт!");
            Console.ResetColor();
        }
    }
}
