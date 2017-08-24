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
	public partial class FormCallback : FormTemplate {
		//private TextBox textBox;
		private Label labelQuestion;
		private KeyValuePair<Button, PictureBox> buttonNo;
		private KeyValuePair<Button, PictureBox> buttonYes;
		private KeyValuePair<Button, PictureBox> buttonNext;

		public FormCallback() {
			InitializeComponent();

			SetLogoVisible(true);

			SetLabelsText(
				Properties.Settings.Default.TextCallbackFormHeaderQuestion,
				Properties.Settings.Default.TextCallbackFormSubtitleQuestion);

			labelQuestion = CreateLabel(
				Properties.Settings.Default.TextCallbackFormQuestion,
				startX,
				startY,
				availableWidth,
				availableHeight);

			buttonNo = CreateDefaultButton(
				startX + availableWidth / 2 - gap - buttonClose.Key.Width,
				startY + availableHeight / 2 + gap * 3,
				Properties.Resources.ButtonClose);
			buttonNo.Key.Click += ButtonNo_Click;

			buttonYes = CreateDefaultButton(
				buttonNo.Key.Location.X + buttonNo.Key.Width + gap * 2,
				buttonNo.Key.Location.Y,
				Properties.Resources.ButtonOk);
			buttonYes.Key.BackColor = Properties.Settings.Default.ColorButtonOk;
			buttonYes.Key.Click += ButtonYes_Click;
		}

		private void ButtonYes_Click(object sender, EventArgs e) {
			List<Control> controlsToRemove = new List<Control>() {
				labelQuestion,
				buttonNo.Key,
				buttonNo.Value,
				buttonYes.Key,
				buttonYes.Value
			};

			foreach (Control control in controlsToRemove)
				Controls.Remove(control);

			maskedTextBox.Visible = true;
			SetLabelsText(
				Properties.Settings.Default.TextCallbackFormHeaderEnterNumber,
				Properties.Settings.Default.TextCallbackFormSubtitleEnterNumber);

			OnscreenKeyboard onscreenKeyboard = new OnscreenKeyboard(
				maskedTextBox,
				availableWidth,
				availableHeight,
				startX,
				startY,
				gap,
				fontSize,
				OnscreenKeyboard.KeyboardType.Number);
			Panel panelKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
			Controls.Add(panelKeyboard);
			panelKeyboard.Location = new Point(
				panelKeyboard.Location.X,
				startY + availableHeight / 2 - panelKeyboard.Height / 2);

			buttonNext = CreateDefaultButton(
				startX + availableWidth - buttonClose.Key.Width,
				buttonClose.Key.Location.Y,
				Properties.Resources.ButtonOk);
			buttonNext.Key.BackColor = Properties.Settings.Default.ColorButtonOk;
			buttonNext.Key.Click += ButtonNext_Click;
			SetButtonNextVisible(false);
			SetLogoVisible(false);

			maskedTextBox.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, fontSize);
			maskedTextBox.SetBounds(
				panelKeyboard.Location.X,
				panelKeyboard.Location.Y - gap - buttonClose.Key.Height,
				panelKeyboard.Width,
				buttonClose.Key.Height);

			maskedTextBox.TextChanged += MaskedTextBox_TextChanged;
		}

		private void MaskedTextBox_TextChanged(object sender, EventArgs e) {
			SetButtonNextVisible((sender as MaskedTextBox).MaskCompleted);
		}

		private void SetButtonNextVisible(bool isVisible) {
			buttonNext.Key.Visible = isVisible;
			buttonNext.Value.Visible = isVisible;
		}

		private void ButtonNext_Click(object sender, EventArgs e) {
			FormThanks formThanks = new FormThanks();
			formThanks.ShowDialog();
		}

		private void ButtonNo_Click(object sender, EventArgs e) {
			FormThanks formThanks = new FormThanks();
			formThanks.ShowDialog();
		}
	}
}
