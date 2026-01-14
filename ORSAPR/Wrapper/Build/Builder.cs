using Kompas6API5;
using Kompas6Constants3D;
using KompasAPI7;
using System;
using System.IO;
using System.Security.Cryptography;

namespace ORSAPR
{
    /// <summary>
    /// Класс для построения 3D-модели сверла в КОМПАС-3D.
    /// </summary>
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
        public bool Build(Parameters parameters)
        {
            try
            {
                if (!InitializeKompas())
                {
                    return false;
                }

                CreateMainDrillBody(parameters);
                CreateSpiralFlutes(parameters);
                CreateConicalFlutes(parameters);
                CreateDrillPoint(parameters);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при построении: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Инициализирует подключение к КОМПАС-3D и создает документ.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если подключение успешно; 
        /// <c>false</c> - в случае ошибки.
        /// </returns>
        private bool InitializeKompas()
        {
            if (!_wrapper.ConnectCAD())
            {
                return false;
            }

            if (!_wrapper.CreateDocument())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Создает основное тело сверла.
        /// </summary>
        /// <param name="parameters">Параметры сверла.</param>
        private void CreateMainDrillBody(Parameters parameters)
        {
            ksEntity sketch = _wrapper.CreateSketch((short)Obj3dType.o3d_planeXOZ);
            ksDocument2D doc2D = _wrapper.BeginSketchEdit(sketch);

            double diameter = parameters.Diameter;
            double radius = diameter / 2;
            double totalLength = parameters.TotalLength;
            double workingLength = parameters.Length;
            double tailLength = totalLength - workingLength;

            if (parameters.ClearanceCone)
            {
                CreateProfileWithClearanceCone(doc2D, parameters, radius, totalLength, tailLength);
            }
            else
            {
                CreateProfileWithoutClearanceCone(doc2D, radius, totalLength);
            }

            _wrapper.EndSketchEdit(sketch);
            _wrapper.CreateRotation(sketch, (short)Direction_Type.dtReverse, false, 360);
        }

        /// <summary>
        /// Создает профиль сверла с обратным конусом.
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования.</param>
        /// <param name="parameters">Параметры сверла.</param>
        /// <param name="radius">Радиус сверла.</param>
        /// <param name="totalLength">Общая длина сверла.</param>
        /// <param name="tailLength">Длина хвостовика.</param>
        private void CreateProfileWithClearanceCone(ksDocument2D doc2D, Parameters parameters,
            double radius, double totalLength, double tailLength)
        {
            double cone = parameters.ConeValue / 2;

            double x1 = 0;
            double z1 = 0;
            double x2 = cone;
            double x3 = radius;
            double z3 = tailLength;
            double z4 = totalLength;

            _wrapper.DrawLineSeg(doc2D, x1, z1, x2, z1);
            _wrapper.DrawLineSeg(doc2D, x2, z1, x3, z3);
            _wrapper.DrawLineSeg(doc2D, x3, z3, x3, z4);
            _wrapper.DrawLineSeg(doc2D, x3, z4, x1, z4);
            _wrapper.DrawAxisLine(doc2D, x1, z3, x1, z1);
        }

        /// <summary>
        /// Создает профиль сверла без обратного конуса.
        /// </summary>
        /// <param name="doc2D">2D-документ для рисования.</param>
        /// <param name="radius">Радиус сверла.</param>
        /// <param name="totalLength">Общая длина сверла.</param>
        private void CreateProfileWithoutClearanceCone(ksDocument2D doc2D, double radius, double totalLength)
        {
            double x1 = 0;
            double z1 = 0;
            double x2 = radius;
            double z3 = totalLength;

            _wrapper.DrawLineSeg(doc2D, x1, z1, x2, z1);
            _wrapper.DrawLineSeg(doc2D, x2, z1, x2, z3);
            _wrapper.DrawLineSeg(doc2D, x2, z3, x1, z3);
            _wrapper.DrawAxisLine(doc2D, x1, z3, x1, z1);
        }

        /// <summary>
        /// Создает спиральные канавки на рабочей части сверла.
        /// </summary>
        /// <param name="parameters">Параметры сверла.</param>
        private void CreateSpiralFlutes(Parameters parameters)
        {
            double diameter = parameters.Diameter;
            double radius = diameter / 2;
            double totalLength = parameters.TotalLength;
            double workingLength = parameters.Length;

            ksEntity spiral = _wrapper.CreateDrillSpiral(workingLength, diameter, totalLength);
            ksEntity fluteProfile = _wrapper.CreateFluteProfile(totalLength, radius, 0.6);
            ksEntity firstFlute = _wrapper.CreateHelicalFlute(fluteProfile, spiral);
            _wrapper.CreateCircularCopy(2, firstFlute);
        }

        /// <summary>
        /// Создает конические спиральные канавки на хвостовике.
        /// </summary>
        /// <param name="parameters">Параметры сверла.</param>
        private void CreateConicalFlutes(Parameters parameters)
        {
            double diameter = parameters.Diameter;
            double radius = diameter / 2;
            double totalLength = parameters.TotalLength;
            double workingLength = parameters.Length;
            double tailLength = totalLength - workingLength;

            ksEntity conicSpiral = _wrapper.CreateConicSpiral(totalLength * 0.54, diameter, tailLength);
            ksEntity fluteProfile2 = _wrapper.CreateFluteProfile(tailLength, radius, 0.55);
            ksEntity secondFlut = _wrapper.CreateHelicalFlute(fluteProfile2, conicSpiral);
            _wrapper.CreateCircularCopy(2, secondFlut);
        }

        /// <summary>
        /// Создает угол при вершине сверла.
        /// </summary>
        /// <param name="parameters">Параметры сверла.</param>
        private void CreateDrillPoint(Parameters parameters)
        {
            double diameter = parameters.Diameter;
            double radius = diameter / 2;
            double totalLength = parameters.TotalLength;
            double angleRad = parameters.Angle * (Math.PI / 180);

            ksEntity sketchAngle = _wrapper.CreateSketch((short)Obj3dType.o3d_planeYOZ);
            ksDocument2D doc2DAngle = _wrapper.BeginSketchEdit(sketchAngle);

            DrawDrillPointProfile(doc2DAngle, radius, totalLength, angleRad);

            _wrapper.EndSketchEdit(sketchAngle);

            ksEntity cut = _wrapper.CutRotation(sketchAngle, (short)Direction_Type.dtNormal, true, 360);
            _wrapper.CreateCircularCopy(2, cut);
        }

        /// <summary>
        /// Рисует профиль угла при вершине сверла.
        /// </summary>
        /// <param name="doc2DAngle">2D-документ для рисования.</param>
        /// <param name="radius">Радиус сверла.</param>
        /// <param name="totalLength">Общая длина сверла.</param>
        /// <param name="angleRad">Угол при вершине в радианах.</param>
        private void DrawDrillPointProfile(ksDocument2D doc2DAngle, double radius, double totalLength, double angleRad)
        {
            double ya4 = 0;
            double za4 = totalLength;
            double ya1 = -radius * 2;
            double za1 = za4 - (ya4 - ya1) / Math.Tan(angleRad);
            double ya2 = ya1;
            double za2 = za4 + 5;
            double ya3 = radius * 2;
            double za3 = za2;

            _wrapper.DrawLineSeg(doc2DAngle, za1, ya1, za2, ya2);
            _wrapper.DrawLineSeg(doc2DAngle, za2, ya2, za3, ya3);
            _wrapper.DrawLineSeg(doc2DAngle, za3, ya3, za4, ya4);
            _wrapper.DrawLineSeg(doc2DAngle, za4, ya4, za1, ya1);
            _wrapper.DrawAxisLine(doc2DAngle, za1, ya1, za2, ya2);
        }
    }
}