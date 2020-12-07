using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newAquarium
{
	public partial class Form1 : Form
	{
		List<Fish> Fishs=new List<Fish>();
		Timer timer = new Timer();
		static int scroll=2;
		int tic = 0;
		public static int _Scroll { get { return scroll; } set { if (value >= 1&&value<4) scroll = value; } } 
		PictureBox box;
		bool fullScreen = false;
		public static Random random = new Random();//Так последовательно не идут одни и теже цифры
		Size size_box;
		public Form1()
		{
			InitializeComponent();
			 box = new PictureBox()
			{
				Image = newAquarium.Properties.Resources.aquarium,
				SizeMode = PictureBoxSizeMode.StretchImage,
				Size = ClientSize = new Size(900, 400)
			};
			Controls.Add(box);
			this.Text = "Мышь : левая-кормить правая-рыбки ";
			box.ContextMenuStrip = contextMenuStrip1;
			box.MouseClick += Box_MouseClick;

			timer.Interval = 1;
			timer.Start();
			timer.Tick += Timer_Tick;

			box.MouseWheel += Box_MouseWheel;
			box.MouseDoubleClick += Box_MouseDoubleClick;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
				tic++;
				foreach (var item in Fishs)
				{
					item.Move();
				}
			box.Invalidate();
			
		}

		private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!fullScreen)
			{
				WindowState = FormWindowState.Maximized;
				FormBorderStyle = FormBorderStyle.None;
				fullScreen = true;
				size_box = box.Size;
				box.Size = new Size(this.Size.Width, this.Size.Height);
				//foreach (var item in Fishs)
					//item.Picture.Size = new Size(item.Picture.Size.Width * 2, item.Picture.Size.Height * 2);
			}
			else
			{
				WindowState = FormWindowState.Normal;
				FormBorderStyle = FormBorderStyle.Sizable;
				fullScreen = false;
				box.Size = new Size(this.Size.Width, this.Size.Height);
				int width = SystemInformation.PrimaryMonitorSize.Width / size_box.Width;
				int height = SystemInformation.PrimaryMonitorSize.Height / size_box.Height;

				foreach (var item in Fishs)
					item.Picture.Location = new Point(item.Picture.Location.X / width, item.Picture.Location.Y / height);

			}
		}

		private void Box_MouseWheel(object sender, MouseEventArgs e)
		{
			
			if(e.Delta>0&&scroll<4)
			this.Text = "Мышь: левая - кормить правая - рыбки скролл - турбо :x" + scroll++.ToString();
			if(e.Delta<0)
				if(scroll > 1)
			this.Text = "Мышь: левая - кормить правая - рыбки скролл - турбо :x" + scroll--.ToString();
		}

		private void Box_MouseClick(object sender, MouseEventArgs e)
		{
			//box.Focus();
			if (e.Button == MouseButtons.Right)
				contextMenuStrip1.Show();
			//if (e.Button == MouseButtons.Middle)
			//	this.Text = "Мышь: левая - кормить правая - рыбки скролл - турбо :x" + scroll--.ToString();
		}

		private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
		{
			Fishs.Add(new Fish());
			foreach (var item in Fishs)
				box.Controls.Add(item.Picture);
		}
	}
}
