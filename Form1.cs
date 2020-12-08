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
		List<Food> foods = new List<Food>();
		Timer timer = new Timer();
		static int scroll=2;
		public static int _Scroll { get { return scroll; } set { if (value >= 1&&value<4) scroll = value; } } 
		public static PictureBox box;
		bool fullScreen = false;
		bool IsFood = false;
		public static Random random = new Random();//Так последовательно не идут одни и теже цифры
		Size size_box;//Смещение рыбок при возврате с полного экрана
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
				foreach (var item in Fishs)
				{
					item.Move();
					FoodFish();
				}
			for (int i = 0; i < foods.Count; i++)
			{
				if (foods[i].Beyond)
					foods[i].Move();
				else foods.RemoveAt(i);
			}
			this.Text = foods.Count.ToString();
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
		/// <summary> отключено  </summary>
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
			if(e.Button==MouseButtons.Left)
			{
				IsFood = true;
				if(foods.Count<5)
				foods.Add(new Food(e.X));
			}
			//if (e.Button == MouseButtons.Middle)
			//	this.Text = "Мышь: левая - кормить правая - рыбки скролл - турбо :x" + scroll--.ToString();
		}

		private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
		{
			Fishs.Add(new Fish());
			foreach (var item in Fishs)
				box.Controls.Add(item.Picture);
		}
		private void FoodFish()
		{
			try
			{


				for (int i = 0; i < foods.Count; i++)
				{
					foreach (var item in Fishs)
					{
						if (i<foods.Count)
						{
							if (IsFood)
							{
								item.Xx = item.Picture.Location.X < foods[i].Location.X ? item.Xx = 2 : item.Xx = -2;
								item.Yy = item.Yy < foods[i].Location.Y ? item.Yy = -2 : item.Yy = 2;
								
							}
							double collis = Math.Sqrt(Math.Pow(item.Picture.Location.X - foods[i].Location.X, 2) + Math.Pow(item.Picture.Location.Y - foods[i].Location.Y, 2));
							if (collis < 50)
							{
								foods[i].Dispose();
								foods.RemoveAt(i);
							}
						}
					}
					IsFood = false;
				}
			}
			catch (Exception ex) { MessageBox.Show("FoodFish\n\r" + ex.Message); }

		}
	}
}
