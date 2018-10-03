namespace LoyaltyQuiz {
	partial class FormCallback {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.maskedTextBox = new System.Windows.Forms.MaskedTextBox();
			this.SuspendLayout();
			// 
			// maskedTextBox
			// 
			this.maskedTextBox.Location = new System.Drawing.Point(746, 354);
			this.maskedTextBox.Mask = "+9 (999) 000-0000";
			this.maskedTextBox.Name = "maskedTextBox";
			this.maskedTextBox.Size = new System.Drawing.Size(100, 20);
			this.maskedTextBox.TabIndex = 7;
			this.maskedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.maskedTextBox.Visible = false;
			// 
			// FormCallback
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1612, 912);
			this.Controls.Add(this.maskedTextBox);
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "FormCallback";
			this.Text = "FormCallback";
			this.Controls.SetChildIndex(this.maskedTextBox, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MaskedTextBox maskedTextBox;
	}
}