using System;
using System.Reflection;
using Xunit;

namespace UnitTest
{
    public class ParametersTests
    {
        /// <summary>
        /// Проверяет, что конструктор инициализирует свойства значениями по умолчанию.
        /// </summary>
        [Fact]
        public void ConstructorShouldInitializeWithDefaultValues()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Equal(45.0, parameters.Angle);
            Assert.True(parameters.ClearanceCone);
            Assert.Equal(5.0, parameters.ConeValue);
            Assert.Equal(10.0, parameters.Diameter);
            Assert.Equal(55.0, parameters.Length);
            Assert.Equal(75.0, parameters.TotalLength);
        }

        /// <summary>
        /// Проверяет, что конструктор корректно вычисляет зависимые свойства.
        /// </summary>
        [Fact]
        public void ConstructorShouldCalculateDependentProperties()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Equal(30.0, parameters.MinLength);
            Assert.Equal(80.0, parameters.MaxLength);
            Assert.Equal(75.0, parameters.MinTotalLength);
            Assert.Equal(205.0, parameters.MaxTotalLength);
            Assert.Equal(2.5, parameters.MinConeValue);
            Assert.Equal(7.5, parameters.MaxConeValue);
        }

        /// <summary>
        /// Проверяет, что константные диапазоны свойств имеют корректные значения.
        /// </summary>
        [Fact]
        public void PropertiesShouldHaveCorrectConstantRanges()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Equal(30.0, parameters.MinAngle);
            Assert.Equal(60.0, parameters.MaxAngle);
            Assert.Equal(1.0, parameters.MinDiameter);
            Assert.Equal(20.0, parameters.MaxDiameter);
            Assert.Equal(205.0, parameters.MaxTotalLength);
        }

        /// <summary>
        /// Проверяет корректную установку допустимого значения угла при вершине.
        /// </summary>
        [Fact]
        public void AngleShouldSetValidValue()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Angle = 50.0;
            Assert.Equal(50.0, parameters.Angle);
        }

        /// <summary>
        /// Проверяет корректную установку диаметра и автоматический пересчет зависимых свойств.
        /// </summary>
        [Fact]
        public void DiameterShouldSetValueAndRecalculateDependentProperties()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 15.0;
            Assert.Equal(15.0, parameters.Diameter);
            Assert.Equal(45.0, parameters.MinLength);
            Assert.Equal(120.0, parameters.MaxLength);
            Assert.Equal(3.75, parameters.MinConeValue);
            Assert.Equal(11.25, parameters.MaxConeValue);
        }

        /// <summary>
        /// Проверяет корректную установку длины рабочей части и обновление диапазона общей длины.
        /// </summary>
        [Fact]
        public void LengthShouldSetValueAndUpdateTotalLengthRange()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            Assert.Equal(50.0, parameters.Length);
            Assert.Equal(70.0, parameters.MinTotalLength);
        }

        /// <summary>
        /// Проверяет корректную установку значения наличия обратного конуса.
        /// </summary>
        [Fact]
        public void ClearanceConeShouldSetTrueAndFalseValues()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.ClearanceCone = false;
            Assert.False(parameters.ClearanceCone);
            parameters.ClearanceCone = true;
            Assert.True(parameters.ClearanceCone);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает пустой список для валидных параметров.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnEmptyListForValidParameters()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Angle = 35.0;
            parameters.Diameter = 12.5;
            parameters.Length = 75.0;
            parameters.TotalLength = 95.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 6.25;

            var errors = parameters.ValidateAndCalculate();
            Assert.Empty(errors);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate не валидирует значение обратного конуса, когда обратный конус отключен.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldNotValidateConeValueWhenClearanceConeIsFalse()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.ClearanceCone = false;
            SetConeValueUsingReflection(parameters, 10.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.Empty(errors);
        }

        /// <summary>
        /// Проверяет, что установка значения угла меньше минимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void AngleShouldThrowArgumentExceptionWhenBelowMin()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Throws<ArgumentException>(() => parameters.Angle = 29.9);
        }

        /// <summary>
        /// Проверяет, что установка значения угла больше максимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void AngleShouldThrowArgumentExceptionWhenAboveMax()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Throws<ArgumentException>(() => parameters.Angle = 60.1);
        }

        /// <summary>
        /// Проверяет, что установка значения диаметра меньше минимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void DiameterShouldThrowArgumentExceptionWhenBelowMin()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Throws<ArgumentException>(() => parameters.Diameter = 0.5);
        }

        /// <summary>
        /// Проверяет, что установка значения диаметра больше максимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void DiameterShouldThrowArgumentExceptionWhenAboveMax()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Throws<ArgumentException>(() => parameters.Diameter = 20.1);
        }

        /// <summary>
        /// Проверяет, что установка значения длины рабочей части меньше минимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void LengthShouldThrowArgumentExceptionWhenBelowMin()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            Assert.Throws<ArgumentException>(() => parameters.Length = 29.9);
        }

        /// <summary>
        /// Проверяет, что установка значения длины рабочей части больше максимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void LengthShouldThrowArgumentExceptionWhenAboveMax()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            Assert.Throws<ArgumentException>(() => parameters.Length = 80.1);
        }

        /// <summary>
        /// Проверяет, что установка значения обратного конуса меньше минимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void ConeValueShouldThrowArgumentExceptionWhenBelowMin()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            Assert.Throws<ArgumentException>(() => parameters.ConeValue = 2.4);
        }

        /// <summary>
        /// Проверяет, что установка значения обратного конуса больше максимального вызывает исключение ArgumentException.
        /// </summary>
        [Fact]
        public void ConeValueShouldThrowArgumentExceptionWhenAboveMax()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            Assert.Throws<ArgumentException>(() => parameters.ConeValue = 7.6);
        }

        /// <summary>
        /// Проверяет, что максимальное значение общей длины всегда равно 205 мм.
        /// </summary>
        [Fact]
        public void TotalLengthMaximumValueShouldBe205()
        {
            var parameters = new ORSAPR.Parameters();
            Assert.Equal(205.0, parameters.MaxTotalLength);
        }

        /// <summary>
        /// Проверяет, что установка свойств вызывает пересчет зависимых свойств.
        /// </summary>
        [Fact]
        public void PropertySettersShouldTriggerRecalculations()
        {
            var parameters = new ORSAPR.Parameters();
            double initialMinLength = parameters.MinLength;
            double initialMinConeValue = parameters.MinConeValue;

            parameters.Diameter = 15.0;

            Assert.NotEqual(initialMinLength, parameters.MinLength);
            Assert.Equal(45.0, parameters.MinLength);
            Assert.NotEqual(initialMinConeValue, parameters.MinConeValue);
            Assert.Equal(3.75, parameters.MinConeValue);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает ошибку для невалидного угла.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidAngle()
        {
            var parameters = new ORSAPR.Parameters();
            SetAngleUsingReflection(parameters, 25.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("Угол при вершине", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает ошибку для невалидного диаметра.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidDiameter()
        {
            var parameters = new ORSAPR.Parameters();
            SetDiameterUsingReflection(parameters, 25.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("диаметра", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает ошибку для невалидной длины рабочей части.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidLength()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            SetLengthUsingReflection(parameters, 25.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("длины рабочей части", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает ошибку для невалидной общей длины.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidTotalLength()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            SetTotalLengthUsingReflection(parameters, 65.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("длины сверла", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает ошибку для невалидного значения обратного конуса.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidConeValue()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceCone = true;
            SetConeValueUsingReflection(parameters, 10.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("обратного конуса", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод ValidateAndCalculate возвращает все ошибки при наличии нескольких невалидных параметров.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldReturnAllErrorsForMultipleInvalidParameters()
        {
            var parameters = new ORSAPR.Parameters();
            SetAngleUsingReflection(parameters, 25.0);
            SetDiameterUsingReflection(parameters, 25.0);
            parameters.ClearanceCone = true;
            SetConeValueUsingReflection(parameters, 20.0);

            var errors = parameters.ValidateAndCalculate();
            Assert.Equal(3, errors.Count);
        }


        /// <summary>
        /// Проверяет обработку исключения в методе ValidateAndCalculate.
        /// </summary>
        [Fact]
        public void ValidateAndCalculateShouldHandleException()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();

            parameters.Angle = 45.0;
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 70.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 5.0;

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.NotNull(errors);
        }

        /// <summary>
        /// Проверяет корректность расчета диапазона длины рабочей части для различных значений диаметра.
        /// </summary>
        [Theory]
        [InlineData(1.0, 3.0, 8.0, 0.25, 0.75)]
        [InlineData(10.0, 30.0, 80.0, 2.5, 7.5)]
        [InlineData(20.0, 60.0, 160.0, 5.0, 15.0)]
        [InlineData(5.5, 16.5, 44.0, 1.375, 4.125)]
        public void LengthRangeShouldBeCorrectForDiameter(double diameter, double expectedMinLength, double expectedMaxLength,
            double expectedMinConeValue, double expectedMaxConeValue)
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = diameter;

            Assert.Equal(expectedMinLength, parameters.MinLength);
            Assert.Equal(expectedMaxLength, parameters.MaxLength);
            Assert.Equal(expectedMinConeValue, parameters.MinConeValue);
            Assert.Equal(expectedMaxConeValue, parameters.MaxConeValue);
        }

        /// <summary>
        /// Проверяет корректность расчета минимальной общей длины для различных значений длины рабочей части.
        /// </summary>
        [Theory]
        [InlineData(30.0, 50.0)]
        [InlineData(80.0, 100.0)]
        [InlineData(55.0, 75.0)]
        public void MinTotalLengthShouldBeLengthPlus20(double length, double expectedMinTotalLength)
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = length;

            Assert.Equal(expectedMinTotalLength, parameters.MinTotalLength);
        }

        /// <summary>
        /// Проверяет установку значения TotalLength в допустимом диапазоне.
        /// </summary>
        [Fact]
        public void TotalLengthShouldSetValidValue()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 100.0;

            Assert.Equal(100.0, parameters.TotalLength);
        }

        /// <summary>
        /// Проверяет, что установка значения TotalLength вне диапазона вызывает исключение.
        /// </summary>
        [Fact]
        public void TotalLengthShouldThrowArgumentExceptionWhenInvalid()
        {
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;

            Assert.Throws<ArgumentException>(() => parameters.TotalLength = 65.0);
        }

        private static void SetInvalidFieldForException(ORSAPR.Parameters parameters)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_minLength",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(parameters, double.NaN);
        }

        private static void SetAngleUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_angle",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        private static void SetDiameterUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_diameter",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        private static void SetLengthUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_length",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        private static void SetTotalLengthUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_totalLength",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        private static void SetConeValueUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_coneValue",
                BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }
    }
}