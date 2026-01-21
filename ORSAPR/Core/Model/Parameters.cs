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
        /// Хвостовик.
        /// </summary>
        private double _shankDiameterValue;

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

        private double _minShankDiameterValue;

        private double _maxShankDiameterValue;

        private double _minShankLengthValue;

        private double _maxShankLengthValue;

        /// <summary>
        /// Формат отображения чисел с одним десятичным знаком.
        /// </summary>
        private const string NumberFormat = "F1";

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
        /// Диаметр хвостовика.
        /// </summary>
        public double ShankDiameterValue
        {
            get => _shankDiameterValue;
            set => _shankDiameterValue = value;
        }

        /// <summary>
        /// Длина хвостовика.
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
            _coneValue = (_diameter * 0.25 + _diameter * 0.75) / 2;
            _length = (3 * _diameter + 8 * _diameter) / 2;
            _totalLength = Math.Min(_length + 20, 205);

            _clearanceShank = false;
            _shankDiameterValue = (_diameter * 1.25 + _diameter * 2) / 2;
            _shankLengthValue = ((_totalLength - _length) * 2 +
                (_totalLength - _length) * 3) / 2;

            CalculateDepended();
        }

        /// <summary>
        /// Расчёт зависимых параметров на основе текущих значений 
        /// диаметра и длины.
        /// </summary>
        private void CalculateDepended()
        {
            _minLength = 3 * _diameter;
            _maxLength = 8 * _diameter;
            _minTotalLength = _length + 20;
            _minConeValue = _diameter * 0.25;
            _maxConeValue = _diameter * 0.75;
            _minShankDiameterValue = _diameter * 1.25;
            _maxShankDiameterValue = _diameter * 2;
            _minShankLengthValue = (_totalLength - _length) * 2;
            _maxShankLengthValue = (_totalLength - _length) * 3;
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
                          $"{_maxLength.ToString(NumberFormat)} мм (3×d - 8×d)");
            }

            // Проверка общей длины
            if (_totalLength < _minTotalLength ||
                _totalLength > MaxTotalLength)
            {
                errors.Add($"Общая длина должна быть в диапазоне " +
                          $"{_minTotalLength.ToString(NumberFormat)} - " +
                          $"{MaxTotalLength.ToString(NumberFormat)} мм " +
                          $"(L+20 - 205)");
            }

            // Проверка обратного конуса (если включен)
            if (_clearanceCone && (_coneValue < _minConeValue ||
                _coneValue > _maxConeValue))
            {
                errors.Add($"Значение обратного конуса должно быть в диапазоне " +
                          $"{_minConeValue.ToString(NumberFormat)} - " +
                          $"{_maxConeValue.ToString(NumberFormat)} мм " +
                          $"(0.25×d - 0.75×d)");
            }

            // Проверка диаметра хвостовика (если включен)
            if (_clearanceShank && (_shankDiameterValue < _minShankDiameterValue ||
                _shankDiameterValue > _maxShankDiameterValue))
            {
                errors.Add($"Значение диаметра хвостовика должно быть в диапазоне " +
                          $"{_minShankDiameterValue.ToString(NumberFormat)} - " +
                          $"{_maxShankDiameterValue.ToString(NumberFormat)} мм " +
                          $"(1.25×d - 2×d)");
            }

            // Проверка длины хвостовика (если включен)
            if (_clearanceShank && (_shankLengthValue < _minShankLengthValue ||
                _shankLengthValue > _maxShankLengthValue))
            {
                errors.Add($"Значение диаметра хвостовика должно быть в диапазоне " +
                          $"{_minShankLengthValue.ToString(NumberFormat)} - " +
                          $"{_maxShankLengthValue.ToString(NumberFormat)} мм " +
                          $"(1.25×d - 2×d)");
            }

            return errors;
        }
    }
}
