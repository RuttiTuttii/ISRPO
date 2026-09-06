using System.Diagnostics;

namespace LabWork5.Tasks;

public static class Task3_Assert
{
    public static void Run()
    {
        Console.WriteLine("=== Задание 5.3: Debug.Assert ===");

        // вызываем метод с нормальными валидными данными
        var price = 1000.0;
        var discount = 0.2;
        var total = CalculateDiscount(price, discount);
        Console.WriteLine($"Цена: {price}, скидка: {discount * 100}%, итог: {total}");

        // теперь намеренно дергаем с кривыми параметрами, чтобы сработал ассерт в Debug
        Console.WriteLine("\nПроверяем срабатывание Debug.Assert при невалидной скидке (1.5)...");
        CalculateDiscount(500, 1.5);
    }

    public static double CalculateDiscount(double price, double discountRate)
    {
        // проверяем что цена строго положительная
        Debug.Assert(price > 0, "цена товара должна быть больше нуля");

        // проверяем что коэффициент скидки от 0 до 1
        Debug.Assert(discountRate is > 0 and < 1, "скидка должна быть в диапазоне (0, 1)");

        // считаем итоговую цену со скидкой
        var discounted = price * (1.0 - discountRate);

        // проверяем что цена со скидкой не превышает исходную стоимость
        Debug.Assert(discounted <= price, "итоговая цена не может быть больше исходной");

        return discounted;
    }
}
