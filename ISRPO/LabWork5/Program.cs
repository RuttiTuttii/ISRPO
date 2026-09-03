double CalculateDiscount(double price, double discountRate)
{
    if (discountRate is > 0 and < 1)
    {
        if (price  > 0)
        {
            return price *= discountRate;
        }

        else { throw new Exception("Bad Arguments"); };
    }
    else { throw new Exception("Bad Arguments"); };
}

Console.WriteLine(CalculateDiscount(50, 2));