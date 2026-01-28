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

        /// <summary>
        /// Формат отображения чисел с одним десятичным знаком.
        /// </summary>
        private const string NumberFormat = "F2";

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
            double min, double max, string unit, bool required = true)
        {
            // Проверка на пустое поле
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

            // Попытка парсинга
            if (!double.TryParse(input, NumberStyles.Any,
                CultureInfo.CurrentCulture, out double value))
            {
                return ValidationResult.Error(
                    $"Неверный формат числа в поле '{fieldName}'");
            }

            // Проверка диапазона
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
            return ValidateField(input, "Длина рабочей части",
                _parameters.MinLength, _parameters.MaxLength, "мм");
        }

        /// <summary>
        /// Валидирует поле общей длины.
        /// </summary>
        /// <param name="input">Введенное значение общей длины.</param>
        /// <returns>Результат валидации.</returns>
        public ValidationResult ValidateTotalLength(string input)
        {
            return ValidateField(input, "Общая длина",
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

            return ValidateField(input, "Обратный конус",
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

            return ValidateField(input, "Диаметр хвостовика",
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

            return ValidateField(input, "Длина хвостовика",
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
                    ValidateShankDiameterValue
                    (shankDiameterValue, shankEnabled)),
                new FieldValidationResult("Длина хвостовика",
                    ValidateShankLengthValue
                    (shankLengthValue, shankEnabled)),
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
        /// Унифицированный метод валидации и обновления параметров.
        /// </summary>
        /// <param name="input">Входное значение.</param>
        /// <param name="validationFunc">Функция валидации.</param>
        /// <param name="updateAction">Действие для обновления
        /// параметра.</param>
        /// <param name="errors">Список ошибок.</param>
        /// <param name="isEnabled">Флаг активности параметра
        /// (для опциональных полей).</param>
        /// <returns>True, если параметр успешно обновлён.</returns>
        private bool ValidateAndUpdate<T>(
            string input,
            Func<string, ValidationResult> validationFunc,
            Action<T> updateAction,
            List<string> errors,
            bool isEnabled = true)
        {
            var validationResult = validationFunc(input);
            //TODO: ??
            if (validationResult.IsValid)
            {
                updateAction((T)Convert.ChangeType
                    (validationResult.Value, typeof(T)));
                return true;
            }
            else
            {
                errors.Add(validationResult.ErrorMessage);
                return false;
            }
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
            allValid &= ValidateAndUpdate<double>(diameter, ValidateDiameter,
                x => _parameters.Diameter = x, errors);

            allValid &= ValidateAndUpdate<double>(length, ValidateLength,
                x => _parameters.Length = x, errors);

            allValid &= ValidateAndUpdate<double>(totalLength,
                ValidateTotalLength,
                x => _parameters.TotalLength = x, errors);

            allValid &= ValidateAndUpdate<double>(angle, ValidateAngle,
                x => _parameters.Angle = x, errors);

            _parameters.ClearanceCone = coneEnabled;
            allValid &= ValidateAndUpdate<double>(coneValue, 
                x => ValidateConeValue(x, coneEnabled), 
                x => _parameters.ConeValue = x, errors, coneEnabled);

            _parameters.ClearanceShank = shankEnabled;
            allValid &= ValidateAndUpdate<double>(shankDiameterValue, 
                x => ValidateShankDiameterValue(x, shankEnabled), 
                x => _parameters.ShankDiameterValue = x, errors,
                shankEnabled);

            allValid &= ValidateAndUpdate<double>(shankLengthValue, 
                x => ValidateShankLengthValue(x, shankEnabled),
                x => _parameters.ShankLengthValue = x, errors, shankEnabled);

            return allValid;
        }
    }
}
