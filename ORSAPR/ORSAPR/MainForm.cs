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
        /// Cтроитель 3D-моделей сверла.
        /// </summary>
        private Builder _builder;

        /// <summary>
        /// Параметры сверла.
        /// </summary>
        private Parameters _parameters;

        /// <summary>
        /// Список пресетов параметров сверла.
        /// </summary>
        private readonly List<DrillPreset> _presets;

        /// <summary>
        /// Флаг указывающий, что происходит применение пресета.
        /// </summary>
        private bool _isApplyingPreset = false;

        /// <summary>
        /// Формат отображения чисел с двумя десятичными знаками.
        /// </summary>
        private const string NumberFormat = "F2";

        /// <summary>
        /// Инициализирует новый экземпляр класса MainForm.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _parameters = new Parameters();
            _builder = new Builder();
            _presets = CreatePresets();
            SetupPresetsComboBox();
            InitializeUI();
            InitializeEventHandlers();
        }

        /// <summary>
        /// Инициализирует пользовательский интерфейс.
        /// </summary>
        private void InitializeUI()
        {
            UpdateFormFromParameters();
            UpdateRangeLabels();
            UpdateVisibility();
            ResetFieldColors();
        }

        /// <summary>
        /// Инициализирует обработчики событий для элементов управления.
        /// </summary>
        private void InitializeEventHandlers()
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
        /// Обновляет форму из текущих параметров.
        /// </summary>
        private void UpdateFormFromParameters()
        {
            TextDiameter.Text = _parameters.Diameter.ToString(NumberFormat);
            TextLength.Text = _parameters.Length.ToString(NumberFormat);
            TextTotalLength.Text = _parameters.TotalLength.ToString(
                NumberFormat);
            TextAngle.Text = _parameters.Angle.ToString(NumberFormat);
            CheckClearanceCone.Checked = _parameters.ClearanceCone;
            TextConeValue.Text = _parameters.ConeValue.ToString(NumberFormat);
            CheckClearanceShank.Checked = _parameters.ClearanceShank;
            TextShankDiameterValue.Text = _parameters.ShankDiameterValue
                .ToString(NumberFormat);
            TextShankLengthValue.Text = _parameters.ShankLengthValue
                .ToString(NumberFormat);
        }

        /// <summary>
        /// Обновляет метки диапазонов на форме.
        /// </summary>
        private void UpdateRangeLabels()
        {
            LabelLengthRange.Text = FormatRangeLabel(
                _parameters.MinLength, _parameters.MaxLength, "мм");
            LabelTotalLengthRange.Text = FormatRangeLabel(
                _parameters.MinTotalLength, _parameters.MaxTotalLength, "мм");
            LabelDiameterRange.Text = FormatRangeLabel(
                _parameters.MinDiameter, _parameters.MaxDiameter, "мм");
            LabelAngleRange.Text = FormatRangeLabel(
                _parameters.MinAngle, _parameters.MaxAngle, "°");
            LabelConeValueRange.Text = FormatRangeLabel(
                _parameters.MinConeValue, _parameters.MaxConeValue, "мм");
            LabelShankDiameterValueRange.Text = FormatRangeLabel(
                _parameters.MinShankDiameterValue,
                _parameters.MaxShankDiameterValue, "мм");
            LabelShankLengthValueRange.Text = FormatRangeLabel(
                _parameters.MinShankLengthValue,
                _parameters.MaxShankLengthValue, "мм");
        }

        /// <summary>
        /// Форматирует метку диапазона.
        /// </summary>
        private string FormatRangeLabel(double min, double max, string unit)
        {
            return $"{min:F2} — {max:F2} {unit}";
        }

        /// <summary>
        /// Обновляет видимость элементов интерфейса.
        /// </summary>
        private void UpdateVisibility()
        {
            bool isConeChecked = CheckClearanceCone.Checked;
            bool isShankChecked = CheckClearanceShank.Checked;

            // Элементы для обратного конуса
            LabelConeValueName.Visible = isConeChecked;
            TextConeValue.Visible = isConeChecked;
            LabelConeValueRange.Visible = isConeChecked;

            // Элементы для хвостовика
            LabelShankDiameterValueName.Visible = isShankChecked;
            TextShankDiameterValue.Visible = isShankChecked;
            LabelShankDiameterValueRange.Visible = isShankChecked;
            LabelShankLengthValueName.Visible = isShankChecked;
            TextShankLengthValue.Visible = isShankChecked;
            LabelShankLengthValueRange.Visible = isShankChecked;

            // Также обновляем активность полей ввода
            TextConeValue.Enabled = isConeChecked;
            TextShankDiameterValue.Enabled = isShankChecked;
            TextShankLengthValue.Enabled = isShankChecked;
        }

        /// <summary>
        /// Сбрасывает цвета полей ввода.
        /// </summary>
        private void ResetFieldColors()
        {
            ResetTextBoxColor(TextDiameter);
            ResetTextBoxColor(TextLength);
            ResetTextBoxColor(TextTotalLength);
            ResetTextBoxColor(TextAngle);
            ResetTextBoxColor(TextConeValue);
            ResetTextBoxColor(TextShankDiameterValue);
            ResetTextBoxColor(TextShankLengthValue);
        }

        /// <summary>
        /// Сбрасывает цвет текстового поля.
        /// </summary>
        private void ResetTextBoxColor(TextBox textBox)
        {
            textBox.BackColor = SystemColors.Window;
        }

        /// <summary>
        /// Создает список предустановок.
        /// </summary>
        private List<DrillPreset> CreatePresets()
        {
            return new List<DrillPreset>
            {
                new DrillPreset("Сверло Ø10", 10, 55, 140, 45),
                new DrillPreset("Сверло Ø20мм", 20, 160, 205, 60),
                new DrillPreset("Сверло Ø1мм", 1, 3, 23, 30),
                new DrillPreset("Сверло Ø10мм с конусом", 10, 55, 140, 45,
                    hasCone: true, coneValue: 5),
                new DrillPreset("Сверло Ø20мм с конусом", 20, 160, 205, 60,
                    hasCone: true, coneValue: 15),
                new DrillPreset("Сверло Ø1мм с конусом", 1, 3, 23, 30,
                    hasCone: true, coneValue: 0.25),
                new DrillPreset("Сверло Ø10мм с хвостовиком", 10, 55, 140, 45,
                    hasShank: true, shankDiameter: 18.75,
                    shankLength: 212.5),
                new DrillPreset("Сверло Ø20мм с хвостовиком", 20, 160, 205, 60,
                    hasShank: true, shankDiameter: 40,
                    shankLength: 135),
                new DrillPreset("Сверло Ø1мм с хвостовиком", 1, 3, 23, 30,
                    hasShank: true, shankDiameter: 1.75,
                    shankLength: 40),
                new DrillPreset("Пользовательский", 10, 55, 88, 45)
            };
        }

        /// <summary>
        /// Настраивает ComboBox с пресетами.
        /// </summary>
        private void SetupPresetsComboBox()
        {
            ComboBoxPresets.DataSource = _presets;
            ComboBoxPresets.SelectedIndex = _presets.Count - 1;
        }

        /// <summary>
        /// Применяет выбранный пресет к интерфейсу.
        /// </summary>
        private void ApplyPreset(DrillPreset preset)
        {
            ToggleEventHandlers(false);

            // Установка значений
            TextDiameter.Text = preset.Diameter.ToString(NumberFormat);
            TextLength.Text = preset.Length.ToString(NumberFormat);
            TextTotalLength.Text = preset.TotalLength.ToString(NumberFormat);
            TextAngle.Text = preset.Angle.ToString(NumberFormat);

            CheckClearanceCone.Checked = preset.HasCone;
            TextConeValue.Text = preset.ConeValue.ToString(NumberFormat);

            CheckClearanceShank.Checked = preset.HasShank;
            TextShankDiameterValue.Text = preset.ShankDiameter.ToString(
                NumberFormat);
            TextShankLengthValue.Text = preset.ShankLength.ToString(
                NumberFormat);

            // Обновляем параметры из UI
            UpdateParametersFromForm();

            UpdateRangeLabels();

            ToggleEventHandlers(true);
        }

        /// <summary>
        /// Переключает на пользовательский пресет при изменении параметров.
        /// </summary>
        private void SwitchToCustomPreset()
        {
            if (_isApplyingPreset) return;

            int customIndex = _presets.FindIndex(p =>
                p.Name == "Пользовательский");
            if (customIndex >= 0 &&
                ComboBoxPresets.SelectedIndex != customIndex)
            {
                ComboBoxPresets.SelectedIndex = customIndex;
            }
        }

        /// <summary>
        /// Обрабатывает выбор пресета.
        /// </summary>
        private void ComboBoxPresetsSelectedIndexChanged(object sender,
            EventArgs e)
        {
            _isApplyingPreset = true;

            try
            {
                if (ComboBoxPresets.SelectedItem is DrillPreset preset)
                {
                    ApplyPreset(preset);
                }
            }
            finally
            {
                _isApplyingPreset = false;
            }
        }

        /// <summary>
        /// Включает/выключает обработчики событий для избежания рекурсии.
        /// </summary>
        private void ToggleEventHandlers(bool enable)
        {
            if (enable)
            {
                InitializeEventHandlers();
            }
            else
            {
                TextDiameter.TextChanged -= TextBoxTextChanged;
                TextLength.TextChanged -= TextBoxTextChanged;
                TextTotalLength.TextChanged -= TextBoxTextChanged;
                TextAngle.TextChanged -= TextBoxTextChanged;
                TextConeValue.TextChanged -= TextBoxTextChanged;
                TextShankDiameterValue.TextChanged -= TextBoxTextChanged;
                TextShankLengthValue.TextChanged -= TextBoxTextChanged;
                CheckClearanceCone.CheckedChanged -=
                    CheckClearanceConeCheckedChanged;
                CheckClearanceShank.CheckedChanged -=
                    CheckClearanceShankCheckedChanged;
            }
        }

        /// <summary>
        /// Обрабатывает изменение текста в текстовых полях.
        /// </summary>
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            SwitchToCustomPreset();

            var textBox = sender as TextBox;
            if (textBox == null) return;

            ValidateAndUpdateTextField(textBox);
            UpdateDependentFields(textBox);
            UpdateRangeLabels(); // Добавляем обновление меток
        }

        /// <summary>
        /// Обновляет все зависимые поля после изменения ключевого параметра.
        /// </summary>
        private void UpdateDependentFields(TextBox changedTextBox)
        {
            if (changedTextBox == TextDiameter)
            {
                // Diameter влияет на Length, TotalLength, ConeValue,
                // ShankDiameterValue, ShankLengthValue
                ValidateAndUpdateTextField(TextLength);
                ValidateAndUpdateTextField(TextTotalLength);
                ValidateAndUpdateTextField(TextConeValue);
                ValidateAndUpdateTextField(TextShankDiameterValue);
                ValidateAndUpdateTextField(TextShankLengthValue);
            }
            else if (changedTextBox == TextLength)
            {
                // Length влияет на TotalLength и ShankLengthValue
                ValidateAndUpdateTextField(TextTotalLength);
                ValidateAndUpdateTextField(TextShankLengthValue);
            }
            else if (changedTextBox == TextTotalLength)
            {
                // TotalLength влияет на ShankLengthValue
                ValidateAndUpdateTextField(TextShankLengthValue);
            }
        }

        /// <summary>
        /// Валидирует и обновляет текстовое поле.
        /// </summary>
        private void ValidateAndUpdateTextField(TextBox textBox)
        {
            var fieldName = GetFieldName(textBox);
            var result = Parameters.TryParseDouble(textBox.Text, fieldName);

            bool isValid = result.success && ValidateRange(textBox, result.value);

            textBox.BackColor = isValid ?
                SystemColors.Window : Color.LightPink;

            if (isValid)
            {
                UpdateParameterFromTextBox(textBox, result.value);
                UpdateRangeLabels();
            }
        }

        /// <summary>
        /// Проверяет диапазон значения.
        /// </summary>
        private bool ValidateRange(TextBox textBox, double value)
        {
            if (textBox == TextDiameter)
            {
                return value >= _parameters.MinDiameter &&
                       value <= _parameters.MaxDiameter;
            }
            if (textBox == TextLength)
            {
                return value >= _parameters.MinLength &&
                       value <= _parameters.MaxLength;
            }
            if (textBox == TextTotalLength)
            {
                return value >= _parameters.MinTotalLength &&
                       value <= _parameters.MaxTotalLength;
            }
            if (textBox == TextAngle)
            {
                return value >= _parameters.MinAngle &&
                       value <= _parameters.MaxAngle;
            }
            if (textBox == TextConeValue && CheckClearanceCone.Checked)
            {
                return value >= _parameters.MinConeValue &&
                       value <= _parameters.MaxConeValue;
            }
            if (textBox == TextShankDiameterValue &&
                CheckClearanceShank.Checked)
            {
                return value >= _parameters.MinShankDiameterValue &&
                       value <= _parameters.MaxShankDiameterValue;
            }
            if (textBox == TextShankLengthValue &&
                CheckClearanceShank.Checked)
            {
                return value >= _parameters.MinShankLengthValue &&
                       value <= _parameters.MaxShankLengthValue;
            }

            return true;
        }

        /// <summary>
        /// Получает имя поля для сообщения об ошибке.
        /// </summary>
        private string GetFieldName(TextBox textBox)
        {
            if (textBox == TextDiameter) return "Диаметр";
            if (textBox == TextLength) return "Длина рабочей части";
            if (textBox == TextTotalLength) return "Общая длина";
            if (textBox == TextAngle) return "Угол при вершине";
            if (textBox == TextConeValue) return "Обратный конус";
            if (textBox == TextShankDiameterValue) return "Диаметр хвостовика";
            if (textBox == TextShankLengthValue) return "Длина хвостовика";
            return "Поле";
        }

        /// <summary>
        /// Обновляет параметр из текстового поля.
        /// </summary>
        private void UpdateParameterFromTextBox(TextBox textBox, double value)
        {
            if (textBox == TextDiameter) _parameters.Diameter = value;
            else if (textBox == TextLength) _parameters.Length = value;
            else if (textBox == TextTotalLength) _parameters.TotalLength = value;
            else if (textBox == TextAngle) _parameters.Angle = value;
            else if (textBox == TextConeValue) _parameters.ConeValue = value;
            else if (textBox == TextShankDiameterValue)
                _parameters.ShankDiameterValue = value;
            else if (textBox == TextShankLengthValue)
                _parameters.ShankLengthValue = value;
        }

        /// <summary>
        /// Обрабатывает изменение состояния флажка обратного конуса.
        /// </summary>
        private void CheckClearanceConeCheckedChanged(object sender,
            EventArgs e)
        {
            SwitchToCustomPreset();

            // Делаем чекбоксы взаимоисключающими
            if (CheckClearanceCone.Checked)
            {
                CheckClearanceShank.Checked = false;
                _parameters.ClearanceCone = true;
                _parameters.ClearanceShank = false;
            }
            else
            {
                // Если оба сняты, можно оставить как есть
                _parameters.ClearanceCone = false;
            }

            UpdateVisibility();

            // Перевалидируем поле обратного конуса при изменении чекбокса
            ValidateAndUpdateTextField(TextConeValue);
        }

        /// <summary>
        /// Обрабатывает изменение состояния флажка хвостовика.
        /// </summary>
        private void CheckClearanceShankCheckedChanged(object sender,
            EventArgs e)
        {
            SwitchToCustomPreset();

            // Делаем чекбоксы взаимоисключающими
            if (CheckClearanceShank.Checked)
            {
                CheckClearanceCone.Checked = false;
                _parameters.ClearanceShank = true;
                _parameters.ClearanceCone = false;
            }
            else
            {
                // Если оба сняты, можно оставить как есть
                _parameters.ClearanceShank = false;
            }

            UpdateVisibility();

            // Перевалидируем поля хвостовика при изменении чекбокса
            ValidateAndUpdateTextField(TextShankDiameterValue);
            ValidateAndUpdateTextField(TextShankLengthValue);
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Построить".
        /// </summary>
        private void ButtonBuildClick(object sender, EventArgs e)
        {
            // Обновляем все параметры из формы
            UpdateParametersFromForm();

            // Валидируем бизнес-правила
            var validationErrors = _parameters.ValidateAll();

            if (validationErrors.Count > 0)
            {
                MessageBox.Show(string.Join("\n\n", validationErrors),
                    "Ошибки валидации", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Строим модель
            bool success = _builder.Build(_parameters);

            if (success)
            {

            }
            else
            {
                MessageBox.Show("Ошибка при построении модели.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обновляет все параметры из формы.
        /// </summary>
        private void UpdateParametersFromForm()
        {
            // Основные параметры
            UpdateParameterFromForm(TextDiameter, "Диаметр",
                value => _parameters.Diameter = value);
            UpdateParameterFromForm(TextLength, "Длина рабочей части",
                value => _parameters.Length = value);
            UpdateParameterFromForm(TextTotalLength, "Общая длина",
                value => _parameters.TotalLength = value);
            UpdateParameterFromForm(TextAngle, "Угол при вершине",
                value => _parameters.Angle = value);

            // Опциональные параметры
            _parameters.ClearanceCone = CheckClearanceCone.Checked;
            if (CheckClearanceCone.Checked)
            {
                UpdateParameterFromForm(TextConeValue, "Обратный конус",
                    value => _parameters.ConeValue = value);
            }
            else
            {
                _parameters.ConeValue = 0;
            }

            _parameters.ClearanceShank = CheckClearanceShank.Checked;
            if (CheckClearanceShank.Checked)
            {
                UpdateParameterFromForm(TextShankDiameterValue,
                    "Диаметр хвостовика",
                    value => _parameters.ShankDiameterValue = value);
                UpdateParameterFromForm(TextShankLengthValue,
                    "Длина хвостовика",
                    value => _parameters.ShankLengthValue = value);
            }
            else
            {
                _parameters.ShankDiameterValue = 0;
                _parameters.ShankLengthValue = 0;
            }
        }

        /// <summary>
        /// Обновляет параметр из формы.
        /// </summary>
        private void UpdateParameterFromForm(TextBox textBox,
            string fieldName, Action<double> setter)
        {
            var result = Parameters.TryParseDouble(textBox.Text, fieldName);
            if (result.success && ValidateRange(textBox, result.value))
            {
                setter(result.value);
            }
        }
    }
}