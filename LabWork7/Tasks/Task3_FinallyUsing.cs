namespace LabWork7.Tasks
{

    using System.Diagnostics;

    public static class Task4_TraceSourceCalculator
    {
        public static void Run()
        {
            // Настройка TraceSource с именем "Calculator"
            var ts = new TraceSource("Calculator")
            {
                Switch = new SourceSwitch("CalculatorSwitch") { Level = SourceLevels.Information }
            };

            // Очищаем слушателей, чтобы убрать DefaultTraceListener и избежать дублирования в Output
            ts.Listeners.Clear();

            // Добавляем слушателя для записи в файл trace.log
            var fileListener = new TextWriterTraceListener("trace.log", "fileListener");
            ts.Listeners.Add(fileListener);

            // Добавляем слушателя для вывода в консоль
            ts.Listeners.Add(new ConsoleTraceListener());

            //// Автоматически сбрасывать буфер после каждой записи
            //ts.AutoFlush = true;

            try
            {
                Console.Write("Введите первое целое число: ");
                if (!int.TryParse(Console.ReadLine(), out var num1))
                {
                    ts.TraceEvent(TraceEventType.Error, 1, "Ошибка ввода: первое число введено некорректно.");
                    return;
                }

                Console.Write("Введите второе целое число: ");
                if (!int.TryParse(Console.ReadLine(), out var num2))
                {
                    ts.TraceEvent(TraceEventType.Error, 2, "Ошибка ввода: второе число введено некорректно.");
                    return;
                }

                // Логирование входных параметров на уровне Verbose (будет отсечено при текущем уровне Information)
                ts.TraceEvent(TraceEventType.Verbose, 3, $"Входные параметры: num1 = {num1}, num2 = {num2}");

                // Сложение
                var sum = num1 + num2;
                ts.TraceInformation($"Выполнено сложение: {num1} + {num2} = {sum}");
                Console.WriteLine($"Сумма: {sum}");

                // Вычитание
                var diff = num1 - num2;
                ts.TraceInformation($"Выполнено вычитание: {num1} - {num2} = {diff}");
                Console.WriteLine($"Разность: {diff}");

                // Умножение
                var mul = num1 * num2;
                ts.TraceInformation($"Выполнено умножение: {num1} * {num2} = {mul}");
                Console.WriteLine($"Произведение: {mul}");

                // Деление
                if (num2 == 0)
                {
                    var errorMsg = "Ошибка: деление на ноль.";
                    Console.WriteLine(errorMsg);
                    ts.TraceEvent(TraceEventType.Error, 4, errorMsg);
                }
                else
                {
                    var div = num1 / num2;
                    ts.TraceInformation($"Выполнено деление: {num1} / {num2} = {div}");
                    Console.WriteLine($"Частное: {div}");
                }

                // Проверка на переполнение при сложении (пример обработки возможной ошибки)
                checked
                {
                    try
                    {
                        _ = num1 + num2; // Эта операция уже выполнена выше, но здесь демонстрируем обработку OverflowException
                    }
                    catch (OverflowException ex)
                    {
                        ts.TraceEvent(TraceEventType.Error, 5, $"Произошло переполнение: {ex.Message}");
                        Console.WriteLine($"Произошло переполнение: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                ts.TraceEvent(TraceEventType.Error, 6, $"Непредвиденная ошибка: {ex.Message}");
                Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            }
            finally
            {
                // Сброс буфера и закрытие слушателей
                ts.Flush();
                ts.Close();

                Console.WriteLine("\nЛогирование завершено. Проверьте файл trace.log для просмотра записей.");
                Console.WriteLine("Обратите внимание: сообщения уровня Verbose не попали в лог, так как SourceSwitch.Level = SourceLevels.Information.");

                // Демонстрация того, что после Close() запись невозможна
                try
                {
                    ts.TraceInformation("Эта запись не появится в логе, так как TraceSource закрыт.");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Повторная запись после Close() вызвала ObjectDisposedException — это ожидаемое поведение.");
                }
            }
        }
    }
}