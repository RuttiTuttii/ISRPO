using LabWork8.Data;
using LabWork8.Models;
using Microsoft.EntityFrameworkCore;

namespace LabWork8.Services;

public class OrderService
{
    private readonly AppDbContext _dbContext;
    private readonly OrderPriceCalculator _priceCalculator;

    public OrderService(AppDbContext dbContext, OrderPriceCalculator? priceCalculator = null)
    {
        _dbContext = dbContext;
        _priceCalculator = priceCalculator ?? new OrderPriceCalculator();
    }

    public void AddOrder(Order order)
    {
        _dbContext.Orders.Add(order);
        _dbContext.SaveChanges();
    }

    public void PrintOrderDetails(int orderId)
    {
        var order = _dbContext.Orders
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.Id == orderId);

        if (order == null)
        {
            Console.WriteLine($"[Предупреждение] Заказ с ID {orderId} не найден.");
            return;
        }

        PrintOrderIdentifier(order);
        PrintOrderTotal(order);
        PrintExpressShippingStatus(order);
        PrintCustomerContact(order);
    }

    private void PrintOrderIdentifier(Order order) => Console.WriteLine($"Order Id: {order.Id}");
    private void PrintOrderTotal(Order order) => Console.WriteLine($"Total: {order.Total:C}");
    private void PrintExpressShippingStatus(Order order) => Console.WriteLine($"Express Shipping: {(order.IsExpress ? "Yes" : "No")}");
    private void PrintCustomerContact(Order order)
    {
        if (order.Customer != null)
            Console.WriteLine($"Customer Email: {order.Customer.Email}");
    }

    public double CalculateFinalPrice(Order order) => _priceCalculator.CalculateFinalPrice(order);
}
