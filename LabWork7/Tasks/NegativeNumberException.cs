namespace LabWork7.Tasks;

// класс пользовательского исключения с поддержкой передачи message
public class NegativeNumberException : Exception
{
    public NegativeNumberException() : base("Значение не может быть отрицательным.") { }
    public NegativeNumberException(string message) : base(message) { }
    public NegativeNumberException(string message, Exception innerException) : base(message, innerException) { }
}
