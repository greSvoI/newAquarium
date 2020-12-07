using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newAquarium
{
	class Fish
	{
		private Random random = new Random();
		public PictureBox Picture { get; set; }
		int speed = Form1._Scroll;
		bool rotate = true;
		int x=1, y=1;
		int Xx { get { return x; } 
			set {
				if (x < 0)
				{
					x = -speed;
					Picture.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
					rotate = false;
				}
				else
				{
					x = speed;
					if (!rotate) rotate = true;
				}
			    }
		}
		int Yy { get { return y; }set { if (y < 0) y = -speed; else y = speed; } }
		public Fish()
		{
			Picture = new PictureBox()
			{
				Image = Properties.Resources.new2,
				SizeMode = PictureBoxSizeMode.Zoom,
				Size = new Size(90, 45),
				BackColor = Color.Transparent
			};
			
		}
		public void Locate()
		{
			Picture.Location = new Point(
				random.Next(Picture.Parent.Width - Picture.Width),
				random.Next(Picture.Parent.Height - Picture.Height));
		}
		public void Move()
		{
			
			if (Picture.Location.X + Picture.Width >= Picture.Parent.Width || Picture.Location.X <= 0)
			{
				Picture.Location = new Point(Picture.Location.X <= 0 ? Xx = 1 : Xx = -1);
				
				
			}
			if (Picture.Location.Y + Picture.Height >= Picture.Parent.Height || Picture.Location.Y <= 0)
			{
				Picture.Location = new Point(Picture.Location.X, Picture.Location.Y <= 0 ? Yy=1 : Yy=-1);
				
			}
			Picture.Location = new Point(Picture.Location.X + Xx, Picture.Location.Y + Yy);
		}
	}
}
