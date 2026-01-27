using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Validation
{
    /// <summary>
    /// Результат валидации конкретного поля с именем поля.
    /// </summary>
    public class FieldValidationResult
    {
        /// <summary>
        /// Название поля.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Результат валидации поля.
        /// </summary>
        public ValidationResult Result { get; }

        /// <summary>
        /// Инициализирует новый экземпляр класса FieldValidationResult.
        /// </summary>
        /// <param name="fieldName">Название поля.</param>
        /// <param name="result">Результат валидации.</param>
        public FieldValidationResult(string fieldName,
            ValidationResult result)
        {
            FieldName = fieldName;
            Result = result;
        }
    }
}
