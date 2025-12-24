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
            this.lblDiameterName = new System.Windows.Forms.Label();
            this.lblLengthName = new System.Windows.Forms.Label();
            this.lblTotalLengthName = new System.Windows.Forms.Label();
            this.lblAngleName = new System.Windows.Forms.Label();
            this.lblConeValueName = new System.Windows.Forms.Label();
            this.txtDiameter = new System.Windows.Forms.TextBox();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.txtTotalLength = new System.Windows.Forms.TextBox();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.lblDiameterRange = new System.Windows.Forms.Label();
            this.lblLengthRange = new System.Windows.Forms.Label();
            this.lblTotalLengthRange = new System.Windows.Forms.Label();
            this.lblAngleRange = new System.Windows.Forms.Label();
            this.btnBuild = new System.Windows.Forms.Button();
            this.chkClearanceCone = new System.Windows.Forms.CheckBox();
            this.lblConeValueRange = new System.Windows.Forms.Label();
            this.txtConeValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblDiameterName
            // 
            this.lblDiameterName.AutoSize = true;
            this.lblDiameterName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDiameterName.Location = new System.Drawing.Point(13, 16);
            this.lblDiameterName.Name = "lblDiameterName";
            this.lblDiameterName.Size = new System.Drawing.Size(88, 23);
            this.lblDiameterName.TabIndex = 0;
            this.lblDiameterName.Text = "Диаметр d";
            // 
            // lblLengthName
            // 
            this.lblLengthName.AutoSize = true;
            this.lblLengthName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLengthName.Location = new System.Drawing.Point(13, 51);
            this.lblLengthName.Name = "lblLengthName";
            this.lblLengthName.Size = new System.Drawing.Size(123, 23);
            this.lblLengthName.TabIndex = 1;
            this.lblLengthName.Text = "Рабочая часть l";
            // 
            // lblTotalLengthName
            // 
            this.lblTotalLengthName.AutoSize = true;
            this.lblTotalLengthName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotalLengthName.Location = new System.Drawing.Point(13, 83);
            this.lblTotalLengthName.Name = "lblTotalLengthName";
            this.lblTotalLengthName.Size = new System.Drawing.Size(123, 23);
            this.lblTotalLengthName.TabIndex = 2;
            this.lblTotalLengthName.Text = "Общая длина L";
            // 
            // lblAngleName
            // 
            this.lblAngleName.AutoSize = true;
            this.lblAngleName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAngleName.Location = new System.Drawing.Point(13, 121);
            this.lblAngleName.Name = "lblAngleName";
            this.lblAngleName.Size = new System.Drawing.Size(157, 23);
            this.lblAngleName.TabIndex = 3;
            this.lblAngleName.Text = "Угол при вершине a";
            // 
            // lblConeValueName
            // 
            this.lblConeValueName.AutoSize = true;
            this.lblConeValueName.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConeValueName.Location = new System.Drawing.Point(38, 159);
            this.lblConeValueName.Name = "lblConeValueName";
            this.lblConeValueName.Size = new System.Drawing.Size(130, 23);
            this.lblConeValueName.TabIndex = 4;
            this.lblConeValueName.Text = "Обратный конус";
            // 
            // txtDiameter
            // 
            this.txtDiameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.txtDiameter.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtDiameter.Location = new System.Drawing.Point(176, 13);
            this.txtDiameter.Name = "txtDiameter";
            this.txtDiameter.Size = new System.Drawing.Size(76, 29);
            this.txtDiameter.TabIndex = 5;
            this.txtDiameter.Text = "21";
            this.txtDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDiameter.TextChanged += new System.EventHandler(this.txtDiameter_TextChanged);
            // 
            // txtLength
            // 
            this.txtLength.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtLength.Location = new System.Drawing.Point(175, 48);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(76, 29);
            this.txtLength.TabIndex = 6;
            this.txtLength.Text = "64";
            this.txtLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtLength.TextChanged += new System.EventHandler(this.txtLength_TextChanged);
            // 
            // txtTotalLength
            // 
            this.txtTotalLength.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtTotalLength.Location = new System.Drawing.Point(176, 83);
            this.txtTotalLength.Name = "txtTotalLength";
            this.txtTotalLength.Size = new System.Drawing.Size(76, 29);
            this.txtTotalLength.TabIndex = 7;
            this.txtTotalLength.Text = "88";
            this.txtTotalLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalLength.TextChanged += new System.EventHandler(this.txtTotalLength_TextChanged);
            // 
            // txtAngle
            // 
            this.txtAngle.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtAngle.Location = new System.Drawing.Point(176, 118);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(76, 29);
            this.txtAngle.TabIndex = 8;
            this.txtAngle.Text = "97";
            this.txtAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAngle.TextChanged += new System.EventHandler(this.txtAngle_TextChanged);
            // 
            // lblDiameterRange
            // 
            this.lblDiameterRange.AutoSize = true;
            this.lblDiameterRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDiameterRange.Location = new System.Drawing.Point(257, 16);
            this.lblDiameterRange.Name = "lblDiameterRange";
            this.lblDiameterRange.Size = new System.Drawing.Size(87, 23);
            this.lblDiameterRange.TabIndex = 10;
            this.lblDiameterRange.Text = "1 — 20 мм";
            // 
            // lblLengthRange
            // 
            this.lblLengthRange.AutoSize = true;
            this.lblLengthRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblLengthRange.Location = new System.Drawing.Point(257, 51);
            this.lblLengthRange.Name = "lblLengthRange";
            this.lblLengthRange.Size = new System.Drawing.Size(130, 23);
            this.lblLengthRange.TabIndex = 11;
            this.lblLengthRange.Text = "3 × d — 8 × d мм";
            // 
            // lblTotalLengthRange
            // 
            this.lblTotalLengthRange.AutoSize = true;
            this.lblTotalLengthRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotalLengthRange.Location = new System.Drawing.Point(257, 86);
            this.lblTotalLengthRange.Name = "lblTotalLengthRange";
            this.lblTotalLengthRange.Size = new System.Drawing.Size(125, 23);
            this.lblTotalLengthRange.TabIndex = 12;
            this.lblTotalLengthRange.Text = "l + 20 — 205 мм";
            // 
            // lblAngleRange
            // 
            this.lblAngleRange.AutoSize = true;
            this.lblAngleRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAngleRange.Location = new System.Drawing.Point(258, 121);
            this.lblAngleRange.Name = "lblAngleRange";
            this.lblAngleRange.Size = new System.Drawing.Size(87, 23);
            this.lblAngleRange.TabIndex = 13;
            this.lblAngleRange.Text = "90 — 140°";
            // 
            // btnBuild
            // 
            this.btnBuild.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnBuild.Location = new System.Drawing.Point(174, 200);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(77, 31);
            this.btnBuild.TabIndex = 15;
            this.btnBuild.Text = "Построить";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // chkClearanceCone
            // 
            this.chkClearanceCone.AutoSize = true;
            this.chkClearanceCone.Checked = true;
            this.chkClearanceCone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearanceCone.Location = new System.Drawing.Point(17, 165);
            this.chkClearanceCone.Name = "chkClearanceCone";
            this.chkClearanceCone.Size = new System.Drawing.Size(15, 14);
            this.chkClearanceCone.TabIndex = 17;
            this.chkClearanceCone.UseVisualStyleBackColor = true;
            this.chkClearanceCone.CheckedChanged += new System.EventHandler(this.chkClearanceCone_CheckedChanged);
            // 
            // lblConeValueRange
            // 
            this.lblConeValueRange.AutoSize = true;
            this.lblConeValueRange.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblConeValueRange.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConeValueRange.Location = new System.Drawing.Point(257, 156);
            this.lblConeValueRange.Name = "lblConeValueRange";
            this.lblConeValueRange.Size = new System.Drawing.Size(109, 23);
            this.lblConeValueRange.TabIndex = 14;
            this.lblConeValueRange.Text = "0,05 — 10 мм";
            // 
            // txtConeValue
            // 
            this.txtConeValue.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtConeValue.Location = new System.Drawing.Point(175, 156);
            this.txtConeValue.Name = "txtConeValue";
            this.txtConeValue.Size = new System.Drawing.Size(76, 29);
            this.txtConeValue.TabIndex = 9;
            this.txtConeValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtConeValue.Visible = false;
            this.txtConeValue.TextChanged += new System.EventHandler(this.txtConeValue_TextChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 243);
            this.Controls.Add(this.chkClearanceCone);
            this.Controls.Add(this.lblConeValueRange);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.lblAngleRange);
            this.Controls.Add(this.lblTotalLengthRange);
            this.Controls.Add(this.lblLengthRange);
            this.Controls.Add(this.lblDiameterRange);
            this.Controls.Add(this.txtConeValue);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtTotalLength);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.txtDiameter);
            this.Controls.Add(this.lblConeValueName);
            this.Controls.Add(this.lblAngleName);
            this.Controls.Add(this.lblTotalLengthName);
            this.Controls.Add(this.lblLengthName);
            this.Controls.Add(this.lblDiameterName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Плагин для создания модели сверла";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDiameterName;
        private System.Windows.Forms.Label lblLengthName;
        private System.Windows.Forms.Label lblTotalLengthName;
        private System.Windows.Forms.Label lblAngleName;
        private System.Windows.Forms.Label lblConeValueName;
        private System.Windows.Forms.TextBox txtDiameter;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.TextBox txtTotalLength;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.Label lblDiameterRange;
        private System.Windows.Forms.Label lblLengthRange;
        private System.Windows.Forms.Label lblTotalLengthRange;
        private System.Windows.Forms.Label lblAngleRange;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.CheckBox chkClearanceCone;
        private System.Windows.Forms.Label lblConeValueRange;
        private System.Windows.Forms.TextBox txtConeValue;
    }
}

