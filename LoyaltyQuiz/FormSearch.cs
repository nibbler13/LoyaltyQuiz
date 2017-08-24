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
	public partial class FormSearch : FormTemplate {
		private Dictionary<string, List<Doctor>> dictionaryOfDoctors;
		private Panel panelResult;
		private KeyValuePair<Button, PictureBox> buttonScrollLeft;
		private KeyValuePair<Button, PictureBox> buttonScrollRight;
		private Label labelInfo;
		private int panelResultWidth;
		private int panelResultHeight;
		private int minTextBoxSearchLength = 3;

		public FormSearch(Dictionary<string, List<Doctor>> dictionaryOfDoctors) {
			Console.WriteLine("FormSearch");
			InitializeComponent();

			this.dictionaryOfDoctors = dictionaryOfDoctors;

			SetLabelsText(
				Properties.Settings.Default.TextSearchFormHeader,
				Properties.Settings.Default.TextSearchFormSubtitleEmpty);

			SetLogoVisible(false);

			OnscreenKeyboard onscreenKeyboard = new OnscreenKeyboard(
				textBox, availableWidth, availableHeight, 
				startX, startY, gap, fontSize, OnscreenKeyboard.KeyboardType.Short);
			Panel panelKeyboard = onscreenKeyboard.CreateOnscreenKeyboard();
			Controls.Add(panelKeyboard);

			textBox.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, fontSize);
			textBox.SetBounds(panelKeyboard.Location.X, startY, panelKeyboard.Width, buttonClose.Key.Height);
			textBox.Focus();
			textBox.TextChanged += TextBox_TextChanged;

			KeyValuePair<Button, PictureBox> buttonClear = CreateDefaultButton(
				textBox.Location.X + textBox.Width + gap,
				textBox.Location.Y,
				Properties.Resources.ButtonClear);
			buttonClear.Key.Click += ButtonClear_Click;

			int panelResultX = startX;
			int panelResultY = textBox.Location.Y + textBox.Height + gap;
			panelResultWidth = availableWidth;
			panelResultHeight = panelKeyboard.Location.Y - gap - panelResultY;

			panelResult = new Panel();
			panelResult.SetBounds(
				panelResultX - leftCornerShadow, 
				panelResultY - leftCornerShadow, 
				panelResultWidth + leftCornerShadow + rightCornerShadow,
				panelResultHeight + leftCornerShadow + rightCornerShadow);
			Controls.Add(panelResult);

			if (Properties.Settings.Default.IsDebugMode) {
				textBox.BackColor = Color.AliceBlue;
				panelResult.BackColor = Color.AliceBlue;
			}

			buttonScrollLeft = CreateDefaultButton(
				startX,
				panelResult.Location.Y + panelResult.Height + gap - rightCornerShadow,
				Properties.Resources.ButtonLeft);
			buttonScrollLeft.Key.Click += ButtonLeft_Click;

			buttonScrollRight = CreateDefaultButton(
				startX + availableWidth - buttonClose.Key.Width,
				buttonScrollLeft.Key.Location.Y,
				Properties.Resources.ButtonRight);
			buttonScrollRight.Key.Click += ButtonRight_Click;

			labelInfo = CreateLabel("", panelResult.Location.X, panelResult.Location.Y, panelResult.Width, panelResult.Height);
			SetLabelInfoToInitial();

			onscreenKeyboard.SetEnterButtonClick(ButtonEnter_Click);
		}

		private void ButtonEnter_Click(object sender, EventArgs e) {
			StartSearch();
		}

		private void ButtonRight_Click(object sender, EventArgs e) {
			Console.WriteLine("ButtonRight_Click");
		}

		private void ButtonLeft_Click(object sender, EventArgs e) {
			Console.WriteLine("ButtonLeft_Click");
		}

		private void ButtonClear_Click(object sender, EventArgs e) {
			textBox.Text = "";
			SetLabelInfoToInitial();
		}

		private void SetPanelResultVisible(bool isVisible) {
			panelResult.Visible = isVisible;
			SetButtonVisible(buttonScrollLeft, false);
			SetButtonVisible(buttonScrollRight, false);
		}

		private void SetLabelInfoToInitial() {
			Console.WriteLine("SetLabelInfoToInitial");
			SetLabelInfoText(Properties.Settings.Default.TextSearchFormInitial);
		}

		private void SetLabelInfoToNothingFound() {
			Console.WriteLine("SetLabelInfoToNothingFoun");
			SetLabelInfoText(Properties.Settings.Default.TextSearchFormNothingFound);
		}

		private void SetLabelInfoText(string str) {
			SetPanelResultVisible(false);
			SetLabelSubtitleText(Properties.Settings.Default.TextSearchFormSubtitleEmpty);
			labelInfo.Text = str;
		}

		private void SetButtonVisible(KeyValuePair<Button, PictureBox> button, bool isVisible) {
			Console.WriteLine("SetButtonVisible");
			button.Key.Visible = isVisible;
			button.Value.Visible = isVisible;
		}

		private void TextBox_TextChanged(object sender, EventArgs e) {
			if (textBox.Text.Length < minTextBoxSearchLength)
				return;

			StartSearch();
		}

		private string NormalizeString(string str) {
			return str.ToLower().Replace("ё", "е");
		}

		private void StartSearch() {
			string text = textBox.Text;

			if (string.IsNullOrWhiteSpace(text) ||
				string.IsNullOrEmpty(text)) {
				SetLabelInfoToInitial();
				return;
			}

			List<Doctor> doctors = new List<Doctor>();
			doctors.Sort();

			foreach (KeyValuePair<string, List<Doctor>> dictionaryDepartment in dictionaryOfDoctors)
				foreach (Doctor doctor in dictionaryDepartment.Value) {
					if (!NormalizeString(doctor.Name).StartsWith(NormalizeString(textBox.Text)))
						continue;

					doctors.Add(doctor);
				}

			if (doctors.Count == 0) {
				SetLabelInfoToNothingFound();
				return;
			}

			UpdateResultPanelContent(doctors);
		}

		private void UpdateResultPanelContent(List<Doctor> doctors) {
			panelResult.Controls.Clear();
			SetLabelSubtitleText(Properties.Settings.Default.TextSearchFormSubtitleFound);
			SetPanelResultVisible(true);

			int currentX = leftCornerShadow;
			int currentY = leftCornerShadow;

			int elementHeight = panelResultHeight;
			int elementWidth = (panelResultWidth - gap * 2) / 3;

			if (doctors.Count == 1)
				currentX = panelResult.Width / 2 - elementWidth / 2;
			else if (doctors.Count == 2)
				currentX = panelResult.Width / 2 - (elementWidth * 2 + gap) / 2;

			foreach (Doctor doctor in doctors) {
				string info = doctor.Name + Environment.NewLine + Environment.NewLine +
					FirstCharToUpper(doctor.Department) + Environment.NewLine + 
					doctor.Position;
				Panel panelDoctor = CreateInnerPanel(info, currentX, currentY, elementWidth, elementHeight, ElementType.Search);
				panelResult.Controls.Add(panelDoctor);
				currentX += elementWidth + gap;

				PictureBox dropShadow = CreateDropShadow(panelDoctor, Properties.Resources.DropShadowSearch);
				panelResult.Controls.Add(dropShadow);

				panelDoctor.Tag = doctor;
				panelDoctor.Click += PanelDoctor_Click;

				foreach (Control control in panelDoctor.Controls) {
					control.Tag = doctor;
					control.Click += PanelDoctor_Click;
				}
			}

			if (doctors.Count > 3)
				SetButtonVisible(buttonScrollRight, true);
		}

		private void PanelDoctor_Click(object sender, EventArgs e) {
			Doctor doctor = (sender as Control).Tag as Doctor;
			FormRateDoctor formRateDoctor = new FormRateDoctor(doctor);
			formRateDoctor.ShowDialog();
		}
	}
}
