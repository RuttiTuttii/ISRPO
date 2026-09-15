namespace LabWork8.Models;

public class Order
{
    public int Id { get; set; }
    public double Total { get; set; }
    public bool IsExpress { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
