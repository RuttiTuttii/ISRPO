using System;
using System.Collections.Generic;

namespace LabWork8.Models;

public class Customer
{
    private string _name = string.Empty;

    public int Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя клиента не может быть пустым.", nameof(value));

            _name = value;
        }
    }

    public string Email { get; set; } = string.Empty;
    public List<Order> Orders { get; set; } = new();

    public void PrintDetails()
    {
        Console.WriteLine($"Customer: {Name}");
        Console.WriteLine($"Email: {Email}");
    }
}
