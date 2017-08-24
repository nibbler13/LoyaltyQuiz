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
	public partial class FormThanks : FormTemplate {
		private Timer timer;

		public FormThanks() {
			InitializeComponent();

			SetButtonCloseVisible(false);

			SetLabelsText(
				Properties.Settings.Default.TextThanksFormHeader,
				Properties.Settings.Default.TextThanksFormSubtitle);
			SetLogoVisible(true);

			string temp = 
				"Анимация или статичная картинка" + Environment.NewLine + 
				"с благодарностью за участие" + Environment.NewLine + 
				"в опросе";

			Label thanks = CreateLabel(temp, startX, startY, availableWidth, availableHeight);

			//KeyValuePair<Button, PictureBox> buttonOk = CreateDefaultButton(buttonClose.Key.Location.X,
			//	buttonClose.Key.Location.Y,
			//	Properties.Resources.ButtonOk);
			//buttonOk.Key.BackColor = Properties.Settings.Default.ColorButtonOk;
			//buttonOk.Key.Click += ButtonOk_Click;

			timer = new Timer();
			timer.Interval = 10 * 1000;
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e) {
			timer.Stop();
			timer.Dispose();
			CloseAllFormsExceptMain();
		}

		//private void ButtonOk_Click(object sender, EventArgs e) {
		//	CloseAllFormsExceptMain();
		//}
	}
}
