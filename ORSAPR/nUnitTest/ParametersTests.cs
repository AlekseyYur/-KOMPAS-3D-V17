using Core.Model;
using ORSAPR;

namespace nUnitTest
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        [Description("Проверка валидации когда длина равна минимальной границе")]
        public void ValidateRules_LengthEqualsMinBoundary_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 30.0; // Минимум: 3×10
            parameters.TotalLength = 50.0; // 30+20

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации когда длина равна максимальной границе")]
        public void ValidateRules_LengthEqualsMaxBoundary_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 80.0; // Максимум: 8×10
            parameters.TotalLength = 100.0; // 80+20

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации когда общая длина равна минимальной границе")]
        public void ValidateRules_TotalLengthEqualsMinBoundary_NoError()
        {
            var parameters = new Parameters();
            parameters.Length = 50.0;
            parameters.TotalLength = 70.0; // 50+20

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации когда общая длина равна максимальной границе")]
        public void ValidateRules_TotalLengthEqualsMaxBoundary_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 20.0;
            parameters.Length = 160.0; // 205-20
            parameters.TotalLength = 205.0; // Максимум

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации обратного конуса на нижней границе")]
        public void ValidateRules_ConeValueAtMinBoundaryEnabled_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 2.5; // Минимум: 0.25×10

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации обратного конуса на верхней границе")]
        public void ValidateRules_ConeValueAtMaxBoundaryEnabled_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 7.5; // Максимум: 0.75×10

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации обратного конуса ниже минимума")]
        public void ValidateRules_ConeValueBelowMinEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 2.4; // Ниже 2.5

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("обратного конуса"));
        }

        [Test]
        [Description("Проверка валидации обратного конуса выше максимума")]
        public void ValidateRules_ConeValueAboveMaxEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 7.6; // Выше 7.5

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("обратного конуса"));
        }

        [Test]
        [Description("Проверка валидации диаметра хвостовика на нижней границе")]
        public void ValidateRules_ShankDiameterAtMinBoundaryEnabled_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 12.5; // Минимум: 1.25×10

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации диаметра хвостовика на верхней границе")]
        public void ValidateRules_ShankDiameterAtMaxBoundaryEnabled_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 20.0; // Максимум: 2×10

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации диаметра хвостовика ниже минимума")]
        public void ValidateRules_ShankDiameterBelowMinEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 12.4; // Ниже 12.5

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("диаметра хвостовика"));
        }

        [Test]
        [Description("Проверка валидации диаметра хвостовика выше максимума")]
        public void ValidateRules_ShankDiameterAboveMaxEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 20.1; // Выше 20.0

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("диаметра хвостовика"));
        }

        [Test]
        [Description("Проверка валидации длины хвостовика на нижней границе")]
        public void ValidateRules_ShankLengthAtMinBoundaryEnabled_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 80.0;
            parameters.ClearanceShank = true;
            parameters.ShankLengthValue = 60.0; // Минимум: 2×(80-50)

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации длины хвостовика на верхней границе")]
        public void ValidateRules_ShankLengthAtMaxBoundaryEnabled_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 80.0;
            parameters.ClearanceShank = true;
            parameters.ShankLengthValue = 90.0; // Максимум: 3×(80-50)

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации длины хвостовика ниже минимума")]
        public void ValidateRules_ShankLengthBelowMinEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 80.0;
            parameters.ClearanceShank = true;
            parameters.ShankLengthValue = 59.9; // Ниже 60.0

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("диаметра хвостовика"));
        }

        [Test]
        [Description("Проверка валидации длины хвостовика выше максимума")]
        public void ValidateRules_ShankLengthAboveMaxEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 80.0;
            parameters.ClearanceShank = true;
            parameters.ShankLengthValue = 90.1; // Выше 90.0

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("диаметра хвостовика"));
        }

        [Test]
        [Description("Проверка валидации с включенным конусом и выключенным хвостовиком")]
        public void ValidateRules_ConeEnabledShankDisabled_ValidatesOnlyCone()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 55.0;
            parameters.TotalLength = 75.0;
            parameters.ClearanceCone = true;
            parameters.ConeValue = 5.0;
            parameters.ClearanceShank = false;
            parameters.ShankDiameterValue = 100.0; // Не должно проверяться
            parameters.ShankLengthValue = 200.0;   // Не должно проверяться

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка валидации с выключенным конусом и включенным хвостовиком")]
        public void ValidateRules_ConeDisabledShankEnabled_ValidatesOnlyShank()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 55.0;
            parameters.TotalLength = 75.0;
            parameters.ClearanceCone = false;
            parameters.ConeValue = 100.0; // Не должно проверяться
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 15.0;
            parameters.ShankLengthValue = 60.0;

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка что конус не проверяется при выключенном флаге даже если невалиден")]
        public void ValidateRules_ConeDisabledInvalidValue_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.ClearanceCone = false;
            parameters.ConeValue = 100.0; // Далеко за пределами диапазона

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка что хвостовик не проверяется при выключенном флаге даже если невалиден")]
        public void ValidateRules_ShankDisabledInvalidValues_NoError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 50.0;
            parameters.TotalLength = 80.0;
            parameters.ClearanceShank = false;
            parameters.ShankDiameterValue = 100.0; // Не должно проверяться
            parameters.ShankLengthValue = 200.0;   // Не должно проверяться

            var errors = parameters.ValidateRules();
            Assert.That(errors, Is.Empty);
        }

        [Test]
        [Description("Проверка формата чисел в сообщениях об ошибках")]
        public void ValidateRules_ErrorMessages_UseCorrectNumberFormat()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 100.0; // Выше максимума
            parameters.TotalLength = 120.0; // Чтобы не было ошибки по TotalLength

            var errors = parameters.ValidateRules();
            Assert.That(errors[0], Contains.Substring("30,0 - 80,0").Or.Contains("30.0 - 80.0"));
        }

        [Test]
        [Description("Проверка когда длина меньше минимальной")]
        public void ValidateRules_LengthLessThanMin_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 29.9; // Меньше 30.0
            parameters.TotalLength = 49.9; // 29.9+20

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("Длина рабочей части"));
        }

        [Test]
        [Description("Проверка когда длина больше максимальной")]
        public void ValidateRules_LengthGreaterThanMax_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 80.1; // Больше 80.0
            parameters.TotalLength = 100.1; // 80.1+20

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("Длина рабочей части"));
        }

        [Test]
        [Description("Проверка когда общая длина меньше минимальной")]
        public void ValidateRules_TotalLengthLessThanMin_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Length = 50.0;
            parameters.TotalLength = 69.9; // Меньше 50.0+20=70.0

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("Общая длина"));
        }

        [Test]
        [Description("Проверка когда общая длина больше максимальной")]
        public void ValidateRules_TotalLengthGreaterThanMax_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Length = 50.0;
            parameters.TotalLength = 205.1; // Больше 205.0

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Contains.Substring("Общая длина"));
        }

        [Test]
        [Description("Проверка метода ValidateRules с нулевым значением хвостовика")]
        public void ValidateRules_ZeroShankValuesEnabled_ReturnsError()
        {
            var parameters = new Parameters();
            parameters.Diameter = 10.0;
            parameters.Length = 55.0;
            parameters.TotalLength = 75.0;
            parameters.ClearanceShank = true;
            parameters.ShankDiameterValue = 0.0; // Ноль
            parameters.ShankLengthValue = 0.0;   // Ноль

            var errors = parameters.ValidateRules();
            Assert.That(errors, Has.Count.EqualTo(2));
        }
    }
}