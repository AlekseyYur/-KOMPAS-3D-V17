using ORSAPR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Schema;
using Core.Validation;
using System.Reflection.Emit;
using System.Runtime.InteropServices;


namespace Core.Model
{
    /// <summary>
    /// Класс для валидации полей ввода параметров сверла.
    /// </summary>
    public class ValidationField
    {
        /// <summary>
        /// Параметры сверла.
        /// </summary>
        private Parameters _parameters;

        //TODO: XML
        private const string NumberFormat = "F1";

        /// <summary>
        /// Конструктор класса ValidationField.
        /// </summary>
        /// <param name="parameters">Параметры сверла для валидации.</param>
        public ValidationField(Parameters parameters)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Валидирует поле ввода.
        /// </summary>
        /// <param name="input">Введенное значение.</param>
        /// <param name="fieldName">Название поля.</param>
        /// <param name="min">Минимальное допустимое значение.</param>
        /// <param name="max">Максимальное допустимое значение.</param>
        /// <param name="unit">Единица измерения.</param>
        /// <param name="requaride">Обязательность заполнения.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateField(string input, string fieldName,
            double min, double max, string unit, bool requaride = true)
        {
            //Проверка на пустое поле
            if (string.IsNullOrEmpty(input))
            {
                if (requaride == true)
                {
                    return ValidationResult.Error(
                        $"{fieldName} не может быть пустым");
                }
                else
                {
                    return ValidationResult.Success(0);
                }
            }

            //Попытка парсинга
            if (!double.TryParse(input, NumberStyles.Any,
                CultureInfo.CurrentCulture, out double value))
            {
                return ValidationResult.Error(
                    $"Неверный формат числа в поле '{fieldName}'");
            }

            //Проверка диапазона
            if (value < min || value > max)
            {
                return ValidationResult.Error(
                    $"{fieldName} должен быть в диапазоне " +
                    $"{min.ToString(NumberFormat)}-" +
                    $"{max.ToString(NumberFormat)} {unit}");
            }

            return ValidationResult.Success(value);
        }

        /// <summary>
        /// Валидирует зависимое поле ввода.
        /// </summary>
        /// <param name="input">Введенное значение.</param>
        /// <param name="fieldName">Название поля.</param>
        /// <param name="min">Минимальное допустимое значение.</param>
        /// <param name="max">Максимальное допустимое значение.</param>
        /// <param name="unit">Единица измерения.</param>
        /// <param name="required">Обязательность заполнения.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateDependentField(string input,
            string fieldName, double min, double max, string unit,
            bool required = true)
        {
            if (string.IsNullOrEmpty(input))
            {
                if (required)
                {
                    return ValidationResult.Error(
                        $"{fieldName} не может быть пустым");
                }
                else
                {
                    return ValidationResult.Success(0);
                }
            }

            if (!double.TryParse(input, NumberStyles.Any,
                CultureInfo.CurrentCulture, out double value))
            {
                return ValidationResult.Error(
                    $"Неверный формат числа в поле '{fieldName}'");
            }

            if (value < min || value > max)
            {
                return ValidationResult.Error(
                    $"{fieldName} должен быть в диапазоне " +
                    $"{min.ToString(NumberFormat)}-" +
                    $"{max.ToString(NumberFormat)} {unit}");
            }

            return ValidationResult.Success(value);
        }

        /// <summary>
        /// Валидирует поле диаметра сверла.
        /// </summary>
        /// <param name="input">Введенное значение диаметра.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateDiameter(string input)
        {
            return ValidateField(input, "Диаметр",
                _parameters.MinDiameter, _parameters.MaxDiameter, "мм");
        }

        /// <summary>
        /// Валидирует поле длины рабочей части.
        /// </summary>
        /// <param name="input">Введенное значение длины.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateLength(string input)
        {
            return ValidateDependentField(input, "Длина рабочей части",
                _parameters.MinLength, _parameters.MaxLength, "мм");
        }

        /// <summary>
        /// Валидирует поле общей длины.
        /// </summary>
        /// <param name="input">Введенное значение общей длины.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateTotalLength(string input)
        {
            return ValidateDependentField(input, "Общая длина",
                _parameters.MinTotalLength, _parameters.MaxTotalLength, "мм");
        }

        /// <summary>
        /// Валидирует поле угла при вершине.
        /// </summary>
        /// <param name="input">Введенное значение угла.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateAngle(string input)
        {
            return ValidateField(input, "Угол при вершине",
                _parameters.MinAngle, _parameters.MaxAngle, "°");
        }

        /// <summary>
        /// Проверка поля обратного конуса.
        /// </summary>
        /// <param name="input">Введенное значение конуса.</param>
        /// <param name="isConeEnabled">Включен ли обратный конус.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateConeValue(string input,
            bool isConeEnabled)
        {
            if (!isConeEnabled)
            {
                return ValidationResult.Success(0);
            }

            return ValidateDependentField(input, "Обратный конус",
                _parameters.MinConeValue, _parameters.MaxConeValue, "мм");
        }

        /// <summary>
        /// Проверка поля диаметра хвостовика.
        /// </summary>
        /// <param name="input">Введенное значение диаметра хвостовика.</param>
        /// <param name="isShankEnabled">Включен ли хвостовик.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateShankDiameterValue(string input,
            bool isShankEnabled)
        {
            if (!isShankEnabled)
            {
                return ValidationResult.Success(0);
            }

            return ValidateDependentField(input, "Диаметр хвостовика",
                _parameters.MinShankDiameterValue,
                _parameters.MaxShankDiameterValue, "мм");
        }

        /// <summary>
        /// Проверка поля длины хвостовика.
        /// </summary>
        /// <param name="input">Введенное значение длины хвостовика.</param>
        /// <param name="isShankEnabled">Включен ли хвостовик.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateShankLengthValue(string input,
            bool isShankEnabled)
        {
            if (!isShankEnabled)
            {
                return ValidationResult.Success(0);
            }

            return ValidateDependentField(input, "Длина хвостовика",
                _parameters.MinShankLengthValue,
                _parameters.MaxShankLengthValue, "мм");
        }

        /// <summary>
        /// Комплексная проверка всех полей.
        /// </summary>
        /// <param name="diameter">Диаметр сверла.</param>
        /// <param name="length">Длина рабочей части.</param>
        /// <param name="totalLength">Общая длина.</param>
        /// <param name="angle">Угол при вершине.</param>
        /// <param name="coneValue">Значение обратного конуса.</param>
        /// <param name="coneEnabled">Включен ли обратный конус.</param>
        /// <param name="shankDiameterValue">Диаметр хвостовика.</param>
        /// <param name="shankLengthValue">Длина хвостовика.</param>
        /// <param name="shankEnabled">Включен ли хвостовик.</param>
        /// <returns>Список результатов валидации для каждого поля.</returns>
        public List<FieldValidationResult> ValidateAllFields(
            string diameter, string length, string totalLength,
            string angle, string coneValue, bool coneEnabled,
            string shankDiameterValue, string shankLengthValue,
            bool shankEnabled)
        {
            var results = new List<FieldValidationResult>
            {
                new FieldValidationResult("Диаметр",
                    ValidateDiameter(diameter)),
                new FieldValidationResult("Длина рабочей части",
                    ValidateLength(length)),
                new FieldValidationResult("Общая длина",
                    ValidateTotalLength(totalLength)),
                new FieldValidationResult("Угол при вершине",
                    ValidateAngle(angle)),
                new FieldValidationResult("Обратный конус",
                    ValidateConeValue(coneValue, coneEnabled)),
                new FieldValidationResult("Диаметр хвостовика",
                    ValidateConeValue(shankDiameterValue, shankEnabled)),
                new FieldValidationResult("Длина хвостовика",
                    ValidateConeValue(shankLengthValue, shankEnabled)),
            };

            return results;
        }

        /// <summary>
        /// Валидирует все поля и возвращает список ошибок.
        /// </summary>
        /// <param name="diameter">Диаметр сверла.</param>
        /// <param name="length">Длина рабочей части.</param>
        /// <param name="totalLength">Общая длина.</param>
        /// <param name="angle">Угол при вершине.</param>
        /// <param name="coneValue">Значение обратного конуса.</param>
        /// <param name="coneEnabled">Включен ли обратный конус.</param>
        /// <param name="shankDiameterValue">Диаметр хвостовика.</param>
        /// <param name="shankLengthValue">Длина хвостовика.</param>
        /// <param name="shankEnabled">Включен ли хвостовик.</param>
        /// <returns>Список сообщений об ошибках.</returns>
        public List<string> ValidateAllFieldsWithErrors(
            string diameter, string length, string totalLength,
            string angle, string coneValue, bool coneEnabled,
            string shankDiameterValue, string shankLengthValue,
            bool shankEnabled)
        {
            var results = ValidateAllFields(diameter, length, totalLength,
                angle, coneValue, coneEnabled, shankDiameterValue,
                shankLengthValue, shankEnabled);

            var errors = new List<string>();

            foreach (var result in results)
            {
                if (!result.Result.IsValid &&
                    !string.IsNullOrEmpty(result.Result.ErrorMessage))
                {
                    errors.Add(result.Result.ErrorMessage);
                }
            }

            // Дополнительная проверка: если конус включен, но его значение 0
            if (coneEnabled && double.TryParse(coneValue,
                out double coneVal) && coneVal == 0)
            {
                errors.Add("Обратный конус не может быть равен 0 " +
                    "при включенном флаге");
            }

            // Дополнительная проверка: если хвостовик включен, 
            // но значение его диаметра 0
            else if (shankEnabled && double.TryParse(shankDiameterValue,
                out double shankDiameterVal) && shankDiameterVal == 0)
            {
                errors.Add("Диаметр хвостовика не может быть равен 0 " +
                    "при включенном флаге");
            }

            // Дополнительная проверка: если хвостовик включен, 
            // но значение его длины 0
            else if (shankEnabled && double.TryParse(shankLengthValue,
                out double shankLengthVal) && shankLengthVal == 0)
            {
                errors.Add("Длина хвостовика не может быть равен 0 " +
                    "при включенном флаге");
            }

            return errors;
        }

        /// <summary>
        /// Пытается обновить параметры на основе введенных значений.
        /// </summary>
        /// <param name="diameter">Диаметр сверла.</param>
        /// <param name="length">Длина рабочей части.</param>
        /// <param name="totalLength">Общая длина.</param>
        /// <param name="angle">Угол при вершине.</param>
        /// <param name="coneValue">Значение обратного конуса.</param>
        /// <param name="coneEnabled">Включен ли обратный конус.</param>
        /// <param name="shankDiameterValue">Диаметр хвостовика.</param>
        /// <param name="shankLengthValue">Длина хвостовика.</param>
        /// <param name="shankEnabled">Включен ли хвостовик.</param>
        /// <param name="errors">Список ошибок валидации.</param>
        /// <returns>True, если все параметры успешно обновлены.</returns>
        public bool TryUpdateParameters(string diameter, string length,
            string totalLength, string angle, string coneValue,
            bool coneEnabled, string shankDiameterValue,
            string shankLengthValue, bool shankEnabled,
            out List<string> errors)
        {
            errors = new List<string>();
            bool allValid = true;

            // Валидируем и обновляем каждое поле
            //TODO: duplication
            var diameterResult = ValidateDiameter(diameter);
            if (diameterResult.IsValid)
            {
                _parameters.Diameter = diameterResult.Value;
            }
            else
            {
                errors.Add(diameterResult.ErrorMessage);
                allValid = false;
            }

            //TODO: duplication
            var lengthResult = ValidateLength(length);
            if (lengthResult.IsValid)
            {
                _parameters.Length = lengthResult.Value;
            }
            else
            {
                errors.Add(lengthResult.ErrorMessage);
                allValid = false;
            }

            //TODO: duplication
            var totalLengthResult = ValidateTotalLength(totalLength);
            if (totalLengthResult.IsValid)
            {
                _parameters.TotalLength = totalLengthResult.Value;
            }
            else
            {
                errors.Add(totalLengthResult.ErrorMessage);
                allValid = false;
            }

            //TODO: duplication
            var angleResult = ValidateAngle(angle);
            if (angleResult.IsValid)
            {
                _parameters.Angle = angleResult.Value;
            }
            else
            {
                errors.Add(angleResult.ErrorMessage);
                allValid = false;
            }

            _parameters.ClearanceCone = coneEnabled;
            //TODO: duplication
            var coneResult = ValidateConeValue(coneValue, coneEnabled);
            if (coneResult.IsValid)
            {
                _parameters.ConeValue = coneResult.Value;
            }
            else if (coneEnabled)
            {
                errors.Add(coneResult.ErrorMessage);
                allValid = false;
            }

            _parameters.ClearanceShank = shankEnabled;
            //TODO: duplication
            var shankDiameterResult = ValidateShankDiameterValue(
                shankDiameterValue, shankEnabled);
            if (shankDiameterResult.IsValid)
            {
                _parameters.ShankDiameterValue = shankDiameterResult.Value;
            }
            else if (shankEnabled)
            {
                errors.Add(shankDiameterResult.ErrorMessage);
                allValid = false;
            }
            var shankLengthResult = ValidateShankLengthValue(
                shankLengthValue, shankEnabled);
            //TODO: duplication
            if (shankLengthResult.IsValid)
            {
                _parameters.ShankLengthValue = shankLengthResult.Value;
            }
            else if (shankEnabled)
            {
                errors.Add(shankLengthResult.ErrorMessage);
                allValid = false;
            }

            return allValid;
        }
    }
}
