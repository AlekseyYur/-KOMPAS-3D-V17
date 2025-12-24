using Kompas6API5;
using Kompas6Constants3D;
using KompasAPI7;
using System;
using System.IO;

namespace ORSAPR
{
    /// <summary>
    /// Класс для построения 3D-модели сверла в КОМПАС-3D.
    /// </summary>
    /// <remarks>
    /// Класс обеспечивает создание 3D-модели сверла на основе заданных параметров,
    /// включая формирование профиля, спиральных канавок и сохранение результата.
    /// Для работы требуется установленный и запущенный КОМПАС-3D.
    /// </remarks>
    public class Builder
    {
        /// <summary>
        /// Обертка для работы с API КОМПАС-3D.
        /// </summary>
        private Wrapper _wrapper = new Wrapper();

        /// <summary>
        /// Выполняет построение 3D-модели сверла на основе заданных параметров.
        /// </summary>
        /// <param name="parameters">Параметры сверла для построения модели.</param>
        /// <returns>
        /// <c>true</c> - если модель успешно построена; 
        /// <c>false</c> - в случае ошибки подключения к КОМПАС-3D или построения модели.
        /// </returns>
        /// <remarks>
        /// Метод выполняет следующую последовательность действий:
        /// <list type="number">
        /// <item><description>Подключение к КОМПАС-3D через <see cref="Wrapper.ConnectCAD"/></description></item>
        /// <item><description>Создание нового 3D-документа через <see cref="Wrapper.CreateDocument"/></description></item>
        /// <item><description>Создание эскиза профиля сверла в плоскости XOZ</description></item>
        /// <item><description>Построение контура сверла с учетом обратного конуса (если задан)</description></item>
        /// <item><description>Создание тела вращения для формирования основного цилиндра сверла</description></item>
        /// <item><description>Создание цилиндрической спирали для основной спиральной канавки</description></item>
        /// <item><description>Создание конической спирали для хвостовика</description></item>
        /// <item><description>Формирование спиральных канавок с использованием эволюции по спиралям</description></item>
        /// <item><description>Круговое копирование канавок для формирования двух противоположных канавок</description></item>
        /// </list>
        /// В случае возникновения исключения выполняется его обработка и возвращается <c>false</c>.
        /// </remarks>
        /// <example>
        /// <code>
        /// var parameters = new Parameters();
        /// var builder = new Builder();
        /// bool success = builder.Build(parameters);
        /// if (success)
        /// {
        ///     Console.WriteLine("Модель успешно построена");
        /// }
        /// </code>
        /// </example>
        public bool Build(Parameters parameters)
        {
            try
            {
                if (!_wrapper.ConnectCAD())
                {
                    return false;
                }

                if (!_wrapper.CreateDocument())
                {
                    return false;
                }

                ksEntity sketch = _wrapper.CreateSketch((short)Obj3dType.o3d_planeXOZ);

                ksDocument2D doc2D = _wrapper.BeginSketchEdit(sketch);

                // Параметры
                double radius = parameters.Diameter / 2;
                double totalLength = parameters.TotalLength;
                double workingLength = parameters.Length;
                double tailLength = totalLength - workingLength;
                double pointAngle = parameters.Angle;

                if (parameters.ClearanceCone != false)
                {
                    double cone = parameters.ConeValue / 2;

                    // 1. Точка 0, 0
                    double x1 = 0;
                    double z1 = 0;

                    // 2. Точка, по x на cone, z2 = 0 = z1;
                    double x2 = cone;

                    // 3. x3 = radius, z3 = tailLength
                    double x3 = radius;
                    double z3 = tailLength;

                    // 4 x4 = radius, z4 = totalLength
                    double z4 = totalLength;

                    //5 x5 = 0, z5 = totalLength

                    // Рисуем первую линию от 1 точки ко 2
                    _wrapper.DrawLineSeg(doc2D, x1, z1, x2, z1);

                    // Рисуем первую линию от 2 к 3 точке
                    _wrapper.DrawLineSeg(doc2D, x2, z1, x3, z3);

                    // От 3 к 4
                    _wrapper.DrawLineSeg(doc2D, x3, z3, x3, z4);

                    // От 4 к 5
                    _wrapper.DrawLineSeg(doc2D, x3, z4, x1, z4);

                    // От 5 к 1
                    //_wrapper.DrawLineSeg(doc2D, x1, z4, x1, z1);
                    _wrapper.DrawAxisLine(doc2D, x1, z3, x1, z1); // Осевая линия для оси вращения

                }
                else
                {
                    // 1. Точка (начало координат, y везде = 0)
                    double x1 = 0;
                    double z1 = 0;

                    // 2. Двигаемся по x на радиус, z2 = 0, поэтому используем z1
                    double x2 = radius;

                    // 3. x3 равнаяется 0, поэтому используем x1
                    double z3 = totalLength;

                    // 4. Точка, где x4 = radius, z4 = totalLength => x4 = x2, z4 = z3

                    // Рисуем первую линию от 1 точки ко 2
                    _wrapper.DrawLineSeg(doc2D, x1, z1, x2, z1);

                    // Линия от 2 до 3 точки
                    _wrapper.DrawLineSeg(doc2D, x2, z1, x2, z3);

                    // Линия от 3 до 4 точки
                    _wrapper.DrawLineSeg(doc2D, x2, z3, x1, z3); // Исправлено: была ошибка - точки совпадали

                    // Линия от 4 до 1 точки
                    //_wrapper.DrawLineSeg(doc2D, x1, z3, x1, z1); // Исправлено: была ошибка - точки совпадали
                    _wrapper.DrawAxisLine(doc2D, x1, z3, x1, z1); // Осевая линия для оси вращения

                    // Завершаем редактирование эскиза
                    _wrapper.EndSketchEdit(sketch);

                }

                _wrapper.EndSketchEdit(sketch);
                // Ось вращения
                _wrapper.CreateRotation(sketch, (short)Direction_Type.dtReverse, false, 360);
                ksEntity spiral = _wrapper.CreateDrillSpiral(workingLength, parameters.Diameter, totalLength);
                ksEntity fluteProfile = _wrapper.CreateFluteProfile(totalLength, radius, 0.6);
                ksEntity firstFlute = _wrapper.CreateHelicalFlute(fluteProfile, spiral);
                _wrapper.CreateCircularCopy(2, firstFlute);
                ksEntity conicSpiral = _wrapper.CreateConicSpiral(totalLength * 0.54, parameters.Diameter, tailLength);
                ksEntity fluteProfile2 = _wrapper.CreateFluteProfile(tailLength, radius, 0.55);
                ksEntity secondFlut = _wrapper.CreateHelicalFlute(fluteProfile2, conicSpiral);
                _wrapper.CreateCircularCopy(2, secondFlut);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при построении: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Сохраняет построенную 3D-модель сверла в файл.
        /// </summary>
        /// <param name="parameters">Параметры сверла, используемые для формирования имени файла.</param>
        /// <param name="folderPath">Путь к папке для сохранения файла.</param>
        /// <returns>
        /// <c>true</c> - если файл успешно сохранен; 
        /// <c>false</c> - в случае ошибки при сохранении.
        /// </returns>
        /// <remarks>
        /// Метод выполняет следующие действия:
        /// <list type="number">
        /// <item><description>Проверяет существование указанной папки, создает ее при необходимости</description></item>
        /// <item><description>Формирует имя файла на основе параметров сверла и текущей даты/времени</description></item>
        /// <item><description>Сохраняет документ КОМПАС-3D по указанному пути через <see cref="Wrapper.SaveDocument"/></description></item>
        /// </list>
        /// Формат имени файла: Drill_[диаметр]x[общая длина]_Cone_[наличие конуса]_[значение конуса]_[дата_время].m3d
        /// </remarks>
        /// <example>
        /// <code>
        /// var parameters = new Parameters();
        /// parameters.Diameter = 10;
        /// parameters.TotalLength = 100;
        /// parameters.ClearanceCone = true;
        /// parameters.ConeValue = 0.5;
        /// 
        /// string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        /// string folderPath = Path.Combine(desktopPath, "КОМПАС_Сверла");
        /// 
        /// bool saved = builder.SaveResult(parameters, folderPath);
        /// if (saved)
        /// {
        ///     Console.WriteLine($"Файл сохранен в: {folderPath}");
        /// }
        /// </code>
        /// </example>
        public bool SaveResult(Parameters parameters, string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = $"Drill_{parameters.Diameter}x{parameters.TotalLength}_" +
                                 $"Cone_{parameters.ClearanceCone}_{parameters.ConeValue}_" +
                                 $"{DateTime.Now:yyyyMMdd_HHmmss}.m3d";
                string filePath = Path.Combine(folderPath, fileName);

                return _wrapper.SaveDocument(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
                return false;
            }
        }
    }
}