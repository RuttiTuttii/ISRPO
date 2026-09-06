namespace LabWork5.Tasks;

public static class Task4_CallStack
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.4: Исследование стека вызовов (Call Stack) ---\n");

        // перехватываем ошибку на верхнем уровне для записи полного пути вызовов
        try
        {
            Console.WriteLine("запускаем цепочку: Run -> MethodA -> MethodB -> MethodC");
            MethodA();
        }
        catch (DivideByZeroException ex)
        {
            // выводим информацию о пойманном исключении
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ПЕРЕХВАЧЕНО ИСКЛЮЧЕНИЕ]: {ex.Message}");
            Console.ResetColor();

            // показываем стек вызовов прямо в консоли
            Console.WriteLine("\nстек вызовов из свойства ex.StackTrace:");
            Console.WriteLine(ex.StackTrace);

            // сохраняем стек в файл stacktrace.txt по заданию 5.4.4
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stacktrace.txt");
            File.WriteAllText(logPath, $"Время: {DateTime.Now}\nТип: {ex.GetType()}\nСообщение: {ex.Message}\nСтек вызовов:\n{ex.StackTrace}");

            // информируем пользователя о пути к файлу
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[ФАЙЛ СОХРАНЕН]: {logPath}");
            Console.ResetColor();
        }
    }

    // первый промежуточный метод цепочки
    private static void MethodA()
    {
        // логируем вход и передаем вызов дальше
        Console.WriteLine("  -> внутри MethodA(), вызываем MethodB()...");
        MethodB();
    }

    // второй промежуточный метод цепочки
    private static void MethodB()
    {
        // логируем вход и вызываем финальный метод
        Console.WriteLine("    -> внутри MethodB(), вызываем MethodC()...");
        MethodC();
    }

    // метод с делением на ноль для демонстрации окна Call Stack
    private static void MethodC()
    {
        // брейкпоинт ставится здесь для просмотра цепочки вызовов в окне Call Stack
        Console.WriteLine("      -> внутри MethodC(): провоцируем деление на 0");
        int numerator = 100;
        int zero = 0;

        // вызываем исключение деления на ноль
        int result = numerator / zero;
        Console.WriteLine($"результат: {result}");
    }
}
