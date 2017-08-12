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
	public partial class FormDoctors : FormTemplate {
		private List<Doctor> doctors;
		private Panel panel;
		private int elementWidth;
		private int elementHeight;
		private int currentX;
		private int currentY;
		private int locationY = 0;
		private Button buttonScrollUp;
		private Button buttonScrollDown;
		private int elementsInLine;
		private int elementsLineCount;

		public FormDoctors(List<Doctor> doctors) {
			LoggingSystem.LogMessageToFile("Инициализация формы с врачами");
			InitializeComponent();

			labelHeader.Text = Properties.Settings.Default.TextDoctorsFormHeader;
			labelSubtitle.Text = Properties.Settings.Default.TextDoctorsFormSubtitle;

			pictureBoxLogo.Visible = false;
			this.doctors = doctors;

			closeButton.Visible = true;

			

			elementsInLine = Properties.Settings.Default.FormDoctorsElementsInLine;
			elementsLineCount = Properties.Settings.Default.FormDoctorsElementsLineCount;

			if (doctors.Count > elementsInLine * elementsLineCount)
				CreateUpDownButtons();

			elementWidth = (availableWidth - gap * (elementsInLine - 1)) / elementsInLine;
			elementHeight = (availableHeight - gap * (elementsLineCount - 1)) / elementsLineCount;


			panel = new Panel();
			panel.SetBounds(startX, startY, availableWidth, availableHeight);
			panel.HorizontalScroll.Visible = false;
			panel.VerticalScroll.Visible = false;
			panel.AutoScrollPosition = new Point(0, 0);
			int lines = (int)Math.Ceiling((double)doctors.Count / (double)elementsInLine) - elementsLineCount;
			panel.VerticalScroll.Maximum = lines * elementHeight + (gap * (lines));
			Controls.Add(panel);

			if (Properties.Settings.Default.IsDebugMode)
				panel.BackColor = Color.AliceBlue;

			closeButton.Visible = true;

			FillPanelWithElements();
		}


		private void CreateUpDownButtons() {
			buttonScrollUp = CreateButton("↑", startX + availableWidth - gap * 3, startY, gap * 3, gap * 3);
			Controls.Add(buttonScrollUp);
			buttonScrollUp.Click += ButtonScrollUp_Click;
			buttonScrollUp.Visible = false;

			buttonScrollDown = CreateButton("↓", startX + availableWidth - gap * 3, startY + availableHeight - gap * 3, gap * 3, gap * 3);
			Controls.Add(buttonScrollDown);
			buttonScrollDown.Click += ButtonScrollDown_Click;

			availableWidth -= gap * 4;
		}



		private void ButtonScrollDown_Click(object sender, EventArgs e) {
			Console.WriteLine("DownButton_Click");
			if (locationY + elementHeight + gap >= panel.VerticalScroll.Maximum) {
				locationY = panel.VerticalScroll.Maximum;
				panel.AutoScrollPosition = new Point(0, locationY);
				buttonScrollDown.Visible = false;
			} else {
				locationY += elementHeight + gap;
				panel.VerticalScroll.Value = locationY;
				buttonScrollUp.Visible = true;
			}



		}

		private void ButtonScrollUp_Click(object sender, EventArgs e) {
			Console.WriteLine("UpButton_Click");
			if (locationY - elementHeight - gap <= 0) {
				locationY = 0;
				panel.AutoScrollPosition = new Point(0, 0);
				buttonScrollUp.Visible = false;
			} else {
				locationY -= elementHeight + gap;
				panel.VerticalScroll.Value = locationY;
				buttonScrollDown.Visible = true;
			}
		}


		private void FillPanelWithElements() {
			int elementsCreated = 0;
			int linesCreated = 0;

			currentX = 0;
			currentY = 0;

			foreach (Doctor doctor in doctors) {
				Button button = CreateDoctorButton(doctor.Name, currentX, currentY, elementWidth, elementHeight);
				currentX += elementWidth + gap;
				elementsCreated++;

				if (elementsCreated >= elementsInLine) {
					elementsCreated = 0;
					linesCreated++;
					currentX = 0;
					currentY += elementHeight + gap;
				}

				panel.Controls.Add(button);
				button.Click += ButtonDoctor_Click;
			}
		}

		private void ButtonDoctor_Click(object sender, EventArgs e) {
			string docname = (sender as Button).Tag.ToString();
			Doctor selectedDoctor = new Doctor("", "");

			Console.WriteLine("docname");

			foreach (Doctor doctor in doctors) {
				if (doctor.Name.Equals(docname)) {
					selectedDoctor = doctor;
					break;
				}
			}

			FormRateDoctor formRateDoctor = new FormRateDoctor(selectedDoctor);
			formRateDoctor.ShowDialog();
		}
	}
}
