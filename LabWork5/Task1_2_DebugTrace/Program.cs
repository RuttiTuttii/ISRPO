using System.Diagnostics;

// врубаем трассировку сразу на старте программы, чтобы отследить момент запуска
Trace.WriteLine("[TRACE] программа Task1_2_DebugTrace успешно запущена");
Debug.WriteLine("[DEBUG] инициализация приложения в дебаг-режиме");

Console.WriteLine("=== ЛР №5. Задания 5.1 и 5.2: Отладка и трассировка ===");
Console.WriteLine("введите 'exit' в любой момент для выхода из программы\n");

// крутим бесконечный цикл расчетов, пока юзеру не надоест и он не напишет exit
while (true)
{
    // логируем перед запросом первого числа, чтобы в окне вывода было видно начало итерации
    Debug.WriteLine("[DEBUG] подготовка к запросу первого слагаемого");
    Console.Write("введите первое слагаемое (или exit): ");
    string? input1 = Console.ReadLine();

    // проверяем не захотел ли юзер ливнуть досрочно
    if (string.Equals(input1?.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
    {
        Trace.WriteLine("[TRACE] пользователь ввел exit на первом шаге, завершаем работу");
        break;
    }

    // пытаемся спарсить первое число, а если там мусор — ругаемся и идем на новый круг
    if (!double.TryParse(input1, out double num1))
    {
        Console.WriteLine("ошибка: нужно ввести корректное число!");
        Debug.WriteLine($"[DEBUG] неудачный парсинг первого числа: {input1}");
        continue;
    }

    // логируем процесс перед вторым вводом
    Debug.WriteLine("[DEBUG] первое число успешно получено, запрашиваем второе слагаемое");
    Console.Write("введите второе слагаемое (или exit): ");
    string? input2 = Console.ReadLine();

    // еще раз проверяем на команду выхода, вдруг передумал
    if (string.Equals(input2?.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
    {
        Trace.WriteLine("[TRACE] выход по команде exit на втором числе");
        break;
    }

    // парсим второе число с такой же проверкой от дурака
    if (!double.TryParse(input2, out double num2))
    {
        Console.WriteLine("ошибка: второе слагаемое тоже должно быть числом!");
        Debug.WriteLine($"[DEBUG] косяк при парсинге второго числа: {input2}");
        continue;
    }

    // логируем момент сложения для отладки перед тем как посчитать
    Debug.WriteLine($"[DEBUG] выполняем сложение {num1} + {num2}");
    double sum = num1 + num2;

    // пишем в трейс итоговый результат — это сообщение останется даже в релизной сборке
    Trace.WriteLine($"[TRACE] успешный расчет: {num1} + {num2} = {sum}");

    // выводим красивый ответ пользователю в консоль
    Console.WriteLine($"сумма чисел {num1} и {num2} = {sum}\n");
}

// финальное сообщение о завершении программы для трейса
Trace.WriteLine("[TRACE] работа программы штатно завершена");
Console.WriteLine("программа завершила работу.");
