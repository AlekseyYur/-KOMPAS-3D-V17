using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using ORSAPR;

namespace ORSAPR.StressTest
{
    /// <summary>
    /// Программа для нагрузочного тестирования построителя модели сверла
    /// </summary>
    class Program
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(
            out long TotalMemoryInKilobytes);

        /// <summary>
        /// Коэффициент преобразования байтов в гигабайты
        /// </summary>
        private const double GigabyteInByte = 1.0 / 1073741824.0;

        /// <summary>
        /// Коэффициент преобразования килобайтов в гигабайты
        /// </summary>
        private const double KbToGb = 1.0 / 1048576.0;

        /// <summary>
        /// Точка входа в программу
        /// </summary>
        static void Main()
        {
            CultureInfo.CurrentCulture =
                CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture =
                CultureInfo.InvariantCulture;

            var builder = new ORSAPR.Builder();
            var parameters = CreateTestParameters();

            var logPath = "log.txt";
            var fullLogPath = Path.GetFullPath(logPath);

            Console.WriteLine($"Логирование в файл: {fullLogPath}");
            Console.WriteLine($"Начало нагрузочного тестирования...");
            Console.WriteLine($"Для остановки нажмите Ctrl+C\n");

            var stopWatch = new Stopwatch();
            var currentProcess = Process.GetCurrentProcess();
            var computerInfo = new ComputerInfo();

            using (var streamWriter = new StreamWriter(logPath))
            {
                int count = 0;
                var startTime = DateTime.Now;

                try
                {
                    // Бесконечный цикл для нагрузочного тестирования
                    while (true)
                    {
                        count++;

                        // Замер времени построения
                        stopWatch.Restart();
                        bool success = builder.Build(parameters);
                        stopWatch.Stop();

                        if (!success)
                        {
                            Console.WriteLine(
                                $"Ошибка при построении модели {count}");
                            continue;
                        }

                        // Замер общей используемой памяти в системе (в ГБ)
                        double usedMemoryGB =
                            (computerInfo.TotalPhysicalMemory
                            - computerInfo.AvailablePhysicalMemory)
                            * GigabyteInByte;

                        // Запись в лог в формате: 
                        // номер_модели время_построения память_ГБ
                        streamWriter.WriteLine(
                            $"{count}\t" +
                            $"{stopWatch.Elapsed:hh\\:mm\\:ss}\t" +
                            $"{usedMemoryGB:F9}");
                        streamWriter.Flush();

                        Console.WriteLine($"Модель {count}: " +
                                $"Время = {stopWatch.Elapsed:hh\\:mm\\:ss}, " +
                                $"ОЗУ = {usedMemoryGB:F2} ГБ");

                        stopWatch.Reset();
                    }
                }
                catch (System.Threading.ThreadInterruptedException)
                {
                    // Прерывание по Ctrl+C
                    Console.WriteLine(
                        "\nТестирование прервано пользователем.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nКритическая ошибка: {ex.Message}");
                }
                finally
                {
                    var totalTime = DateTime.Now - startTime;

                    // Вывод общей информации о системе
                    try
                    {
                        if (GetPhysicallyInstalledSystemMemory(
                            out long totalMemoryKB))
                        {
                            double totalMemoryGB =
                                totalMemoryKB * KbToGb;
                            Console.WriteLine(
                                $"Установленная ОЗУ: {totalMemoryGB:F1} ГБ");
                        }
                    }
                    catch
                    {
                        // Игнорируем ошибки получения информации о памяти
                    }

                    Console.WriteLine(
                        $"\nРезультаты нагрузочного тестирования:");
                    Console.WriteLine($"Всего построено моделей: {count}");
                    Console.WriteLine(
                        $"Общее время тестирования: {totalTime:hh\\:mm\\:ss}");
                    Console.WriteLine($"Данные сохранены в: {fullLogPath}");
                    Console.WriteLine($"\nФормат данных в лог-файле:");
                    Console.WriteLine($"Столбец 1: Номер созданной модели");
                    Console.WriteLine($"Столбец 2: Время построения");
                    Console.WriteLine($"Столбец 3: Используемая память (ГБ)");

                    // Пример строки лога
                    Console.WriteLine(
                        $"\nПример строки лога: \"1\t00:00:02\t8.92043264\"");
                }
            }
        }

        /// <summary>
        /// Создает тестовые параметры для нагрузочного тестирования
        /// </summary>
        /// <returns>Параметры со средними значениями</returns>
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