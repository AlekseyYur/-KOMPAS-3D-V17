using System;
using System.Collections.Generic;
using System.Security;

namespace ORSAPR
{
    /// <summary>
    /// Класс для хранения и валидации параметров сверла.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Угол при вершине (a).
        /// </summary>
        private double _angle;

        /// <summary>
        /// Наличие обратного конуcа.
        /// </summary>
        private bool _clearanceCone;

        /// <summary>
        /// Обратный конус.
        /// </summary>
        private double _coneValue;

        /// <summary>
        /// Начилие хвостовика.
        /// </summary>
        private bool _clearanceShank;

        /// <summary>
        /// Диаметр хвостовика.
        /// </summary>
        private double _shankDiameterValue;

        /// <summary>
        /// Длина хвостовика.
        /// </summary>
        private double _shankLengthValue;

        /// <summary>
        /// Диаметр (d).
        /// </summary>
        private double _diameter;

        /// <summary>
        /// Рабочая часть (l).
        /// </summary>
        private double _length;

        /// <summary>
        /// Общая длина (L).
        /// </summary>
        private double _totalLength;

        /// <summary>
        /// Минимальное доступное значение угла при вершине.
        /// </summary>
        private const double _minAngle = 30;

        /// <summary>
        /// Максимальное доступное значение угла при вершине.
        /// </summary>
        private const double _maxAngle = 60;

        /// <summary>
        /// Миинмальное доступное значение диаметра.
        /// </summary>
        private const double _minDiameter = 1;

        /// <summary>
        /// Максимальное доступное значение диаметра.
        /// </summary>
        private const double _maxDiameter = 20;

        /// <summary>
        /// Минимальное доступное значение рабочей части.
        /// </summary>
        private double _minLength;

        /// <summary>
        /// Максимальное доступное значение рабочей части.
        /// </summary>
        private double _maxLength;

        /// <summary>
        /// Минимальное досутпное значение общей длины.
        /// </summary>
        private double _minTotalLength;

        /// <summary>
        /// Максимальное доступное значение общей длины.
        /// </summary>
        private const double _maxTotalLength = 205;

        /// <summary>
        /// Минимальное доступное значение обратного конуса.
        /// </summary>
        private double _minConeValue;

        /// <summary>
        /// Максимальное доступное значение обратного конуса.
        /// </summary>
        private double _maxConeValue;

        /// <summary>
        /// Минимальное доступное значение диаметра хвостовика.
        /// </summary>
        private double _minShankDiameterValue;

        /// <summary>
        /// Максимальное доступное значение диаметра хвостовика.
        /// </summary>
        private double _maxShankDiameterValue;

        /// <summary>
        /// Минимальное доступное значение длины хвостовика.
        /// </summary>
        private double _minShankLengthValue;

        /// <summary>
        /// Максимальное доступное значение длины хвостовика.
        /// </summary>
        private double _maxShankLengthValue;

        /// <summary>
        /// Формат отображения чисел с двумя десятичными знаками.
        /// </summary>
        private const string NumberFormat = "F2";

        /// <summary>
        /// Первый коэфициент для вычисления зависимости 
        /// обратного конуса от диаметра.
        /// </summary>
        private const double CoefficientForDependenciesCone1 = 0.25;

        /// <summary>
        /// Второй коэфициент для вычисления зависимости
        /// обратного конуса от диаметра.
        /// </summary>
        private const double CoefficientForDependenciesCone2 = 0.75;

        /// <summary>
        /// Первый коэфициент для вычисления зависимостей длины.
        /// </summary>
        private const double CoefficientForDependenciesLength1 = 3;

        /// <summary>
        /// Второй коэфициент для вычисления зависимостей длины.
        /// </summary>
        private const double CoefficientForDependenciesLength2 = 8;

        /// <summary>
        /// Третий коэфициент для вычисления зависимостей длины.
        /// </summary>
        private const double CoefficientForDependenciesLength3 = 20;

        /// <summary>
        /// Общий коэфициент для вычисления зависимостей.
        /// </summary>
        private const double CoefficientForDependencies = 2;

        /// <summary>
        /// Коэфициент для вычисления зависимости хвостовика от диаметра.
        /// </summary>
        private const double CoefficientForDependenciesShank = 1.75;

        /// <summary>
        /// Минимальная величина угла при вершине.
        /// </summary>
        public double MinAngle => _minAngle;

        /// <summary>
        /// Максимальная величина угла при вершине.
        /// </summary>
        public double MaxAngle => _maxAngle;

        /// <summary>
        /// Минимальная величина обратного конуса.
        /// </summary>
        public double MinConeValue => _minConeValue;

        /// <summary>
        /// Максимальная величина обратного конуса.
        /// </summary>
        public double MaxConeValue => _maxConeValue;

        /// <summary>
        /// Минимальный диаметр хвостовика.
        /// </summary>
        public double MinShankDiameterValue => _minShankDiameterValue;

        /// <summary>
        /// Максимальный диаметр хвостовика.
        /// </summary>
        public double MaxShankDiameterValue => _maxShankDiameterValue;

        /// <summary>
        /// Минимальная длина хвостовика.
        /// </summary>
        public double MinShankLengthValue => _minShankLengthValue;

        /// <summary>
        /// Максимальная длина хвостовика.
        /// </summary>
        public double MaxShankLengthValue => _maxShankLengthValue;

        /// <summary>
        /// Минимальная величина диаметра.
        /// </summary>
        public double MinDiameter => _minDiameter;

        /// <summary>
        /// Максимальная величина диаметра.
        /// </summary>
        public double MaxDiameter => _maxDiameter;

        /// <summary>
        /// Минимальная длина рабочей части (3×d).
        /// </summary>
        public double MinLength => _minLength;

        /// <summary>
        /// Максимальная длина рабочей части (8×d).
        /// </summary>
        public double MaxLength => _maxLength;

        /// <summary>
        /// Минимальная общая длина (l + 20).
        /// </summary>
        public double MinTotalLength => _minTotalLength;

        /// <summary>
        /// Максимальная общая длина (205).
        /// </summary>
        public double MaxTotalLength => _maxTotalLength;

        /// <summary>
        /// Угол при вершине 30-60 градусов.
        /// </summary>
        public double Angle
        {
            get => _angle;
            set => _angle = value;
        }

        /// <summary>
        /// Наличие обратного конуса.
        /// </summary>
        public bool ClearanceCone
        {
            get => _clearanceCone;
            set => _clearanceCone = value;
        }

        /// <summary>
        /// Значение обратного конуса от 0.25×d до 0.75×d мм.
        /// </summary>
        public double ConeValue
        {
            get => _coneValue;
            set => _coneValue = value;
        }

        /// <summary>
        /// Наличие хвостовика.
        /// </summary>
        public bool ClearanceShank
        {
            get => _clearanceShank;
            set => _clearanceShank = value;
        }

        /// <summary>
        /// Диаметр хвостовика от 1.75×d до 2xd.
        /// </summary>
        public double ShankDiameterValue
        {
            get => _shankDiameterValue;
            set => _shankDiameterValue = value;
        }


        /// <summary>
        /// Длина хвостовика от 2×(L-l) до 3×(L-l)
        /// </summary>
        public double ShankLengthValue
        {
            get => _shankLengthValue;
            set => _shankLengthValue = value;
        }

        /// <summary>
        /// Диаметр сверла от 1 до 20 мм.
        /// </summary>
        public double Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value;
                CalculateDepended();
            }
        }

        /// <summary>
        /// Длина рабочей части от 3×d до 8×d мм.
        /// </summary>
        public double Length
        {
            get => _length;
            set
            {
                _length = value;
                CalculateDepended();
            }
        }

        /// <summary>
        /// Общая длина сверла от l+20 до 205 мм.
        /// </summary>
        public double TotalLength
        {
            get => _totalLength;
            set
            {
                _totalLength = value;
                CalculateDepended();
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Parameters"/> 
        /// со значениями по умолчанию.
        /// </summary>
        public Parameters()
        {
            // Значения по умолчанию.
            _angle = 45.0;
            _diameter = 10.0;
            _clearanceCone = true;

            // Средние значения зависимых параметров
            _coneValue = (_diameter * CoefficientForDependenciesCone1 + 
                _diameter * CoefficientForDependenciesCone2)
                / CoefficientForDependencies;
            _length = (CoefficientForDependenciesLength1 * 
                _diameter + CoefficientForDependenciesLength2 * 
                _diameter) / CoefficientForDependencies;
            _totalLength = Math.Min(_length +
                CoefficientForDependenciesLength3, 205);

            _clearanceShank = false;
            _shankDiameterValue = (_diameter 
                * CoefficientForDependenciesShank + _diameter
                * CoefficientForDependencies) / CoefficientForDependencies;
            _shankLengthValue = ((_totalLength - _length) * 
                CoefficientForDependencies + (_totalLength - _length)
                * CoefficientForDependenciesLength1)
                / CoefficientForDependencies;

            CalculateDepended();
        }

        /// <summary>
        /// Расчёт зависимых параметров на основе текущих значений 
        /// диаметра и длины.
        /// </summary>
        private void CalculateDepended()
        {
            _minLength = CoefficientForDependenciesLength1 * _diameter;
            _maxLength = CoefficientForDependenciesLength2 * _diameter;
            _minTotalLength = _length + CoefficientForDependenciesLength3;
            _minConeValue = _diameter * CoefficientForDependenciesCone1;
            _maxConeValue = _diameter * CoefficientForDependenciesCone2;
            _minShankDiameterValue = _diameter * 
                CoefficientForDependenciesShank;
            _maxShankDiameterValue = _diameter * 
                CoefficientForDependencies;
            _minShankLengthValue = (_totalLength - _length) * 
                CoefficientForDependencies;
            _maxShankLengthValue = (_totalLength - _length) * 
                CoefficientForDependenciesLength1;
        }

        /// <summary>
        /// Валидирует бизнес-правила (зависимости между полями).
        /// </summary>
        /// <returns>Список ошибок валидации правил.</returns>
        public List<string> ValidateRules()
        {
            var errors = new List<string>();

            //Проверка зависимости длины рабочей части от диаметра
            if (_length < MinLength || _length > MaxLength)
            {
                errors.Add($"Длина рабочей части должна быть в диапазоне " +
                   $"{_minLength.ToString(NumberFormat)} - " +
                   $"{_maxLength.ToString(NumberFormat)} мм " +
                   $"({CoefficientForDependenciesLength1}" +
                   $"×{_diameter.ToString(NumberFormat)}" +
                   $" - {CoefficientForDependenciesLength2}" +
                   $"×{_diameter.ToString(NumberFormat)})");
            }

            // Проверка общей длины
            if (_totalLength < _minTotalLength ||
                _totalLength > MaxTotalLength)
            {
                errors.Add($"Общая длина должна быть в диапазоне " +
                   $"{_minTotalLength.ToString(NumberFormat)} - " +
                   $"{MaxTotalLength.ToString(NumberFormat)} мм " +
                   $"({_length.ToString(NumberFormat)}+" +
                   $"{CoefficientForDependenciesLength3} - " +
                   $"{MaxTotalLength})");
            }

            // Проверка обратного конуса (если включен)
            if (_clearanceCone && (_coneValue < _minConeValue ||
                _coneValue > _maxConeValue))
            {
                errors.Add($"Значение обратного конуса должно быть в " +
                  $"диапазоне {_minConeValue.ToString(NumberFormat)} - " +
                  $"{_maxConeValue.ToString(NumberFormat)} мм " +
                  $"({CoefficientForDependenciesCone1}" +
                  $"×{_diameter.ToString(NumberFormat)} - " +
                  $"{CoefficientForDependenciesCone2}" +
                  $"×{_diameter.ToString(NumberFormat)})");
            }

            // Проверка диаметра хвостовика (если включен)
            if (_clearanceShank && (_shankDiameterValue < _minShankDiameterValue ||
                _shankDiameterValue > _maxShankDiameterValue))
            {
                errors.Add($"Значение диаметра хвостовика должно быть в " +
                  $"диапазоне" +
                  $" {_minShankDiameterValue.ToString(NumberFormat)} - " +
                  $"{_maxShankDiameterValue.ToString(NumberFormat)} мм " +
                  $"({CoefficientForDependenciesShank}" +
                  $"×{_diameter.ToString(NumberFormat)} - " +
                  $"{CoefficientForDependencies}" +
                  $"×{_diameter.ToString(NumberFormat)})");
            }

            // Проверка длины хвостовика (если включен)
            if (_clearanceShank && (_shankLengthValue < _minShankLengthValue ||
                _shankLengthValue > _maxShankLengthValue))
            {
                double difference = _totalLength - _length;
                errors.Add($"Длина хвостовика должна быть в диапазоне " + 
                    $"{_minShankLengthValue.ToString(NumberFormat)}" +
                    $" - " +
                    $"{_maxShankLengthValue.ToString(NumberFormat)}" +
                    $" мм ({CoefficientForDependencies}" +
                    $"×({_totalLength.ToString(NumberFormat)}-" +
                    $"{_length.ToString(NumberFormat)}) - " +
                    $"{CoefficientForDependenciesLength1}" +
                    $"×({_totalLength.ToString(NumberFormat)}-" +
                    $"{_length.ToString(NumberFormat)}))");
            }

            return errors;
        }
    }
}
