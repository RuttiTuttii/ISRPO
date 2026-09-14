using System.Diagnostics;

namespace LabWork7.Tasks;

public static class HeavyMath
{
    /// <summary>
    /// Тихая версия для бенчмарка (без внутреннего Stopwatch и Console),
    /// чтобы время честно мерил внешний Stopwatch из Task2.
    /// </summary>
    public static double Compute(int iterations = 1_000_000)
    {
        double totalResult = 0.0;

        for (int i = 1; i <= iterations; i++)
        {
            // sin(i) * ln(i + 1) + sqrt(i) / i^2
            double term = (Math.Sin(i) * Math.Log(i + 1)) + (Math.Sqrt(i) / Math.Pow(i, 2));
            totalResult += term;
        }

        return totalResult;
    }

    public static void StartMath(int iterations = 1_000_000)
    {
        Console.WriteLine("Начинаем тяжелые вычисления...");
        var sw = Stopwatch.StartNew();
        double result = Compute(iterations);
        sw.Stop();
        Console.WriteLine($"Результат (приблизительный): {result:F6}");
        Console.WriteLine($"Время выполнения: {sw.Elapsed.TotalMilliseconds:F1} ms.");
    }
}
