using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTest
{
    /// <summary>
    /// Тестовый класс для проверки функциональности класса <see cref="ORSAPR.Parameters"/>.
    /// </summary>
    /// <remarks>
    /// Класс содержит модульные тесты для проверки корректности работы класса <see cref="ORSAPR.Parameters"/>.
    /// Тесты охватывают все основные сценарии: инициализацию, установку значений, валидацию и расчеты.
    /// Все тесты независимы и используют собственные экземпляры тестируемого класса.
    /// </remarks>
    public class ParametersTests
    {
        #region Тесты конструктора и свойств

        /// <summary>
        /// Проверяет, что конструктор класса <see cref="ORSAPR.Parameters"/> инициализирует свойства значениями по умолчанию.
        /// </summary>
        /// <remarks>
        /// Тест проверяет следующие значения по умолчанию:
        /// <list type="bullet">
        /// <item><description>Угол при вершине: 90°</description></item>
        /// <item><description>Наличие обратного конуса: <c>true</c></description></item>
        /// <item><description>Значение обратного конуса: 0.1 мм</description></item>
        /// <item><description>Диаметр: 10 мм</description></item>
        /// <item><description>Длина рабочей части: 55 мм</description></item>
        /// <item><description>Общая длина: 75 мм</description></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void ConstructorShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var parameters = new ORSAPR.Parameters();

            // Assert
            Assert.Equal(90.0, parameters.Angle);
            Assert.True(parameters.ClearanceCone);
            Assert.Equal(0.1, parameters.ConeValue);
            Assert.Equal(10.0, parameters.Diameter);
            Assert.Equal(55.0, parameters.Length);
            Assert.Equal(75.0, parameters.TotalLength);
        }

        /// <summary>
        /// Проверяет, что конструктор корректно вычисляет зависимые свойства после инициализации.
        /// </summary>
        /// <remarks>
        /// Тест проверяет расчет следующих зависимых свойств:
        /// <list type="bullet">
        /// <item><description>Минимальная длина рабочей части: 3×d = 30 мм</description></item>
        /// <item><description>Максимальная длина рабочей части: 8×d = 80 мм</description></item>
        /// <item><description>Минимальная общая длина: l + 20 = 75 мм</description></item>
        /// <item><description>Максимальная общая длина: 205 мм</description></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void ConstructorShouldCalculateDependentProperties()
        {
            // Arrange & Act
            var parameters = new ORSAPR.Parameters();

            // Assert
            Assert.Equal(30.0, parameters.MinLength);
            Assert.Equal(80.0, parameters.MaxLength);
            Assert.Equal(75.0, parameters.MinTotalLength);
            Assert.Equal(205.0, parameters.MaxTotalLength);
        }

        /// <summary>
        /// Проверяет, что константные диапазоны свойств имеют корректные значения.
        /// </summary>
        /// <remarks>
        /// Тест проверяет следующие константные значения:
        /// <list type="bullet">
        /// <item><description>Минимальный угол: 90°</description></item>
        /// <item><description>Максимальный угол: 140°</description></item>
        /// <item><description>Минимальное значение обратного конуса: 0.05 мм</description></item>
        /// <item><description>Максимальное значение обратного конуса: 30 мм</description></item>
        /// <item><description>Минимальный диаметр: 1 мм</description></item>
        /// <item><description>Максимальный диаметр: 20 мм</description></item>
        /// <item><description>Максимальная общая длина: 205 мм</description></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void PropertiesShouldHaveCorrectConstantRanges()
        {
            // Arrange & Act
            var parameters = new ORSAPR.Parameters();

            // Assert
            Assert.Equal(90.0, parameters.MinAngle);
            Assert.Equal(140.0, parameters.MaxAngle);
            Assert.Equal(0.05, parameters.MinConeValue);
            Assert.Equal(30.0, parameters.MaxConeValue);
            Assert.Equal(1.0, parameters.MinDiameter);
            Assert.Equal(20.0, parameters.MaxDiameter);
            Assert.Equal(205.0, parameters.MaxTotalLength);
        }

        #endregion

        #region Тесты успешной установки значений

        /// <summary>
        /// Проверяет корректную установку допустимого значения угла при вершине.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает значение угла 120° (в допустимом диапазоне 90-140°) 
        /// и проверяет, что значение корректно сохранилось.
        /// </remarks>
        [Fact]
        public void AngleShouldSetValidValue()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double validAngle = 120.0;

            // Act
            parameters.Angle = validAngle;

            // Assert
            Assert.Equal(validAngle, parameters.Angle);
        }

        /// <summary>
        /// Проверяет корректную установку диаметра и автоматический пересчет зависимых свойств.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 15 мм и проверяет:
        /// <list type="number">
        /// <item><description>Значение диаметра корректно установлено</description></item>
        /// <item><description>Минимальная длина пересчитана: 3×15 = 45 мм</description></item>
        /// <item><description>Максимальная длина пересчитана: 8×15 = 120 мм</description></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void DiameterShouldSetValueAndRecalculateDependentProperties()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double newDiameter = 15.0;

            // Act
            parameters.Diameter = newDiameter;

            // Assert
            Assert.Equal(newDiameter, parameters.Diameter);
            Assert.Equal(45.0, parameters.MinLength);
            Assert.Equal(120.0, parameters.MaxLength);
        }

        /// <summary>
        /// Проверяет корректную установку длины рабочей части и обновление диапазона общей длины.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 10 мм и длину рабочей части 50 мм, затем проверяет:
        /// <list type="number">
        /// <item><description>Значение длины корректно установлено</description></item>
        /// <item><description>Минимальная общая длина пересчитана: 50 + 20 = 70 мм</description></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void LengthShouldSetValueAndUpdateTotalLengthRange()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            double validLength = 50.0;

            // Act
            parameters.Length = validLength;

            // Assert
            Assert.Equal(validLength, parameters.Length);
            Assert.Equal(70.0, parameters.MinTotalLength);
        }

        /// <summary>
        /// Проверяет корректную установку значения наличия обратного конуса.
        /// </summary>
        /// <remarks>
        /// Тест проверяет установку обоих возможных значений:
        /// <list type="number">
        /// <item><description>Установка в <c>true</c> и проверка, что значение установлено</description></item>
        /// <item><description>Установка в <c>false</c> и проверка, что значение установлено</description></item>
        /// </list>
        /// </remarks>
        [Fact]
        public void ClearanceConeShouldSetTrueAndFalseValues()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();

            // Act & Assert для true
            parameters.ClearanceCone = true;
            Assert.True(parameters.ClearanceCone);

            // Act & Assert для false
            parameters.ClearanceCone = false;
            Assert.False(parameters.ClearanceCone);
        }

        #endregion

        #region Тесты метода ValidateAndCalculate

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает пустой список для валидных параметров.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает все параметры в допустимые значения:
        /// <list type="bullet">
        /// <item><description>Угол: 118° (в диапазоне 90-140)</description></item>
        /// <item><description>Диаметр: 12.5 мм (в диапазоне 1-20)</description></item>
        /// <item><description>Длина рабочей части: 75 мм (в диапазоне 37.5-100 для диаметра 12.5)</description></item>
        /// <item><description>Общая длина: 95 мм (в диапазоне 95-205 для длины 75)</description></item>
        /// <item><description>Наличие обратного конуса: <c>true</c></description></item>
        /// <item><description>Значение обратного конуса: 0.5 мм (в диапазоне 0.05-30)</description></item>
        /// </list>
        /// Затем проверяет, что метод не возвращает ошибок.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnEmptyListForValidParameters()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Angle = 118.0;
            parameters.Diameter = 12.5;
            parameters.Length = 75.0;
            parameters.TotalLength = 95.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 0.5;

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.Empty(errors);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> не валидирует значение обратного конуса, когда обратный конус отключен.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает <see cref="ORSAPR.Parameters.ClearanceCone"/> в <c>false</c> и невалидное значение конуса 35 мм
        /// (больше максимального значения 30 мм). Поскольку обратный конус отключен, валидация этого параметра не должна выполняться.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldNotValidateConeValueWhenClearanceConeIsFalse()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.ClearanceCone = false;
            // Устанавливаем невалидное значение, но оно должно игнорироваться
            // Нужно использовать рефлексию, так как сеттер выбросит исключение
            SetConeValueUsingReflection(parameters, 35.0);

            // Остальные параметры валидны
            parameters.Angle = 100.0;
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 100.0;

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.Empty(errors);
        }

        #endregion

        #region Тесты проверки исключений при установке значений

        /// <summary>
        /// Проверяет, что установка значения угла меньше минимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест пытается установить угол 89.9° при минимальном допустимом значении 90°.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void AngleShouldThrowArgumentExceptionWhenBelowMin()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double invalidAngle = 89.9;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.Angle = invalidAngle);
        }

        /// <summary>
        /// Проверяет, что установка значения угла больше максимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест пытается установить угол 140.1° при максимальном допустимом значении 140°.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void AngleShouldThrowArgumentExceptionWhenAboveMax()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double invalidAngle = 140.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.Angle = invalidAngle);
        }

        /// <summary>
        /// Проверяет, что установка значения диаметра меньше минимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест пытается установить диаметр 0.5 мм при минимальном допустимом значении 1 мм.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void DiameterShouldThrowArgumentExceptionWhenBelowMin()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double invalidDiameter = 0.5;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.Diameter = invalidDiameter);
        }

        /// <summary>
        /// Проверяет, что установка значения диаметра больше максимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест пытается установить диаметр 20.1 мм при максимальном допустимом значении 20 мм.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void DiameterShouldThrowArgumentExceptionWhenAboveMax()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double invalidDiameter = 20.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.Diameter = invalidDiameter);
        }

        /// <summary>
        /// Проверяет, что установка значения длины рабочей части меньше минимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 10 мм (минимальная длина = 30 мм) и пытается установить длину 29.9 мм.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void LengthShouldThrowArgumentExceptionWhenBelowMin()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0; // MinLength=30
            double invalidLength = 29.9;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.Length = invalidLength);
        }

        /// <summary>
        /// Проверяет, что установка значения длины рабочей части больше максимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 10 мм (максимальная длина = 80 мм) и пытается установить длину 80.1 мм.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void LengthShouldThrowArgumentExceptionWhenAboveMax()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0; // MaxLength=80
            double invalidLength = 80.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.Length = invalidLength);
        }

        /// <summary>
        /// Проверяет, что установка значения обратного конуса меньше минимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест пытается установить значение обратного конуса 0.04 мм при минимальном допустимом значении 0.05 мм.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void ConeValueShouldThrowArgumentExceptionWhenBelowMin()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double invalidConeValue = 0.04;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.ConeValue = invalidConeValue);
        }

        /// <summary>
        /// Проверяет, что установка значения обратного конуса больше максимального вызывает исключение <see cref="ArgumentException"/>.
        /// </summary>
        /// <remarks>
        /// Тест пытается установить значение обратного конуса 30.1 мм при максимальном допустимом значении 30 мм.
        /// Ожидается, что будет выброшено исключение.
        /// </remarks>
        [Fact]
        public void ConeValueShouldThrowArgumentExceptionWhenAboveMax()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double invalidConeValue = 30.1;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => parameters.ConeValue = invalidConeValue);
        }

        #endregion

        #region Тесты зависимых свойств и расчетов

        /// <summary>
        /// Проверяет, что максимальное значение общей длины всегда равно 205 мм.
        /// </summary>
        [Fact]
        public void TotalLengthMaximumValueShouldBe205()
        {
            // Arrange & Act
            var parameters = new ORSAPR.Parameters();

            // Assert
            Assert.Equal(205.0, parameters.MaxTotalLength);
        }

        /// <summary>
        /// Проверяет, что установка свойств вызывает пересчет зависимых свойств.
        /// </summary>
        /// <remarks>
        /// Тест проверяет, что изменение диаметра с 10 мм на 15 мм приводит к пересчету
        /// минимальной длины рабочей части с 30 мм на 45 мм.
        /// </remarks>
        [Fact]
        public void PropertySettersShouldTriggerRecalculations()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            double initialMinLength = parameters.MinLength;

            // Act
            parameters.Diameter = 15.0;

            // Assert
            Assert.NotEqual(initialMinLength, parameters.MinLength);
            Assert.Equal(45.0, parameters.MinLength);
        }

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Устанавливает значение обратного конуса с использованием рефлексии, обходя валидацию.
        /// </summary>
        /// <param name="parameters">Экземпляр класса <see cref="ORSAPR.Parameters"/>.</param>
        /// <param name="value">Значение для установки.</param>
        /// <remarks>
        /// Используется в тестах для установки невалидных значений без выброса исключения.
        /// Позволяет тестировать метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> с невалидными данными.
        /// </remarks>
        private void SetConeValueUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            // Используем рефлексию для установки значения без валидации
            var field = typeof(ORSAPR.Parameters).GetField("_coneValue",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        #endregion
    }

    /// <summary>
    /// Тестовый класс для проверки расчетов зависимых параметров класса <see cref="ORSAPR.Parameters"/>.
    /// </summary>
    /// <remarks>
    /// Класс содержит параметризованные тесты для проверки корректности расчетов
    /// зависимых параметров при различных входных данных.
    /// </remarks>
    public class ParametersCalculationTests
    {
        /// <summary>
        /// Проверяет корректность расчета диапазона длины рабочей части для различных значений диаметра.
        /// </summary>
        /// <param name="diameter">Диаметр сверла для тестирования.</param>
        /// <param name="expectedMin">Ожидаемое минимальное значение длины рабочей части.</param>
        /// <param name="expectedMax">Ожидаемое максимальное значение длины рабочей части.</param>
        /// <remarks>
        /// Тест проверяет расчет по формулам:
        /// <list type="bullet">
        /// <item><description>Минимальная длина: 3 × диаметр</description></item>
        /// <item><description>Максимальная длина: 8 × диаметр</description></item>
        /// </list>
        /// Тестируются следующие сценарии:
        /// <list type="bullet">
        /// <item><description>Минимальный диаметр (1 мм)</description></item>
        /// <item><description>Средний диаметр (10 мм)</description></item>
        /// <item><description>Максимальный диаметр (20 мм)</description></item>
        /// <item><description>Нецелый диаметр (5.5 мм)</description></item>
        /// </list>
        /// </remarks>
        [Theory]
        [InlineData(1.0, 3.0, 8.0)]     // Минимальный диаметр
        [InlineData(10.0, 30.0, 80.0)]  // Средний диаметр
        [InlineData(20.0, 60.0, 160.0)] // Максимальный диаметр
        [InlineData(5.5, 16.5, 44.0)]   // Нецелый диаметр
        public void LengthRangeShouldBeCorrectForDiameter(
            double diameter, double expectedMin, double expectedMax)
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();

            // Act
            parameters.Diameter = diameter;

            // Assert
            Assert.Equal(expectedMin, parameters.MinLength);
            Assert.Equal(expectedMax, parameters.MaxLength);
        }

        /// <summary>
        /// Проверяет корректность расчета минимальной общей длины для различных значений длины рабочей части.
        /// </summary>
        /// <param name="length">Длина рабочей части для тестирования.</param>
        /// <param name="expectedMinTotalLength">Ожидаемое минимальное значение общей длины.</param>
        /// <remarks>
        /// Тест проверяет расчет по формуле: минимальная общая длина = длина рабочей части + 20 мм.
        /// Тестируются следующие сценарии:
        /// <list type="bullet">
        /// <item><description>Минимальная рабочая длина (30 мм)</description></item>
        /// <item><description>Максимальная рабочая длина (80 мм)</description></item>
        /// <item><description>Средняя рабочая длина (55 мм)</description></item>
        /// </list>
        /// Перед тестом устанавливается диаметр 10 мм для обеспечения валидности длины рабочей части.
        /// </remarks>
        [Theory]
        [InlineData(30.0, 50.0)]  // Минимальная рабочая длина
        [InlineData(80.0, 100.0)] // Максимальная рабочая длина
        [InlineData(55.0, 75.0)]  // Средняя рабочая длина
        public void MinTotalLengthShouldBeLengthPlus20(
            double length, double expectedMinTotalLength)
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;

            // Act
            parameters.Length = length;

            // Assert
            Assert.Equal(expectedMinTotalLength, parameters.MinTotalLength);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает ошибку для невалидного угла.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает угол 85° (ниже минимального значения 90°) с использованием рефлексии
        /// и проверяет, что метод валидации возвращает одну ошибку с соответствующим сообщением.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidAngle()
        {
            // Arrange - создаем параметры с невалидным углом через рефлексию
            var parameters = new ORSAPR.Parameters();
            SetAngleUsingReflection(parameters, 85.0); // Невалидно, минимум 90

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("Угол при вершине", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает ошибку для невалидного диаметра.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 25 мм (выше максимального значения 20 мм) с использованием рефлексии
        /// и проверяет, что метод валидации возвращает одну ошибку с соответствующим сообщением.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidDiameter()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            SetDiameterUsingReflection(parameters, 25.0); // Невалидно, максимум 20

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("диаметра", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает ошибку для невалидной длины рабочей части.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 10 мм (диапазон длины: 30-80 мм) и длину 25 мм (ниже минимального значения)
        /// с использованием рефлексии. Проверяет, что метод валидации возвращает одну ошибку с соответствующим сообщением.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidLength()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0; // MinLength=30, MaxLength=80
            SetLengthUsingReflection(parameters, 25.0); // Невалидно

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("длины рабочей части", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает ошибку для невалидной общей длины.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает диаметр 10 мм, длину рабочей части 50 мм (минимальная общая длина = 70 мм)
        /// и общую длину 65 мм (ниже минимального значения) с использованием рефлексии.
        /// Проверяет, что метод валидации возвращает одну ошибку с соответствующим сообщением.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidTotalLength()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0; // MinTotalLength=70
            SetTotalLengthUsingReflection(parameters, 65.0); // Невалидно

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("длины сверла", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает ошибку для невалидного значения обратного конуса.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает наличие обратного конуса в <c>true</c> и значение конуса 35 мм
        /// (выше максимального значения 30 мм) с использованием рефлексии.
        /// Проверяет, что метод валидации возвращает одну ошибку с соответствующим сообщением.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnErrorForInvalidConeValue()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            parameters.ClearanceCone = true;
            SetConeValueUsingReflection(parameters, 35.0); // Невалидно, максимум 30

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Single(errors);
            Assert.Contains("обратного конуса", errors[0]);
        }

        /// <summary>
        /// Проверяет, что метод <see cref="ORSAPR.Parameters.ValidateAndCalculate"/> возвращает все ошибки при наличии нескольких невалидных параметров.
        /// </summary>
        /// <remarks>
        /// Тест устанавливает три невалидных параметра с использованием рефлексии:
        /// <list type="number">
        /// <item><description>Угол: 85° (ниже минимального)</description></item>
        /// <item><description>Диаметр: 25 мм (выше максимального)</description></item>
        /// <item><description>Значение обратного конуса: 35 мм (выше максимального)</description></item>
        /// </list>
        /// Проверяет, что метод валидации возвращает ровно три ошибки.
        /// </remarks>
        [Fact]
        public void ValidateAndCalculateShouldReturnAllErrorsForMultipleInvalidParameters()
        {
            // Arrange
            var parameters = new ORSAPR.Parameters();
            SetAngleUsingReflection(parameters, 85.0); // Ошибка 1
            SetDiameterUsingReflection(parameters, 25.0); // Ошибка 2
            parameters.ClearanceCone = true;
            SetConeValueUsingReflection(parameters, 35.0); // Ошибка 3

            // Act
            var errors = parameters.ValidateAndCalculate();

            // Assert
            Assert.Equal(3, errors.Count);
        }

        #region Вспомогательные методы для рефлексии

        /// <summary>
        /// Устанавливает значение угла с использованием рефлексии, обходя валидацию.
        /// </summary>
        /// <param name="parameters">Экземпляр класса <see cref="ORSAPR.Parameters"/>.</param>
        /// <param name="value">Значение угла для установки.</param>
        private void SetAngleUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_angle",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        /// <summary>
        /// Устанавливает значение диаметра с использованием рефлексии, обходя валидацию.
        /// </summary>
        /// <param name="parameters">Экземпляр класса <see cref="ORSAPR.Parameters"/>.</param>
        /// <param name="value">Значение диаметра для установки.</param>
        private void SetDiameterUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_diameter",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        /// <summary>
        /// Устанавливает значение длины рабочей части с использованием рефлексии, обходя валидацию.
        /// </summary>
        /// <param name="parameters">Экземпляр класса <see cref="ORSAPR.Parameters"/>.</param>
        /// <param name="value">Значение длины рабочей части для установки.</param>
        private void SetLengthUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_length",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        /// <summary>
        /// Устанавливает значение общей длины с использованием рефлексии, обходя валидацию.
        /// </summary>
        /// <param name="parameters">Экземпляр класса <see cref="ORSAPR.Parameters"/>.</param>
        /// <param name="value">Значение общей длины для установки.</param>
        private void SetTotalLengthUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_totalLength",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        /// <summary>
        /// Устанавливает значение обратного конуса с использованием рефлексии, обходя валидацию.
        /// </summary>
        /// <param name="parameters">Экземпляр класса <see cref="ORSAPR.Parameters"/>.</param>
        /// <param name="value">Значение обратного конуса для установки.</param>
        private void SetConeValueUsingReflection(ORSAPR.Parameters parameters, double value)
        {
            var field = typeof(ORSAPR.Parameters).GetField("_coneValue",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(parameters, value);
        }

        #endregion
    }
}