using System;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;

namespace ORSAPR
{
    /// <summary>
    /// Обертка для работы с API КОМПАС-3D.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет методы для взаимодействия с КОМПАС-3D через COM-интерфейсы,
    /// включая создание документов, эскизов, геометрических элементов и операций.
    /// Все методы содержат обработку исключений для обеспечения стабильной работы.
    /// </remarks>
    public class Wrapper
    {
        /// <summary>
        /// Объект КОМПАС-3D.
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Текущий 3D-документ.
        /// </summary>
        private ksDocument3D _document3D;

        /// <summary>
        /// Активная деталь в документе.
        /// </summary>
        private ksPart _part;

        /// <summary>
        /// Получает активную деталь из текущего документа.
        /// </summary>
        /// <returns>Объект активной детали <see cref="ksPart"/>.</returns>
        /// <remarks>
        /// Возвращает деталь, с которой в данный момент ведется работа.
        /// Деталь должна быть предварительно получена через метод <see cref="CreateDocument"/>.
        /// </remarks>
        public ksPart GetPart()
        {
            return _part;
        }

        /// <summary>
        /// Подключается к запущенному экземпляру КОМПАС-3D или запускает новый.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если подключение к КОМПАС-3D выполнено успешно; 
        /// <c>false</c> - в случае ошибки подключения.
        /// </returns>
        /// <remarks>
        /// Метод выполняет следующую последовательность действий:
        /// <list type="number">
        /// <item><description>Проверяет существование уже подключенного объекта КОМПАС</description></item>
        /// <item><description>Пытается получить активный экземпляр КОМПАС-3D через COM</description></item>
        /// <item><description>Если активный экземпляр не найден, создает новый экземпляр</description></item>
        /// <item><description>Делает окно КОМПАС-3D видимым и активирует API</description></item>
        /// </list>
        /// В случае любой ошибки возвращается <c>false</c>.
        /// </remarks>
        public bool ConnectCAD()
        {
            try
            {
                if (_kompas != null)
                {
                    _kompas.Visible = true;
                    _kompas.ActivateControllerAPI();
                    return true;
                }

                try
                {
                    _kompas = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
                }
                catch (COMException)
                {
                    var kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                    _kompas = (KompasObject)Activator.CreateInstance(kompasType);
                }

                if (_kompas == null)
                    return false;

                _kompas.Visible = true;
                _kompas.ActivateControllerAPI();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Создает новый 3D-документ в КОМПАС-3D.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если документ успешно создан; 
        /// <c>false</c> - в случае ошибки создания документа.
        /// </returns>
        /// <remarks>
        /// Метод выполняет следующие действия:
        /// <list type="number">
        /// <item><description>Создает новый 3D-документ</description></item>
        /// <item><description>Активирует созданный документ</description></item>
        /// <item><description>Получает основную деталь документа</description></item>
        /// </list>
        /// Перед вызовом метода необходимо установить соединение с КОМПАС-3D через <see cref="ConnectCAD"/>.
        /// </remarks>
        public bool CreateDocument()
        {
            try
            {
                _document3D = (ksDocument3D)_kompas.Document3D();
                _document3D.Create();
                _document3D = (ksDocument3D)_kompas.ActiveDocument3D();
                _part = (ksPart)_document3D.GetPart((short)Part_Type.pTop_Part);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Сохраняет текущий 3D-документ в файл.
        /// </summary>
        /// <param name="filePath">Полный путь к файлу для сохранения.</param>
        /// <returns>
        /// <c>true</c> - если документ успешно сохранен; 
        /// <c>false</c> - в случае ошибки сохранения.
        /// </returns>
        /// <remarks>
        /// Метод сохраняет текущий активный 3D-документ в указанный файл.
        /// Расширение файла должно соответствовать формату КОМПАС-3D (обычно .m3d).
        /// </remarks>
        public bool SaveDocument(string filePath)
        {
            try
            {
                _document3D.SaveAs(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Создает новый эскиз на указанной плоскости.
        /// </summary>
        /// <param name="plane">Тип плоскости для создания эскиза (из перечисления <see cref="Obj3dType"/>).</param>
        /// <returns>
        /// Объект эскиза <see cref="ksEntity"/> или <c>null</c> в случае ошибки.
        /// </returns>
        /// <remarks>
        /// Метод создает эскиз на одной из стандартных плоскостей:
        /// <list type="bullet">
        /// <item><description><see cref="Obj3dType.o3d_planeXOY"/> - плоскость XOY</description></item>
        /// <item><description><see cref="Obj3dType.o3d_planeXOZ"/> - плоскость XOZ</description></item>
        /// <item><description><see cref="Obj3dType.o3d_planeYOZ"/> - плоскость YOZ</description></item>
        /// </list>
        /// </remarks>
        public ksEntity CreateSketch(short plane)
        {
            try
            {
                var currentPlane = (ksEntity)_part.GetDefaultEntity(plane);
                var entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
                var sketchDefinition = (ksSketchDefinition)entitySketch.GetDefinition();
                sketchDefinition.SetPlane(currentPlane);
                entitySketch.Create();
                return entitySketch;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Начинает редактирование эскиза.
        /// </summary>
        /// <param name="sketch">Эскиз для редактирования.</param>
        /// <returns>
        /// Объект 2D-документа <see cref="ksDocument2D"/> для рисования или <c>null</c> в случае ошибки.
        /// </returns>
        /// <remarks>
        /// Метод переводит эскиз в режим редактирования, позволяя добавлять геометрические элементы.
        /// После завершения редактирования необходимо вызвать <see cref="EndSketchEdit"/>.
        /// </remarks>
        public ksDocument2D BeginSketchEdit(ksEntity sketch)
        {
            try
            {
                var sketchDefinition = (ksSketchDefinition)sketch.GetDefinition();
                return sketchDefinition.BeginEdit();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Завершает редактирование эскиза.
        /// </summary>
        /// <param name="sketch">Эскиз, редактирование которого завершается.</param>
        /// <remarks>
        /// Метод завершает режим редактирования эскиза и фиксирует все внесенные изменения.
        /// Должен вызываться после <see cref="BeginSketchEdit"/>.
        /// </remarks>
        public void EndSketchEdit(ksEntity sketch)
        {
            try
            {
                var sketchDefinition = (ksSketchDefinition)sketch.GetDefinition();
                sketchDefinition.EndEdit();
            }
            catch { }
        }

        /// <summary>
        /// Рисует отрезок линии в 2D-документе.
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования.</param>
        /// <param name="x1">X-координата начальной точки.</param>
        /// <param name="y1">Y-координата начальной точки.</param>
        /// <param name="x2">X-координата конечной точки.</param>
        /// <param name="y2">Y-координата конечной точки.</param>
        /// <remarks>
        /// Метод рисует отрезок линии между двумя точками в текущем эскизе.
        /// Используется тип линии 1 (сплошная линия).
        /// </remarks>
        public void DrawLineSeg(ksDocument2D doc2D, double x1, double y1, double x2, double y2)
        {
            try
            {
                doc2D.ksLineSeg(x1, y1, x2, y2, 1);
            }
            catch { }
        }

        /// <summary>
        /// Рисует осевую линию в 2D-документе.
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования.</param>
        /// <param name="x1">X-координата начальной точки.</param>
        /// <param name="y1">Y-координата начальной точки.</param>
        /// <param name="x2">X-координата конечной точки.</param>
        /// <param name="y2">Y-координата конечной точки.</param>
        /// <remarks>
        /// Метод рисует осевую линию между двумя точками.
        /// Используется тип линии 3 (осевая линия), который распознается КОМПАС-3D как ось вращения.
        /// </remarks>
        public void DrawAxisLine(ksDocument2D doc2D, double x1, double y1, double x2, double y2)
        {
            try
            {
                // Тип линии 3 = ОСЕВАЯ ЛИНИЯ (КОМПАС распознает её как ось вращения)
                doc2D.ksLineSeg(x1, y1, x2, y2, 3);
            }
            catch { }
        }

        /// <summary>
        /// Создает операцию вращения для формирования тела вращения.
        /// </summary>
        /// <param name="sketch">Эскиз профиля для вращения.</param>
        /// <param name="direction">Направление вращения (из перечисления <see cref="Direction_Type"/>).</param>
        /// <param name="side">Направление выдавливания: <c>true</c> - в одну сторону, <c>false</c> - в обе стороны.</param>
        /// <param name="angle">Угол вращения в градусах (обычно 360 для полного вращения).</param>
        /// <returns>
        /// Определение операции вращения <see cref="ksBaseRotatedDefinition"/>.
        /// </returns>
        /// <remarks>
        /// Метод создает тело вращения из плоского эскиза вокруг оси, указанной в эскизе.
        /// Используется для создания цилиндрических тел, таких как основное тело сверла.
        /// </remarks>
        public ksBaseRotatedDefinition CreateRotation(ksEntity sketch, short direction,
            bool side, int angle)
        {
            ksEntity entity = _part.NewEntity((short)Obj3dType.o3d_baseRotated);
            ksBaseRotatedDefinition definition = entity.GetDefinition();
            definition.directionType = direction;
            definition.SetSideParam(side, angle);
            definition.SetSketch(sketch);
            entity.Create();
            return definition;
        }

        /// <summary>
        /// Создает цилиндрическую спираль для формирования спиральной канавки сверла.
        /// </summary>
        /// <param name="drillLength">Длина рабочей части сверла (высота спирали).</param>
        /// <param name="drillDiameter">Диаметр сверла (диаметр спирали).</param>
        /// <param name="totalLength">Общая длина сверла (используется для смещения начала спирали).</param>
        /// <returns>
        /// Объект цилиндрической спирали <see cref="ksEntity"/> или <c>null</c> в случае ошибки.
        /// </returns>
        /// <remarks>
        /// Метод создает цилиндрическую спираль со следующими параметрами:
        /// <list type="bullet">
        /// <item><description>Способ построения: по высоте и виткам (buildMode = 2)</description></item>
        /// <item><description>Высота: равна длине рабочей части сверла</description></item>
        /// <item><description>Количество витков: 2</description></item>
        /// <item><description>Диаметр: равен диаметру сверла</description></item>
        /// <item><description>Направление навивки: правая спираль (turnDir = true)</description></item>
        /// <item><description>Шаг: автоматический расчет (step = 0)</description></item>
        /// <item><description>Начало спирали смещено по оси Z на значение общей длины</description></item>
        /// </list>
        /// Спираль используется в качестве траектории для операции эволюции при создании спиральной канавки.
        /// </remarks>
        public ksEntity CreateDrillSpiral(double drillLength, double drillDiameter, double totalLength)
        {
            try
            {
                // Создаем спираль
                ksEntity spiralEntity = _part.NewEntity((short)Obj3dType.o3d_cylindricSpiral);
                if (spiralEntity == null) return null;

                ksCylindricSpiralDefinition spiralDef = spiralEntity.GetDefinition();
                if (spiralDef == null) return null;


                // 1. Способ построения: 2 = по высоте и виткам
                spiralDef.buildMode = 2;

                // 2. Высота спирали
                spiralDef.height = drillLength;

                // 3. Количество витков
                spiralDef.turn = 2;

                // 4. Диаметр спирали
                spiralDef.diam = drillDiameter;

                // 5. Направление навивки: true - правая
                spiralDef.turnDir = true;

                // 6. Начальный угол = 0 (по умолчанию)
                spiralDef.firstAngle = 0;

                // 7. Шаг спирали (автоматический расчет при step = 0)
                spiralDef.step = 0;

                // 8. Без дополнительной высоты
                spiralDef.heightAdd = 0;

                // 9. Устанавливаем базовую плоскость XOY
                ksEntity basePlane = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
                ksEntity offsetPlane = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
                ksPlaneOffsetDefinition offsetDef = offsetPlane.GetDefinition();
                offsetDef.SetPlane(basePlane);
                offsetDef.direction = true;
                offsetDef.offset = -totalLength; // НАЧАЛО спирали по Z
                offsetPlane.Create();

                spiralDef.SetPlane(offsetPlane);
                spiralDef.SetLocation(0, 0);

                // Создаем спираль
                spiralEntity.Create();

                return spiralEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Создает эскиз профиля спиральной канавки сверла.
        /// </summary>
        /// <param name="totalLength">Общая длина сверла (используется для смещения плоскости).</param>
        /// <param name="radius">Радиус сверла.</param>
        /// <param name="depth">Глубина канавки в долях от радиуса.</param>
        /// <returns>
        /// Эскиз профиля канавки <see cref="ksEntity"/>.
        /// </returns>
        /// <remarks>
        /// Метод создает эскиз на смещенной плоскости XOY, содержащий окружность,
        /// которая будет использована в качестве сечения для операции эволюции.
        /// Глубина канавки определяется как произведение радиуса на параметр depth.
        /// </remarks>
        public ksEntity CreateFluteProfile(double totalLength, double radius, double depth)
        {
            ksEntity basePlane = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            ksEntity offsetPlane = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetDef = offsetPlane.GetDefinition();
            offsetDef.SetPlane(basePlane);
            offsetDef.offset = -totalLength;
            offsetDef.direction = true;
            offsetPlane.Create();

            ksEntity sketch = _part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDef = sketch.GetDefinition();
            sketchDef.SetPlane(offsetPlane);
            sketch.Create();

            ksDocument2D doc2D = sketchDef.BeginEdit();

            long circleHandle = doc2D.ksCircle(radius, 0, radius * depth, 1);

            sketchDef.EndEdit();

            return sketch;
        }

        /// <summary>
        /// Создает спиральную канавку методом эволюции по спиральной траектории.
        /// </summary>
        /// <param name="fluteProfile">Эскиз профиля канавки.</param>
        /// <param name="spiralPath">Спиральная траектория для эволюции.</param>
        /// <returns>
        /// Объект операции вырезания эволюцией <see cref="ksEntity"/> или <c>null</c> в случае ошибки.
        /// </returns>
        /// <remarks>
        /// Метод выполняет операцию "Вырез эволюцией", при которой профиль из эскиза
        /// перемещается вдоль спиральной траектории, создавая спиральную канавку.
        /// Параметры операции:
        /// <list type="bullet">
        /// <item><description>sketchShiftType = 0 - без смещения эскиза</description></item>
        /// <item><description>chooseType = 1 - обычный выбор</description></item>
        /// <item><description>cut = true - операция вырезания (удаление материала)</description></item>
        /// </list>
        /// </remarks>
        public ksEntity CreateHelicalFlute(ksEntity fluteProfile, ksEntity spiralPath)
        {
            ksEntity cutEvolution = _part.NewEntity((short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDef = cutEvolution.GetDefinition();

            // 1. Устанавливаем эскиз сечения
            cutEvolutionDef.SetSketch(fluteProfile);

            // 2. Получаем и заполняем массив траекторий
            // Используем ksEntityCollection (это COM интерфейс ksEntityCollection)
            ksEntityCollection pathArray = cutEvolutionDef.PathPartArray();
            pathArray.Add(spiralPath);

            cutEvolutionDef.sketchShiftType = 0;
            cutEvolutionDef.chooseType = 1;
            cutEvolutionDef.cut = true;

            // 5. Создаем операцию
            bool createResult = cutEvolution.Create();

            return createResult ? cutEvolution : null;
        }

        /// <summary>
        /// Создает операцию кругового копирования элементов.
        /// </summary>
        /// <param name="count">Количество копий (включая оригинал).</param>
        /// <param name="elementToCopy">Элемент для копирования.</param>
        /// <returns>
        /// Определение операции кругового копирования <see cref="ksCircularCopyDefinition"/>.
        /// </returns>
        /// <remarks>
        /// Метод создает круговой массив копий указанного элемента вокруг оси OZ.
        /// Параметры операции:
        /// <list type="bullet">
        /// <item><description>Количество копий: указанное значение count</description></item>
        /// <item><description>Угол: 180 градусов (для двух противоположных канавок)</description></item>
        /// <item><description>Равномерное распределение: true</description></item>
        /// <item><description>Копирование оригинала: false (оригинал не дублируется)</description></item>
        /// <item><description>Ось вращения: ось OZ</description></item>
        /// </list>
        /// Используется для создания двух противоположных спиральных канавок сверла.
        /// </remarks>
        public ksCircularCopyDefinition CreateCircularCopy(int count, ksEntity elementToCopy)
        {
            // Создаем операцию кругового копирования
            ksEntity circularCopyEntity = _part.NewEntity((short)Obj3dType.o3d_circularCopy);
            ksCircularCopyDefinition copyDefinition = circularCopyEntity.GetDefinition();

            // Настраиваем параметры: количество копий, угол, равномерно, без копирования оригинала
            copyDefinition.SetCopyParamAlongDir(count, 180, true, false);

            // Устанавливаем ось вращения (ось OZ)
            ksEntity axisOz = _part.GetDefaultEntity((short)Obj3dType.o3d_axisOZ);
            copyDefinition.SetAxis(axisOz);

            // Получаем коллекцию объектов для копирования
            ksEntityCollection entityCollection = copyDefinition.GetOperationArray();

            // Очищаем коллекцию (на всякий случай)
            entityCollection.Clear();

            // Добавляем элемент для копирования
            entityCollection.Add(elementToCopy);

            // Создаем операцию
            circularCopyEntity.Create();

            return copyDefinition;
        }

        /// <summary>
        /// Создает коническую спираль для формирования спиральной канавки на хвостовике сверла.
        /// </summary>
        /// <param name="height">Высота спирали.</param>
        /// <param name="startDiameter">Начальный диаметр спирали (диаметр сверла).</param>
        /// <param name="tailLength">Длина хвостовика (используется для смещения начала спирали).</param>
        /// <returns>
        /// Объект конической спирали <see cref="ksEntity"/>.
        /// </returns>
        /// <remarks>
        /// Метод создает коническую спираль со следующими параметрами:
        /// <list type="bullet">
        /// <item><description>Способ построения: по высоте и виткам (buildMode = 2)</description></item>
        /// <item><description>Высота: указанное значение height</description></item>
        /// <item><description>Количество витков: 1</description></item>
        /// <item><description>Начальный диаметр: равен диаметру сверла</description></item>
        /// <item><description>Конечный диаметр: в 6.5 раз больше начального диаметра</description></item>
        /// <item><description>Направление навивки: правая спираль (turnDir = true)</description></item>
        /// <item><description>Начало спирали смещено по оси Z на значение длины хвостовика</description></item>
        /// </list>
        /// Коническая спираль используется для создания спиральной канавки на хвостовике сверла.
        /// </remarks>
        public ksEntity CreateConicSpiral(double height, double startDiameter, double tailLength)
        {
            ksEntity conicSpiral = _part.NewEntity((short)Obj3dType.o3d_conicSpiral);
            ksConicSpiralDefinition spiralDef = conicSpiral.GetDefinition();

            // Устанавливаем базовую плоскость (XOY) и смещаем по Z
            ksEntity basePlane = _part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            ksEntity offsetPlane = _part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetDef = offsetPlane.GetDefinition();
            offsetDef.SetPlane(basePlane);
            offsetDef.offset = -tailLength;  // Смещение начала спирали
            offsetDef.direction = true;
            offsetPlane.Create();

            spiralDef.SetPlane(offsetPlane);
            spiralDef.SetLocation(0, 0);

            spiralDef.buildMode = 2;              // 2 = по высоте и виткам
            spiralDef.height = height;            // Высота спирали
            spiralDef.turn = 1;                   // Количество витков
            spiralDef.initialDiam = startDiameter; // Начальный диаметр
            spiralDef.terminalDiam = startDiameter * 6.5;  // Конечный диаметр
            spiralDef.turnDir = true;             // true = правая спираль

            // Создаем спираль
            conicSpiral.Create();

            return conicSpiral;
        }
    }
}