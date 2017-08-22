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
		/// <summary>
		///	main fields
		private KeyValuePair<Button, PictureBox> buttonClose;
		protected int width = 0;
		protected int height = 0;
		protected int gap = 0;
		protected int headerHeight = 0;
		protected int fontSize = 0;
		protected int startX = 0;
		protected int startY = 0;
		protected int availableWidth = 0;
		protected int availableHeight = 0;
		protected int leftCornerShadow = 11;
		protected int rightCornerShadow = 19;
		/// </summary>


		/// <summary>
		///	used for panel with custom buttons
		protected Panel panel;
		protected KeyValuePair<Button, PictureBox> buttonScrollUp;
		protected KeyValuePair<Button, PictureBox> buttonScrollDown;
		protected int locationY = 0;
		protected int scrollDistance = 0;
		protected int elementWidth = 0;
		protected int elementHeight = 0;
		protected int currentX = 0;
		protected int currentY = 0;
		protected int elementsInLine = 0;
		protected int elementsLineCount = 0;
		protected enum ElementType { Department, Doctor };
		/// </summary>
		



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
				//Cursor.Hide();
			}
			
			buttonClose = CreateDefaultButton(
				startX, 
				labelSubtitle.Location.Y,
				Properties.Resources.BackButton2);
			buttonClose.Key.Click += ButtonClose_Click;
			buttonClose.Key.BringToFront();

			SetButtonCloseVisible(false);
		}

		


		protected KeyValuePair<Button, PictureBox> CreateDefaultButton(int x, int y, Image image) {
			Button button = new Button();

			int buttonSide = labelSubtitle.Height;
			
			//button.Text = str;
			button.SetBounds(x, y, buttonSide, buttonSide);
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
				imageHeight = (int)(buttonSide * 0.8f);
				imageWidth = (int)(imageHeight * imageScale);

				if (imageWidth > buttonSide * 0.8) {
					imageWidth = (int)(buttonSide * 0.8);
					imageHeight = (int)(imageWidth / imageScale);
				}

				button.Image = ResizeImage(image, imageWidth, imageHeight);
				button.ImageAlign = ContentAlignment.MiddleCenter;
			}


			PictureBox dropShadow = CreateDropShadow(button, Properties.Resources.DropShadowButtonDefault);
			Controls.Add(dropShadow);
			Controls.Add(button);

			dropShadow.BringToFront();
			button.BringToFront();

			return new KeyValuePair<Button, PictureBox>(button, dropShadow);
		}

		protected Panel CreateInnerPanel(string str, int x, int y, int width, int height, ElementType type) {
			Image image;
			string normalizedStr;
			ContentAlignment alignment;

			if (type == ElementType.Department) {
				image = GetImageForDepartment(str);
				normalizedStr = str.Replace("- ", "-").Replace(" -", "-").Replace("-", " - ");
				normalizedStr = FirstCharToUpper(normalizedStr);
				alignment = ContentAlignment.MiddleLeft;
			} else {
				image = GetImageForDoctor(str);
				normalizedStr = str.Replace(" ", Environment.NewLine).Replace(" ", Environment.NewLine);
				alignment = ContentAlignment.MiddleCenter;
			}

			Panel panel = new Panel();
			panel.SetBounds(x, y, width, height);
			panel.BackColor = Properties.Settings.Default.ColorButtonMain;
			
			bool isHorizontal = width > height;
			int border = 10;// (int)((isHorizontal ? height : width) * 0.1);

			bool isOversized = false;
			int pictureBoxSide = (isHorizontal ? height : width) - border * 2;
			int maxSide = (isHorizontal ? width : height);
			if (pictureBoxSide > maxSide * 0.65) {
				pictureBoxSide = (int)(maxSide * 0.65) - border;
				isOversized = true;
			}

			int pictureBoxX = border;
			int pictureBoxY = border;

			if (isOversized)
				if (isHorizontal)
					pictureBoxY = (height - pictureBoxSide) / 2;
				else
					pictureBoxX = (width - pictureBoxSide) / 2;

			PictureBox pictureBox = new PictureBox();
			pictureBox.Image = image;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.SetBounds(pictureBoxX, pictureBoxY, pictureBoxSide, pictureBoxSide);

			int labelX;
			int labelY;
			int labelWidth;
			int labelHeight;

			if (isHorizontal) {
				labelX = pictureBoxX + pictureBoxSide + border;
				labelY = border;
				labelWidth = width - border - labelX;
				labelHeight = height - border * 2;
			} else {
				labelX = border;
				labelY = pictureBoxY + pictureBoxSide + border;
				labelWidth = width - border * 2;
				labelHeight = height - labelY - border;
			}

			Label label = new Label();
			label.Text = normalizedStr;
			label.ForeColor = Properties.Settings.Default.ColorTextMain;
			label.TextAlign = alignment;
			label.Font = new Font(Properties.Settings.Default.FontSub.FontFamily, (int)(fontSize * 0.6));
			label.SetBounds(labelX, labelY, labelWidth, labelHeight);

			panel.Controls.Add(pictureBox);
			panel.Controls.Add(label);
			panel.Tag = str;

			label.Tag = str;
			pictureBox.Tag = str;

			return panel;
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

		protected void CreateUpDownButtons() {
			int buttonSide = buttonClose.Key.Width;
			Color color = Color.White;

			buttonScrollUp = CreateDefaultButton(
				startX + availableWidth - buttonSide, 
				startY, 
				Properties.Resources.UpButton);
			buttonScrollUp.Key.Click += ButtonScrollUp_Click;
			buttonScrollUp.Key.Visible = false;
			buttonScrollUp.Value.Visible = false;

			buttonScrollDown = CreateDefaultButton(
				startX + availableWidth - buttonSide, 
				startY + availableHeight - buttonSide, 
				Properties.Resources.DownButton);
			buttonScrollDown.Key.Click += ButtonScrollDown_Click;

			availableWidth -= buttonSide + gap;
		}

		protected PictureBox CreateDropShadow(Control control, Image image) {
			PictureBox pictureBox = new PictureBox();

			pictureBox.Image = image;
			pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox.SetBounds(
				control.Location.X - leftCornerShadow,
				control.Location.Y - leftCornerShadow,
				control.Width + leftCornerShadow + rightCornerShadow,
				control.Height + leftCornerShadow + rightCornerShadow);

			return pictureBox;
		}

		protected void CreateMainPanel(int elementsInLine, int elementsLineCount, int totalElements) {
			this.elementsInLine = elementsInLine;
			this.elementsLineCount = elementsLineCount;

			Console.WriteLine("totalElements: " + totalElements);

			if (totalElements > elementsInLine * elementsLineCount)
				CreateUpDownButtons();

			elementWidth = (availableWidth - gap * (elementsInLine - 1)) / elementsInLine;
			elementHeight = (availableHeight - gap * (elementsLineCount - 1)) / elementsLineCount;

			panel = new Panel();
			panel.SetBounds(
				startX - leftCornerShadow,
				startY - leftCornerShadow,
				availableWidth + leftCornerShadow + rightCornerShadow,
				availableHeight + leftCornerShadow + rightCornerShadow);

			panel.HorizontalScroll.Visible = false;
			panel.VerticalScroll.Visible = false;
			panel.AutoScrollPosition = new Point(0, 0);

			int lines = (int)Math.Ceiling((double)totalElements / (double)elementsInLine);// - elementsLineCount;
			Console.WriteLine("lines: " + lines);
			panel.VerticalScroll.Maximum = lines * elementHeight + (gap * (lines - 1)) + leftCornerShadow + rightCornerShadow;
			Controls.Add(panel);

			if (Properties.Settings.Default.IsDebugMode)
				panel.BackColor = Color.AliceBlue;

			scrollDistance = panel.Height;
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

		protected Image GetImageForDepartment(string depname) {
			string mask = Directory.GetCurrentDirectory() + "\\Departments\\*.png";
			string wantedFile = mask.Replace("*", depname);

			if (!File.Exists(wantedFile)) {
				LoggingSystem.LogMessageToFile("Не удалось найти изображение для подразделения: " + depname);
				//
				//send message to stp
				//
				return Properties.Resources.UnknownDepartment;
			}

			//Random random = new Random();
			//int fileNumber = random.Next(0, files.Length - 1);
			try {
				return Image.FromFile(wantedFile);
			} catch (Exception e) {
				LoggingSystem.LogMessageToFile("Не удалось открыть файл с изображением: " + wantedFile);
				return Properties.Resources.UnknownDepartment;
			}

		}

		private Bitmap ResizeImage(Image image, int width, int height) {
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




		protected void ButtonScrollDown_Click(object sender, EventArgs e) {
			Console.WriteLine("DownButton_Click");
			if (locationY + scrollDistance >= panel.VerticalScroll.Maximum) {
				locationY = panel.VerticalScroll.Maximum;
				panel.AutoScrollPosition = new Point(0, locationY);
				buttonScrollDown.Key.Visible = false;
				buttonScrollDown.Value.Visible = false;
			} else {
				locationY += scrollDistance;
				panel.VerticalScroll.Value = locationY;
				buttonScrollUp.Key.Visible = true;
				buttonScrollUp.Value.Visible = true;
			}
		}

		protected void ButtonScrollUp_Click(object sender, EventArgs e) {
			Console.WriteLine("UpButton_Click");
			if (locationY - scrollDistance <= 0) {
				locationY = 0;
				panel.AutoScrollPosition = new Point(0, 0);
				buttonScrollUp.Key.Visible = false;
				buttonScrollUp.Value.Visible = false;
			} else {
				locationY -= scrollDistance;
				panel.VerticalScroll.Value = locationY;
				buttonScrollDown.Key.Visible = true;
				buttonScrollDown.Value.Visible = true;
			}
		}

		protected void ButtonBack_Click(object sender, EventArgs e) {
			Close();
		}

		private void ButtonClose_Click(object sender, EventArgs e) {
			LoggingSystem.LogMessageToFile("Закрытие формы с врачами по нажатию кнопки назад");
			Close();
		}



		
		private static string FirstCharToUpper(string input) {
			if (String.IsNullOrEmpty(input))
				return input;
			return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
		}

		protected void SetButtonCloseVisible(bool isVisible) {
			buttonClose.Key.Visible = isVisible;
			buttonClose.Value.Visible = isVisible;
		}
		
		protected void FillPanelWithElements(List<string> elements, ElementType type, EventHandler eventHandler) {
			int elementsCreated = 0;
			int linesCreated = 0;

			currentX = leftCornerShadow;
			currentY = leftCornerShadow;

			elements.Sort();
			foreach (string element in elements) {
				if (string.IsNullOrEmpty(element))
					continue;

				Panel innerPanel = CreateInnerPanel(element, currentX, currentY, elementWidth, elementHeight, type);

				panel.Controls.Add(innerPanel);
				innerPanel.Click += eventHandler;
				foreach (Control control in innerPanel.Controls)
					control.Click += eventHandler;

				Image image;
				if (type == ElementType.Department) {
					image = Properties.Resources.DropShadowDepartment;
				} else {
					image = Properties.Resources.DropShadowDoctor;
				}

				PictureBox dropShadow = CreateDropShadow(innerPanel, image);

				panel.Controls.Add(dropShadow);
				dropShadow.SendToBack();
				innerPanel.BringToFront();

				currentX += elementWidth + gap;
				elementsCreated++;

				if (elementsCreated >= elementsInLine) {
					elementsCreated = 0;
					linesCreated++;
					currentX = leftCornerShadow;
					currentY += elementHeight + gap;
				}
			}
		}



		private void FormTemplate_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape)
				Application.Exit();
		}
	}
}
