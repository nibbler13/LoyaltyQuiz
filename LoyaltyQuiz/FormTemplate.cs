using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LoyaltyQuiz {
	public partial class FormTemplate : Form {
		protected int width;
		protected int height;
		protected int gap;
		protected int headerHeight;
		protected int fontSize;
		protected int startX;
		protected int startY;
		protected int availableWidth;
		protected int availableHeight;
		protected Button closeButton;

		public FormTemplate() {
			Console.WriteLine("FormTemplate initialize");
			InitializeComponent();

			width = Screen.FromControl(this).Bounds.Width;
			height = Screen.FromControl(this).Bounds.Height;

			SetBounds(0, 0, width, height);

			gap = (int)(height * 0.03f);
			headerHeight = (int)(height * 0.15f);
			fontSize = (int)(height * 0.033f);

			labelHeader.Font = new Font(Properties.Settings.Default.FontMain.FontFamily, fontSize);
			labelHeader.SetBounds(0, 0, width, headerHeight);
			labelHeader.BackColor = Properties.Settings.Default.ColorHeader;

			int logoWidth = Properties.Resources.ButterflyClear.Width;
			int logoHeight = Properties.Resources.ButterflyClear.Height;
			float logoScale = (float)logoWidth / (float)logoHeight;
			logoHeight = (int)(height * 0.15f);
			logoWidth = (int)(logoHeight * logoScale);

			int colorLineWidth = Properties.Resources.BottomLineContinuesClear.Width;
			int colorLineHeight = Properties.Resources.BottomLineContinuesClear.Height;
			float colorLineScale = (float)colorLineWidth / (float)colorLineHeight;
			colorLineHeight = (int)(logoHeight * 0.058f);
			colorLineWidth = (int)(colorLineHeight * colorLineScale);

			pictureBoxLogo.SetBounds(width - gap - logoWidth, height - gap - colorLineHeight - logoHeight, logoWidth, logoHeight);
			pictureBoxBottomLineColors.SetBounds(width - colorLineWidth + 1, height - colorLineHeight + 1, colorLineWidth, colorLineHeight);
			pictureBoxBottomLineMain.SetBounds(0, height - colorLineHeight, width, colorLineHeight);

			labelSubtitle.SetBounds(0, height - colorLineHeight - (int)(fontSize * 2) - gap, width, (int)(fontSize * 2));
			labelSubtitle.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, fontSize);
			labelSubtitle.ForeColor = Properties.Settings.Default.ColorTextMain;

			startX = gap;
			startY = headerHeight + gap;
			availableWidth = width - gap * 2;
			availableHeight = labelSubtitle.Location.Y - headerHeight - gap * 2;

			if (Properties.Settings.Default.IsDebugMode) {
				pictureBoxLogo.BackColor = Color.AliceBlue;
				labelSubtitle.BackColor = Color.AliceBlue;
			} else {
				Cursor.Hide();
			}
			
			closeButton = CreateButton("", startX, labelSubtitle.Location.Y, labelSubtitle.Height, labelSubtitle.Height, Properties.Resources.BackButton);
			closeButton.Click += CloseButton_Click;
			Controls.Add(closeButton);
			closeButton.BringToFront();
			closeButton.Visible = false;
		}
		
		private void CloseButton_Click(object sender, EventArgs e) {
			LoggingSystem.LogMessageToFile("Закрытие формы с врачами по нажатию кнопки назад");
			Close();
		}

		private void FormTemplate_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape)
				Application.Exit();
		}

		protected Button CreateDoctorButton(string str, int x, int y, int width, int height) {
			Button button = CreateButton(str, x, y, width, height, GetImageForDoctor(str));
			//button.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(fontSize * 0.6));
			button.Tag = str;
			//str = str.Replace("- ", "-").Replace(" -", "-").Replace("-", " - ");
			button.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(height * 0.05));
			button.TextAlign = ContentAlignment.BottomCenter;
			button.ImageAlign = ContentAlignment.TopCenter;
			button.TextImageRelation = TextImageRelation.Overlay;

			return button;
		}

		protected Button CreateDepartmentButton(string str, int x, int y, int width, int height) {
			Button button = CreateButton(str, x, y, width, height, GetImageForDepartment(str));
			//button.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(fontSize * 0.6));
			button.Tag = str;
			str = str.Replace("- ", "-").Replace(" -", "-").Replace("-", " - ");
			button.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(height * 0.07));
			button.TextAlign = ContentAlignment.BottomCenter;
			button.ImageAlign = ContentAlignment.TopCenter;
			button.TextImageRelation = TextImageRelation.Overlay;

			return button;
		}

		protected Image GetImageForDoctor(string docname) {
			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Doctors\\", "*.jpg");

			if (files.Length == 0) {
				LoggingSystem.LogMessageToFile("Не удалось найти изображение для доктора: " + docname);
				//
				//send message to stp
				//
				return Properties.Resources.UnknownDoctor;
			}

			Random random = new Random();
			int fileNumber = random.Next(0, files.Length - 1);
			try {
				return Image.FromFile(files[fileNumber]);
			} catch (Exception e) {
				LoggingSystem.LogMessageToFile("Не удалось открыть файл с изображением: " + files[fileNumber]);
				return Properties.Resources.UnknownDoctor;
			}

		}

		private Image GetImageForDepartment(string depname) {
			string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\Departments\\", "*.png");

			if (files.Length == 0) {
				LoggingSystem.LogMessageToFile("Не удалось найти изображение для подразделения: " + depname);
				//
				//send message to stp
				//
				return Properties.Resources.UnknownDepartment;
			}

			Random random = new Random();
			int fileNumber = random.Next(0, files.Length - 1);
			try {
				return Image.FromFile(files[fileNumber]);
			} catch (Exception e) {
				LoggingSystem.LogMessageToFile("Не удалось открыть файл с изображением: " + files[fileNumber]);
				return Properties.Resources.UnknownDepartment;
			}

		}

		protected Button CreateButton(string str, int x, int y, int width, int height, Image image = null) {
			Button button = new Button();

			button.Text = str;
			button.SetBounds(x, y, width, height);
			button.BackColor = Properties.Settings.Default.ColorButtonMain;
			button.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(fontSize * 0.5));
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			button.FlatAppearance.MouseDownBackColor = Properties.Settings.Default.ColorButtonMainPressed;
			button.FlatAppearance.MouseOverBackColor = Properties.Settings.Default.ColorButtonMainSelected;

			if (image != null) {
				int imageWidth = image.Width;
				int imageHeight = image.Height;
				float imageScale = (float)imageWidth / (float)imageHeight;
				imageHeight = (int)(height * 0.6f);
				imageWidth = (int)(imageHeight * imageScale);

				if (imageWidth > width * 0.8) {
					imageWidth = (int)(width * 0.8);
					imageHeight = (int)(imageWidth / imageScale);
				}


				button.Image = ResizeImage(image, imageWidth, imageHeight);
				button.TextImageRelation = TextImageRelation.ImageBeforeText;
			}

			//Controls.Add(button);

			return button;
		}

		public Bitmap ResizeImage(Image image, int width, int height) {
			Rectangle destRect = new Rectangle(0, 0, width, height);
			Bitmap destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage)) {
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes()) {
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
				graphics.Dispose();
			}

			return destImage;
		}

		protected Label CreateLabel(string str, int x, int y, int width, int height) {
			Label label = new Label();
			label.Text = str;
			label.TextAlign = ContentAlignment.MiddleCenter;
			label.SetBounds(x, y, width, height);
			label.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, fontSize);
			label.ForeColor = Properties.Settings.Default.ColorTextMain;

			if (Properties.Settings.Default.IsDebugMode)
				label.BackColor = Color.AliceBlue;

			Controls.Add(label);

			return label;
		}

		protected void ButtonBack_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
