using Core.Model;
using Core.Validation;
using ORSAPR;

namespace nUnitTest
{
    [TestFixture]
    public class FieldValidationResultTests
    {
        [Test]
        [Description("Проверка установки свойств в конструкторе")]
        public void ConstructorShouldSetPropertiesCorrectly()
        {
            var fieldName = "TestField";
            var validationResult = ValidationResult.Success(10.0);

            var fieldValidationResult = new FieldValidationResult(
                fieldName, validationResult);

            Assert.That(fieldValidationResult.FieldName, Is.EqualTo(fieldName));
            Assert.That(fieldValidationResult.Result, Is.EqualTo(validationResult));
        }
    }
}