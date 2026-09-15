using LabWork8.Models;

namespace LabWork8.Services;

public class OrderPriceCalculator
{
    private const double VatRate = 0.20;
    private const double DiscountRate = 0.10;
    private const double DiscountThreshold = 10000;

    public double CalculateFinalPrice(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        double vat = order.Total * VatRate;
        double discount = order.Total > DiscountThreshold ? order.Total * DiscountRate : 0.0;

        return order.Total - discount + vat;
    }
}
