namespace ORSAPR
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelDiameterName = new System.Windows.Forms.Label();
            this.LabelLengthName = new System.Windows.Forms.Label();
            this.LabelTotalLengthName = new System.Windows.Forms.Label();
            this.LabelAngleName = new System.Windows.Forms.Label();
            this.LabelConeClearanceName = new System.Windows.Forms.Label();
            this.TextDiameter = new System.Windows.Forms.TextBox();
            this.TextLength = new System.Windows.Forms.TextBox();
            this.TextTotalLength = new System.Windows.Forms.TextBox();
            this.TextAngle = new System.Windows.Forms.TextBox();
            this.LabelDiameterRange = new System.Windows.Forms.Label();
            this.LabelLengthRange = new System.Windows.Forms.Label();
            this.LabelTotalLengthRange = new System.Windows.Forms.Label();
            this.LabelAngleRange = new System.Windows.Forms.Label();
            this.ButtonBuild = new System.Windows.Forms.Button();
            this.CheckClearanceCone = new System.Windows.Forms.CheckBox();
            this.LabelConeValueRange = new System.Windows.Forms.Label();
            this.TextConeValue = new System.Windows.Forms.TextBox();
            this.LabelShankClearanceName = new System.Windows.Forms.Label();
            this.CheckClearanceShank = new System.Windows.Forms.CheckBox();
            this.LabelShankDiameterValueName = new System.Windows.Forms.Label();
            this.TextShankLengthValue = new System.Windows.Forms.TextBox();
            this.TextShankDiameterValue = new System.Windows.Forms.TextBox();
            this.LabelShankLengthValueName = new System.Windows.Forms.Label();
            this.LabelNessesaryName = new System.Windows.Forms.Label();
            this.LabelOptionalName = new System.Windows.Forms.Label();
            this.LabelConeValueName = new System.Windows.Forms.Label();
            this.LabelShankDiameterValueRange = new System.Windows.Forms.Label();
            this.LabelShankLengthValueRange = new System.Windows.Forms.Label();
            this.ComboBoxPresets = new System.Windows.Forms.ComboBox();
            this.LablePreset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LabelDiameterName
            // 
            this.LabelDiameterName.AutoSize = true;
            this.LabelDiameterName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelDiameterName.Location = new System.Drawing.Point(19, 99);
            this.LabelDiameterName.Name = "LabelDiameterName";
            this.LabelDiameterName.Size = new System.Drawing.Size(88, 23);
            this.LabelDiameterName.TabIndex = 0;
            this.LabelDiameterName.Text = "Диаметр d";
            // 
            // LabelLengthName
            // 
            this.LabelLengthName.AutoSize = true;
            this.LabelLengthName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelLengthName.Location = new System.Drawing.Point(19, 134);
            this.LabelLengthName.Name = "LabelLengthName";
            this.LabelLengthName.Size = new System.Drawing.Size(123, 23);
            this.LabelLengthName.TabIndex = 1;
            this.LabelLengthName.Text = "Рабочая часть l";
            // 
            // LabelTotalLengthName
            // 
            this.LabelTotalLengthName.AutoSize = true;
            this.LabelTotalLengthName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelTotalLengthName.Location = new System.Drawing.Point(19, 169);
            this.LabelTotalLengthName.Name = "LabelTotalLengthName";
            this.LabelTotalLengthName.Size = new System.Drawing.Size(123, 23);
            this.LabelTotalLengthName.TabIndex = 2;
            this.LabelTotalLengthName.Text = "Общая длина L";
            // 
            // LabelAngleName
            // 
            this.LabelAngleName.AutoSize = true;
            this.LabelAngleName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelAngleName.Location = new System.Drawing.Point(19, 204);
            this.LabelAngleName.Name = "LabelAngleName";
            this.LabelAngleName.Size = new System.Drawing.Size(157, 23);
            this.LabelAngleName.TabIndex = 3;
            this.LabelAngleName.Text = "Угол при вершине a";
            // 
            // LabelConeClearanceName
            // 
            this.LabelConeClearanceName.AutoSize = true;
            this.LabelConeClearanceName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelConeClearanceName.Location = new System.Drawing.Point(44, 287);
            this.LabelConeClearanceName.Name = "LabelConeClearanceName";
            this.LabelConeClearanceName.Size = new System.Drawing.Size(130, 23);
            this.LabelConeClearanceName.TabIndex = 4;
            this.LabelConeClearanceName.Text = "Обратный конус";
            // 
            // TextDiameter
            // 
            this.TextDiameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.TextDiameter.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextDiameter.Location = new System.Drawing.Point(193, 96);
            this.TextDiameter.Name = "TextDiameter";
            this.TextDiameter.Size = new System.Drawing.Size(76, 29);
            this.TextDiameter.TabIndex = 5;
            this.TextDiameter.Text = "21";
            this.TextDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextDiameter.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // TextLength
            // 
            this.TextLength.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextLength.Location = new System.Drawing.Point(193, 131);
            this.TextLength.Name = "TextLength";
            this.TextLength.Size = new System.Drawing.Size(76, 29);
            this.TextLength.TabIndex = 6;
            this.TextLength.Text = "64";
            this.TextLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextLength.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // TextTotalLength
            // 
            this.TextTotalLength.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextTotalLength.Location = new System.Drawing.Point(193, 166);
            this.TextTotalLength.Name = "TextTotalLength";
            this.TextTotalLength.Size = new System.Drawing.Size(76, 29);
            this.TextTotalLength.TabIndex = 7;
            this.TextTotalLength.Text = "88";
            this.TextTotalLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextTotalLength.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // TextAngle
            // 
            this.TextAngle.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextAngle.Location = new System.Drawing.Point(193, 201);
            this.TextAngle.Name = "TextAngle";
            this.TextAngle.Size = new System.Drawing.Size(76, 29);
            this.TextAngle.TabIndex = 8;
            this.TextAngle.Text = "97";
            this.TextAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextAngle.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // LabelDiameterRange
            // 
            this.LabelDiameterRange.AutoSize = true;
            this.LabelDiameterRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelDiameterRange.Location = new System.Drawing.Point(297, 99);
            this.LabelDiameterRange.Name = "LabelDiameterRange";
            this.LabelDiameterRange.Size = new System.Drawing.Size(87, 23);
            this.LabelDiameterRange.TabIndex = 10;
            this.LabelDiameterRange.Text = "1 — 20 мм";
            // 
            // LabelLengthRange
            // 
            this.LabelLengthRange.AutoSize = true;
            this.LabelLengthRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelLengthRange.Location = new System.Drawing.Point(297, 134);
            this.LabelLengthRange.Name = "LabelLengthRange";
            this.LabelLengthRange.Size = new System.Drawing.Size(130, 23);
            this.LabelLengthRange.TabIndex = 11;
            this.LabelLengthRange.Text = "3 × d — 8 × d мм";
            // 
            // LabelTotalLengthRange
            // 
            this.LabelTotalLengthRange.AutoSize = true;
            this.LabelTotalLengthRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelTotalLengthRange.Location = new System.Drawing.Point(297, 169);
            this.LabelTotalLengthRange.Name = "LabelTotalLengthRange";
            this.LabelTotalLengthRange.Size = new System.Drawing.Size(125, 23);
            this.LabelTotalLengthRange.TabIndex = 12;
            this.LabelTotalLengthRange.Text = "l + 20 — 205 мм";
            // 
            // LabelAngleRange
            // 
            this.LabelAngleRange.AutoSize = true;
            this.LabelAngleRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelAngleRange.Location = new System.Drawing.Point(297, 204);
            this.LabelAngleRange.Name = "LabelAngleRange";
            this.LabelAngleRange.Size = new System.Drawing.Size(87, 23);
            this.LabelAngleRange.TabIndex = 13;
            this.LabelAngleRange.Text = "90 — 140°";
            // 
            // ButtonBuild
            // 
            this.ButtonBuild.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonBuild.Location = new System.Drawing.Point(193, 398);
            this.ButtonBuild.Name = "ButtonBuild";
            this.ButtonBuild.Size = new System.Drawing.Size(76, 31);
            this.ButtonBuild.TabIndex = 15;
            this.ButtonBuild.Text = "Построить";
            this.ButtonBuild.UseVisualStyleBackColor = true;
            this.ButtonBuild.Click += new System.EventHandler(this.ButtonBuildClick);
            // 
            // CheckClearanceCone
            // 
            this.CheckClearanceCone.AutoSize = true;
            this.CheckClearanceCone.Checked = true;
            this.CheckClearanceCone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckClearanceCone.Location = new System.Drawing.Point(23, 295);
            this.CheckClearanceCone.Name = "CheckClearanceCone";
            this.CheckClearanceCone.Size = new System.Drawing.Size(15, 14);
            this.CheckClearanceCone.TabIndex = 17;
            this.CheckClearanceCone.UseVisualStyleBackColor = true;
            this.CheckClearanceCone.CheckedChanged += new System.EventHandler(this.CheckClearanceConeCheckedChanged);
            // 
            // LabelConeValueRange
            // 
            this.LabelConeValueRange.AutoSize = true;
            this.LabelConeValueRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelConeValueRange.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabelConeValueRange.Location = new System.Drawing.Point(297, 318);
            this.LabelConeValueRange.Name = "LabelConeValueRange";
            this.LabelConeValueRange.Size = new System.Drawing.Size(109, 23);
            this.LabelConeValueRange.TabIndex = 14;
            this.LabelConeValueRange.Text = "0,05 — 10 мм";
            // 
            // TextConeValue
            // 
            this.TextConeValue.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextConeValue.Location = new System.Drawing.Point(193, 318);
            this.TextConeValue.Name = "TextConeValue";
            this.TextConeValue.Size = new System.Drawing.Size(76, 29);
            this.TextConeValue.TabIndex = 9;
            this.TextConeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextConeValue.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // LabelShankClearanceName
            // 
            this.LabelShankClearanceName.AutoSize = true;
            this.LabelShankClearanceName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelShankClearanceName.Location = new System.Drawing.Point(201, 287);
            this.LabelShankClearanceName.Name = "LabelShankClearanceName";
            this.LabelShankClearanceName.Size = new System.Drawing.Size(85, 23);
            this.LabelShankClearanceName.TabIndex = 19;
            this.LabelShankClearanceName.Text = "Хвостовик";
            // 
            // CheckClearanceShank
            // 
            this.CheckClearanceShank.AutoSize = true;
            this.CheckClearanceShank.Location = new System.Drawing.Point(180, 295);
            this.CheckClearanceShank.Name = "CheckClearanceShank";
            this.CheckClearanceShank.Size = new System.Drawing.Size(15, 14);
            this.CheckClearanceShank.TabIndex = 20;
            this.CheckClearanceShank.UseVisualStyleBackColor = true;
            // 
            // LabelShankDiameterValueName
            // 
            this.LabelShankDiameterValueName.AutoSize = true;
            this.LabelShankDiameterValueName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelShankDiameterValueName.Location = new System.Drawing.Point(19, 356);
            this.LabelShankDiameterValueName.Name = "LabelShankDiameterValueName";
            this.LabelShankDiameterValueName.Size = new System.Drawing.Size(161, 23);
            this.LabelShankDiameterValueName.TabIndex = 21;
            this.LabelShankDiameterValueName.Text = "Диаметр хвостовика";
            // 
            // TextShankLengthValue
            // 
            this.TextShankLengthValue.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextShankLengthValue.Location = new System.Drawing.Point(193, 318);
            this.TextShankLengthValue.Name = "TextShankLengthValue";
            this.TextShankLengthValue.Size = new System.Drawing.Size(76, 29);
            this.TextShankLengthValue.TabIndex = 22;
            this.TextShankLengthValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextShankDiameterValue
            // 
            this.TextShankDiameterValue.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TextShankDiameterValue.Location = new System.Drawing.Point(193, 353);
            this.TextShankDiameterValue.Name = "TextShankDiameterValue";
            this.TextShankDiameterValue.Size = new System.Drawing.Size(76, 29);
            this.TextShankDiameterValue.TabIndex = 23;
            this.TextShankDiameterValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelShankLengthValueName
            // 
            this.LabelShankLengthValueName.AutoSize = true;
            this.LabelShankLengthValueName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelShankLengthValueName.Location = new System.Drawing.Point(19, 321);
            this.LabelShankLengthValueName.Name = "LabelShankLengthValueName";
            this.LabelShankLengthValueName.Size = new System.Drawing.Size(143, 23);
            this.LabelShankLengthValueName.TabIndex = 24;
            this.LabelShankLengthValueName.Text = "Длина хвостовика";
            // 
            // LabelNessesaryName
            // 
            this.LabelNessesaryName.AutoSize = true;
            this.LabelNessesaryName.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelNessesaryName.Location = new System.Drawing.Point(12, 64);
            this.LabelNessesaryName.Name = "LabelNessesaryName";
            this.LabelNessesaryName.Size = new System.Drawing.Size(257, 29);
            this.LabelNessesaryName.TabIndex = 25;
            this.LabelNessesaryName.Text = "Обязательные параметры";
            // 
            // LabelOptionalName
            // 
            this.LabelOptionalName.AutoSize = true;
            this.LabelOptionalName.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelOptionalName.Location = new System.Drawing.Point(12, 257);
            this.LabelOptionalName.Name = "LabelOptionalName";
            this.LabelOptionalName.Size = new System.Drawing.Size(278, 29);
            this.LabelOptionalName.TabIndex = 26;
            this.LabelOptionalName.Text = "Необязательные параметры";
            // 
            // LabelConeValueName
            // 
            this.LabelConeValueName.AutoSize = true;
            this.LabelConeValueName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelConeValueName.Location = new System.Drawing.Point(19, 321);
            this.LabelConeValueName.Name = "LabelConeValueName";
            this.LabelConeValueName.Size = new System.Drawing.Size(130, 23);
            this.LabelConeValueName.TabIndex = 27;
            this.LabelConeValueName.Text = "Обратный конус";
            // 
            // LabelShankDiameterValueRange
            // 
            this.LabelShankDiameterValueRange.AutoSize = true;
            this.LabelShankDiameterValueRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelShankDiameterValueRange.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabelShankDiameterValueRange.Location = new System.Drawing.Point(297, 356);
            this.LabelShankDiameterValueRange.Name = "LabelShankDiameterValueRange";
            this.LabelShankDiameterValueRange.Size = new System.Drawing.Size(109, 23);
            this.LabelShankDiameterValueRange.TabIndex = 28;
            this.LabelShankDiameterValueRange.Text = "0,05 — 10 мм";
            // 
            // LabelShankLengthValueRange
            // 
            this.LabelShankLengthValueRange.AutoSize = true;
            this.LabelShankLengthValueRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelShankLengthValueRange.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabelShankLengthValueRange.Location = new System.Drawing.Point(297, 318);
            this.LabelShankLengthValueRange.Name = "LabelShankLengthValueRange";
            this.LabelShankLengthValueRange.Size = new System.Drawing.Size(109, 23);
            this.LabelShankLengthValueRange.TabIndex = 29;
            this.LabelShankLengthValueRange.Text = "0,05 — 10 мм";
            // 
            // ComboBoxPresets
            // 
            this.ComboBoxPresets.FormattingEnabled = true;
            this.ComboBoxPresets.Location = new System.Drawing.Point(293, 23);
            this.ComboBoxPresets.Name = "ComboBoxPresets";
            this.ComboBoxPresets.Size = new System.Drawing.Size(134, 21);
            this.ComboBoxPresets.TabIndex = 30;
            this.ComboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPresetsSelectedIndexChanged);
            // 
            // LablePreset
            // 
            this.LablePreset.AutoSize = true;
            this.LablePreset.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LablePreset.Location = new System.Drawing.Point(12, 15);
            this.LablePreset.Name = "LablePreset";
            this.LablePreset.Size = new System.Drawing.Size(266, 29);
            this.LablePreset.TabIndex = 31;
            this.LablePreset.Text = "Предустановка параметров";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 435);
            this.Controls.Add(this.LablePreset);
            this.Controls.Add(this.ComboBoxPresets);
            this.Controls.Add(this.LabelShankLengthValueRange);
            this.Controls.Add(this.LabelShankDiameterValueRange);
            this.Controls.Add(this.LabelConeValueName);
            this.Controls.Add(this.LabelOptionalName);
            this.Controls.Add(this.LabelNessesaryName);
            this.Controls.Add(this.LabelShankLengthValueName);
            this.Controls.Add(this.TextShankDiameterValue);
            this.Controls.Add(this.TextShankLengthValue);
            this.Controls.Add(this.LabelShankDiameterValueName);
            this.Controls.Add(this.CheckClearanceShank);
            this.Controls.Add(this.LabelShankClearanceName);
            this.Controls.Add(this.CheckClearanceCone);
            this.Controls.Add(this.LabelConeValueRange);
            this.Controls.Add(this.ButtonBuild);
            this.Controls.Add(this.LabelAngleRange);
            this.Controls.Add(this.LabelTotalLengthRange);
            this.Controls.Add(this.LabelLengthRange);
            this.Controls.Add(this.LabelDiameterRange);
            this.Controls.Add(this.TextConeValue);
            this.Controls.Add(this.TextAngle);
            this.Controls.Add(this.TextTotalLength);
            this.Controls.Add(this.TextLength);
            this.Controls.Add(this.TextDiameter);
            this.Controls.Add(this.LabelConeClearanceName);
            this.Controls.Add(this.LabelAngleName);
            this.Controls.Add(this.LabelTotalLengthName);
            this.Controls.Add(this.LabelLengthName);
            this.Controls.Add(this.LabelDiameterName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Плагин для создания модели сверла";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelDiameterName;
        private System.Windows.Forms.Label LabelLengthName;
        private System.Windows.Forms.Label LabelTotalLengthName;
        private System.Windows.Forms.Label LabelAngleName;
        private System.Windows.Forms.Label LabelConeClearanceName;
        private System.Windows.Forms.TextBox TextDiameter;
        private System.Windows.Forms.TextBox TextLength;
        private System.Windows.Forms.TextBox TextTotalLength;
        private System.Windows.Forms.TextBox TextAngle;
        private System.Windows.Forms.Label LabelDiameterRange;
        private System.Windows.Forms.Label LabelLengthRange;
        private System.Windows.Forms.Label LabelTotalLengthRange;
        private System.Windows.Forms.Label LabelAngleRange;
        private System.Windows.Forms.Button ButtonBuild;
        private System.Windows.Forms.CheckBox CheckClearanceCone;
        private System.Windows.Forms.Label LabelConeValueRange;
        private System.Windows.Forms.TextBox TextConeValue;
        private System.Windows.Forms.Label LabelShankClearanceName;
        private System.Windows.Forms.CheckBox CheckClearanceShank;
        private System.Windows.Forms.Label LabelShankDiameterValueName;
        private System.Windows.Forms.TextBox TextShankLengthValue;
        private System.Windows.Forms.TextBox TextShankDiameterValue;
        private System.Windows.Forms.Label LabelShankLengthValueName;
        private System.Windows.Forms.Label LabelNessesaryName;
        private System.Windows.Forms.Label LabelOptionalName;
        private System.Windows.Forms.Label LabelConeValueName;
        private System.Windows.Forms.Label LabelShankDiameterValueRange;
        private System.Windows.Forms.Label LabelShankLengthValueRange;
        private System.Windows.Forms.ComboBox ComboBoxPresets;
        private System.Windows.Forms.Label LablePreset;
    }
}

