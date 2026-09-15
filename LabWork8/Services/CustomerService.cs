using LabWork8.Data;
using LabWork8.Models;
using Microsoft.EntityFrameworkCore;

namespace LabWork8.Services;

public class CustomerService
{
    private readonly AppDbContext _dbContext;

    public CustomerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddCustomer(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();
    }

    public void PrintCustomerInfo(int customerId)
    {
        var customer = _dbContext.Customers
            .Include(c => c.Orders)
            .FirstOrDefault(c => c.Id == customerId);

        if (customer == null)
        {
            Console.WriteLine($"[Предупреждение] Клиент с ID {customerId} не найден.");
            return;
        }

        customer.PrintDetails();
    }
}
