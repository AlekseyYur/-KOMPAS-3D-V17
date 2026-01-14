using System;
using System.Collections.Generic;

namespace ORSAPR
{
    /// <summary>
    /// Класс для хранения и валидации параметров сверла.
    /// </summary>
    public class Parameters
    {
        private double _angle;
        private bool _clearanceCone;
        private double _coneValue;
        private double _diameter;
        private double _length;
        private double _totalLength;

        private const double _minAngle = 30;
        private const double _maxAngle = 60;
        private const double _minDiameter = 1;
        private const double _maxDiameter = 20;
        private double _minLength;
        private double _maxLength;
        private double _minTotalLength;
        private const double _maxTotalLength = 205;
        private double _minConeValue;
        private double _maxConeValue;

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
        public double Diameter
        {
            get => _diameter;
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
        public double Length
        {
            get => _length;
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
        public double TotalLength
        {
            get => _totalLength;
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
        public Parameters()
        {
            _angle = 45.0;
            _diameter = 10.0;
            _clearanceCone = true;
            _coneValue = (_diameter * 0.25 + _diameter * 0.75) / 2;
            _length = (3 * _diameter + 8 * _diameter) / 2;
            _totalLength = Math.Min(_length + 20, 205);
            CalculateDepended();
        }

        /// <summary>
        /// Расчёт зависимых параметров на основе текущих значений диаметра и длины.
        /// </summary>
        private void CalculateDepended()
        {
            _minLength = 3 * _diameter;
            _maxLength = 8 * _diameter;
            _minTotalLength = _length + 20;
            _minConeValue = _diameter * 0.25;
            _maxConeValue = _diameter * 0.75;
        }

        /// <summary>
        /// Выполняет валидацию угла при вершине сверла.
        /// </summary>
        private string ValidateAngle(double value)
        {
            if (value < _minAngle || value > _maxAngle)
                return $"Угол при вершине сверла должен быть в диапазоне {_minAngle}-{_maxAngle}";
            return null;
        }

        /// <summary>
        /// Выполняет валидацию значения обратного конуса.
        /// </summary>
        private string ValidateConeValue(double value)
        {
            if (value < _minConeValue || value > _maxConeValue)
                return $"Значение обратного конуса должно быть в диапазоне {_minConeValue:F1}-{_maxConeValue:F1}";
            return null;
        }

        /// <summary>
        /// Выполняет валидацию диаметра сверла.
        /// </summary>
        private string ValidateDiameter(double value)
        {
            if (value < _minDiameter || value > _maxDiameter)
                return $"Значение диаметра сверла должно быть в диапазоне {_minDiameter}-{_maxDiameter}";
            return null;
        }

        /// <summary>
        /// Выполняет валидацию длины рабочей части сверла.
        /// </summary>
        private string ValidateWorkingLength(double value)
        {
            if (value < _minLength || value > _maxLength)
                return $"Значение длины рабочей части сверла должно быть в диапазоне {_minLength:F1}-{_maxLength:F1}";
            return null;
        }

        /// <summary>
        /// Выполняет валидацию общей длины сверла.
        /// </summary>
        private string ValidateTotalLength(double value)
        {
            if (value < _minTotalLength || value > _maxTotalLength)
                return $"Значение длины сверла должно быть в диапазоне {_minTotalLength:F1}-{_maxTotalLength:F1}";
            return null;
        }

        /// <summary>
        /// Выполняет комплексную валидацию всех параметров сверла.
        /// </summary>
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