namespace LabWork6.Tasks;

public static class Task2_CustomException
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.2: Пользовательские исключения ---");
        Console.WriteLine("проверка возраста с кастомным классом NegativeNumberException\n");

        while (true)
        {
            Console.Write("введите ваш возраст (или 'exit' для выхода): ");
            string? input = Console.ReadLine();
            if (string.Equals(input?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

            // перехватываем наше кастомное исключение
            try
            {
                if (!int.TryParse(input, out int age))
                {
                    Console.WriteLine("ошибка: возраст должен быть целым числом!\n");
                    continue;
                }

                // вызываем метод проверки бизнес-правила
                ValidateAge(age);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"возраст {age} успешно прошел валидацию!\n");
                Console.ResetColor();
            }
            catch (NegativeNumberException ex)
            {
                // выводим понятное сообщение из конструктора нашего исключения
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ПЕРЕХВАЧЕНО NegativeNumberException]: {ex.Message}\n");
                Console.ResetColor();
            }
        }
    }

    // метод валидации возраста, выбрасывающий пользовательское исключение
    private static void ValidateAge(int age)
    {
        // если возраст отрицательный — бросаем наше кастомное исключение с текстом
        if (age < 0)
        {
            throw new NegativeNumberException($"Введен недопустимый отрицательный возраст: {age}. Возраст не может быть меньше 0!");
        }
    }
}

// класс пользовательского исключения с поддержкой передачи message
public class NegativeNumberException : Exception
{
    public NegativeNumberException() : base("Значение не может быть отрицательным.") { }
    public NegativeNumberException(string message) : base(message) { }
    public NegativeNumberException(string message, Exception innerException) : base(message, innerException) { }
}
