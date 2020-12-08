using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace newAquarium
{
	public partial class Form1 : Form
	{
		
		List<Fish> Fishs=new List<Fish>();
		List<Food> foods = new List<Food>();
		Timer timer = new Timer();
		static int scroll=2;
		public static int _Scroll { get { return scroll; } set { if (value >= 1&&value<4) scroll = value; } } //отключено
		public static PictureBox box;
		bool fullScreen = false;
		bool IsFood = false;
		public static Random random = new Random();//Меньше варианта повтора
		Size size_box;//Смещение рыбок при возврате с полного экрана
		static SqliteConnection conn = new SqliteConnection();
		static SqliteCommand command = new SqliteCommand();
		static void Conn()
		{
			conn.ConnectionString = ConfigurationManager.ConnectionStrings["Fish"].ConnectionString;
			command.Connection = conn;
		}
		public Form1()
		{
			InitializeComponent();
			Conn();
			box = LoadFish(1);
			box.Size = ClientSize = new Size(900, 450);
			box.SizeMode = PictureBoxSizeMode.StretchImage;
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
			int index = random.Next(2, 6);

			Fishs.Add(new Fish(LoadFish(index)));
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
		private PictureBox LoadFish(int index)
		{

			conn.Open();
			PictureBox picture = new PictureBox();
			try
			{
					
					using (command = new SqliteCommand($"SELECT png FROM Fish WHERE Id =={index}", conn))
					{
						var reader = command.ExecuteReader();
						while (reader.Read())
						{
							byte[] bytes = (System.Byte[])reader[0];
							MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
							ms.Write(bytes, 0, bytes.Length);
							picture.Image = new Bitmap(ms);
						}

					
					}
				return picture;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			conn.Close();
			return null;
		}
		private void LoadDBFish()
		{
			PictureBox picture = new PictureBox();
			conn.Open();
			//using (SqliteCommand command = new SqliteCommand("CREATE TABLE Fish(Id INTEGER PRIMARY KEY AUTOINCREMENT, png BLOB);", conn))
			//{
			//	command.ExecuteNonQuery();
			//}
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
					picture.Image = Image.FromFile(dialog.FileName);
					MemoryStream ms = new MemoryStream();
					picture.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					var buf = ms.ToArray();
			using (SqliteCommand command = new SqliteCommand("INSERT INTO Fish(png) VALUES(@png)", conn))
			{
					command.Parameters.Add("@png",(SqliteType)DbType.Binary).Value = buf;
					command.ExecuteNonQuery();
			}
				
				conn.Close();
			}

		}
	}
}
	

