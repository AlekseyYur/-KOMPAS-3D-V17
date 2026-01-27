using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORSAPR
{
    /// <summary>
    /// Класс представляющий пресет параметров сверла.
    /// </summary>
    public class DrillPreset
    {

        /// <summary>
        /// Название пресета.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Диаметр.
        /// </summary>
        public double Diameter { get; }

        /// <summary>
        /// Длина рабочей части.
        /// </summary>
        public double Length { get; }

        /// <summary>
        /// Общая длина.
        /// </summary>
        public double TotalLength { get; }

        /// <summary>
        /// Угол при вершине.
        /// </summary>
        public double Angle { get; }

        /// <summary>
        /// Наличие обратного конуса.
        /// </summary>
        public bool HasCone { get; }

        /// <summary>
        /// Значение обратного конуса.
        /// </summary>
        public double ConeValue { get; }

        /// <summary>
        /// Наличие хвостовика.
        /// </summary>
        public bool HasShank { get; }

        /// <summary>
        /// Значение диаметра хвостовика.
        /// </summary>
        public double ShankDiameter { get; }

        /// <summary>
        /// Значение длины востовика.
        /// </summary>
        public double ShankLength { get; }

        /// <summary>
        /// Создаёт новый экземпляр пресета параметров сверла.
        /// </summary>
        /// <param name="name">Название пресета.</param>
        /// <param name="diameter">Диаметр.</param>
        /// <param name="length">Длина рабочей части.</param>
        /// <param name="totalLength">Общая длина.</param>
        /// <param name="angle">Угол при вершине.</param>
        /// <param name="hasCone">Наличие обратного конуса.</param>
        /// <param name="coneValue">Значение обратного конуса.</param>
        /// <param name="hasShank">Наличие хвостовика.</param>
        /// <param name="shankDiameter">Значение диаметра хвостовика.</param>
        /// <param name="shankLength">Значение длины хвостовика.</param>
        public DrillPreset(string name, double diameter, double length,
            double totalLength, double angle, bool hasCone = false,
            double coneValue = 0, bool hasShank = false,
            double shankDiameter = 0, double shankLength = 0)
        {
            Name = name;
            Diameter = diameter;
            Length = length;
            TotalLength = totalLength;
            Angle = angle;
            HasCone = hasCone;
            ConeValue = coneValue;
            HasShank = hasShank;
            ShankDiameter = shankDiameter;
            ShankLength = shankLength;
        }

        /// <summary>
        /// Возвращает название пресета для отображения в UI
        /// </summary>
        /// <returns>Название пресета</returns>
        public override string ToString() => Name;
    }
}
