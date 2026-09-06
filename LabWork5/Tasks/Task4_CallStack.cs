namespace LabWork5.Tasks;

public static class Task4_CallStack
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 5.4: Call Stack ===");

        // ловим деление на ноль на верхнем уровне стека
        try
        {
            MethodA();
        }
        catch (DivideByZeroException ex)
        {
            // выводим стек вызовов в консоль
            Console.WriteLine($"\n[Поймано исключение]: {ex.Message}");
            Console.WriteLine("Стек вызовов:");
            Console.WriteLine(ex.StackTrace);

            // сохраняем стек вызовов в файл по заданию 5.4.4
            File.WriteAllText("stacktrace.txt", ex.StackTrace);
            Console.WriteLine("\nСтек вызовов успешно сохранен в stacktrace.txt");
        }
    }

    private static void MethodA()
    {
        // просто зовем следующий метод
        MethodB();
    }

    private static void MethodB()
    {
        // отсюда вызываем метод на дне стека
        MethodC();
    }

    private static void MethodC()
    {
        // тут ставим брейкпоинт и провоцируем ошибку
        int x = 100;
        int y = 0;
        int z = x / y;
        Console.WriteLine(z);
    }
}
