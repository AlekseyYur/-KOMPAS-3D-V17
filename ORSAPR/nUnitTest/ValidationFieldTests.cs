using Core.Model;
using Core.Validation;
using ORSAPR;
using System.Globalization;

namespace nUnitTest
{
    [TestFixture]
    public class ValidationFieldTests
    {
        //TODO: XML
        private Parameters _parameters;
        //TODO: XML
        private ValidationField _validationField;

        //TODO: refactor
        [SetUp]
        public void Setup()
        {
            _parameters = new Parameters();
            _validationField = new ValidationField(_parameters);
        }

        //TODO: XML
        private string FormatNumber(double value)
        {
            return value.ToString(CultureInfo.CurrentCulture);
        }

        [Test]
        [Description("Проверка пустой строки с requaride = true")]
        public void ValidateField_EmptyStringRequarideTrue_ReturnsError()
        {
            var result = _validationField.ValidateField(
                "", "Test", 0.0, 100.0, "mm", requaride: true);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("не может быть пустым"));
        }

        [Test]
        [Description("Проверка пустой строки с requaride = false")]
        public void ValidateField_EmptyStringRequarideFalse_ReturnsSuccessZero()
        {
            var result = _validationField.ValidateField(
                "", "Test", 0.0, 100.0, "mm", requaride: false);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка непустой строки с requaride = true")]
        public void ValidateField_NotEmptyStringRequarideTrue_ProcessesNormally()
        {
            string number = FormatNumber(50.0);
            var result = _validationField.ValidateField(
                number, "Test", 0.0, 100.0, "mm", requaride: true);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(50.0));
        }

        [Test]
        [Description("Проверка значения равного минимуму")]
        public void ValidateField_ValueEqualsMin_ReturnsSuccess()
        {
            string number = FormatNumber(0.0);
            var result = _validationField.ValidateField(
                number, "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка значения равного максимуму")]
        public void ValidateField_ValueEqualsMax_ReturnsSuccess()
        {
            string number = FormatNumber(100.0);
            var result = _validationField.ValidateField(
                number, "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(100.0));
        }

        [Test]
        [Description("Проверка значения ниже минимума")]
        public void ValidateField_ValueBelowMin_ReturnsError()
        {
            string number = FormatNumber(-1.0);
            var result = _validationField.ValidateField(
                number, "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("диапазоне"));
        }

        [Test]
        [Description("Проверка значения выше максимума")]
        public void ValidateField_ValueAboveMax_ReturnsError()
        {
            string number = FormatNumber(101.0);
            var result = _validationField.ValidateField(
                number, "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("диапазоне"));
        }

        [Test]
        [Description("Проверка пустой строки с required = true")]
        public void ValidateDependentField_EmptyStringRequiredTrue_ReturnsError()
        {
            var result = _validationField.ValidateDependentField(
                "", "Test", 0.0, 100.0, "mm", required: true);
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("не может быть пустым"));
        }

        [Test]
        [Description("Проверка пустой строки с required = false")]
        public void ValidateDependentField_EmptyStringRequiredFalse_ReturnsSuccessZero()
        {
            var result = _validationField.ValidateDependentField(
                "", "Test", 0.0, 100.0, "mm", required: false);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка неудавшегося парсинга")]
        public void ValidateDependentField_ParseFails_ReturnsFormatError()
        {
            var result = _validationField.ValidateDependentField(
                "not-a-number", "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("Неверный формат"));
        }

        [Test]
        [Description("Проверка успешного парсинга с валидным значением")]
        public void ValidateDependentField_SuccessfulParseValidValue_ReturnsSuccess()
        {
            string number = FormatNumber(75.5);
            var result = _validationField.ValidateDependentField(
                number, "Test", 0.0, 100.0, "mm", required: true);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(75.5));
        }

        [Test]
        [Description("Проверка значения ниже минимума в ValidateDependentField")]
        public void ValidateDependentField_ValueBelowMin_ReturnsError()
        {
            string number = FormatNumber(-1.0);
            var result = _validationField.ValidateDependentField(
                number, "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("диапазоне"));
        }

        [Test]
        [Description("Проверка значения выше максимума в ValidateDependentField")]
        public void ValidateDependentField_ValueAboveMax_ReturnsError()
        {
            string number = FormatNumber(101.0);
            var result = _validationField.ValidateDependentField(
                number, "Test", 0.0, 100.0, "mm");
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.ErrorMessage, Contains.Substring("диапазоне"));
        }

        [Test]
        [Description("Проверка ValidateConeValue с выключенным конусом")]
        public void ValidateConeValue_ConeDisabled_ReturnsSuccessZero()
        {
            var result = _validationField.ValidateConeValue("10.0", false);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка ValidateShankDiameterValue с выключенным хвостовиком")]
        public void ValidateShankDiameterValue_ShankDisabled_ReturnsSuccessZero()
        {
            var result = _validationField.ValidateShankDiameterValue("10.0", false);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка ValidateShankLengthValue с выключенным хвостовиком")]
        public void ValidateShankLengthValue_ShankDisabled_ReturnsSuccessZero()
        {
            var result = _validationField.ValidateShankLengthValue("10.0", false);
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        [Test]
        [Description("Проверка нулевого конуса с включенным флагом")]
        public void ValidateAllFieldsWithErrors_ZeroConeEnabled_AddsError()
        {
            string zeroValue = FormatNumber(0.0);
            var errors = _validationField.ValidateAllFieldsWithErrors(
                "10.0", "55.0", "75.0", "45.0", zeroValue, true,
                "15.0", "60.0", false);
            Assert.That(errors, Has.Some.Contains("Обратный конус"));
        }

        [Test]
        [Description("Проверка нулевого диаметра хвостовика с включенным флагом")]
        public void ValidateAllFieldsWithErrors_ZeroShankDiameterEnabled_AddsError()
        {
            string zeroValue = FormatNumber(0.0);
            var errors = _validationField.ValidateAllFieldsWithErrors(
                "10.0", "55.0", "75.0", "45.0", "5.0", false,
                zeroValue, "60.0", true);
            Assert.That(errors, Has.Some.Contains("Диаметр хвостовика"));
        }

        [Test]
        [Description("Проверка нулевой длины хвостовика с включенным флагом")]
        public void ValidateAllFieldsWithErrors_ZeroShankLengthEnabled_AddsError()
        {
            string zeroValue = FormatNumber(0.0);
            var errors = _validationField.ValidateAllFieldsWithErrors(
                "10.0", "55.0", "75.0", "45.0", "5.0", false,
                "15.0", zeroValue, true);
            Assert.That(errors, Has.Some.Contains("Длина хвостовика"));
        }

        [Test]
        [Description("Проверка ненулевого конуса с включенным флагом")]
        public void ValidateAllFieldsWithErrors_NonZeroConeEnabled_NoError()
        {
            string coneValue = FormatNumber(5.0);
            var errors = _validationField.ValidateAllFieldsWithErrors(
                "10.0", "55.0", "75.0", "45.0", coneValue, true,
                "15.0", "60.0", false);
            Assert.That(errors, Has.None.Contains("Обратный конус"));
        }

        [Test]
        [Description("Проверка нулевого конуса с выключенным флагом")]
        public void ValidateAllFieldsWithErrors_ZeroConeDisabled_NoError()
        {
            string zeroValue = FormatNumber(0.0);
            var errors = _validationField.ValidateAllFieldsWithErrors(
                "10.0", "55.0", "75.0", "45.0", zeroValue, false,
                "15.0", "60.0", false);
            Assert.That(errors, Has.None.Contains("Обратный конус"));
        }

        [Test]
        [Description("Проверка цепочки else if с несколькими нулевыми значениями")]
        public void ValidateAllFieldsWithErrors_MultipleZeroValues_AddsFirstErrorOnly()
        {
            string zeroValue = FormatNumber(0.0);
            var errors = _validationField.ValidateAllFieldsWithErrors(
                "10.0", "55.0", "75.0", "45.0", zeroValue, true,
                zeroValue, zeroValue, true);
            Assert.That(errors, Has.Some.Contains("Обратный конус"));
            Assert.That(errors, Has.None.Contains("Диаметр хвостовика"));
            Assert.That(errors, Has.None.Contains("Длина хвостовика"));
        }

        [Test]
        [Description("Проверка установки всех параметров при валидных данных")]
        public void TryUpdateParameters_AllValid_SetsAllParameters()
        {
            string diameter = FormatNumber(12.0);
            string length = FormatNumber(65.0);
            string totalLength = FormatNumber(90.0);
            string angle = FormatNumber(50.0);
            string coneValue = FormatNumber(6.0);
            string shankDiameter = FormatNumber(18.0);
            string shankLength = FormatNumber(75.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: diameter,
                length: length,
                totalLength: totalLength,
                angle: angle,
                coneValue: coneValue,
                coneEnabled: true,
                shankDiameterValue: shankDiameter,
                shankLengthValue: shankLength,
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(errors, Is.Empty);
                Assert.That(_parameters.Diameter, Is.EqualTo(12.0));
                Assert.That(_parameters.Length, Is.EqualTo(65.0));
                Assert.That(_parameters.TotalLength, Is.EqualTo(90.0));
                Assert.That(_parameters.Angle, Is.EqualTo(50.0));
                Assert.That(_parameters.ConeValue, Is.EqualTo(6.0));
                Assert.That(_parameters.ShankDiameterValue, Is.EqualTo(18.0));
                Assert.That(_parameters.ShankLengthValue, Is.EqualTo(75.0));
            });
        }

        [Test]
        [Description("Проверка невалидного диаметра")]
        public void TryUpdateParameters_InvalidDiameter_ReturnsError()
        {
            var initialDiameter = _parameters.Diameter;
            string invalidDiameter = FormatNumber(25.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: invalidDiameter,
                length: FormatNumber(65.0),
                totalLength: FormatNumber(90.0),
                angle: FormatNumber(50.0),
                coneValue: FormatNumber(6.0),
                coneEnabled: true,
                shankDiameterValue: FormatNumber(18.0),
                shankLengthValue: FormatNumber(75.0),
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Is.Not.Empty);
                Assert.That(_parameters.Diameter, Is.EqualTo(initialDiameter));
            });
        }

        [Test]
        [Description("Проверка невалидной длины")]
        public void TryUpdateParameters_InvalidLength_ReturnsError()
        {
            var initialLength = _parameters.Length;
            string invalidLength = FormatNumber(100.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: FormatNumber(12.0),
                length: invalidLength,
                totalLength: FormatNumber(90.0),
                angle: FormatNumber(50.0),
                coneValue: FormatNumber(6.0),
                coneEnabled: true,
                shankDiameterValue: FormatNumber(18.0),
                shankLengthValue: FormatNumber(75.0),
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Is.Not.Empty);
                Assert.That(_parameters.Length, Is.EqualTo(initialLength));
            });
        }

        [Test]
        [Description("Проверка невалидной общей длины")]
        public void TryUpdateParameters_InvalidTotalLength_ReturnsError()
        {
            var initialTotalLength = _parameters.TotalLength;
            string invalidTotalLength = FormatNumber(50.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: FormatNumber(12.0),
                length: FormatNumber(65.0),
                totalLength: invalidTotalLength,
                angle: FormatNumber(50.0),
                coneValue: FormatNumber(6.0),
                coneEnabled: true,
                shankDiameterValue: FormatNumber(18.0),
                shankLengthValue: FormatNumber(75.0),
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Is.Not.Empty);
                Assert.That(_parameters.TotalLength, Is.EqualTo(initialTotalLength));
            });
        }

        [Test]
        [Description("Проверка невалидного угла")]
        public void TryUpdateParameters_InvalidAngle_ReturnsError()
        {
            var initialAngle = _parameters.Angle;
            string invalidAngle = FormatNumber(25.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: FormatNumber(12.0),
                length: FormatNumber(65.0),
                totalLength: FormatNumber(90.0),
                angle: invalidAngle,
                coneValue: FormatNumber(6.0),
                coneEnabled: true,
                shankDiameterValue: FormatNumber(18.0),
                shankLengthValue: FormatNumber(75.0),
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Is.Not.Empty);
                Assert.That(_parameters.Angle, Is.EqualTo(initialAngle));
            });
        }

        [Test]
        [Description("Проверка невалидного конуса при включенном флаге")]
        public void TryUpdateParameters_InvalidConeValueConeEnabled_ReturnsError()
        {
            _parameters.Diameter = 10.0;
            var initialConeValue = _parameters.ConeValue;
            string invalidConeValue = FormatNumber(10.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: FormatNumber(10.0),
                length: FormatNumber(55.0),
                totalLength: FormatNumber(75.0),
                angle: FormatNumber(45.0),
                coneValue: invalidConeValue,
                coneEnabled: true,
                shankDiameterValue: FormatNumber(15.0),
                shankLengthValue: FormatNumber(60.0),
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Has.Some.Contains("Обратный конус"));
                Assert.That(_parameters.ConeValue, Is.EqualTo(initialConeValue));
            });
        }

        [Test]
        [Description("Проверка невалидного диаметра хвостовика при включенном флаге")]
        public void TryUpdateParameters_InvalidShankDiameterShankEnabled_ReturnsError()
        {
            _parameters.Diameter = 10.0;
            var initialShankDiameter = _parameters.ShankDiameterValue;
            string invalidShankDiameter = FormatNumber(25.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: FormatNumber(10.0),
                length: FormatNumber(55.0),
                totalLength: FormatNumber(75.0),
                angle: FormatNumber(45.0),
                coneValue: FormatNumber(5.0),
                coneEnabled: true,
                shankDiameterValue: invalidShankDiameter,
                shankLengthValue: FormatNumber(60.0),
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Has.Some.Contains("Диаметр хвостовика"));
                Assert.That(_parameters.ShankDiameterValue, Is.EqualTo(initialShankDiameter));
            });
        }

        [Test]
        [Description("Проверка невалидной длины хвостовика при включенном флаге")]
        public void TryUpdateParameters_InvalidShankLengthShankEnabled_ReturnsError()
        {
            _parameters.Diameter = 10.0;
            _parameters.Length = 50.0;
            _parameters.TotalLength = 80.0;
            var initialShankLength = _parameters.ShankLengthValue;
            string invalidShankLength = FormatNumber(100.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: FormatNumber(10.0),
                length: FormatNumber(50.0),
                totalLength: FormatNumber(80.0),
                angle: FormatNumber(45.0),
                coneValue: FormatNumber(5.0),
                coneEnabled: true,
                shankDiameterValue: FormatNumber(15.0),
                shankLengthValue: invalidShankLength,
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(errors, Has.Some.Contains("Длина хвостовика"));
                Assert.That(_parameters.ShankLengthValue, Is.EqualTo(initialShankLength));
            });
        }

        [Test]
        [Description("Проверка выключенного конуса с невалидным значением")]
        public void TryUpdateParameters_InvalidConeValueConeDisabled_NoError()
        {
            string invalidConeValue = FormatNumber(100.0);
            string diameter = FormatNumber(12.0);
            string length = FormatNumber(65.0);
            string totalLength = FormatNumber(90.0);
            string angle = FormatNumber(50.0);
            string shankDiameter = FormatNumber(18.0);
            string shankLength = FormatNumber(75.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: diameter,
                length: length,
                totalLength: totalLength,
                angle: angle,
                coneValue: invalidConeValue,
                coneEnabled: false,
                shankDiameterValue: shankDiameter,
                shankLengthValue: shankLength,
                shankEnabled: true,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(errors, Is.Empty);
                Assert.That(_parameters.ClearanceCone, Is.False);
            });
        }

        [Test]
        [Description("Проверка выключенного хвостовика с невалидными значениями")]
        public void TryUpdateParameters_InvalidShankValuesShankDisabled_NoError()
        {
            string invalidShankDiameter = FormatNumber(100.0);
            string invalidShankLength = FormatNumber(200.0);
            string diameter = FormatNumber(12.0);
            string length = FormatNumber(65.0);
            string totalLength = FormatNumber(90.0);
            string angle = FormatNumber(50.0);
            string coneValue = FormatNumber(6.0);

            bool success = _validationField.TryUpdateParameters(
                diameter: diameter,
                length: length,
                totalLength: totalLength,
                angle: angle,
                coneValue: coneValue,
                coneEnabled: true,
                shankDiameterValue: invalidShankDiameter,
                shankLengthValue: invalidShankLength,
                shankEnabled: false,
                errors: out var errors);

            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(errors, Is.Empty);
                Assert.That(_parameters.ClearanceShank, Is.False);
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
                var result = _validationField.ValidateField(
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
        [Description("Проверка что ValidateAllFields возвращает все результаты")]
        public void ValidateAllFields_ReturnsAllFieldResults()
        {
            var results = _validationField.ValidateAllFields(
                "10.0", "55.0", "75.0", "45.0", "5.0", true,
                "15.0", "60.0", true);

            Assert.That(results, Has.Count.EqualTo(7));
            Assert.That(results[0].FieldName, Is.EqualTo("Диаметр"));
            Assert.That(results[1].FieldName, Is.EqualTo("Длина рабочей части"));
        }
    }
}