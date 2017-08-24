namespace LoyaltyQuiz {
	partial class FormTemplate {
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent() {
			this.labelHeader = new System.Windows.Forms.Label();
			this.labelSubtitle = new System.Windows.Forms.Label();
			this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
			this.pictureBoxBottomLineColors = new System.Windows.Forms.PictureBox();
			this.pictureBoxBottomLineMain = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottomLineColors)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottomLineMain)).BeginInit();
			this.SuspendLayout();
			// 
			// labelHeader
			// 
			this.labelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelHeader.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.labelHeader.Location = new System.Drawing.Point(0, 0);
			this.labelHeader.Name = "labelHeader";
			this.labelHeader.Size = new System.Drawing.Size(800, 68);
			this.labelHeader.TabIndex = 0;
			this.labelHeader.Text = "Template";
			this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelSubtitle
			// 
			this.labelSubtitle.Location = new System.Drawing.Point(369, 561);
			this.labelSubtitle.Name = "labelSubtitle";
			this.labelSubtitle.Size = new System.Drawing.Size(100, 23);
			this.labelSubtitle.TabIndex = 4;
			this.labelSubtitle.Text = "label1";
			this.labelSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBoxLogo
			// 
			this.pictureBoxLogo.Image = global::LoyaltyQuiz.Properties.Resources.ButterflyClear;
			this.pictureBoxLogo.Location = new System.Drawing.Point(688, 500);
			this.pictureBoxLogo.Name = "pictureBoxLogo";
			this.pictureBoxLogo.Size = new System.Drawing.Size(100, 84);
			this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxLogo.TabIndex = 3;
			this.pictureBoxLogo.TabStop = false;
			// 
			// pictureBoxBottomLineColors
			// 
			this.pictureBoxBottomLineColors.Image = global::LoyaltyQuiz.Properties.Resources.BottomLineContinuesClear;
			this.pictureBoxBottomLineColors.Location = new System.Drawing.Point(613, 590);
			this.pictureBoxBottomLineColors.Name = "pictureBoxBottomLineColors";
			this.pictureBoxBottomLineColors.Size = new System.Drawing.Size(187, 10);
			this.pictureBoxBottomLineColors.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxBottomLineColors.TabIndex = 2;
			this.pictureBoxBottomLineColors.TabStop = false;
			// 
			// pictureBoxBottomLineMain
			// 
			this.pictureBoxBottomLineMain.Image = global::LoyaltyQuiz.Properties.Resources.BottomLineTemplate;
			this.pictureBoxBottomLineMain.Location = new System.Drawing.Point(0, 590);
			this.pictureBoxBottomLineMain.Name = "pictureBoxBottomLineMain";
			this.pictureBoxBottomLineMain.Size = new System.Drawing.Size(800, 10);
			this.pictureBoxBottomLineMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxBottomLineMain.TabIndex = 1;
			this.pictureBoxBottomLineMain.TabStop = false;
			// 
			// FormTemplate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(this.pictureBoxLogo);
			this.Controls.Add(this.pictureBoxBottomLineColors);
			this.Controls.Add(this.pictureBoxBottomLineMain);
			this.Controls.Add(this.labelHeader);
			this.Controls.Add(this.labelSubtitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormTemplate";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottomLineColors)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxBottomLineMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.PictureBox pictureBoxLogo;
		private System.Windows.Forms.Label labelSubtitle;
		private System.Windows.Forms.PictureBox pictureBoxBottomLineMain;
		private System.Windows.Forms.PictureBox pictureBoxBottomLineColors;
	}
}

