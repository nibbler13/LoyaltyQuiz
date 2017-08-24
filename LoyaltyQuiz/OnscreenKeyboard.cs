using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoyaltyQuiz {
	public class OnscreenKeyboard {
		/// <summary>
		/// virtual keyboard fields
		private TextBoxBase textBoxInput = null;
		private enum ShiftKeyStatus { Unpressed, Pressed, Capslock };
		private ShiftKeyStatus currentShiftKeyStatus = ShiftKeyStatus.Unpressed;
		private TimeSpan previousShiftKeyPress = new TimeSpan();
		private int availableWidth;
		private int availableHeight;
		private int startX;
		private int startY;
		private int gap;
		private int fontSize;
		private Button buttonShift;
		private Button buttonEnter;
		public enum KeyboardType { Full, Short, Number }
		private KeyboardType keyboardType;
		/// </summary>

		public OnscreenKeyboard(
			TextBoxBase textBoxInput,
			int availableWidth,
			int availableHeight,
			int startX,
			int startY,
			int gap,
			int fontSize,
			KeyboardType keyboardType) {
			this.textBoxInput = textBoxInput;
			this.availableWidth = availableWidth;
			this.availableHeight = availableHeight;
			this.startX = startX;
			this.startY = startY;
			this.gap = gap;
			this.fontSize = fontSize;
			this.keyboardType = keyboardType;
		}

		public void SetEnterButtonClick(EventHandler eventHandler) {
			RemoveClickEvent(buttonEnter);
			buttonEnter.Click += eventHandler;
		}

		private void RemoveClickEvent(Button b) {
			FieldInfo f1 = typeof(Control).GetField("EventClick",
				BindingFlags.Static | BindingFlags.NonPublic);
			object obj = f1.GetValue(b);
			PropertyInfo pi = b.GetType().GetProperty("Events",
				BindingFlags.NonPublic | BindingFlags.Instance);
			EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
			list.RemoveHandler(obj, list[obj]);
		}

		public Panel CreateOnscreenKeyboard() {
			List<List<string>> keys = new List<List<string>>() {
				new List<string>() {
					"(", ")", "'", "", "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "", "7", "8", "9"
				},
				new List<string>() {
					 "-", "+", "=", "", "ф", "ы", "в", "а", "п", "р", "о", "л", "д", "ж", "э", "", "4", "5", "6"
				},
				new List<string>() {
					":", ";", "!", "", "shift", "я", "ч", "с", "м", "и", "т", "ь", "б", "ю", "backspace", "", "1", "2", "3"
				},
				new List<string>() {
					",", ".", "?", "", "ё", "ъ", "Пробел", "Ввод", "", "", "0", ""
				}
			};

			int optionalKeysCount = 4;
			if (keyboardType == KeyboardType.Short || keyboardType == KeyboardType.Number)
				for (int i = 0; i < keys.Count; i++) {
					if (keyboardType == KeyboardType.Short) {
						keys[i].RemoveRange(keys[i].Count - optionalKeysCount, optionalKeysCount);
						keys[i].RemoveRange(0, optionalKeysCount);
					} else if (keyboardType == KeyboardType.Number) {
						keys[i].RemoveRange(0, keys[i].Count - optionalKeysCount + 1);
					}
				}

			double keyboardSizeCoefficient = 0.4;

			if (keyboardType == KeyboardType.Number) {
				keys[3][0] = "clear";
				keys[3][2] = "backspace";
				keyboardSizeCoefficient = 0.6;
			}

			int keysInLine = keys[0].Count;
			int keysLines = keys.Count;

			int keyboardHeight = (int)(availableHeight * keyboardSizeCoefficient);

			int distanceBetween = gap / 3;

			int buttonHeight = (keyboardHeight - distanceBetween * (keysLines - 1)) / keysLines;
			int buttonWidth = buttonHeight;

			int keyboardWidth = buttonWidth * keysInLine + distanceBetween * (keysInLine - 1);
			
			int leftCornerShadow = 4;
			int rightCornerShadow = 8;

			int keyboardX = startX + (availableWidth - keyboardWidth) / 2;
			int keyboardY = startY + availableHeight - keyboardHeight;
			int keyCurrentX = leftCornerShadow;
			int keyCurrentY = leftCornerShadow;


			Panel panel = new Panel();
			panel.SetBounds(
				keyboardX - leftCornerShadow, 
				keyboardY - leftCornerShadow, 
				keyboardWidth + leftCornerShadow + rightCornerShadow, 
				keyboardHeight + leftCornerShadow + rightCornerShadow);

			if (Properties.Settings.Default.IsDebugMode)
				panel.BackColor = Color.AliceBlue;

			currentShiftKeyStatus = ShiftKeyStatus.Unpressed;


			foreach (List<string> keysLine in keys) {
				foreach (string keyName in keysLine) {
					if (string.IsNullOrEmpty(keyName)) {
						keyCurrentX += buttonWidth + distanceBetween;
						continue;
					}

					double fontScale = 1.0;

					if (keyName.Equals("Пробел") || keyName.Equals("Ввод"))
						fontScale = 0.7;

					Button buttonKey = FormTemplate.CreateButton(keyName, keyCurrentX, keyCurrentY, buttonWidth, buttonHeight, fontSize, fontScale);
					panel.Controls.Add(buttonKey);

					Image imageDropShadow = Properties.Resources.DropShadowKeyboardMain;
					Image image = null;

					switch (keyName) {
						case "shift":
							buttonKey.Tag = "shift";
							image = Properties.Resources.ButtonShiftUnpressed;
							buttonKey.Click += ButtonKeyShift_Click;
							buttonShift = buttonKey;
							break;
						case "backspace":
							buttonKey.Tag = "backspace";
							image = Properties.Resources.ButtonBackspace;
							buttonKey.Click += ButtonKeyBackspace_Click;
							break;
						case "Пробел":
							buttonKey.Tag = "space";
							buttonKey.Width = buttonWidth * 7 + distanceBetween * 6;
							keyCurrentX += buttonKey.Width - buttonWidth;
							imageDropShadow = Properties.Resources.DropShadowKeyboardSpace;
							buttonKey.Click += ButtonKeySpace_Click;
							break;
						case "Ввод":
							buttonKey.Tag = "enter";
							buttonKey.Width = buttonWidth * 2 + distanceBetween;
							keyCurrentX += buttonKey.Width - buttonWidth;
							imageDropShadow = Properties.Resources.DropShadowKeyboardDoubledKey;
							buttonKey.Click += ButtonKeyEnter_Click;
							buttonEnter = buttonKey;
							break;
						case "clear":
							buttonKey.Tag = "clear";
							image = Properties.Resources.ButtonClear;
							buttonKey.Click += ButtonClear_Click;
							break;
						default:
							break;
					}

					if (buttonKey.Tag == null)
						buttonKey.Click += ButtonKey_Click;

					if (keyName.Equals("shift") || keyName.Equals("backspace") || keyName.Equals("Ввод"))
						buttonKey.BackColor = Properties.Settings.Default.ColorDisabled;

					if (image != null) {
						buttonKey.Text = "";
						FormTemplate.SetImageToButton(buttonKey, image);
					}

					PictureBox dropShadow = FormTemplate.CreateDropShadow(buttonKey, imageDropShadow, leftCornerShadow, rightCornerShadow);
					panel.Controls.Add(dropShadow);
					dropShadow.SendToBack();

					keyCurrentX += buttonWidth + distanceBetween;
				}

				keyCurrentX = leftCornerShadow;
				keyCurrentY += buttonHeight + distanceBetween;
			}

			return panel;
		}

		private void ButtonClear_Click(object sender, EventArgs e) {
			textBoxInput.Text = "";
		}

		private void ButtonKeyEnter_Click(object sender, EventArgs e) {
			SendKeyToTextBox("~");
		}

		private void ButtonKeySpace_Click(object sender, EventArgs e) {
			SendKeyToTextBox(" ");
		}

		private void ButtonKeyBackspace_Click(object sender, EventArgs e) {
			SendKeyToTextBox("{BS}");
		}

		private void ButtonKeyShift_Click(object sender, EventArgs e) {
			Console.WriteLine("ButtonKeyShift_Click");
			UpdateShiftKey();
		}

		private void ButtonKey_Click(object sender, EventArgs e) {
			SendKeyToTextBox((sender as Button).Text);

			if (currentShiftKeyStatus == ShiftKeyStatus.Pressed)
				UpdateShiftKey(true);
		}



		private void UpdateShiftKey(bool ignoreDoubleClick = false) {
			Color color;
			Image image;

			bool isDoubleClick = (new TimeSpan(DateTime.Now.Ticks) - previousShiftKeyPress).TotalSeconds < 0.5 ? true : false;
			if (ignoreDoubleClick)
				isDoubleClick = false;

			if (isDoubleClick) {
				currentShiftKeyStatus = ShiftKeyStatus.Capslock;
				color = Properties.Settings.Default.ColorButtonMain;
				image = Properties.Resources.ButtonCapslock;
			} else if (currentShiftKeyStatus == ShiftKeyStatus.Unpressed) {
				currentShiftKeyStatus = ShiftKeyStatus.Pressed;
				color = Properties.Settings.Default.ColorButtonMain;
				image = Properties.Resources.ButtonShiftPressed;
			} else {
				currentShiftKeyStatus = ShiftKeyStatus.Unpressed;
				color = Properties.Settings.Default.ColorDisabled;
				image = Properties.Resources.ButtonShiftUnpressed;
			}

			buttonShift.BackColor = color;
			FormTemplate.SetImageToButton(buttonShift, image);
			ChangeKeyboardCapitalizeStatus(buttonShift);

			previousShiftKeyPress = new TimeSpan(DateTime.Now.Ticks);
		}

		private void ChangeKeyboardCapitalizeStatus(Button buttonKey) {
			Panel keyboardPanel = buttonKey.Parent as Panel;

			foreach (Control control in keyboardPanel.Controls) {
				if (control.Tag != null)
					continue;

				control.Text = currentShiftKeyStatus == ShiftKeyStatus.Unpressed ?
					control.Text.ToLower() : control.Text.ToUpper();
			}
		}

		private void SendKeyToTextBox(string code) {
			if (textBoxInput == null)
				return;

			if (code.Equals("(") ||
				code.Equals(")") ||
				code.Equals("+"))
				code = "{" + code + "}";

			textBoxInput.Focus();
			SendKeys.Send(code);

		}
	}
}
