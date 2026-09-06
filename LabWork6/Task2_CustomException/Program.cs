Console.WriteLine("=== ЛР №6. Задание 5.2: Пользовательские исключения ===\n");
Console.WriteLine("проверка возраста пользователя с кастомным классом NegativeNumberException\n");

// крутим цикл проверки возраста
while (true)
{
    Console.Write("введите ваш возраст в годах (или 'exit' для выхода): ");
    string? input = Console.ReadLine();

    // проверяем команду выхода
    if (string.Equals(input?.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;

    // оборачиваем валидацию возраста в блок обработки исключений
    try
    {
        // парсим строку в число
        if (!int.TryParse(input, out int age))
        {
            Console.WriteLine("ошибка: возраст должен быть целым числом!\n");
            continue;
        }

        // проверяем бизнес-правило через отдельный метод валидации
        ValidateAge(age);

        // если всё ок — поздравляем пользователя
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"возраст {age} успешно принят системой!\n");
        Console.ResetColor();
    }
    catch (NegativeNumberException ex)
    {
        // перехватываем наше кастомное исключение и выводим понятный юзеру текст из ex.Message
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ПОЙМАНО NegativeNumberException]: {ex.Message}\n");
        Console.ResetColor();
    }
}

Console.WriteLine("работа завершена.");

// метод проверки возраста, который бросает наше кастомное исключение при отрицательном числе
static void ValidateAge(int age)
{
    // проверяем на отрицательное значение по заданию 5.2.1
    if (age < 0)
    {
        // создаем объект нашего исключения и передаем туда понятное сообщение в конструктор
        throw new NegativeNumberException($"Введен отрицательный возраст: {age}. Возраст не может быть меньше нуля!");
    }
}

// пользовательский дочерний класс исключения для проверки отрицательных чисел
public class NegativeNumberException : Exception
{
    // дефолтный конструктор с базовым сообщением
    public NegativeNumberException() : base("Значение не может быть отрицательным.") { }

    // основной конструктор, принимающий понятное сообщение об ошибке (п. 5.2.2)
    public NegativeNumberException(string message) : base(message) { }

    // конструктор с поддержкой внутреннего исключения для полноты иерархии
    public NegativeNumberException(string message, Exception innerException) : base(message, innerException) { }
}
