using ORSAPR;
using System.Globalization;

namespace nUnitTest
{
    [TestFixture]
    public class DrillPresetTests
    {
        [Test]
        [Description("Создание пресета с минимальными параметрами")]
        public void Constructor_MinimalParameters_CreatesCorrectly()
        {
            var preset = new DrillPreset("Тестовый пресет", 10.0, 50.0,
                80.0, 45.0);

            Assert.That(preset.Name, Is.EqualTo("Тестовый пресет"));
            Assert.That(preset.Diameter, Is.EqualTo(10.0));
            Assert.That(preset.Length, Is.EqualTo(50.0));
            Assert.That(preset.TotalLength, Is.EqualTo(80.0));
            Assert.That(preset.Angle, Is.EqualTo(45.0));
            Assert.That(preset.HasCone, Is.False);
            Assert.That(preset.ConeValue, Is.EqualTo(0));
            Assert.That(preset.HasShank, Is.False);
            Assert.That(preset.ShankDiameter, Is.EqualTo(0));
            Assert.That(preset.ShankLength, Is.EqualTo(0));
        }

        [Test]
        [Description("Создание пресета со всеми параметрами")]
        public void Constructor_WithAllParameters_CreatesCorrectly()
        {
            var preset = new DrillPreset("Полный пресет", 15.0, 75.0,
                110.0, 60.0, hasCone: true, coneValue: 4.5,
                hasShank: true, shankDiameter: 26.0,
                shankLength: 35.0);

            Assert.That(preset.Name, Is.EqualTo("Полный пресет"));
            Assert.That(preset.Diameter, Is.EqualTo(15.0));
            Assert.That(preset.Length, Is.EqualTo(75.0));
            Assert.That(preset.TotalLength, Is.EqualTo(110.0));
            Assert.That(preset.Angle, Is.EqualTo(60.0));
            Assert.That(preset.HasCone, Is.True);
            Assert.That(preset.ConeValue, Is.EqualTo(4.5));
            Assert.That(preset.HasShank, Is.True);
            Assert.That(preset.ShankDiameter, Is.EqualTo(26.0));
            Assert.That(preset.ShankLength, Is.EqualTo(35.0));
        }

        [Test]
        [Description("Проверка свойств только для чтения")]
        public void Properties_AreReadOnly()
        {
            var preset = new DrillPreset("Тест", 10.0, 50.0, 80.0, 45.0);

            Assert.That(preset, Has.Property("Name"));
            Assert.That(preset, Has.Property("Diameter"));
            Assert.That(preset, Has.Property("Length"));
            Assert.That(preset, Has.Property("TotalLength"));
            Assert.That(preset, Has.Property("Angle"));
            Assert.That(preset, Has.Property("HasCone"));
            Assert.That(preset, Has.Property("ConeValue"));
            Assert.That(preset, Has.Property("HasShank"));
            Assert.That(preset, Has.Property("ShankDiameter"));
            Assert.That(preset, Has.Property("ShankLength"));
        }

        [Test]
        [Description("Проверка метода ToString возвращает имя")]
        public void ToString_ReturnsName()
        {
            var preset = new DrillPreset("Мой пресет", 10.0, 50.0,
                80.0, 45.0);

            Assert.That(preset.ToString(), Is.EqualTo("Мой пресет"));
        }

        [Test]
        [Description("Проверка с граничными значениями параметров")]
        public void Constructor_BoundaryValues_WorksCorrectly()
        {
            var preset = new DrillPreset("Граничный", 1.0, 3.0,
                23.0, 30.0, hasCone: true, coneValue: 0.25,
                hasShank: true, shankDiameter: 1.75,
                shankLength: 40.0);

            Assert.That(preset.Diameter, Is.EqualTo(1.0));
            Assert.That(preset.Length, Is.EqualTo(3.0));
            Assert.That(preset.TotalLength, Is.EqualTo(23.0));
            Assert.That(preset.Angle, Is.EqualTo(30.0));
            Assert.That(preset.ConeValue, Is.EqualTo(0.25));
            Assert.That(preset.ShankDiameter, Is.EqualTo(1.75));
            Assert.That(preset.ShankLength, Is.EqualTo(40.0));
        }

        [Test]
        [Description("Проверка с дробными значениями")]
        public void Constructor_FractionalValues_WorksCorrectly()
        {
            var preset = new DrillPreset("Дробный", 10.5, 52.5,
                85.7, 47.3, hasCone: true, coneValue: 3.14,
                hasShank: true, shankDiameter: 18.9,
                shankLength: 45.6);

            Assert.That(preset.Diameter, Is.EqualTo(10.5));
            Assert.That(preset.Length, Is.EqualTo(52.5));
            Assert.That(preset.TotalLength, Is.EqualTo(85.7));
            Assert.That(preset.Angle, Is.EqualTo(47.3));
            Assert.That(preset.ConeValue, Is.EqualTo(3.14));
            Assert.That(preset.ShankDiameter, Is.EqualTo(18.9));
            Assert.That(preset.ShankLength, Is.EqualTo(45.6));
        }
    }
}
