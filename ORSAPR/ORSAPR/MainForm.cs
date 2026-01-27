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
        /// Cтроитель 3D-моделей сверла.
        /// </summary>
        private Builder _builder;

        /// <summary>
        /// Параметры сверла.
        /// </summary>
        private Parameters _parameters;

        /// <summary>
        /// Объект класса ValidationField.
        /// </summary>
        private ValidationField _validator;

        /// <summary>
        /// Список пресетов параметров сверла.
        /// </summary>
        private readonly List<DrillPreset> _presets;

        /// <summary>
        /// Флаг указывающий, что происходит применение пресета
        /// </summary>
        private bool _isApplyingPreset = false;

        /// <summary>
        /// Формат отображения чисел с двумя десятичными знаками.
        /// </summary>
        private const string NumberFormat = "F2";

        /// <summary>
        /// Словарь для сопоставления текстовых полей с функциями валидации.
        /// </summary>
        private Dictionary<TextBox, Func<ValidationResult>> _validationMap;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainForm"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _parameters = new Parameters();
            _validator = new ValidationField(_parameters);
            _builder = new Builder();

            _presets = CreatePresets();
            SetupPresetsComboBox();
            InitializeParameters();
            InitializeValidationMap();
            InitilizeEventHandlers();
        }

        /// <summary>
        /// Инициализирует параметры сверла значениями по умолчанию.
        /// </summary>
        private void InitializeParameters()
        {
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
        /// Инициализирует словарь для сопоставления текстовых полей 
        /// с функциями валидации.
        /// </summary>
        private void InitializeValidationMap()
        {
            _validationMap = new Dictionary<TextBox, Func<ValidationResult>>
            {
                {
                    TextDiameter,
                    () => _validator.ValidateDiameter(TextDiameter.Text)
                },
                {
                    TextLength,
                    () => _validator.ValidateLength(TextLength.Text)
                },
                {
                    TextTotalLength,
                    () => _validator.ValidateTotalLength(TextTotalLength.Text)
                },
                {
                    TextAngle,
                    () => _validator.ValidateAngle(TextAngle.Text)
                },
                {
                    TextConeValue,
                    () => _validator.ValidateConeValue(
                        TextConeValue.Text, CheckClearanceCone.Checked)
                },
                {
                    TextShankDiameterValue,
                    () => _validator.ValidateShankDiameterValue(
                        TextShankDiameterValue.Text,
                        CheckClearanceShank.Checked)
                },
                {
                    TextShankLengthValue,
                    () => _validator.ValidateShankLengthValue(
                        TextShankLengthValue.Text,
                        CheckClearanceShank.Checked)
                }
            };
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
        /// Создает список предустановок
        /// </summary>
        private List<DrillPreset> CreatePresets()
        {
            return new List<DrillPreset>
            {
                // Средние значения.
            new DrillPreset("Сверло Ø10", 10, 55, 140, 45),
            // Максимальные значения.
            new DrillPreset("Сверло Ø20мм", 20, 160, 205, 60),
            // Минимальные значения
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
        /// Настраивает ComboBox с пресетами
        /// </summary>
        private void SetupPresetsComboBox()
        {
            ComboBoxPresets.DataSource = _presets;
            ComboBoxPresets.SelectedIndex = _presets.Count - 1;
            // Последний - пользовательский
        }

        /// <summary>
        /// Применяет выбранный пресет к интерфейсу
        /// </summary>
        private void ApplyPreset(DrillPreset preset)
        {
            // Временное отключение обработчика событий
            ToggleEventHandlers(false);

            // Установка значений
            TextDiameter.Text = preset.Diameter.ToString(NumberFormat);
            TextLength.Text = preset.Length.ToString(NumberFormat);
            TextTotalLength.Text = preset.TotalLength.ToString(NumberFormat);
            TextAngle.Text = preset.Angle.ToString(NumberFormat);

            CheckClearanceCone.Checked = preset.HasCone;
            TextConeValue.Text = preset.ConeValue.ToString(NumberFormat);

            CheckClearanceShank.Checked = preset.HasShank;
            TextShankDiameterValue.Text = preset.ShankDiameter.ToString(NumberFormat);
            TextShankLengthValue.Text = preset.ShankLength.ToString(NumberFormat);

            // Обновление параметров
            _validator.TryUpdateParameters(
                TextDiameter.Text, TextLength.Text, TextTotalLength.Text,
                TextAngle.Text, TextConeValue.Text, preset.HasCone,
                TextShankDiameterValue.Text, TextShankLengthValue.Text,
                preset.HasShank, out _
            );

            UpdateRanges();

            // Включаем обработчики обратно
            ToggleEventHandlers(true);
        }

        /// <summary>
        /// Переключает на пользовательский пресет при изменении параметров
        /// </summary>
        private void SwitchToCustomPreset()
        {
            // Не переключаем если применяем пресет
            if (_isApplyingPreset) return;

            int customIndex = _presets.FindIndex(p => 
            p.Name == "Пользовательский");
            if (customIndex >= 0 && ComboBoxPresets.SelectedIndex
                != customIndex)
            {
                ComboBoxPresets.SelectedIndex = customIndex;
            }
        }

        /// <summary>
        /// Обработчик событий ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxPresetsSelectedIndexChanged(object sender,
            EventArgs e)
        {

            // Временно отключаем SwitchToCustomPreset
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
        /// Включает/выключает обработчики событий для избежания рекурсии
        /// </summary>
        private void ToggleEventHandlers(bool enable)
        {
            if (enable)
            {
                InitilizeEventHandlers();
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
        /// Обработчик изменения текста в текстовых полях.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void TextBoxTextChanged(object sender, EventArgs e)
        {
            SwitchToCustomPreset();

            // TODO: refactor +
            var textBox = sender as TextBox;
            if (textBox == null || _validationMap == null) return;

            if (_validationMap.TryGetValue(textBox, out var validateFunc))
            {
                ValidationResult result = validateFunc();
                UpdateTextBoxAppearance(textBox, result);

                // Обновляем ключевые параметры, влияющие на диапазоны
                UpdateKeyParametersFromUI();
            }

            UpdateRanges();
        }

        /// <summary>
        /// Обновляет ключевые параметры из UI.
        /// </summary>
        private void UpdateKeyParametersFromUI()
        {
            // Обновляем только параметры, влияющие на диапазоны
            if (double.TryParse(TextDiameter.Text, out double diameter))
            {
                _parameters.Diameter = diameter;
            }

            if (double.TryParse(TextLength.Text, out double length))
            {
                _parameters.Length = length;
            }

            if (double.TryParse(TextTotalLength.Text, out double totalLength))
            {
                _parameters.TotalLength = totalLength;
            }
        }

        /// <summary>
        /// Обновляет внешний вид текстового поля на основе результата
        /// валидации.
        /// </summary>
        /// <param name="textBox">Текстовое поле для обновления.</param>
        /// <param name="result">Результат валидации.</param>
        private void UpdateTextBoxAppearance(TextBox textBox,
            ValidationResult result)
        {
            textBox.BackColor = result.IsValid ?
                SystemColors.Window : Color.LightPink;
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
        /// обратным конусом и хвостовиком.
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
        /// Обработчик события изменения состояния чекбокса 
        /// "Наличие обратного конуса".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void CheckClearanceConeCheckedChanged(object sender, EventArgs e)
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
            if (_validationMap != null &&
                _validationMap.ContainsKey(TextConeValue))
            {
                TextBoxTextChanged(TextConeValue, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Обработчик события изменения состояния чекбокса хвостовика.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Данные события.</param>
        private void CheckClearanceShankCheckedChanged(object sender, EventArgs e)
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
            if (_validationMap != null)
            {
                if (_validationMap.ContainsKey(TextShankDiameterValue))
                {
                    TextBoxTextChanged(TextShankDiameterValue, EventArgs.Empty);
                }

                if (_validationMap.ContainsKey(TextShankLengthValue))
                {
                    TextBoxTextChanged(TextShankLengthValue, EventArgs.Empty);
                }
            }
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