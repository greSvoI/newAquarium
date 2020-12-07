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
		
		public PictureBox Picture { get; set; }
		int speed { get; set; }
		
		private bool rotate=true;
		public bool Rotate 
		{
			get => rotate; 
			set { 
				if (!value&&rotate||value&&!rotate)
				{
					Picture.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
					rotate = value; 
				}
			} 
		}
		private int x, y;
		int Xx { get { return x; } 
			set {
				Rotate = x < 0 ? Rotate = false : Rotate = true;
					x = value;
			    }
		}
		int Yy { get => y; set { y = value; } }
		public Fish()
		{
			Picture = new PictureBox()
			{
				Image = Properties.Resources.new2,
				SizeMode = PictureBoxSizeMode.Zoom,
				Size = new Size(90, 45),
				BackColor = Color.Transparent
			};
		    Xx = Form1.random.Next(0, 2) == 0 ? x = -1 : x = 1;
			Yy = Form1.random.Next(0, 2) == 0 ? y = -1 : y = 1;

			Picture.Location = new Point(
				Form1.random.Next(0,400-Picture.Width), Form1.random.Next(0,200 - Picture.Height));


		}
		public void SpeedRand()
		{
			speed = Form1._Scroll;
			Xx = Xx < 0 ? Form1.random.Next(1, speed) : Form1.random.Next(-speed, -1);
			Yy = Yy < 0 ? Form1.random.Next(1, speed) : Form1.random.Next(-speed, -1);
			Rotate = Xx < 0 ? Rotate = false : Rotate = true;
		}
		public void Locate()
		{
			
			Picture.Location = new Point(
				Form1.random.Next(Picture.Parent.Width - Picture.Width),
				Form1.random.Next(Picture.Parent.Height - Picture.Height));
		}
		public void Move()
		{
			
			if (Picture.Location.X + Picture.Width >= Picture.Parent.Width || Picture.Location.X <= 0)
			{
				Xx = -Xx;
				Rotate = Xx < 0 ? Rotate = false : Rotate = true;
			}

			if (Picture.Location.Y + Picture.Height >= Picture.Parent.Height || Picture.Location.Y <= 0)
				Yy = -Yy;
			
			Picture.Location = new Point(Picture.Location.X + Xx, Picture.Location.Y + Yy);
		}
	}
}
