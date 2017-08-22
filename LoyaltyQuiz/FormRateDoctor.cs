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
			
			pictureBoxLogo.Visible = false;

			Dictionary<string, Image> smiles = new Dictionary<string, Image>() {
				{"Плохо", Properties.Resources.smile_angry },
				{"Не очень", Properties.Resources.smile_sad },
				{"Затрудняюсь ответить", Properties.Resources.smile_neutral },
				{"Хорошо", Properties.Resources.smile_happy },
				{"Отлично", Properties.Resources.smile_love }
			};

			int elements = smiles.Count;
			int elementWidth = (availableWidth  - gap * (elements - 1)) / elements;
			int elementHeight = (int)(availableHeight * 0.3);


			int currentX = startX;
			int currentY = startY + availableHeight - elementHeight;

			foreach (KeyValuePair<string, Image> smile in smiles) {
				//Panel panelSmile = CreateInnerPanel()
				//Button button = Createdefaultbutton(smile.key, currentx, currenty, elementwidth, elementheight, smile.value);
				//button.textimagerelation = textimagerelation.overlay;
				//button.imagealign = contentalignment.middlecenter;
				//button.textalign = contentalignment.bottomcenter;
				//currentx += elementwidth + gap;
				//controls.add(button);
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

			SetButtonCloseVisible(true);
		}
	}
}
