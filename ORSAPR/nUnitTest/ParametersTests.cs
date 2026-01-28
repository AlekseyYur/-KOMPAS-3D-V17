using ORSAPR;
using System.Globalization;

namespace nUnitTest
{
    [TestFixture]
    public class ParametersTests
    {

        ///<summary>
        ///Тестовый диаметр сверла.
        ///</summary>
        private const double TestDiameter = 10.0;

        /// <summary>
        /// Допустимая погрешность для сравнений double.
        /// </summary>
        private const double Tolerance = 0.001;

        [Test]
        [Description("Проверка установки значений по умолчанию")]
        public void Constructor_DefaultValues_Valid()
        {
            var parameters = new Parameters();
            var errors = parameters.ValidateAll();

            Assert.That(errors, Is.Empty);
            Assert.That(parameters.Angle, Is.EqualTo(45.0));
            Assert.That(parameters.Diameter, Is.EqualTo(10.0));
            Assert.That(parameters.ClearanceCone, Is.True);

            // Проверка геттеров значений по умолчанию
            Assert.That(parameters.Length, Is.EqualTo(55.0));
            Assert.That(parameters.TotalLength, Is.EqualTo(75.0));
            Assert.That(parameters.ConeValue, Is.EqualTo(5.0));
        }

        [Test]
        [Description("Валидация длины рабочей части вне диапазона")]
        public void ValidateAll_LengthOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.Length = parameters.MinLength - 0.1;

            var errors = parameters.ValidateAll();

            AssertSingleError(errors, "Длина рабочей части");
        }

        [Test]
        [Description("Валидация общей длины вне диапазона")]
        public void ValidateAll_TotalLengthOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.TotalLength = parameters.MinTotalLength - 0.1;

            var errors = parameters.ValidateAll();

            AssertSingleError(errors, "Общая длина");
        }

        [Test]
        [Description("Валидация обратного конуса вне диапазона")]
        public void ValidateAll_ConeValueOutOfRangeEnabled_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceCone = true;
            parameters.ConeValue = parameters.MinConeValue - 0.1;

            var errors = parameters.ValidateAll();

            AssertSingleError(errors, "Обратный конус");
        }

        [Test]
        [Description("Проверка отключения валидации обратного конуса")]
        public void ValidateAll_ConeDisabled_NoValidation()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceCone = false;
            parameters.ConeValue = 100.0;

            var errors = parameters.ValidateAll();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Валидация диаметра хвостовика вне диапазона")]
        public void ValidateAll_ShankDiameterOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue =
                parameters.MinShankDiameterValue - 0.1;

            var errors = parameters.ValidateAll();

            AssertSingleError(errors, "Диаметр хвостовика");
        }

        [Test]
        [Description("Валидация длины хвостовика вне диапазона")]
        public void ValidateAll_ShankLengthOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = true;
            parameters.ShankLengthValue =
                parameters.MinShankLengthValue - 0.1;

            var errors = parameters.ValidateAll();

            AssertSingleError(errors, "Длина хвостовика");
        }

        [Test]
        [Description("Проверка отключения валидации хвостовика")]
        public void ValidateAll_ShankDisabled_NoValidation()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = false;
            parameters.ShankDiameterValue = 5.0;
            parameters.ShankLengthValue = 10.0;

            var errors = parameters.ValidateAll();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка всех допустимых граничных значений")]
        public void ValidateAll_AllValidBoundaryValues_NoErrors()
        {
            var parameters = new Parameters();
            parameters.Diameter = TestDiameter;
            parameters.Angle = 60.0;
            parameters.Length = parameters.MaxLength;
            parameters.TotalLength = parameters.MaxTotalLength;
            parameters.ClearanceCone = true;
            parameters.ConeValue = parameters.MaxConeValue;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue =
                parameters.MaxShankDiameterValue;
            parameters.ShankLengthValue =
                parameters.MaxShankLengthValue;

            var errors = parameters.ValidateAll();

            Assert.That(errors, Is.Empty);

            // Проверка геттеров установленных значений
            Assert.That(parameters.Length,
                Is.EqualTo(parameters.MaxLength));
            Assert.That(parameters.TotalLength,
                Is.EqualTo(parameters.MaxTotalLength));
            Assert.That(parameters.ConeValue,
                Is.EqualTo(parameters.MaxConeValue));
            Assert.That(parameters.ShankDiameterValue,
                Is.EqualTo(parameters.MaxShankDiameterValue));
            Assert.That(parameters.ShankLengthValue,
                Is.EqualTo(parameters.MaxShankLengthValue));
        }

        [Test]
        [Description("Проверка пересчета зависимых значений")]
        public void DiameterSetter_RecalculatesDependentValues()
        {
            var parameters = new Parameters();
            parameters.Diameter = 15.0;

            Assert.That(parameters.MinLength,
                Is.EqualTo(3 * 15.0).Within(Tolerance));
            Assert.That(parameters.MaxLength,
                Is.EqualTo(8 * 15.0).Within(Tolerance));
            Assert.That(parameters.MinConeValue,
                Is.EqualTo(0.25 * 15.0).Within(Tolerance));

            // Проверка геттера диаметра
            Assert.That(parameters.Diameter, Is.EqualTo(15.0));
        }

        [Test]
        [Description("Проверка возврата всех ошибок")]
        public void ValidateAll_MultipleErrors_ReturnsAll()
        {
            var parameters = new Parameters();
            parameters.Diameter = TestDiameter;
            parameters.Length = 25.0;
            parameters.TotalLength = 30.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 1.0;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 10.0;
            parameters.ShankLengthValue = 9.9;

            var errors = parameters.ValidateAll();

            Assert.That(errors, Has.Count.AtLeast(3));
        }

        [Test]
        [Description("Проверка значений доступных только для чтения")]
        public void ReadOnlyProperties_ReturnCorrectValues()
        {
            var parameters = new Parameters();

            Assert.That(parameters.MinAngle, Is.EqualTo(30.0));
            Assert.That(parameters.MaxAngle, Is.EqualTo(60.0));
            Assert.That(parameters.MinDiameter, Is.EqualTo(1.0));
            Assert.That(parameters.MaxDiameter, Is.EqualTo(20.0));
            Assert.That(parameters.MaxTotalLength, Is.EqualTo(205.0));
        }

        [Test]
        [Description("Проверка установки и получения угла")]
        public void AngleProperty_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.Angle = 55.5;
            Assert.That(parameters.Angle, Is.EqualTo(55.5));
        }

        [Test]
        [Description("Проверка установки и получения булевых свойств")]
        public void BooleanProperties_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.ClearanceCone = false;
            parameters.ClearanceShank = true;

            Assert.That(parameters.ClearanceCone, Is.False);
            Assert.That(parameters.ClearanceShank, Is.True);
        }

        [Test]
        [Description("Проверка минимальных граничных значений")]
        public void ValidateAll_MinimalBoundaryValues_NoErrors()
        {
            var parameters = new Parameters();
            parameters.Diameter = TestDiameter;
            parameters.Angle = parameters.MinAngle;
            parameters.Length = parameters.MinLength;
            parameters.TotalLength = parameters.MinTotalLength;
            parameters.ClearanceCone = true;
            parameters.ConeValue = parameters.MinConeValue;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue =
                parameters.MinShankDiameterValue;
            parameters.ShankLengthValue =
                parameters.MinShankLengthValue;

            var errors = parameters.ValidateAll();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации угла вне диапазона")]
        public void ValidateAll_AngleOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.Angle = 25.0;

            var errors = parameters.ValidateAll();

            AssertSingleError(errors, "Угол при вершине");
        }

        [Test]
        [Description("Проверка валидации диаметра вне диапазона")]
        public void ValidateAll_DiameterOutOfRange_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 0.5;

            var errors = parameters.ValidateAll();

            Assert.That(errors, Has.Count.AtLeast(1));
            Assert.That(errors.Exists(e => e.Contains("Диаметр")),
                Is.True);
        }

        [Test]
        [Description("Проверка метода TryParseDouble с валидным числом")]
        public void TryParseDouble_ValidNumber_ReturnsSuccess()
        {
            var originalCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture =
                    CultureInfo.InvariantCulture;

                var result = Parameters.TryParseDouble("10.5",
                    "Тестовое поле");

                Assert.That(result.success, Is.True);
                Assert.That(result.value, Is.EqualTo(10.5));
                Assert.That(result.error, Is.Null);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Test]
        [Description("Проверка метода TryParseDouble с невалидным числом")]
        public void TryParseDouble_InvalidNumber_ReturnsError()
        {
            var result = Parameters.TryParseDouble("не число",
                "Тестовое поле");

            Assert.That(result.success, Is.False);
            Assert.That(result.error,
                Contains.Substring("Неверный формат"));
        }

        [Test]
        [Description("Проверка метода TryParseDouble с пустой строкой")]
        public void TryParseDouble_EmptyString_ReturnsError()
        {
            var result = Parameters.TryParseDouble("",
                "Тестовое поле");

            Assert.That(result.success, Is.False);
            Assert.That(result.error,
                Contains.Substring("не может быть пустым"));
        }

        [Test]
        [Description("Проверка метода TryParseDouble с null")]
        public void TryParseDouble_NullString_ReturnsError()
        {
            var result = Parameters.TryParseDouble(null,
                "Тестовое поле");

            Assert.That(result.success, Is.False);
            Assert.That(result.error,
                Contains.Substring("не может быть пустым"));
        }

        [Test]
        [Description("Проверка метода TryParseDouble с разными культурами")]
        public void TryParseDouble_DifferentCultures_WorksCorrectly()
        {
            var originalCulture =
                System.Threading.Thread.CurrentThread.CurrentCulture;

            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new CultureInfo("ru-RU");
                var resultRu = Parameters.TryParseDouble("10,5",
                    "Поле");
                Assert.That(resultRu.success, Is.True);
                Assert.That(resultRu.value, Is.EqualTo(10.5));

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new CultureInfo("en-US");
                var resultEn = Parameters.TryParseDouble("10.5",
                    "Field");
                Assert.That(resultEn.success, Is.True);
                Assert.That(resultEn.value, Is.EqualTo(10.5));
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    originalCulture;
            }
        }

        [Test]
        [Description("Проверка свойств конуса при разных диаметрах")]
        public void ConeProperties_DifferentDiameters_CalculatedCorrectly()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;

            Assert.That(parameters.MinConeValue,
                Is.EqualTo(2.5).Within(Tolerance));
            Assert.That(parameters.MaxConeValue,
                Is.EqualTo(7.5).Within(Tolerance));

            parameters.Diameter = 20.0;
            Assert.That(parameters.MinConeValue,
                Is.EqualTo(5.0).Within(Tolerance));
            Assert.That(parameters.MaxConeValue,
                Is.EqualTo(15.0).Within(Tolerance));
        }

        [Test]
        [Description("Проверка свойств хвостовика при разных диаметрах")]
        public void ShankProperties_DifferentDiameters_CalculatedCorrectly()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;

            Assert.That(parameters.MinShankDiameterValue,
                Is.EqualTo(17.5).Within(Tolerance));
            Assert.That(parameters.MaxShankDiameterValue,
                Is.EqualTo(20.0).Within(Tolerance));
        }

        [Test]
        [Description("Проверка максимальных значений без ошибок")]
        public void ValidateAll_AllMaximumValuesValid_NoErrors()
        {
            var parameters = new Parameters();

            parameters.Diameter = parameters.MaxDiameter;
            parameters.Length = parameters.MaxLength;
            parameters.TotalLength = parameters.MaxTotalLength;
            parameters.Angle = parameters.MaxAngle;
            parameters.ClearanceCone = true;
            parameters.ConeValue = parameters.MaxConeValue;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue =
                parameters.MaxShankDiameterValue;
            parameters.ShankLengthValue =
                parameters.MaxShankLengthValue;

            var errors = parameters.ValidateAll();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка свойств только для чтения после изменения")]
        public void ReadOnlyProperties_AfterChanges_UpdatedCorrectly()
        {
            var parameters = new Parameters();
            parameters.Diameter = 15.0;
            parameters.Length = 60.0;
            parameters.TotalLength = 90.0;

            Assert.That(parameters.MinLength, Is.EqualTo(45.0));
            Assert.That(parameters.MaxLength, Is.EqualTo(120.0));
            Assert.That(parameters.MinTotalLength, Is.EqualTo(80.0));
            Assert.That(parameters.MinConeValue, Is.EqualTo(3.75));
            Assert.That(parameters.MaxConeValue, Is.EqualTo(11.25));
        }

        [Test]
        [Description("Проверка установки и получения длины рабочей части")]
        public void LengthProperty_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.Length = 55.5;

            Assert.That(parameters.Length, Is.EqualTo(55.5));
            Assert.That(parameters.MinTotalLength,
                Is.EqualTo(75.5).Within(Tolerance));
        }

        [Test]
        [Description("Проверка установки и получения общей длины")]
        public void TotalLengthProperty_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.TotalLength = 100.0;

            Assert.That(parameters.TotalLength, Is.EqualTo(100.0));
        }

        [Test]
        [Description("Проверка установки и получения конуса")]
        public void ConeValueProperty_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.ClearanceCone = true;
            parameters.ConeValue = 5.5;

            Assert.That(parameters.ConeValue, Is.EqualTo(5.5));
        }

        [Test]
        [Description("Проверка установки и получения диаметра хвостовика")]
        public void ShankDiameterValueProperty_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 18.5;

            Assert.That(parameters.ShankDiameterValue,
                Is.EqualTo(18.5));
        }

        [Test]
        [Description("Проверка установки и получения длины хвостовика")]
        public void ShankLengthValueProperty_CanBeSetAndGet()
        {
            var parameters = new Parameters();
            parameters.ClearanceShank = true;
            parameters.ShankLengthValue = 50.0;

            Assert.That(parameters.ShankLengthValue, Is.EqualTo(50.0));
        }

        private Parameters CreateTestParameters()
        {
            return new Parameters
            {
                Diameter = TestDiameter
            };
        }

        private void AssertSingleError(List<string> errors,
            string substring)
        {
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring(substring));
        }
    }
}