using System.Diagnostics;

namespace LabWork7.Tasks
{
    public class HeavyMath
    {
        public static void StartMath()
        {
            Console.WriteLine("Начинаем тяжелые вычисления...");
            Stopwatch stopWatch = new();

            double totalResult = 0.0;
            int iterations = 5_000_000; // 5 миллионов повторений

            stopWatch.Start();

            for (int i = 1; i <= iterations; i++)
            {
                // Сложная формула внутри цикла:
                // sin(i) * ln(i + 1) + sqrt(i) / i^2
                double part1 = Math.Sin(i);
                double part2 = Math.Log(i + 1);
                double part3 = Math.Sqrt(i);
                double part4 = Math.Pow(i, 2);

                double term = (part1 * part2) + (part3 / part4);
                totalResult += term;
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine($"Результат (приблизительный): {totalResult:F6}");
            Console.WriteLine($"Время выполнения: {(ts).TotalSeconds} ms.");
        }
    }
}
