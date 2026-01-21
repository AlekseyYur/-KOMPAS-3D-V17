using Core.Model;
using Core.Validation;
using ORSAPR;

namespace nUnitTest
{
    [TestFixture]
    public class ValidationResultTests
    {
        [Test]
        [Description("Проверка создания успешного результата валидации")]
        public void SuccessShouldCreateValidResultWithValue()
        {
            var result = ValidationResult.Success(42.5);

            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Value, Is.EqualTo(42.5));
            Assert.That(result.ErrorMessage, Is.Null);
        }

        [Test]
        [Description("Проверка создания результата валидации с ошибкой")]
        public void ErrorShouldCreateInvalidResultWithErrorMessage()
        {
            var errorMessage = "Test error message";
            var result = ValidationResult.Error(errorMessage);

            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Value, Is.EqualTo(0));
            Assert.That(result.ErrorMessage, Is.EqualTo(errorMessage));
        }
    }
}
