using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ORSAPR
{
    /// <summary>
    /// Главная форма приложения для построения 3D-модели сверла.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Построитель 3D-моделей сверла.
        /// </summary>
        private Builder _builder;

        /// <summary>
        /// Параметры сверла.
        /// </summary>
        private Parameters _parameters;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainForm"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            txtDiameter.TextChanged += txtDiameter_TextChanged;
            txtLength.TextChanged += txtLength_TextChanged;
            txtTotalLength.TextChanged += txtTotalLength_TextChanged;
            txtAngle.TextChanged += txtAngle_TextChanged;
            txtConeValue.TextChanged += txtConeValue_TextChanged;
            chkClearanceCone.CheckedChanged += chkClearanceCone_CheckedChanged;

            InitializeParameters();
            UpdateRanges();
        }

        /// <summary>
        /// Инициализирует параметры сверла значениями по умолчанию.
        /// </summary>
        private void InitializeParameters()
        {
            _parameters = new Parameters();
            _builder = new Builder();

            txtDiameter.Text = _parameters.Diameter.ToString("F1");
            txtLength.Text = _parameters.Length.ToString("F1");
            txtTotalLength.Text = _parameters.TotalLength.ToString("F1");
            txtAngle.Text = _parameters.Angle.ToString("F1");
            chkClearanceCone.Checked = _parameters.ClearanceCone;
            txtConeValue.Text = _parameters.ConeValue.ToString("F1");

            UpdateConeValueVisibility();
            ResetAllFieldColors();
        }

        /// <summary>
        /// Сбрасывает цвета фона всех полей ввода к значениям по умолчанию.
        /// </summary>
        private void ResetAllFieldColors()
        {
            txtDiameter.BackColor = SystemColors.Window;
            txtLength.BackColor = SystemColors.Window;
            txtTotalLength.BackColor = SystemColors.Window;
            txtAngle.BackColor = SystemColors.Window;
            txtConeValue.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Обновляет видимость элементов интерфейса, связанных с обратным конусом.
        /// </summary>
        private void UpdateConeValueVisibility()
        {
            bool visible = chkClearanceCone.Checked;
            txtConeValue.Visible = visible;
            lblConeValueName.Visible = visible;
            lblConeValueRange.Visible = visible;
        }

        /// <summary>
        /// Выполняет валидацию поля ввода диаметра сверла.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если значение диаметра валидно; 
        /// <c>false</c> - если значение невалидно или произошла ошибка при установке.
        /// </returns>
        private bool ValidateDiameterField()
        {
            if (string.IsNullOrWhiteSpace(txtDiameter.Text))
            {
                txtDiameter.BackColor = Color.LightPink;
                return false;
            }

            if (double.TryParse(txtDiameter.Text, out double value))
            {
                if (value < _parameters.MinDiameter || value > _parameters.MaxDiameter)
                {
                    txtDiameter.BackColor = Color.LightPink;
                    return false;
                }

                try
                {
                    _parameters.Diameter = value;
                    txtDiameter.BackColor = SystemColors.Window;
                    UpdateRanges();
                    return true;
                }
                catch
                {
                    txtDiameter.BackColor = Color.LightPink;
                    return false;
                }
            }
            else
            {
                txtDiameter.BackColor = Color.LightPink;
                return false;
            }
        }

        /// <summary>
        /// Выполняет валидацию поля ввода длины рабочей части сверла.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если значение длины валидно; 
        /// <c>false</c> - если значение невалидно или произошла ошибка при установке.
        /// </returns>
        private bool ValidateLengthField()
        {
            if (string.IsNullOrWhiteSpace(txtLength.Text))
            {
                txtLength.BackColor = Color.LightPink;
                return false;
            }

            if (double.TryParse(txtLength.Text, out double value))
            {
                if (value < _parameters.MinLength || value > _parameters.MaxLength)
                {
                    txtLength.BackColor = Color.LightPink;
                    return false;
                }
                try
                {
                    _parameters.Length = value;
                    txtLength.BackColor = SystemColors.Window;
                    UpdateRanges();
                    return true;
                }
                catch
                {
                    txtLength.BackColor = Color.LightPink;
                    return false;
                }
            }
            else
            {
                txtLength.BackColor = Color.LightPink;
                return false;
            }
        }

        /// <summary>
        /// Выполняет валидацию поля ввода общей длины сверла.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если значение общей длины валидно; 
        /// <c>false</c> - если значение невалидно или произошла ошибка при установке.
        /// </returns>
        private bool ValidateTotalLengthField()
        {
            if (string.IsNullOrWhiteSpace(txtTotalLength.Text))
            {
                txtTotalLength.BackColor = Color.LightPink;
                return false;
            }

            if (double.TryParse(txtTotalLength.Text, out double value))
            {
                if (value < _parameters.MinTotalLength || value > _parameters.MaxTotalLength)
                {
                    txtTotalLength.BackColor = Color.LightPink;
                    return false;
                }
                try
                {
                    _parameters.TotalLength = value;
                    txtTotalLength.BackColor = SystemColors.Window;
                    return true;
                }
                catch
                {
                    txtTotalLength.BackColor = Color.LightPink;
                    return false;
                }
            }
            else
            {
                txtTotalLength.BackColor = Color.LightPink;
                return false;
            }
        }

        /// <summary>
        /// Выполняет валидацию поля ввода угла при вершине сверла.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если значение угла валидно; 
        /// <c>false</c> - если значение невалидно или произошла ошибка при установке.
        /// </returns>
        private bool ValidateAngleField()
        {
            if (string.IsNullOrWhiteSpace(txtAngle.Text))
            {
                txtAngle.BackColor = Color.LightPink;
                return false;
            }

            if (double.TryParse(txtAngle.Text, out double value))
            {
                if (value < _parameters.MinAngle || value > _parameters.MaxAngle)
                {
                    txtAngle.BackColor = Color.LightPink;
                    return false;
                }
                try
                {
                    _parameters.Angle = value;
                    txtAngle.BackColor = SystemColors.Window;
                    return true;
                }
                catch
                {
                    txtAngle.BackColor = Color.LightPink;
                    return false;
                }
            }
            else
            {
                txtAngle.BackColor = Color.LightPink;
                return false;
            }
        }

        /// <summary>
        /// Выполняет валидацию поля ввода значения обратного конуса.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если значение обратного конуса валидно или валидация не требуется; 
        /// <c>false</c> - если значение невалидно или произошла ошибка при установке.
        /// </returns>
        private bool ValidateConeValueField()
        {
            if (!chkClearanceCone.Checked)
            {
                txtConeValue.BackColor = SystemColors.Window;
                return true;
            }

            if (string.IsNullOrWhiteSpace(txtConeValue.Text))
            {
                txtConeValue.BackColor = Color.LightPink;
                return false;
            }

            if (double.TryParse(txtConeValue.Text, out double value))
            {
                if (value < _parameters.MinConeValue || value > _parameters.MaxConeValue)
                {
                    txtConeValue.BackColor = Color.LightPink;
                    return false;
                }
                try
                {
                    _parameters.ConeValue = value;
                    txtConeValue.BackColor = SystemColors.Window;
                    return true;
                }
                catch
                {
                    txtConeValue.BackColor = Color.LightPink;
                    return false;
                }
            }
            else
            {
                txtConeValue.BackColor = Color.LightPink;
                return false;
            }
        }

        /// <summary>
        /// Выполняет построение 3D-модели сверла на основе введенных параметров.
        /// </summary>
        private void BuildModel()
        {
            if (!ValidateAllFields())
            {
                ShowErrorMessage("Исправьте ошибки в полях ввода");
                return;
            }

            UpdateParametersFromUI();

            var errors = _parameters.ValidateAndCalculate();

            if (errors.Count > 0)
            {
                ShowErrorMessage(errors[0]);
                return;
            }

            bool success = _builder.Build(_parameters);

            if (success) { }
            else
            {
                ShowErrorMessage("Не удалось построить сверло. Проверьте подключение к КОМПАС-3D.");
            }
        }

        /// <summary>
        /// Обновляет отображение диапазонов допустимых значений в интерфейсе.
        /// </summary>
        private void UpdateRanges()
        {
            lblLengthRange.Text = $"{_parameters.MinLength:F1}-{_parameters.MaxLength:F1} мм";
            lblTotalLengthRange.Text = $"{_parameters.MinTotalLength:F1}-{_parameters.MaxTotalLength} мм";
            lblDiameterRange.Text = $"{_parameters.MinDiameter:F1}-{_parameters.MaxDiameter:F1} мм";
            lblAngleRange.Text = $"{_parameters.MinAngle:F1}-{_parameters.MaxAngle:F1}°";
            lblConeValueRange.Text = $"{_parameters.MinConeValue:F2}-{_parameters.MaxConeValue:F1} мм";
        }

        /// <summary>
        /// Обновляет параметры сверла на основе значений из интерфейса.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Выбрасывается при некорректном формате числовых значений в полях ввода.
        /// </exception>
        private void UpdateParametersFromUI()
        {
            try
            {
                double oldDiameter = _parameters.Diameter;
                double oldLength = _parameters.Length;

                if (!double.TryParse(txtDiameter.Text, out double diameter))
                    throw new ArgumentException("Неверный формат диаметра");
                _parameters.Diameter = diameter;

                if (!double.TryParse(txtLength.Text, out double length))
                    throw new ArgumentException("Неверный формат длины");
                _parameters.Length = length;

                if (!double.TryParse(txtTotalLength.Text, out double totalLength))
                    throw new ArgumentException("Неверный формат общей длины");
                _parameters.TotalLength = totalLength;

                if (!double.TryParse(txtAngle.Text, out double angle))
                    throw new ArgumentException("Неверный формат угла");
                _parameters.Angle = angle;

                _parameters.ClearanceCone = chkClearanceCone.Checked;

                if (chkClearanceCone.Checked)
                {
                    if (!double.TryParse(txtConeValue.Text, out double coneValue))
                        throw new ArgumentException("Неверный формат значения конуса");
                    _parameters.ConeValue = coneValue;
                }

                if (oldDiameter != _parameters.Diameter || oldLength != _parameters.Length)
                {
                    UpdateRanges();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Ошибка обновления параметров: {ex.Message}");
            }
        }

        /// <summary>
        /// Выполняет валидацию всех полей ввода на форме.
        /// </summary>
        /// <returns>
        /// <c>true</c> - если все поля содержат валидные значения; 
        /// <c>false</c> - если хотя бы одно поле содержит невалидное значение.
        /// </returns>
        private bool ValidateAllFields()
        {
            bool diameterValid = ValidateDiameterField();
            bool lengthValid = ValidateLengthField();
            bool totalLengthValid = ValidateTotalLengthField();
            bool angleValid = ValidateAngleField();
            bool coneValueValid = ValidateConeValueField();

            return diameterValid && lengthValid && totalLengthValid && angleValid && coneValueValid;
        }

        /// <summary>
        /// Отображает сообщение об ошибке в диалоговом окне.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Обработчик события изменения текста в поле ввода диаметра.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void txtDiameter_TextChanged(object sender, EventArgs e)
        {
            ValidateDiameterField();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле ввода длины рабочей части.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            ValidateLengthField();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле ввода общей длины.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void txtTotalLength_TextChanged(object sender, EventArgs e)
        {
            ValidateTotalLengthField();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле ввода угла при вершине.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void txtAngle_TextChanged(object sender, EventArgs e)
        {
            ValidateAngleField();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле ввода значения обратного конуса.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void txtConeValue_TextChanged(object sender, EventArgs e)
        {
            ValidateConeValueField();
        }

        /// <summary>
        /// Обработчик события изменения состояния чекбокса "Наличие обратного конуса".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void chkClearanceCone_CheckedChanged(object sender, EventArgs e)
        {
            UpdateConeValueVisibility();
            ValidateConeValueField();
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Построить модель".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void btnBuild_Click(object sender, EventArgs e)
        {
            BuildModel();
        }
    }
}