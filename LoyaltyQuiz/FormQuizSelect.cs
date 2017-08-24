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
	public partial class FormQuizSelect : FormTemplate {
		private Dictionary<string, List<Doctor>> dictionaryOfDoctors;
		private Timer timer;

		public FormQuizSelect(Dictionary<string, List<Doctor>> dictionaryOfDoctors) {
			InitializeComponent();

			this.dictionaryOfDoctors = dictionaryOfDoctors;
			SetLogoVisible(false);
			SetButtonCloseVisible(false);
			SetLabelsText(
				Properties.Settings.Default.TextQuizSelectFormHeader,
				Properties.Settings.Default.TextQuizSelectFormSubtitle);

			int buttonHeight = (availableHeight - gap) / 2;

			Button buttonQuizClinicRecommendtation = CreateButton(
				"Порекомендуете ли Вы нашу клинику своим друзьям и знакомым?",
				startX,
				startY,
				availableWidth,
				buttonHeight,
				fontSize);
			Controls.Add(buttonQuizClinicRecommendtation);
			buttonQuizClinicRecommendtation.Click += ButtonQuizClinicRecommendtation_Click;

			Button buttonQuizDocQuality = CreateButton(
				"Оцените качество приёма у врача",
				startX,
				startY + buttonHeight + gap,
				availableWidth,
				buttonHeight,
				fontSize);
			Controls.Add(buttonQuizDocQuality);
			buttonQuizDocQuality.Click += ButtonQuizDocQuality_Click;

			timer = new Timer();
			timer.Interval = 10 * 1000;
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e) {
			timer.Stop();
			timer.Dispose();
			Close();
		}

		private void ButtonQuizDocQuality_Click(object sender, EventArgs e) {
			timer.Stop();
			FormDepartments formDepartments = new FormDepartments(dictionaryOfDoctors);
			formDepartments.ShowDialog();
			timer.Start();
		}

		private void ButtonQuizClinicRecommendtation_Click(object sender, EventArgs e) {
			Console.WriteLine("ButtonQuizClinicRecommendtation_Click");
		}
	}
}
