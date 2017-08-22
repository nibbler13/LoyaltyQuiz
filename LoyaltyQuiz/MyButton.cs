using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoyaltyQuiz {
	public class MyButton : Button {

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x20;
				cp.ClassStyle |= 0x00020000;
				return cp;
			}
		}

		//protected override void OnPaint(PaintEventArgs pevent) {
		//	Console.WriteLine("------OnPaint");
		//	if (this.Parent != null) {
		//		Console.WriteLine("------MyButton OnPaint");
		//		GraphicsContainer cstate = pevent.Graphics.BeginContainer();
		//		pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
		//		Rectangle clip = pevent.ClipRectangle;
		//		clip.Offset(this.Left, this.Top);
		//		PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, clip);

		//		//paint the container's bg
		//		InvokePaintBackground(this.Parent, pe);
		//		//paints the container fg
		//		InvokePaint(this.Parent, pe);
		//		//restores graphics to its original state
		//		pevent.Graphics.EndContainer(cstate);
		//	} else
		//		base.OnPaint(pevent); // or base.OnPaint(pevent);...
		//}


		protected override void OnPaintBackground(PaintEventArgs pevent) {
			Console.WriteLine("------OnPaintBackground");
			if (this.Parent != null) {
				Console.WriteLine("------MyButton OnPaintBackground");
				GraphicsContainer cstate = pevent.Graphics.BeginContainer();
				pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
				Rectangle clip = pevent.ClipRectangle;
				clip.Offset(this.Left, this.Top);
				PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, clip);

				//paint the container's bg
				InvokePaintBackground(this.Parent, pe);
				//paints the container fg
				InvokePaint(this.Parent, pe);
				//restores graphics to its original state
				pevent.Graphics.EndContainer(cstate);
			} else
				base.OnPaintBackground(pevent); // or base.OnPaint(pevent);...
		}
	}
}
