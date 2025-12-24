using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORSAPR
{
    /// <summary>
    /// Класс для хранения и валидации параметров сверла.
    /// </summary>
    /// <remarks>
    /// Класс обеспечивает хранение параметров сверла, их валидацию
    /// в соответствии с заданными диапазонами и расчет зависимых параметров.
    /// Все параметры имеют защищенные сеттеры с проверкой валидности.
    /// </remarks>
    public class Parameters
    {
        /// <summary>
        /// Параметры сверла
        /// </summary>
        private double _angle;
        private bool _clearanceCone;
        private double _coneValue;
        private double _diameter;
        private double _length;
        private double _totalLength;

        /// <summary>
        /// Вычисленные диапазоны
        /// </summary>
        private const double _minAngle = 90;
        private const double _maxAngle = 140;
        private const double _minConeValue = 0.05;
        private const double _maxConeValue = 30;
        private const double _minDiameter = 1;
        private const double _maxDiameter = 20;
        private double _minLength;
        private double _maxLength;
        private double _minTotalLength;
        private double _maxTotalLength = 205;

        /// <summary>
        /// Минимальная величина угла при вершине
        /// </summary>
        public double MinAngle
        {
            get { return _minAngle; }
        }

        /// <summary>
        /// Максимальная величина угла при вершине
        /// </summary>
        public double MaxAngle
        {
            get { return _maxAngle; }
        }

        /// <summary>
        /// Минимальная величина обратного конуса
        /// </summary>
        public double MinConeValue
        {
            get { return _minConeValue; }
        }

        /// <summary>
        /// Максимальная величина обратного конуса
        /// </summary>
        public double MaxConeValue
        {
            get { return _maxConeValue; }
        }

        /// <summary>
        /// Минимальная величина диаметра
        /// </summary>
        public double MinDiameter
        {
            get { return _minDiameter; }
        }

        /// <summary>
        /// Максимальная величина диаметра
        /// </summary>
        public double MaxDiameter
        {
            get { return _maxDiameter; }
        }


        /// <summary>
        /// Минимальная длина рабочей части (3×d)
        /// </summary>
        public double MinLength
        {
            get { return _minLength; }
        }

        /// <summary>
        /// Максимальная длина рабочей части (8×d)
        /// </summary>
        public double MaxLength
        {
            get { return _maxLength; }
        }

        /// <summary>
        /// Минимальная общая длина (l + 20)
        /// </summary>
        public double MinTotalLength
        {
            get { return _minTotalLength; }
        }

        /// <summary>
        /// Максимальная общая длина (205)
        /// </summary>
        public double MaxTotalLength
        {
            get { return _maxTotalLength; }
        }

        /// <summary>
        /// Угол при вершине 90-140 градусов.
        /// </summary>
        /// <value>Значение угла в градусах.</value>
        /// <exception cref="ArgumentException">
        /// Выбрасывается при попытке установить значение вне допустимого диапазона 90-140.
        /// </exception>
        public double Angle
        {
            get { return _angle; }
            set
            {
                var error = ValidateAngle(value);
                if (!string.IsNullOrEmpty(error))
                    throw new ArgumentException(error);
                _angle = value;
            }
        }

        /// <summary>
        /// Наличие обратного конуса.
        /// </summary>
        /// <value>
        /// <c>true</c> - обратный конус присутствует; 
        /// <c>false</c> - обратный конус отсутствует.
        /// </value>
        public bool ClearanceCone
        {
            get
            {
                return _clearanceCone;
            }
            set
            {
                _clearanceCone = value;
            }
        }

        /// <summary>
        /// Значение обратного конуса от 0.05 до 30 мм.
        /// </summary>
        /// <value>Значение обратного конуса в миллиметрах.</value>
        /// <exception cref="ArgumentException">
        /// Выбрасывается при попытке установить значение вне допустимого диапазона 0.05-30.
        /// </exception>
        /// <remarks>
        /// Валидация этого параметра выполняется только если <see cref="ClearanceCone"/> равно <c>true</c>.
        /// </remarks>
        public double ConeValue
        {
            get { return _coneValue; }
            set
            {
                var error = ValidateConeValue(value);
                if (!string.IsNullOrEmpty(error))
                    throw new ArgumentException(error);
                _coneValue = value;
            }
        }

        /// <summary>
        /// Диаметр сверла от 1 до 20 мм.
        /// </summary>
        /// <value>Диаметр сверла в миллиметрах.</value>
        /// <exception cref="ArgumentException">
        /// Выбрасывается при попытке установить значение вне допустимого диапазона 1-20.
        /// </exception>
        /// <remarks>
        /// При изменении диаметра автоматически пересчитываются зависимые параметры:
        /// <see cref="MinLength"/>, <see cref="MaxLength"/>, <see cref="MinTotalLength"/>.
        /// </remarks>
        public double Diameter
        {
            get { return _diameter; }
            set
            {
                var error = ValidateDiameter(value);
                if (!string.IsNullOrEmpty(error))
                    throw new ArgumentException(error);
                _diameter = value;
                CalculateDepended();
            }
        }

        /// <summary>
        /// Длина рабочей части от 3×d до 8×d мм.
        /// </summary>
        /// <value>Длина рабочей части в миллиметрах.</value>
        /// <exception cref="ArgumentException">
        /// Выбрасывается при попытке установить значение вне допустимого диапазона,
        /// который зависит от текущего значения <see cref="Diameter"/>.
        /// </exception>
        /// <remarks>
        /// Допустимый диапазон вычисляется как 3×d - 8×d, где d - текущий диаметр сверла.
        /// При изменении длины автоматически пересчитываются зависимые параметры:
        /// <see cref="MinTotalLength"/>.
        /// </remarks>
        public double Length
        {
            get { return _length; }
            set
            {
                var error = ValidateWorkingLength(value);
                if (!string.IsNullOrEmpty(error))
                    throw new ArgumentException(error);
                _length = value;
                CalculateDepended();
            }
        }

        /// <summary>
        /// Общая длина сверла от l+20 до 205 мм.
        /// </summary>
        /// <value>Общая длина сверла в миллиметрах.</value>
        /// <exception cref="ArgumentException">
        /// Выбрасывается при попытке установить значение вне допустимого диапазона,
        /// который зависит от текущего значения <see cref="Length"/>.
        /// </exception>
        /// <remarks>
        /// Допустимый диапазон вычисляется как l+20 - 205, где l - текущая длина рабочей части.
        /// </remarks>
        public double TotalLength
        {
            get { return _totalLength; }
            set
            {
                var error = ValidateTotalLength(value);
                if (!string.IsNullOrEmpty(error))
                    throw new ArgumentException(error);
                _totalLength = value;
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Parameters"/> со значениями по умолчанию.
        /// </summary>
        /// <remarks>
        /// Устанавливаются следующие значения по умолчанию:
        /// <list type="bullet">
        /// <item><description>Угол при вершине: 90°</description></item>
        /// <item><description>Наличие обратного конуса: <c>true</c></description></item>
        /// <item><description>Значение обратного конуса: 0.1 мм</description></item>
        /// <item><description>Диаметр: 10 мм</description></item>
        /// <item><description>Длина рабочей части: 55 мм (среднее значение между 3×d и 8×d)</description></item>
        /// <item><description>Общая длина: 75 мм (минимальное значение между l+20 и 205)</description></item>
        /// </list>
        /// После инициализации автоматически вызывается метод <see cref="CalculateDepended"/>.
        /// </remarks>
        public Parameters()
        {
            _angle = 90.0;
            _clearanceCone = true;
            _coneValue = 0.1;
            _diameter = 10.0;
            _length = (3 * _diameter + 8 * _diameter) / 2;
            _totalLength = Math.Min(_length + 20, 205);
            CalculateDepended();
        }

        /// <summary>
        /// Расчёт зависимых параметров на основе текущих значений диаметра и длины.
        /// </summary>
        /// <remarks>
        /// Выполняет следующие расчеты:
        /// <list type="number">
        /// <item><description>Диапазон для длины рабочей части: 3×d - 8×d</description></item>
        /// <item><description>Диапазон для общей длины: l + 20 - 205</description></item>
        /// </list>
        /// Метод вызывается автоматически при изменении <see cref="Diameter"/> или <see cref="Length"/>.
        /// </remarks>
        private void CalculateDepended()
        {
            // 1. Расчет диапазона для длины рабочей части: 3×d - 8×d
            _minLength = 3 * _diameter;
            _maxLength = 8 * _diameter;

            // 2. Расчет диапазона для общей длины: l + 20 - 205
            _minTotalLength = _length + 20;
            _maxTotalLength = 205;
        }

        /// <summary>
        /// Выполняет валидацию угла при вершине сверла.
        /// </summary>
        /// <param name="value">Проверяемое значение угла в градусах.</param>
        /// <returns>
        /// Строку с сообщением об ошибке, если значение невалидно; 
        /// в противном случае - <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Допустимый диапазон: от <see cref="_minAngle"/> (90) до <see cref="_maxAngle"/> (140) градусов.
        /// </remarks>
        private string ValidateAngle(double value)
        {
            if (value < _minAngle || value > _maxAngle)
            {
                return $"Угол при вершине сверла должен быть в диапазоне {_minAngle}-{_maxAngle}";
            }

            return null;
        }

        /// <summary>
        /// Выполняет валидацию значения обратного конуса.
        /// </summary>
        /// <param name="value">Проверяемое значение обратного конуса в миллиметрах.</param>
        /// <returns>
        /// Строку с сообщением об ошибке, если значение невалидно; 
        /// в противном случае - <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Допустимый диапазон: от <see cref="_minConeValue"/> (0.05) до <see cref="_maxConeValue"/> (30) мм.
        /// </remarks>
        private string ValidateConeValue(double value)
        {
            if (value < _minConeValue || value > _maxConeValue)
            {
                return $"Значение обратного конуса должно быть в диапазоне {_minConeValue}-{_maxConeValue}";
            }

            return null;
        }

        /// <summary>
        /// Выполняет валидацию диаметра сверла.
        /// </summary>
        /// <param name="value">Проверяемое значение диаметра в миллиметрах.</param>
        /// <returns>
        /// Строку с сообщением об ошибке, если значение невалидно; 
        /// в противном случае - <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Допустимый диапазон: от <see cref="_minDiameter"/> (1) до <see cref="_maxDiameter"/> (20) мм.
        /// </remarks>
        private string ValidateDiameter(double value)
        {
            if (value < _minDiameter || value > _maxDiameter)
            {
                return $"Значение диаметра сверла должно быть в диапазоне {_minDiameter}-{_maxDiameter}";
            }

            return null;
        }

        /// <summary>
        /// Выполняет валидацию длины рабочей части сверла.
        /// </summary>
        /// <param name="value">Проверяемое значение длины в миллиметрах.</param>
        /// <returns>
        /// Строку с сообщением об ошибке, если значение невалидно; 
        /// в противном случае - <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Допустимый диапазон зависит от текущего значения диаметра:
        /// от <see cref="_minLength"/> (3×d) до <see cref="_maxLength"/> (8×d) мм.
        /// </remarks>
        private string ValidateWorkingLength(double value)
        {
            if (value < _minLength || value > _maxLength)
            {
                return $"Значение длины рабочей части сверла должно быть в диапазоне {_minLength}-{_maxLength}";
            }

            return null;
        }

        /// <summary>
        /// Выполняет валидацию общей длины сверла.
        /// </summary>
        /// <param name="value">Проверяемое значение общей длины в миллиметрах.</param>
        /// <returns>
        /// Строку с сообщением об ошибке, если значение невалидно; 
        /// в противном случае - <c>null</c>.
        /// </returns>
        /// <remarks>
        /// Допустимый диапазон зависит от текущей длины рабочей части:
        /// от <see cref="_minTotalLength"/> (l+20) до <see cref="_maxTotalLength"/> (205) мм.
        /// </remarks>
        private string ValidateTotalLength(double value)
        {
            if (value < _minTotalLength || value > _maxTotalLength)
            {
                return $"Значение длины сверла должно быть в диапазоне {_minTotalLength}-{_maxTotalLength}";
            }

            return null;
        }

        /// <summary>
        /// Выполняет комплексную валидацию всех параметров сверла.
        /// </summary>
        /// <returns>
        /// Список строк с сообщениями об ошибках валидации. 
        /// Возвращает пустой список, если все параметры валидны.
        /// </returns>
        /// <remarks>
        /// Метод выполняет следующую последовательность проверок:
        /// <list type="number">
        /// <item><description>Валидация угла при вершине (<see cref="ValidateAngle"/>)</description></item>
        /// <item><description>Если установлен обратный конус (<see cref="ClearanceCone"/> = <c>true</c>), 
        /// выполняется валидация значения обратного конуса (<see cref="ValidateConeValue"/>)</description></item>
        /// <item><description>Валидация диаметра сверла (<see cref="ValidateDiameter"/>)</description></item>
        /// <item><description>Если предыдущие проверки прошли успешно, выполняется валидация 
        /// длины рабочей части (<see cref="ValidateWorkingLength"/>) и общей длины (<see cref="ValidateTotalLength"/>)</description></item>
        /// </list>
        /// В случае возникновения исключения в процессе валидации, оно добавляется в список ошибок.
        /// </remarks>
        public List<string> ValidateAndCalculate()
        {
            var errors = new List<string>();

            try
            {
                string error;

                error = ValidateAngle(_angle);
                if (error != null) errors.Add(error);

                if (_clearanceCone)
                {
                    error = ValidateConeValue(_coneValue);
                    if (error != null) errors.Add(error);
                }

                error = ValidateDiameter(_diameter);
                if (error != null) errors.Add(error);

                if (errors.Count == 0)
                {
                    error = ValidateWorkingLength(_length);
                    if (error != null) errors.Add(error);

                    error = ValidateTotalLength(_totalLength);
                    if (error != null) errors.Add(error);
                }
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка при валидации: {ex.Message}");
            }
            return errors;
        }
    }
}