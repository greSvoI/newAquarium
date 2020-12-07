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
		public static int _Scroll { get { return scroll; } set { if (value >= 1) scroll = value; } } 
		PictureBox box;
		bool fullScreen = false;
		public static Random random = new Random();
		public Form1()
		{
			InitializeComponent();
			 box = new PictureBox()
			{
				Image = newAquarium.Properties.Resources.back,
				SizeMode = PictureBoxSizeMode.StretchImage,
				Size = ClientSize = new Size(900, 400)
			};
			Controls.Add(box);
			this.BackgroundImage = newAquarium.Properties.Resources.back;
			

			//box.Controls.Add(new Fish().Picture);
			//box.Controls.Add(new Fish().Picture);
		    //box.Controls.Add(new Fish().Picture);
			//box.Controls.Add(new Fish().Picture);



			box.ContextMenuStrip = contextMenuStrip1;
			box.MouseClick += Box_MouseClick;

			timer.Interval = 1;
			timer.Start();
			timer.Tick += (s, e) =>
			  {
				  tic++;
				  foreach (var item in Fishs)
				  { item.Move();
					 if(tic%1000==0) item.SpeedRand();
					  }
			  };

			box.MouseWheel += Box_MouseWheel;
			box.MouseDoubleClick += Box_MouseDoubleClick;
		}

		private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!fullScreen)
			{
				WindowState = FormWindowState.Maximized;
				FormBorderStyle = FormBorderStyle.None;
				fullScreen = true;
				box.Size = new Size(this.Size.Width, this.Size.Height);
			}
			else
			{
				WindowState = FormWindowState.Normal;
				FormBorderStyle = FormBorderStyle.Sizable;
				fullScreen = false;
				box.Size = new Size(this.Size.Width, this.Size.Height);
			}
		}

		private void Box_MouseWheel(object sender, MouseEventArgs e)
		{
			
			if(e.Delta>0)
			this.Text = scroll++.ToString();
			else this.Text = scroll--.ToString();
		}

		private void Box_MouseClick(object sender, MouseEventArgs e)
		{
			box.Focus();
			if (e.Button == MouseButtons.Right)
				contextMenuStrip1.Show();
			if (e.Button == MouseButtons.Middle)
				this.Text = scroll++.ToString();
		}

		private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
		{
			Fishs.Add(new Fish());
			foreach (var item in Fishs)
				box.Controls.Add(item.Picture);
		}
	}
}
