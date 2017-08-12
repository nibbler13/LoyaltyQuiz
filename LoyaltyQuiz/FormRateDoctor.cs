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

			labelHeader.Text = Properties.Settings.Default.TextRateDoctorHeader;
			labelSubtitle.Text = Properties.Settings.Default.TextRateDoctorSubtitle;

			closeButton.Visible = true;
			pictureBoxLogo.Visible = false;

			Dictionary<string, Image> smiles = new Dictionary<string, Image>() {
				{"Отлично", Properties.Resources.smile_love },
				{"Хорошо", Properties.Resources.smile_happy },
				{"Затрудняюсь ответить", Properties.Resources.smile_neutral },
				{"Не очень", Properties.Resources.smile_sad },
				{"Плохо", Properties.Resources.smile_angry }
			};

			int elements = smiles.Count;
			int elementWidth = (availableWidth  - gap * (elements - 1)) / elements;
			int elementHeight = (int)(availableHeight * 0.3);


			int currentX = startX;
			int currentY = startY + availableHeight - elementHeight;

			foreach (KeyValuePair<string, Image> smile in smiles) {
				Button button = CreateButton(smile.Key, currentX, currentY, elementWidth, elementHeight, smile.Value);
				button.TextImageRelation = TextImageRelation.Overlay;
				button.ImageAlign = ContentAlignment.MiddleCenter;
				button.TextAlign = ContentAlignment.BottomCenter;
				currentX += elementWidth + gap;
				Controls.Add(button);
			}

			Label labelDocposition = CreateLabel("Должность: " + doctor.Position, startX, startY + availableHeight - elementHeight - gap * 3, availableWidth, gap * 2);
			labelDocposition.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(fontSize * 0.7));
			Label labelDocname = CreateLabel("ФИО: " + doctor.Name, startX, labelDocposition.Location.Y - gap * 2, availableWidth, gap * 2);
			labelDocname.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(fontSize * 0.7));

			PictureBox pb = new PictureBox();
			pb.Image = GetImageForDoctor(doctor.Name);
			pb.SizeMode = PictureBoxSizeMode.Zoom;
			int imageSize = labelDocname.Location.Y - gap - startY;
			pb.SetBounds(startX + availableWidth / 2 - imageSize / 2, startY, imageSize, imageSize);
			Controls.Add(pb);

			int labelWidth = pb.Location.X - gap - startX;
			int labelHeight = pb.Height;
			Label labelInfo1 = CreateLabel("Справочная информация", pb.Location.X + pb.Width + gap, startY, labelWidth, labelHeight);
			Label labelInfo2 = CreateLabel("Информация о враче", startX, startY, labelWidth, labelHeight);
		}
	}
}
