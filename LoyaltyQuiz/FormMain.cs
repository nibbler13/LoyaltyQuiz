using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoyaltyQuiz {
	public partial class FormMain : FormTemplate {
		private FBClient fbClient = new FBClient(
			Properties.Settings.Default.MisInfoclinicaDbAddress, 
			Properties.Settings.Default.MisInfoclinicaDbName,
			Properties.Settings.Default.MisInfoclinicaDbUser,
			Properties.Settings.Default.MisInfoclinicaDbPassword);
		private Dictionary<string, List<Doctor>> dictionaryOfDoctors = new Dictionary<string, List<Doctor>>();
		private FormError formError = new FormError();

		public FormMain() {
			LoggingSystem.LogMessageToFile("Инициализация основной формы");
			InitializeComponent();

			SetLabelsText(
				Properties.Settings.Default.TextMainFormHeader,
				Properties.Settings.Default.TextMainFormSubtitle);

			string info = "Информация про систему опросов" + Environment.NewLine + Environment.NewLine +
				"Статичное или анимированное изображение" + Environment.NewLine +
				"Или видеоролик";
			Label labelInfo = CreateLabel(info, startX, startY, availableWidth, availableHeight);

			foreach (Control control in Controls)
				control.Click += FormMain_Click;
			Click += FormMain_Click;

			_hookID = SetHook(_proc);
			FormClosed += FormMain_FormClosed;
			backgroundWorker.RunWorkerAsync();

			SetButtonCloseVisible(false);
		}

		private void UpdateListOfDoctors() {
			LoggingSystem.LogMessageToFile("Обновление данных из базы ИК");
			DataTable dataTable = fbClient.GetDataTable(Properties.Settings.Default.SqlQueryDoctors);

			if (dataTable.Rows.Count == 0) {
				LoggingSystem.LogMessageToFile("Из базы ИК вернулась пустая таблица");
				return;
			}

			Dictionary<string, List<Doctor>> dictionary = new Dictionary<string, List<Doctor>>();

			foreach (DataRow dataRow in dataTable.Rows) {
				try {
					string department = dataRow["DEPARTMENT"].ToString().ToLower();
					string docname = dataRow["DOCNAME"].ToString();
					string docposition = dataRow["DOCPOSITION"].ToString();

					Doctor doctor = new Doctor(docname, docposition, department, "123");

					if (dictionary.ContainsKey(department)) {
						if (dictionary[department].Contains(doctor))
							continue;

						dictionary[department].Add(doctor);
					} else {
						dictionary.Add(department, new List<Doctor>() { doctor });
					}
				} catch (Exception e) {
					LoggingSystem.LogMessageToFile("Не удалось обработать строку с данными: " + dataRow.ToString() + ", " + e.Message);
				}
			}

			LoggingSystem.LogMessageToFile("Обработано строк:" + dataTable.Rows.Count);

			dictionaryOfDoctors = dictionary;
		}

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e) {
			LoggingSystem.LogMessageToFile("Основная форма закрыта");
			Console.WriteLine("FormTemplate_FormClosed");
			UnhookWindowsHookEx(_hookID);
		}

		private void FormMain_Click(object sender, EventArgs e) {
			if (dictionaryOfDoctors.Count == 0) {
				formError.ShowDialog();
				return;
			}

			FormQuizSelect formQuizSelect = new FormQuizSelect(dictionaryOfDoctors);
			formQuizSelect.ShowDialog();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
			UpdateListOfDoctors();
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (dictionaryOfDoctors.Count == 0) {
				formError.ShowDialog();
			} else {
				formError.Close();
			}
		}
	}
}
