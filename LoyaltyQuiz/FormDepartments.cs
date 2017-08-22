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
	public partial class FormDepartments : FormTemplate {
		private Dictionary<string, List<Doctor>> dictionaryOfDoctors;

		public FormDepartments(Dictionary<string, List<Doctor>> dictionaryOfDoctors) {
			LoggingSystem.LogMessageToFile("Инициализация формы с департаментами");
			InitializeComponent();

			labelHeader.Text = Properties.Settings.Default.TextDepartmentFormHeader;
			labelSubtitle.Text = Properties.Settings.Default.TextDepartmentFormSubtitle;

			pictureBoxLogo.Visible = false;
			this.dictionaryOfDoctors = dictionaryOfDoctors;

			CreateMainPanel(
				Properties.Settings.Default.FormDepartmentsElementsInLine,
				Properties.Settings.Default.FormDepartmentsElementsLineCount,
				dictionaryOfDoctors.Count);

			List<string> keys = dictionaryOfDoctors.Keys.ToList();
			FillPanelWithElements(keys, ElementType.Department, PanelDepartment_Click);

			SetButtonCloseVisible(true);
		}

		

		private void PanelDepartment_Click(object sender, EventArgs e) {
			string depname = (sender as Control).Tag.ToString();
			Console.WriteLine("ButtonDepartment_Click : " + depname);

			FormDoctors formDoctors = new FormDoctors(dictionaryOfDoctors[depname], depname);
			formDoctors.ShowDialog();
		}
	}
}
