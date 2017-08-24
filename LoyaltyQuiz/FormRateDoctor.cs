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
	public partial class FormRateDoctor : FormTemplate {
		private Doctor doctor;

		public FormRateDoctor(Doctor doctor) {
			InitializeComponent();

			this.doctor = doctor;

			string docInfo = "";
			docInfo += doctor.Name + Environment.NewLine;// + Environment.NewLine;
			docInfo += doctor.Position;

			SetLabelsText(
				docInfo,
				Properties.Settings.Default.TextRateDoctorSubtitle);

			SetLogoVisible(false);

			List<string> rates = new List<string>() { "1", "2", "3", "4", "5" };

			elementsInLine = rates.Count;
			elementWidth = ((int)(availableWidth * 0.66)  - gap * (elementsInLine - 1)) / elementsInLine;
			elementHeight = (int)(availableHeight * 0.35);
			
			currentX = (int)(startX + (availableWidth * 0.33) / 2);
			currentY = startY + availableHeight - elementHeight;

			foreach (string rate in rates) {
				Panel panelRate = CreateInnerPanel(rate, currentX, currentY, elementWidth, elementHeight, ElementType.Rate);
				currentX += elementWidth + gap;
				Controls.Add(panelRate);

				PictureBox dropShadow = CreateDropShadow(panelRate, Properties.Resources.DropShadowRate);
				Controls.Add(dropShadow);
				dropShadow.SendToBack();

				BindEventHandlerToPanel(panelRate, PanelRate_Click);
			}

			PictureBox docPhoto = new PictureBox();
			docPhoto.Image = GetImageForDoctor(doctor.Name);
			docPhoto.SizeMode = PictureBoxSizeMode.Zoom;
			int imageSide = availableHeight - elementHeight - gap;
			docPhoto.SetBounds(startX + availableWidth / 2 - imageSide / 2, startY, imageSide, imageSide);
			Controls.Add(docPhoto);
		}

		private void PanelRate_Click(object sender, EventArgs e) {
			Console.WriteLine("PanelRate_Click");
			string tag = (sender as Control).Tag.ToString();
			Console.WriteLine("tag: " + tag);

			if (tag.Equals("1") || tag.Equals("2")) {
				FormComment formComment = new FormComment();
				formComment.ShowDialog();
			} else {
				FormThanks formThanks = new FormThanks();
				formThanks.ShowDialog();
			}
		}
	}
}
