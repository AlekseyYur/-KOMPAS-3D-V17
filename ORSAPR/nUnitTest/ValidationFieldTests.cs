using Core.Model;
using Core.Validation;
using ORSAPR;
using System.Globalization;
using System.Reflection;

namespace nUnitTest
{
    [TestFixture]
    public class ValidationFieldTests
    {
        ///<summary>
        /// Тестовый ввод диаметра.
        ///</summary>
        private const string TestDiameterInput = "10,0";

        /// <summary>
        /// Тестовый ввод длины рабочей части.
        /// </summary>
        private const string TestLengthInput = "55,0";

        /// <summary>
        /// Тестовый ввод общей длины.
        /// </summary>
        private const string TestTotalLengthInput = "75,0";

        /// <summary>
        /// Тестовый ввод угла при вершине.
        /// </summary>
        private const string TestAngleInput = "45,0";

        /// <summary>
        /// Тестовый ввод обратного конуса.
        /// </summary>
        private const string TestConeValueInput = "5,0";

        /// <summary>
        /// Тестовый ввод диаметра хвостовика.
        /// </summary>
        private const string TestShankDiameterInput = "17,5";

        /// <summary>
        /// Тестовый ввод длины хвостовика.
        /// </summary>
        private const string TestShankLengthInput = "40,0";

        /// <summary>
        /// Невалидный ввод (не число).
        /// </summary>
        private const string InvalidInput = "abc";

        /// <summary>
        /// Ввод нулевого значения.
        /// </summary>
        private const string ZeroInput = "0,0";

        /// <summary>
        /// Создает валидатор с параметрами по умолчанию.
        /// </summary>
        /// <returns>Валидатор с тестовыми параметрами.</returns>
        private ValidationField CreateDefaultValidator()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 55.0;
            parameters.TotalLength = 75.0;
            return new ValidationField(parameters);
        }

        [Test]
        [Description("Проверка всех основных случаев ValidateField")]
        [TestCase("", true, false, "не может быть пустым")]
        [TestCase("", false, true, 0.0)]
        [TestCase("abc", true, false, "Неверный формат")]
        [TestCase("-1", true, false, "диапазоне")]
        [TestCase("101", true, false, "диапазоне")]
        [TestCase("0", true, true, 0.0)]
        [TestCase("100", true, true, 100.0)]
        [TestCase("50,5", true, true, 50.5)]
        public void ValidateField_CoverAllScenarios_ReturnsCorrectResult(
            string input, bool required, bool expectedIsValid,
            object expectedResult)
        {
            var parameters = new Parameters();
            var validator = new ValidationField(parameters);

            var result = validator.ValidateField(input, "Test", 0.0,
                100.0, "mm", required);

            if (expectedResult is string errorMessage)
            {
                Assert.That(result.IsValid, Is.EqualTo(expectedIsValid));
                Assert.That(result.ErrorMessage,
                    Contains.Substring(errorMessage));
            }
            else if (expectedResult is double expectedValue)
            {
                Assert.That(result.IsValid, Is.True);
                Assert.That(result.Value, Is.EqualTo(expectedValue));
            }
        }

        [Test]
        [Description("Проверка валидации опциональных полей при выключенном " +
            "флаге")]
        [TestCase("ValidateConeValue", false, "10,0")]
        [TestCase("ValidateShankDiameterValue", false, "15,0")]
        [TestCase("ValidateShankLengthValue", false, "50,0")]
        public void OptionalFieldValidation_Disabled_ReturnsSuccessZero(
    string methodName, bool enabled, string input)
        {
            var validator = CreateDefaultValidator();

            ValidationResult result = methodName switch
            {
                "ValidateConeValue" => validator.ValidateConeValue(input, enabled),
                "ValidateShankDiameterValue" =>
                    validator.ValidateShankDiameterValue(input, enabled),
                "ValidateShankLengthValue" =>
                    validator.ValidateShankLengthValue(input, enabled),
            };

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка нулевых значений при включенных флагах")]
        [TestCase(true, false, false, "Обратный конус не может быть равен 0")]
        [TestCase(false, true, false, "Диаметр хвостовика не может быть " +
                    "равен 0")]
        [TestCase(false, false, true, "Длина хвостовика не может быть " +
                    "равен 0")]
        public void ValidateAllFieldsWithErrors_ZeroValuesEnabled_AddsError(
            bool coneEnabled, bool shankDiameterEnabled,
            bool shankLengthEnabled, string expectedError)
        {
            var validator = CreateDefaultValidator();

            var errors = validator.ValidateAllFieldsWithErrors(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput, coneEnabled ? ZeroInput : TestConeValueInput,
                coneEnabled,
                shankDiameterEnabled ? ZeroInput : TestShankDiameterInput,
                shankLengthEnabled ? ZeroInput : TestShankLengthInput,
                shankDiameterEnabled || shankLengthEnabled);

            Assert.That(errors, Has.Some.Contains(expectedError));
        }

        [Test]
        [Description("Проверка TryUpdateParameters с невалидными " +
                    "опциональными полями")]
        [TestCase(true, false, false, "Обратный конус")]
        [TestCase(false, true, false, "Диаметр хвостовика")]
        [TestCase(false, false, true, "Длина хвостовика")]
        public void TryUpdateParameters_InvalidOptionalFields_ReturnsFalse(
            bool coneEnabled, bool shankEnabledDiameter,
            bool shankEnabledLength, string expectedError)
        {
            var validator = CreateDefaultValidator();

            bool success = validator.TryUpdateParameters(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput,
                coneEnabled ? InvalidInput : TestConeValueInput,
                coneEnabled,
                shankEnabledDiameter ? InvalidInput : TestShankDiameterInput,
                shankEnabledLength ? InvalidInput : TestShankLengthInput,
                shankEnabledDiameter || shankEnabledLength,
                out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Has.Some.Contains(expectedError));
            });
        }

        [Test]
        [Description("Проверка TryUpdateParameters с валидными данными")]
        public void TryUpdateParameters_AllValid_ReturnsTrueAndSetsParameters()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            var validator = new ValidationField(parameters);

            bool success = validator.TryUpdateParameters(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput, TestConeValueInput, true,
                TestShankDiameterInput, TestShankLengthInput, true,
                out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(errors, Is.Empty);
                Assert.That(parameters.Diameter, Is.EqualTo(10.0));
                Assert.That(parameters.Length, Is.EqualTo(55.0));
                Assert.That(parameters.TotalLength, Is.EqualTo(75.0));
                Assert.That(parameters.Angle, Is.EqualTo(45.0));
                Assert.That(parameters.ConeValue, Is.EqualTo(5.0));
                Assert.That(parameters.ShankDiameterValue, Is.EqualTo(17.5));
                Assert.That(parameters.ShankLengthValue, Is.EqualTo(40.0));
            });
        }

        [Test]
        [Description("Проверка TryUpdateParameters с невалидными данными")]
        public void TryUpdateParameters_InvalidData_ReturnsFalseAndAddsErrors()
        {
            var parameters = new Parameters();
            var validator = new ValidationField(parameters);

            bool success = validator.TryUpdateParameters(
                InvalidInput, InvalidInput, InvalidInput, InvalidInput,
                InvalidInput, true, InvalidInput, InvalidInput, true,
                out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors.Count, Is.GreaterThan(0));
            });
        }

        [Test]
        [Description("Проверка парсинга чисел с запятой как разделителем")]
        public void ValidateField_CommaDecimalSeparator_ParsesCorrectly()
        {
            var currentCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("ru-RU");
                var parameters = new Parameters();
                var validator = new ValidationField(parameters);

                var result = validator.ValidateField(
                    "10,5", "Test", 0.0, 20.0, "mm");
                Assert.That(result.IsValid, Is.True);
                Assert.That(result.Value, Is.EqualTo(10.5));
            }
            finally
            {
                CultureInfo.CurrentCulture = currentCulture;
            }
        }

        [Test]
        [Description("Проверка ValidateAllFields возвращает список " +
                    "результатов")]
        public void ValidateAllFields_ReturnsAllValidationResults()
        {
            var validator = CreateDefaultValidator();
            var results = validator.ValidateAllFields(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput, TestConeValueInput, true,
                TestShankDiameterInput, TestShankLengthInput, true);

            Assert.That(results, Has.Count.EqualTo(7));
            Assert.That(results.All(r => r.Result.IsValid), Is.True);
        }

        [Test]
        [Description("Проверка ValidateAllFieldsWithErrors с валидными " +
                    "данными")]
        public void ValidateAllFieldsWithErrors_AllValid_ReturnsNoErrors()
        {
            var validator = CreateDefaultValidator();
            var errors = validator.ValidateAllFieldsWithErrors(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput, TestConeValueInput, true,
                TestShankDiameterInput, "60,0", true);

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка ValidateAllFieldsWithErrors с включенным " +
                    "конусом и невалидным значением")]
        public void ValidateAllFieldsWithErrors_EnabledConeInvalid_ReturnsError()
        {
            var validator = CreateDefaultValidator();

            // Конус включен, ввод невалидный - должна быть ошибка
            var errors = validator.ValidateAllFieldsWithErrors(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput, InvalidInput, true,  // coneEnabled = true
                TestShankDiameterInput, TestShankLengthInput, false);

            Assert.That(errors, Has.Some.Contains("Обратный конус"));
        }

        [Test]
        [Description("Проверка ValidateAllFieldsWithErrors с включенным " +
                    "хвостовиком и невалидными значениями")]
        public void ValidateAllFieldsWithErrors_EnabledShankInvalid_ReturnsError()
        {
            var validator = CreateDefaultValidator();

            // Хвостовик включен, значения невалидные - должны быть ошибки
            var errors = validator.ValidateAllFieldsWithErrors(
                TestDiameterInput, TestLengthInput, TestTotalLengthInput,
                TestAngleInput, TestConeValueInput, false,
                InvalidInput, InvalidInput, true);  // shankEnabled = true

            Assert.That(errors.Count, Is.GreaterThan(0));
        }
    }
}