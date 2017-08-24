using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoyaltyQuiz {
	public partial class FormComment : FormTemplate {
		public FormComment() {
			InitializeComponent();

			//SetButtonCloseVisible(false);

			SetLabelsText(
				Properties.Settings.Default.TextCommentFormHeader,
				Properties.Settings.Default.TextCommentFromSubtitle);

			SetLogoVisible(false);

			OnscreenKeyboard onscreenKeyboard = new OnscreenKeyboard(
				textBox, availableWidth, availableHeight, 
				startX, startY, gap, fontSize, OnscreenKeyboard.KeyboardType.Full);

			Panel panelKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
			Controls.Add(panelKeyboard);

			textBox.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, fontSize);
			textBox.SetBounds(startX, startY, availableWidth, availableHeight - gap - panelKeyboard.Height);

			KeyValuePair<Button, PictureBox> buttonNext = CreateDefaultButton(
				startX + availableWidth - buttonClose.Key.Width,
				buttonClose.Key.Location.Y,
				Properties.Resources.BackNext);
			buttonNext.Key.BackColor = Properties.Settings.Default.ColorButtonOk;
			buttonNext.Key.Click += ButtonNext_Click;

			KeyValuePair<Button, PictureBox> buttonClear = CreateDefaultButton(
				startX + availableWidth - buttonClose.Key.Width,
				textBox.Location.Y + textBox.Height + gap,
				Properties.Resources.ButtonClear);
			buttonClear.Key.Click += ButtonClear_Click;
		}

		private void ButtonClear_Click(object sender, EventArgs e) {
			textBox.Text = "";
		}

		private void ButtonNext_Click(object sender, EventArgs e) {
			FormCallback formCallback = new FormCallback();
			formCallback.ShowDialog();
		}
	}
}
