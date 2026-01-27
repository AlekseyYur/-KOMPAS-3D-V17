using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ORSAPR;

namespace ORSAPR.StressTest
{
    /// <summary>
    /// Программа для стресс-тестирования построителя модели сверла
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в программу
        /// </summary>
        static void Main()
        {
            // Устанавливаем инвариантную культуру для консистентности
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            var builder = new ORSAPR.Builder(); // Полное имя класса
            var parameters = CreateTestParameters();

            // Определяем путь для лог-файла
            var logPath = "stress_test_log.txt";
            var fullLogPath = Path.GetFullPath(logPath);

            Console.WriteLine($"Логирование в файл: {fullLogPath}");
            Console.WriteLine($"Начало стресс-тестирования...\n");

            var stopWatch = new Stopwatch();
            using (var streamWriter = new StreamWriter(logPath))
            {
                // Заголовок лога
                streamWriter.WriteLine("№\tВремя(мс)\tПамять(МБ)\tСтатус");
                streamWriter.Flush();

                int count = 0;
                const int maxIterations = 2; // Ограничиваем количество итераций для теста

                // Получаем информацию о текущем процессе
                var currentProcess = Process.GetCurrentProcess();
                var startTime = DateTime.Now;

                try
                {
                    for (int i = 0; i < maxIterations; i++)
                    {
                        count++;
                        stopWatch.Restart();

                        try
                        {
                            // Вызываем построение модели
                            bool success = builder.Build(parameters);

                            stopWatch.Stop();

                            // Получаем использование памяти
                            double usedMemoryMB = currentProcess.PrivateMemorySize64 / (1024.0 * 1024.0);

                            streamWriter.WriteLine(
                                $"{count}\t" +
                                $"{stopWatch.ElapsedMilliseconds}\t" +
                                $"{usedMemoryMB:F2}\t" +
                                $"{(success ? "Успех" : "Ошибка")}"
                            );
                            streamWriter.Flush();

                            Console.WriteLine($"Итерация {count}: " +
                                $"{stopWatch.ElapsedMilliseconds} мс, " +
                                $"{usedMemoryMB:F2} МБ, " +
                                $"{(success ? "Успех" : "Ошибка")}");

                            // Небольшая пауза для имитации реального использования
                            System.Threading.Thread.Sleep(100);
                        }
                        catch (Exception ex)
                        {
                            stopWatch.Stop();
                            streamWriter.WriteLine(
                                $"{count}\t" +
                                $"{stopWatch.ElapsedMilliseconds}\t" +
                                $"N/A\t" +
                                $"Исключение: {ex.Message}"
                            );
                            streamWriter.Flush();
                            Console.WriteLine($"Ошибка на итерации {count}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Критическая ошибка: {ex.Message}");
                }
                finally
                {
                    var totalTime = DateTime.Now - startTime;
                    Console.WriteLine($"\nТест завершен.");
                    Console.WriteLine($"Всего итераций: {count}");
                    Console.WriteLine($"Общее время: {totalTime:mm\\:ss}");
                    Console.WriteLine($"Лог сохранен в: {fullLogPath}");
                }
            }
        }

        /// <summary>
        /// Создает тестовые параметры для стресс-тестирования
        /// </summary>
        private static Parameters CreateTestParameters()
        {
            return new Parameters
            {
                Diameter = 10.0,
                Length = 55.0,
                TotalLength = 75.0,
                Angle = 45.0,
                ClearanceCone = true,
                ConeValue = 5.0,
                ClearanceShank = true,
                ShankDiameterValue = 17.5,
                ShankLengthValue = 40.0
            };
        }
    }
}