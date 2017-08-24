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
	public partial class FormError : FormTemplate {
		public FormError() {
			LoggingSystem.LogMessageToFile("Инициализация формы отображения ошибки");
			InitializeComponent();

			//labelHeader.BackColor = Properties.Settings.Default.ColorErrorTitle;

			SetLabelsText(
				Properties.Settings.Default.TextErrorFormHeader,
				Properties.Settings.Default.TextErrorFormSubtitle);

			CreateLabel("Картинка с извинениями", startX, startY, availableWidth, availableHeight);

			SetButtonCloseVisible(false);

			SetHeaderColor(Properties.Settings.Default.ColorErrorTitle);
		}
	}
}
