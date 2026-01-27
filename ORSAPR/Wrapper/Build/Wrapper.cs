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
        /// Множитель конечного диаметра конической спирали.
        /// </summary>
        private const double ConicSpiralDiamFactor = 6.5;

        /// <summary>
        /// Подключается к запущенному экземпляру КОМПАС-3D
        /// или запускает новый.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если подключение к КОМПАС-3D выполнено успешно; 
        /// <c>false</c> - в случае ошибки подключения.
        /// </returns>
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
                    _kompas = (KompasObject)Marshal.GetActiveObject(
                        "KOMPAS.Application.5");
                }
                catch (COMException)
                {
                    var kompasType = Type.GetTypeFromProgID(
                        "KOMPAS.Application.5");
                    _kompas = (KompasObject)Activator.CreateInstance(
                        kompasType);
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
        public bool CreateDocument()
        {
            try
            {
                _document3D = (ksDocument3D)_kompas.Document3D();
                _document3D.Create();
                _document3D = (ksDocument3D)_kompas.ActiveDocument3D();
                _part = (ksPart)_document3D.GetPart(
                    (short)Part_Type.pTop_Part);
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
        /// <param name="plane">Тип плоскости для создания эскиза.</param>
        /// <returns>
        /// Объект эскиза <see cref="ksEntity"/> или <c>null</c> в случае
        /// ошибки.
        /// </returns>
        public ksEntity CreateSketch(short plane)
        {
            try
            {
                var currentPlane = (ksEntity)_part.GetDefaultEntity(plane);
                var entitySketch = (ksEntity)_part.NewEntity(
                    (short)Obj3dType.o3d_sketch);
                var sketchDefinition =
                    (ksSketchDefinition)entitySketch.GetDefinition();
                sketchDefinition.SetPlane(currentPlane);
                entitySketch.Create();
                return entitySketch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания эскиза: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Начинает редактирование эскиза.
        /// </summary>
        /// <param name="sketch">Эскиз для редактирования.</param>
        /// <returns>
        /// Объект 2D-документа <see cref="ksDocument2D"/> для рисования
        /// или <c>null</c> в случае ошибки.
        /// </returns>
        public ksDocument2D BeginSketchEdit(ksEntity sketch)
        {
            try
            {
                var sketchDefinition =
                    (ksSketchDefinition)sketch.GetDefinition();
                return sketchDefinition.BeginEdit();
                //TODO: ?? +
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Ошибка редактирования эскиза: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Завершает редактирование эскиза.
        /// </summary>
        /// <param name="sketch">Эскиз, редактирование которого
        /// завершается.</param>
        public void EndSketchEdit(ksEntity sketch)
        {
            try
            {
                var sketchDefinition =
                    (ksSketchDefinition)sketch.GetDefinition();
                sketchDefinition.EndEdit();
                //TODO: ?? +
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Ошибка завершения редактирования эскиза: {ex.Message}");
            }
        }

        /// <summary>
        /// Рисует отрезок линии в 2D-документе.
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования.</param>
        /// <param name="x1">X-координата начальной точки.</param>
        /// <param name="y1">Y-координата начальной точки.</param>
        /// <param name="x2">X-координата конечной точки.</param>
        /// <param name="y2">Y-координата конечной точки.</param>
        public void DrawLineSeg(ksDocument2D doc2D, double x1, double y1,
            double x2, double y2)
        {
            try
            {
                doc2D.ksLineSeg(x1, y1, x2, y2, 1);
                //TODO: ?? +
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка рисования линии: {ex.Message}");
            }
        }

        /// <summary>
        /// Рисует осевую линию в 2D-документе.
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования.</param>
        /// <param name="x1">X-координата начальной точки.</param>
        /// <param name="y1">Y-координата начальной точки.</param>
        /// <param name="x2">X-координата конечной точки.</param>
        /// <param name="y2">Y-координата конечной точки.</param>
        public void DrawAxisLine(ksDocument2D doc2D, double x1, double y1,
            double x2, double y2)
        {
            try
            {
                doc2D.ksLineSeg(x1, y1, x2, y2, 3);
                //TODO: ?? +
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Ошибка рисования осевой линии: {ex.Message}");
            }
        }

        /// <summary>
        /// Рисует дугу в 2D-документе эскиза по центральной точке,
        /// радиусу и углам.
        /// </summary>
        /// <param name="doc2D">2D-документ эскиза.</param>
        /// <param name="centerX">X-координата центра дуги.</param>
        /// <param name="centerY">Y-координата центра дуги.</param>
        /// <param name="radius">Радиус дуги.</param>
        /// <param name="startAngle">Начальный угол в градусах.</param>
        /// <param name="endAngle">Конечный угол в градусах.</param>
        /// <param name="style">Стиль линии.</param>
        public void DrawArc(ksDocument2D doc2D, double centerX,
            double centerY, double radius, double startAngle,
            double endAngle, int style = 1)
        {
            try
            {
                doc2D.ksArcByAngle(centerX, centerY, radius,
                    startAngle, endAngle, 1, style);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка рисования дуги: {ex.Message}");
            }
        }

        /// <summary>
        /// Создает операцию вращения для формирования тела вращения.
        /// </summary>
        /// <param name="sketch">Эскиз профиля для вращения.</param>
        /// <param name="direction">Направление вращения.</param>
        /// <param name="side">Направление выдавливания.</param>
        /// <param name="angle">Угол вращения в градусах.</param>
        /// <returns>
        /// Определение операции вращения <see cref="ksBaseRotatedDefinition"/>.
        /// </returns>
        public ksBaseRotatedDefinition CreateRotation(ksEntity sketch,
            short direction, bool side, int angle)
        {
            ksEntity entity = _part.NewEntity(
                (short)Obj3dType.o3d_baseRotated);
            ksBaseRotatedDefinition definition = entity.GetDefinition();
            definition.directionType = direction;
            definition.SetSideParam(side, angle);
            definition.SetSketch(sketch);
            entity.Create();
            return definition;
        }

        /// <summary>
        /// Создаёт операцию вырезать вращением.
        /// </summary>
        /// <param name="sketch">Эскиз профиля для вращения.</param>
        /// <param name="direction">Направление вращения.</param>
        /// <param name="cut">Признак результата операции.</param>
        /// <param name="angle">Угол вращения в градусах.</param>
        /// <returns>
        /// Объект выреза вращением <see cref="ksEntity"/>.
        /// </returns>
        public ksEntity CutRotation(ksEntity sketch, short direction,
            bool cut, int angle)
        {
            ksEntity entity = _part.NewEntity(
                (short)Obj3dType.o3d_cutRotated);
            ksCutRotatedDefinition definition = entity.GetDefinition();
            definition.directionType = direction;
            definition.SetSideParam(cut, angle);
            definition.SetSketch(sketch);
            entity.Create();
            return entity;
        }

        /// <summary>
        /// Создает цилиндрическую спираль для формирования спиральной
        /// канавки сверла.
        /// </summary>
        /// <param name="drillLength">Длина рабочей части сверла.</param>
        /// <param name="drillDiameter">Диаметр сверла.</param>
        /// <param name="totalLength">Общая длина сверла.</param>
        /// <returns>
        /// Объект цилиндрической спирали <see cref="ksEntity"/>.
        /// </returns>
        public ksEntity CreateDrillSpiral(double drillLength,
            double drillDiameter, double totalLength)
        {
            ksEntity spiralEntity = _part.NewEntity(
                (short)Obj3dType.o3d_cylindricSpiral);
            //TODO: {} +
            if (spiralEntity == null)
            {
                return null;
            }

            ksCylindricSpiralDefinition spiralDef =
                spiralEntity.GetDefinition();
            //TODO: {} +
            if (spiralDef == null)
            {
                return null;
            }

            spiralDef.buildMode = 2;
            spiralDef.height = drillLength;
            spiralDef.turn = 2;
            spiralDef.diam = drillDiameter;
            spiralDef.turnDir = true;
            spiralDef.firstAngle = 0;
            spiralDef.step = 0;
            spiralDef.heightAdd = 0;

            ksEntity basePlane = _part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);
            ksEntity offsetPlane = _part.NewEntity(
                (short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetDef = offsetPlane.GetDefinition();
            offsetDef.SetPlane(basePlane);
            offsetDef.direction = true;
            offsetDef.offset = -totalLength;
            offsetPlane.Create();

            spiralDef.SetPlane(offsetPlane);
            spiralDef.SetLocation(0, 0);

            spiralEntity.Create();

            return spiralEntity;
        }

        /// <summary>
        /// Создаёт эскиз профиля креплений хвостовика.
        /// </summary>
        /// <param name="shankLength1">Длина 1 крепления.</param>
        /// <param name="shankLength2">Длина 2 крепления.</param>
        /// <param name="shankRadius">Радиус хвостовика.</param>
        /// <param name="plane">Значение для определения оси.</param>
        /// <returns>Эскиз профиля крепления хвостовика.</returns>
        public ksEntity CreateCircleGuide(double shankLength1,
            double shankLength2, double shankRadius, bool plane)
        {
            double bindingRadius = shankRadius / 2;
            double arcPoint1 = shankLength1 - bindingRadius;
            double arcPoint2 = shankLength2 + bindingRadius;

            if (plane)
            {
                ksEntity basePlane = _part.GetDefaultEntity(
                    (short)Obj3dType.o3d_planeYOZ);
                ksEntity offsetPlane = _part.NewEntity(
                    (short)Obj3dType.o3d_planeOffset);
                ksPlaneOffsetDefinition offsetDef =
                    offsetPlane.GetDefinition();
                offsetDef.SetPlane(basePlane);
                offsetDef.offset = shankRadius;
                offsetDef.direction = true;
                offsetPlane.Create();

                ksEntity sketch = _part.NewEntity(
                    (short)Obj3dType.o3d_sketch);
                ksSketchDefinition sketchDef = sketch.GetDefinition();
                sketchDef.SetPlane(offsetPlane);
                sketch.Create();

                ksDocument2D doc2D = sketchDef.BeginEdit();

                double z1 = shankLength1;
                double y1 = 0;
                double z2 = shankLength2;
                double y2 = y1;
                double z3 = arcPoint1;
                double y3 = bindingRadius;
                double z4 = arcPoint2;
                double y4 = y3;

                DrawLineSeg(doc2D, z1, y1, z2, y2);
                DrawArc(doc2D, z3, y1, bindingRadius, 0, 90, 1);
                DrawLineSeg(doc2D, z3, y3, z4, y4);
                DrawArc(doc2D, z4, y2, bindingRadius, 90, 180, 1);
                DrawAxisLine(doc2D, z1, y1, z2, y2);

                sketchDef.EndEdit();

                return sketch;
            }
            else
            {
                ksEntity basePlane = _part.GetDefaultEntity(
                    (short)Obj3dType.o3d_planeXOZ);
                ksEntity offsetPlane = _part.NewEntity(
                    (short)Obj3dType.o3d_planeOffset);
                ksPlaneOffsetDefinition offsetDef =
                    offsetPlane.GetDefinition();
                offsetDef.SetPlane(basePlane);
                offsetDef.offset = shankRadius;
                offsetDef.direction = true;
                offsetPlane.Create();

                ksEntity sketch = _part.NewEntity(
                    (short)Obj3dType.o3d_sketch);
                ksSketchDefinition sketchDef = sketch.GetDefinition();
                sketchDef.SetPlane(offsetPlane);
                sketch.Create();

                ksDocument2D doc2D = sketchDef.BeginEdit();

                double x1 = 0;
                double z1 = shankLength1;
                double x2 = x1;
                double z2 = shankLength2;
                double x3 = bindingRadius;
                double z3 = arcPoint1;
                double x4 = x3;
                double z4 = arcPoint2;

                DrawLineSeg(doc2D, x1, z1, x2, z2);
                DrawArc(doc2D, x1, z3, bindingRadius, 0, 90, 1);
                DrawLineSeg(doc2D, x3, z3, x4, z4);
                DrawArc(doc2D, x2, z4, bindingRadius, 270, 0, 1);
                DrawAxisLine(doc2D, x1, z1, x2, z2);

                sketchDef.EndEdit();

                return sketch;
            }
        }

        /// <summary>
        /// Создает эскиз профиля спиральной канавки сверла.
        /// </summary>
        /// <param name="totalLength">Общая длина сверла.</param>
        /// <param name="radius">Радиус сверла.</param>
        /// <param name="depth">Глубина канавки.</param>
        /// <returns>
        /// Эскиз профиля канавки <see cref="ksEntity"/>.
        /// </returns>
        public ksEntity CreateFluteProfile(double totalLength, double radius,
            double depth)
        {
            ksEntity basePlane = _part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);
            ksEntity offsetPlane = _part.NewEntity(
                (short)Obj3dType.o3d_planeOffset);
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

            long circleHandle = doc2D.ksCircle(
                radius, 0, radius * depth, 1);

            sketchDef.EndEdit();

            return sketch;
        }

        /// <summary>
        /// Создает спиральную канавку методом эволюции по спиральной
        /// траектории.
        /// </summary>
        /// <param name="fluteProfile">Эскиз профиля канавки.</param>
        /// <param name="spiralPath">Спиральная траектория для эволюции.</param>
        /// <returns>
        /// Объект операции вырезания эволюцией <see cref="ksEntity"/> или 
        /// <c>null</c> в случае ошибки.
        /// </returns>
        public ksEntity CreateHelicalFlute(ksEntity fluteProfile,
            ksEntity spiralPath)
        {
            ksEntity cutEvolution = _part.NewEntity(
                (short)Obj3dType.o3d_cutEvolution);
            ksCutEvolutionDefinition cutEvolutionDef =
                cutEvolution.GetDefinition();

            cutEvolutionDef.SetSketch(fluteProfile);

            ksEntityCollection pathArray = cutEvolutionDef.PathPartArray();
            pathArray.Add(spiralPath);

            cutEvolutionDef.sketchShiftType = 0;
            cutEvolutionDef.chooseType = 1;
            cutEvolutionDef.cut = true;

            bool createResult = cutEvolution.Create();

            return createResult ? cutEvolution : null;
        }

        /// <summary>
        /// Создает операцию кругового копирования элементов.
        /// </summary>
        /// <param name="count">Количество копий.</param>
        /// <param name="elementToCopy">Элемент для копирования.</param>
        /// <returns>
        /// Определение операции кругового копирования 
        /// <see cref="ksCircularCopyDefinition"/>.
        /// </returns>
        public ksCircularCopyDefinition CreateCircularCopy(int count,
            ksEntity elementToCopy)
        {
            ksEntity circularCopyEntity = _part.NewEntity(
                (short)Obj3dType.o3d_circularCopy);
            ksCircularCopyDefinition copyDefinition =
                circularCopyEntity.GetDefinition();

            copyDefinition.SetCopyParamAlongDir(count, 180, true, false);

            ksEntity axisOz = _part.GetDefaultEntity(
                (short)Obj3dType.o3d_axisOZ);
            copyDefinition.SetAxis(axisOz);

            ksEntityCollection entityCollection =
                copyDefinition.GetOperationArray();
            entityCollection.Clear();
            entityCollection.Add(elementToCopy);

            circularCopyEntity.Create();

            return copyDefinition;
        }

        /// <summary>
        /// Создает коническую спираль для формирования спиральной канавки 
        /// на хвостовике сверла.
        /// </summary>
        /// <param name="height">Высота спирали.</param>
        /// <param name="startDiameter">Начальный диаметр спирали.</param>
        /// <param name="tailLength">Длина хвостовика.</param>
        /// <returns>
        /// Объект конической спирали <see cref="ksEntity"/>.
        /// </returns>
        public ksEntity CreateConicSpiral(double height, double startDiameter,
            double tailLength)
        {
            ksEntity conicSpiral = _part.NewEntity(
                (short)Obj3dType.o3d_conicSpiral);
            ksConicSpiralDefinition spiralDef = conicSpiral.GetDefinition();

            ksEntity basePlane = _part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);
            ksEntity offsetPlane = _part.NewEntity(
                (short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetDef = offsetPlane.GetDefinition();
            offsetDef.SetPlane(basePlane);
            offsetDef.offset = -tailLength;
            offsetDef.direction = true;
            offsetPlane.Create();

            spiralDef.SetPlane(offsetPlane);
            spiralDef.SetLocation(0, 0);

            spiralDef.buildMode = 2;
            spiralDef.height = height;
            spiralDef.turn = 1;
            spiralDef.initialDiam = startDiameter;
            spiralDef.terminalDiam = startDiameter * ConicSpiralDiamFactor;
            spiralDef.turnDir = true;

            conicSpiral.Create();

            return conicSpiral;
        }
    }
}