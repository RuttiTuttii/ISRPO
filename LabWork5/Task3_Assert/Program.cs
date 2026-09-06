using System.Diagnostics;

Console.WriteLine("=== ЛР №5. Задание 5.3: Использование Debug.Assert ===\n");

// вызываем метод с полностью валидными данными, тут ассерты должны молча пройти
Console.WriteLine("--- Тест 1: Корректный расчет (цена: 1000, скидка: 15%) ---");
double validResult = CalculateDiscount(1000, 0.15);
Console.WriteLine($"цена со скидкой: {validResult} руб.\n");

// даем пользователю выбор: проверить еще один валидный или словить Debug.Assert
Console.WriteLine("выберите сценарий для проверки:");
Console.WriteLine("1 - еще один корректный расчет (цена 500, скидка 30%)");
Console.WriteLine("2 - спровоцировать Debug.Assert (отрицательная цена: -100)");
Console.WriteLine("3 - спровоцировать Debug.Assert (невалидная скидка: 1.5 / 150%)");
Console.Write("ваш выбор (1-3): ");
string? choice = Console.ReadLine();

// отрабатываем выбранный сценарий для демонстрации срабатывания окна ассерта
switch (choice?.Trim())
{
    case "1":
        // рассчитываем штатную скидку 30%
        double res = CalculateDiscount(500, 0.30);
        Console.WriteLine($"результат: {res} руб.");
        break;
    case "2":
        // намеренно передаем отрицательную цену, чтобы выстрелил первый ассерт
        Console.WriteLine("вызываем CalculateDiscount(-100, 0.1)... сейчас сработает Debug.Assert!");
        CalculateDiscount(-100, 0.1);
        break;
    case "3":
        // намеренно передаем скидку больше 1, чтобы выстрелил второй ассерт
        Console.WriteLine("вызываем CalculateDiscount(200, 1.5)... сейчас сработает Debug.Assert!");
        CalculateDiscount(200, 1.5);
        break;
    default:
        // если ввели что-то левое, просто делаем стандартный вызов
        Console.WriteLine("неизвестный выбор, завершаем работу.");
        break;
}

// метод расчета скидки с обязательными контрактными проверками через Debug.Assert
static double CalculateDiscount(double price, double discountRate)
{
    // проверяем через ассерт, что начальная цена строго больше нуля
    Debug.Assert(price > 0, $"цена товара должна быть строго положительной, а передано {price}");

    // проверяем через ассерт, что скидка лежит в допустимом диапазоне от 0 до 1 (0%..100%)
    Debug.Assert(discountRate > 0 && discountRate < 1, $"коэффициент скидки должен быть в диапазоне (0, 1), а передано {discountRate}");

    // считаем итоговую сумму с учетом скидки (отнимаем процент от стоимости)
    double discountedPrice = price * (1.0 - discountRate);

    // проверяем финальный инвариант: итоговая цена не может превышать исходную
    Debug.Assert(discountedPrice <= price, "итоговая цена со скидкой не может быть больше исходной цены");

    // возвращаем честно посчитанную цену со скидкой
    return discountedPrice;
}
