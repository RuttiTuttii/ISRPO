Console.WriteLine("=== ЛР №5. Задание 5.4: Исследование стека вызовов (Call Stack) ===\n");

// оборачиваем корневой запуск в try-catch, чтобы поймать ошибку из глубины стека и залогировать
try
{
    // начинаем цепочку вызовов с первого метода
    Console.WriteLine("запуск цепочки вызовов: Main -> MethodA -> MethodB -> MethodC");
    MethodA();
}
catch (DivideByZeroException ex)
{
    // радостно перехватываем наше искусственное деление на ноль
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n[ПЕРЕХВАЧЕНО ИСКЛЮЧЕНИЕ]: {ex.Message}");
    Console.ResetColor();

    // выводим стек вызовов прямо в консоль для наглядности
    Console.WriteLine("\nстек вызовов из объекта исключения (ex.StackTrace):");
    Console.WriteLine(ex.StackTrace);

    // сохраняем стек вызовов в текстовый файл, как строго требует пункт 5.4.4 методички
    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stacktrace.txt");
    File.WriteAllText(logPath, $"Время ошибки: {DateTime.Now}\nТип: {ex.GetType()}\nСообщение: {ex.Message}\nСтек вызовов:\n{ex.StackTrace}");
    
    // сообщаем юзеру куда именно лег сохраненный файл со стеком
    Console.WriteLine($"\n[ИНФО] стек вызовов успешно сохранен в файл: {logPath}");
}

// первый промежуточный метод цепочки
static void MethodA()
{
    // логируем вход и сразу передаем эстафету следующему методу
    Console.WriteLine("-> внутри MethodA(), вызываем MethodB()...");
    MethodB();
}

// второй промежуточный метод цепочки
static void MethodB()
{
    // логируем вход и вызываем финальный метод, где спрятана засада
    Console.WriteLine("  -> внутри MethodB(), вызываем MethodC()...");
    MethodC();
}

// метод на самом дне стека, в котором намеренно бросается исключение
static void MethodC()
{
    // тут ставится брейкпоинт по заданию 5.4.3 для просмотра окна Call Stack
    Console.WriteLine("    -> внутри MethodC(): готовимся делить на ноль!");
    int numerator = 42;
    int denominator = 0;

    // провоцируем честное DivideByZeroException на уровне рантайма
    int result = numerator / denominator;
    Console.WriteLine($"результат: {result}");
}
