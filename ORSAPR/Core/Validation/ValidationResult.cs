using Core.Model;
using Core.Validation;
using ORSAPR;

namespace Core.Model
{
    /// <summary>
    /// Результат валидации поля ввода.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Успешна ли валидация.
        /// </summary>
        public bool IsValid;

        /// <summary>
        /// Распарсенное числовое значение (если успешно).
        /// </summary>
        public double Value;

        /// <summary>
        /// Сообщение об ошибке (если есть).
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// Создает успешный результат валидации.
        /// </summary>
        /// <param name="value">Успешно распарсенное числовое
        /// значение.</param>
        /// <returns>Экземпляр <see cref="ValidationResult"/> с флагом 
        /// IsValid = true и установленным значением.</returns>
        public static ValidationResult Success(double value)
        {
            ValidationResult result = new ValidationResult();
            result.IsValid = true;
            result.Value = value;
            result.ErrorMessage = null;
            return result;
        }

        /// <summary>
        /// Создает результат валидации с ошибкой.
        /// </summary>
        /// <param name="error">Сообщение об ошибке, описывающее 
        /// причину неудачной валидации.</param>
        /// <returns>Экземпляр <see cref="ValidationResult"/> с флагом 
        /// IsValid = false и установленным сообщением об ошибке.</returns>
        public static ValidationResult Error(string error)
        {
            ValidationResult result = new ValidationResult();
            result.IsValid = false;
            result.Value = 0;
            result.ErrorMessage = error;
            return result;
        }
    }
}
