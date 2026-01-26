using Core.Model;
using ORSAPR;

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
        /// Тестовая длина рабочей части.
        /// </summary>
        private const double TestLength = 50.0;

        /// <summary>
        /// Тестовая общая длина сверла.
        /// </summary>
        private const double TestTotalLength = 80.0;

        /// <summary>
        /// Минимальная длина рабочей части для тестового диаметра.
        /// </summary>
        private const double TestMinLength = 30.0;

        /// <summary>
        /// Максимальная длина рабочей части для тестового диаметра.
        /// </summary>
        private const double TestMaxLength = 80.0;

        /// <summary>
        /// Минимальный обратный конус для тестового диаметра.
        /// </summary>
        private const double TestMinConeValue = 2.5;

        /// <summary>
        /// Максимальный обратный конус для тестового диаметра.
        /// </summary>
        private const double TestMaxConeValue = 7.5;

        /// <summary>
        /// Минимальный диаметр хвостовика для тестового диаметра.
        /// </summary>
        private const double TestMinShankDiameter = 17.5;

        /// <summary>
        /// Максимальный диаметр хвостовика для тестового диаметра.
        /// </summary>
        private const double TestMaxShankDiameter = 20.0;

        /// <summary>
        /// Минимальная длина хвостовика для минимальной разницы длин.
        /// </summary>
        private const double TestMinShankLength = 80.0;

        /// <summary>
        /// Минимальная общая длина для минимальной рабочей части.
        /// </summary>
        private const double TestMinTotalLength = 70.0;

        /// <summary>
        /// Максимальная общая длина сверла.
        /// </summary>
        private const double MaxTotalLength = 205.0;

        /// <summary>
        /// Допустимая погрешность для сравнений double.
        /// </summary>
        private const double Tolerance = 0.001;

        [Test]
        [Description("Проверка установки значений по умолчанию")]
        public void Constructor_DefaultValues_Valid()
        {
            var parameters = new Parameters();
            var errors = parameters.ValidateRules();

            Assert.That(errors, Is.Empty);
            Assert.That(parameters.Angle, Is.EqualTo(45.0));
            Assert.That(parameters.Diameter, Is.EqualTo(10.0));
            Assert.That(parameters.ClearanceCone, Is.True);
        }

        [Test]
        [Description("Валидация длины рабочей части вне диапазона")]
        public void ValidateRules_LengthOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.Length = TestMinLength - 0.1;

            var errors = parameters.ValidateRules();

            AssertSingleError(errors, "Длина рабочей части");
        }

        [Test]
        [Description("Валидация общей длины вне диапазона")]
        public void ValidateRules_TotalLengthOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.TotalLength = TestMinTotalLength - 0.1;

            var errors = parameters.ValidateRules();

            AssertSingleError(errors, "Общая длина");
        }

        [Test]
        [Description("Валидация обратного конуса вне диапазона при включенном")]
        public void ValidateRules_ConeValueOutOfRangeEnabled_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceCone = true;
            parameters.ConeValue = TestMinConeValue - 0.1;

            var errors = parameters.ValidateRules();

            AssertSingleError(errors, "обратного конуса");
        }

        [Test]
        [Description("Проверка отключения валидации обратного конуса")]
        public void ValidateRules_ConeDisabled_NoValidation()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceCone = false;
            parameters.ConeValue = 100.0;

            var errors = parameters.ValidateRules();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Валидация диаметра хвостовика вне диапазона")]
        public void ValidateRules_ShankDiameterOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = TestMinShankDiameter - 0.1;
            parameters.ShankLengthValue = 75.0;

            var errors = parameters.ValidateRules();

            AssertSingleError(errors, "диаметра хвостовика");
        }

        [Test]
        [Description("Валидация длины хвостовика вне диапазона")]
        public void ValidateRules_ShankLengthOutOfRange_ReturnsError()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = TestMinShankDiameter;
            parameters.ShankLengthValue = 59.9;

            var errors = parameters.ValidateRules();

            AssertSingleError(errors, "Длина хвостовика");
        }

        [Test]
        [Description("Валидация обоих параметров хвостовика вне диапазона")]
        public void ValidateRules_BothShankParamsOutOfRange_ReturnsTwoErrors()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = TestMinShankDiameter - 0.1;
            parameters.ShankLengthValue = 59.9;

            var errors = parameters.ValidateRules();

            Assert.That(errors, Has.Count.EqualTo(2));
        }

        [Test]
        [Description("Проверка отключения валидации хвостовика")]
        public void ValidateRules_ShankDisabled_NoValidation()
        {
            var parameters = CreateTestParameters();
            parameters.ClearanceShank = false;
            parameters.ShankDiameterValue = 5.0;
            parameters.ShankLengthValue = 10.0;

            var errors = parameters.ValidateRules();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка всех допустимых граничных значений")]
        public void ValidateRules_AllValidBoundaryValues_NoErrors()
        {
            var parameters = new Parameters();
            parameters.Diameter = TestDiameter;
            parameters.Angle = 60.0;
            parameters.Length = TestMaxLength;
            parameters.TotalLength = 100.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = TestMaxConeValue;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = TestMaxShankDiameter;

            var lengthDiff = 100.0 - TestMaxLength;
            parameters.ShankLengthValue = 2 * lengthDiff + 0.1;

            var errors = parameters.ValidateRules();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка пересчета зависимых значений при изменении диаметра")]
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
        }

        [Test]
        [Description("Проверка пересчета общей длины при изменении рабочей длины")]
        public void LengthSetter_RecalculatesTotalLength()
        {
            var parameters = new Parameters();
            parameters.Length = 60.0;

            Assert.That(parameters.MinTotalLength,
                Is.EqualTo(80.0).Within(Tolerance));
        }

        [Test]
        [Description("Проверка пересчета длины хвостовика при изменении общей длины")]
        public void TotalLengthSetter_RecalculatesShankLength()
        {
            var parameters = new Parameters();
            parameters.Length = 50.0;
            parameters.TotalLength = 100.0;

            var diff = 100.0 - parameters.Length;
            Assert.That(parameters.MinShankLengthValue,
                Is.EqualTo(2 * diff).Within(Tolerance));
            Assert.That(parameters.MaxShankLengthValue,
                Is.EqualTo(3 * diff).Within(Tolerance));
        }

        [Test]
        [Description("Проверка возврата всех ошибок при множественных нарушениях")]
        public void ValidateRules_MultipleErrors_ReturnsAll()
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

            var errors = parameters.ValidateRules();

            Assert.That(errors, Has.Count.EqualTo(5));
        }

        [Test]
        [Description("Проверка значений доступных только для чтения свойств")]
        public void ReadOnlyProperties_ReturnCorrectValues()
        {
            var parameters = new Parameters();

            Assert.That(parameters.MinAngle, Is.EqualTo(30.0));
            Assert.That(parameters.MaxAngle, Is.EqualTo(60.0));
            Assert.That(parameters.MinDiameter, Is.EqualTo(1.0));
            Assert.That(parameters.MaxDiameter, Is.EqualTo(20.0));
            Assert.That(parameters.MaxTotalLength,
                Is.EqualTo(MaxTotalLength));
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
        public void ValidateRules_MinimalBoundaryValues_NoErrors()
        {
            var parameters = new Parameters();
            parameters.Diameter = TestDiameter;
            parameters.Angle = 30.0;
            parameters.Length = TestMinLength;
            parameters.TotalLength = TestMinTotalLength;
            parameters.ClearanceCone = true;
            parameters.ConeValue = TestMinConeValue;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = TestMinShankDiameter;
            parameters.ShankLengthValue = TestMinShankLength;

            var errors = parameters.ValidateRules();

            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка формата сообщений об ошибках")]
        public void ValidateRules_ErrorMessages_HaveCorrectFormat()
        {
            var parameters = new Parameters();
            parameters.Diameter = TestDiameter;
            parameters.Length = TestMaxLength + 10.0;
            parameters.TotalLength = TestMaxLength + 30.0;

            var errors = parameters.ValidateRules();

            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("30,0 - 80,0")
                .Or.Contains("30.0 - 80.0"));
        }

        private Parameters CreateTestParameters()
        {
            return new Parameters
            {
                Diameter = TestDiameter,
                Length = TestLength,
                TotalLength = TestTotalLength
            };
        }

        private void AssertSingleError(List<string> errors, string substring)
        {
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring(substring));
        }
    }
}