using Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private ValidationField _validator;

        /// <summary>
        /// Формат отображения чисел с одним десятичным знаком.
        /// </summary>
        private const string NumberFormat = "F1";

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainForm"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitializeParameters();
            InitilizeEventHandlers();
        }

        /// <summary>
        /// Инициализирует параметры сверла значениями по умолчанию.
        /// </summary>
        private void InitializeParameters()
        {
            _parameters = new Parameters();
            _builder = new Builder();
            _validator = new ValidationField(_parameters);

            // Установка начальных значений из уже рассчитанных параметров
            TextDiameter.Text = _parameters.Diameter.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            TextLength.Text = _parameters.Length.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            TextTotalLength.Text = _parameters.TotalLength.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            TextAngle.Text = _parameters.Angle.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            TextConeValue.Text = _parameters.ConeValue.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            CheckClearanceCone.Checked = _parameters.ClearanceCone;
            TextShankDiameterValue.Text =
                _parameters.ShankDiameterValue.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            TextShankLengthValue.Text =
                _parameters.ShankLengthValue.ToString(
                NumberFormat, CultureInfo.CurrentCulture);
            CheckClearanceShank.Checked = _parameters.ClearanceShank;

            UpdateVisibility();
            UpdateRanges();
            ResetFieldColors();
        }

        /// <summary>
        /// Инициализирует обработчики событий для элементов управления.
        /// </summary>
        private void InitilizeEventHandlers()
        {
            TextDiameter.TextChanged += TextBoxTextChanged;
            TextLength.TextChanged += TextBoxTextChanged;
            TextTotalLength.TextChanged += TextBoxTextChanged;
            TextAngle.TextChanged += TextBoxTextChanged;
            TextConeValue.TextChanged += TextBoxTextChanged;
            CheckClearanceCone.CheckedChanged +=
                CheckClearanceConeCheckedChanged;
            TextShankDiameterValue.TextChanged += TextBoxTextChanged;
            TextShankLengthValue.TextChanged += TextBoxTextChanged;
            CheckClearanceShank.CheckedChanged +=
                CheckClearanceShankCheckedChanged;
        }

        /// <summary>
        /// Обработчик изменения текста в текстовых полях.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
            {
                return;
            }

            ValidationResult result = null;

            // Только валидация, без обновления параметров
            if (textBox == TextDiameter)
            {
                result = _validator.ValidateDiameter(textBox.Text);
            }
            else if (textBox == TextLength)
            {
                result = _validator.ValidateLength(textBox.Text);
            }
            else if (textBox == TextTotalLength)
            {
                result = _validator.ValidateTotalLength(textBox.Text);
            }
            else if (textBox == TextAngle)
            {
                result = _validator.ValidateAngle(textBox.Text);
            }
            else if (textBox == TextConeValue)
            {
                result = _validator.ValidateConeValue(
                    textBox.Text, CheckClearanceCone.Checked);
            }
            else if (textBox == TextShankDiameterValue)
            {
                result = _validator.ValidateShankDiameterValue(
                    textBox.Text, CheckClearanceShank.Checked);
            }
            else if (textBox == TextShankLengthValue)
            {
                result = _validator.ValidateShankLengthValue(
                    textBox.Text, CheckClearanceShank.Checked);
            }

            // Обновление UI
            if (result != null)
            {
                textBox.BackColor = result.IsValid ?
                    SystemColors.Window : Color.LightPink;
            }
        }

        /// <summary>
        /// Обновляет отображение диапазонов допустимых значений в интерфейсе.
        /// </summary>
        private void UpdateRanges()
        {
            var culture = CultureInfo.CurrentCulture;

            LabelLengthRange.Text =
                $"{_parameters.MinLength.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxLength.ToString(NumberFormat, culture)} мм";
            LabelTotalLengthRange.Text =
                $"{_parameters.MinTotalLength.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxTotalLength.ToString(NumberFormat, culture)} мм";
            LabelDiameterRange.Text =
                $"{_parameters.MinDiameter.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxDiameter.ToString(NumberFormat, culture)} мм";
            LabelAngleRange.Text =
                $"{_parameters.MinAngle.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxAngle.ToString(NumberFormat, culture)}°";
            LabelConeValueRange.Text =
                $"{_parameters.MinConeValue.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxConeValue.ToString(NumberFormat, culture)} мм";
            LabelShankDiameterValueRange.Text =
                $"{_parameters.MinShankDiameterValue.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxShankDiameterValue.ToString(NumberFormat, culture)} мм";
            LabelShankLengthValueRange.Text =
                $"{_parameters.MinShankLengthValue.ToString(NumberFormat, culture)}-" +
                $"{_parameters.MaxShankLengthValue.ToString(NumberFormat, culture)} мм";
        }

        /// <summary>
        /// Сбрасывает цвета фона всех полей ввода к значениям по умолчанию.
        /// </summary>
        private void ResetFieldColors()
        {
            // Убираем цвет, установленный в конструкторе
            TextDiameter.BackColor = SystemColors.Window;
            TextLength.BackColor = SystemColors.Window;
            TextTotalLength.BackColor = SystemColors.Window;
            TextAngle.BackColor = SystemColors.Window;
            TextConeValue.BackColor = SystemColors.Window;
            TextShankDiameterValue.BackColor = SystemColors.Window;
            TextShankLengthValue.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Обновляет видимость элементов интерфейса, связанных с 
        /// обратным конусом.
        /// </summary>
        private void UpdateVisibility()
        {
            TextConeValue.Enabled = CheckClearanceCone.Checked;
            TextShankDiameterValue.Enabled = CheckClearanceShank.Checked;
            TextShankLengthValue.Enabled = CheckClearanceShank.Checked;
        }

        /// <summary>
        /// Обработчик события изменения состояния чекбокса 
        /// "Наличие обратного конуса".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void CheckClearanceConeCheckedChanged(object sender, EventArgs e)
        {
            // Если включаем этот чекбокс, выключаем другой
            if (CheckClearanceCone.Checked && CheckClearanceShank.Checked)
            {
                CheckClearanceShank.Checked = false;
            }

            UpdateVisibility();
            TextBoxTextChanged(TextConeValue, EventArgs.Empty);
        }

        /// <summary>
        /// Обработчик события изменения состояния чекбокса хвостовика.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void CheckClearanceShankCheckedChanged(object sender, EventArgs e)
        {
            // Если включаем этот чекбокс, выключаем другой
            if (CheckClearanceShank.Checked && CheckClearanceCone.Checked)
            {
                CheckClearanceCone.Checked = false;
            }

            UpdateVisibility();
            TextBoxTextChanged(TextShankDiameterValue, EventArgs.Empty);
            TextBoxTextChanged(TextShankLengthValue, EventArgs.Empty);
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Построить модель".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void ButtonBuildClick(object sender, EventArgs e)
        {
            // Используем TryUpdateParameters для валидации и обновления параметров
            if (!_validator.TryUpdateParameters(TextDiameter.Text,
                TextLength.Text, TextTotalLength.Text, TextAngle.Text,
                TextConeValue.Text, CheckClearanceCone.Checked,
                TextShankDiameterValue.Text, TextShankLengthValue.Text,
                CheckClearanceShank.Checked,
                out List<string> errors))
            {
                ShowErrorMessage(string.Join("\n", errors));
                return;
            }

            // Проверяем бизнес-правила (зависимости между полями)
            List<string> rulesErrors = _parameters.ValidateRules();
            if (rulesErrors.Count > 0)
            {
                ShowErrorMessage(string.Join("\n", rulesErrors));
                return;
            }

            // Все проверки пройдены - строим модель
            bool success = _builder.Build(_parameters);
            if (!success)
            {
                ShowErrorMessage("Не удалось построить модель. " +
                    "Проверьте подключение к КОМПАС-3D");
            }
        }

        /// <summary>
        /// Отображает сообщение об ошибке в диалоговом окне.
        /// </summary>
        /// <param name="message">Текст сообщения об ошибке.</param>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}