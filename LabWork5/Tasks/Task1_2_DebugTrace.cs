using System.Diagnostics;

namespace LabWork5.Tasks;

/// <summary>
/// Задания 5.1 и 5.2: отладка и трассировка (Debug / Trace).
/// </summary>
public static class Task1_2_DebugTrace
{
    public static void Run()
    {
        // логируем старт подзадачи в трассировку
        Trace.WriteLine("[TRACE] запуск задания 5.1 и 5.2: отладка и трассировка");
        Debug.WriteLine("[DEBUG] инициализация модуля трассировки");

        Console.WriteLine("Сложение двух чисел. Введите 'exit' для выхода.\n");

        // крутим цикл ввода чисел, пока пользователь не решит выйти
        while (true)
        {
            // пишем в дебаг перед запросом первого числа для отслеживания шага
            Debug.WriteLine("[DEBUG] ожидание ввода первого слагаемого");
            Console.Write("введите первое слагаемое (или exit): ");
            string? input1 = Console.ReadLine();

            // проверяем выход из цикла (null — закрытый stdin при pipe)
            if (input1 is null ||
                string.Equals(input1.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
            {
                Trace.WriteLine("[TRACE] пользователь запросил выход из таски");
                break;
            }

            // пробуем преобразовать строку в double
            if (!double.TryParse(input1, out double num1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ошибка: первое значение должно быть числом!");
                Console.ResetColor();
                Debug.WriteLine($"[DEBUG] ошибка парсинга: {input1}");
                continue;
            }

            // пишем в дебаг перед запросом второго слагаемого
            Debug.WriteLine("[DEBUG] первое число получено, ожидание ввода второго слагаемого");
            Console.Write("введите второе слагаемое (или exit): ");
            string? input2 = Console.ReadLine();

            // повторная проверка на команду выхода
            if (input2 is null ||
                string.Equals(input2.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
            {
                Trace.WriteLine("[TRACE] выход по команде exit на втором числе");
                break;
            }

            // парсим второе число
            if (!double.TryParse(input2, out double num2))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ошибка: второе значение должно быть числом!");
                Console.ResetColor();
                Debug.WriteLine($"[DEBUG] ошибка парсинга второго числа: {input2}");
                continue;
            }

            // выполняем сложение и логируем в отладку
            Debug.WriteLine($"[DEBUG] сложение: {num1} + {num2}");
            double sum = num1 + num2;

            // пишем результат в Trace — это сообщение сохраняется даже в Release сборке
            Trace.WriteLine($"[TRACE] расчет выполнен: {num1} + {num2} = {sum}");

            // выводим итоговую сумму в консоль
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"сумма {num1} + {num2} = {sum}\n");
            Console.ResetColor();
        }
    }
}
