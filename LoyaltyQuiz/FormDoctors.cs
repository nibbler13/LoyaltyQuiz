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

		public FormDoctors(List<Doctor> doctors, string depName) {
			LoggingSystem.LogMessageToFile("Инициализация формы с врачами");
			InitializeComponent();

			SetLabelsText(
				Properties.Settings.Default.TextDoctorsFormHeader + Environment.NewLine + "Отделение: " + depName,
				Properties.Settings.Default.TextDoctorsFormSubtitle);

			SetLogoVisible(false);
			
			this.doctors = doctors;

			CreateRootPanel(
				Properties.Settings.Default.FormDoctorsElementsInLine,
				Properties.Settings.Default.FormDoctorsElementsLineCount,
				doctors.Count);

			List<string> keys = new List<string>();
			foreach (Doctor doctor in doctors)
				keys.Add(doctor.Name);

			FillPanelWithElements(keys, ElementType.Doctor, PanelDoctor_Click);
		}




		private void PanelDoctor_Click(object sender, EventArgs e) {
			string docname = (sender as Control).Tag.ToString();
			Doctor selectedDoctor = new Doctor("", "", "", "");

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
