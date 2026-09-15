using LabWork8.Data;
using LabWork8.Models;
using LabWork8.Services;

namespace LabWork8;

internal class Program
{
    static void Main(string[] args)
    {
        using var dbContext = new AppDbContext();
        dbContext.Database.EnsureCreated();

        var customerService = new CustomerService(dbContext);
        var orderService = new OrderService(dbContext);

        var customer = new Customer { Name = "Егор", Email = "egor@example.com" };
        customerService.AddCustomer(customer);

        var order = new Order { Total = 12500, IsExpress = true, Customer = customer };
        orderService.AddOrder(order);

        Console.WriteLine("Данные клиента:");
        customerService.PrintCustomerInfo(customer.Id);

        Console.WriteLine("\nДетали заказа:");
        orderService.PrintOrderDetails(order.Id);

        double finalPrice = orderService.CalculateFinalPrice(order);
        Console.WriteLine($"\nИтоговая цена: {finalPrice:F2}");
    }
}
