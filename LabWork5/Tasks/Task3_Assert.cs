using System.Diagnostics;

namespace LabWork5.Tasks;

public static class Task3_Assert
{
    public static void Run()
    {
        Console.WriteLine("--- Задание 5.3: Использование Debug.Assert ---\n");

        // показываем успешный расчет со 100% валидными аргументами
        Console.WriteLine("демонстрация штатного расчета скидки:");
        double validPrice = 1000;
        double validDiscount = 0.20;
        double result = CalculateDiscount(validPrice, validDiscount);
        Console.WriteLine($"исходная цена: {validPrice}, скидка: {validDiscount:P0} -> цена со скидкой: {result} руб.\n");

        Console.WriteLine("выберите сценарий тестирования контрактов Debug.Assert:");
        Console.WriteLine("1 - корректный вызов (цена 500, скидка 10%)");
        Console.WriteLine("2 - спровоцировать Debug.Assert: отрицательная цена (-250)");
        Console.WriteLine("3 - спровоцировать Debug.Assert: невалидная скидка (1.5 / 150%)");
        Console.Write("ваш выбор (1-3): ");

        string? choice = Console.ReadLine();

        // запускаем выбранный вариант для проверки всплывающего окна ассерта
        switch (choice?.Trim())
        {
            case "1":
                double r = CalculateDiscount(500, 0.10);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"успех: итоговая цена {r} руб.");
                Console.ResetColor();
                break;
            case "2":
                Console.WriteLine("\nвызываем CalculateDiscount(-250, 0.1)... сейчас сработает Debug.Assert!");
                CalculateDiscount(-250, 0.1);
                break;
            case "3":
                Console.WriteLine("\nвызываем CalculateDiscount(300, 1.5)... сейчас сработает Debug.Assert!");
                CalculateDiscount(300, 1.5);
                break;
            default:
                Console.WriteLine("неизвестный вариант.");
                break;
        }
    }

    // метод расчета цены со скидкой с контрактными утверждениями
    public static double CalculateDiscount(double price, double discountRate)
    {
        // проверяем через ассерт, что цена строго положительная
        Debug.Assert(price > 0, $"цена должна быть больше нуля, передано: {price}");

        // проверяем через ассерт, что скидка лежит в диапазоне (0, 1)
        Debug.Assert(discountRate > 0 && discountRate < 1, $"размер скидки должен быть строго от 0 до 1, передано: {discountRate}");

        // производим вычисление цены с учетом скидки
        double discountedPrice = price * (1.0 - discountRate);

        // проверяем инвариант: цена со скидкой не может превышать исходную стоимость
        Debug.Assert(discountedPrice <= price, "цена со скидкой превышает базовую цену");

        // возвращаем итоговую цену товара
        return discountedPrice;
    }
}
