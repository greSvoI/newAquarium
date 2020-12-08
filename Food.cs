using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newAquarium
{
	class Food:IDisposable
	{
		private Point[] points = new Point[9];
		public Point Location => points[0];
		public int Side { get; set; } = 20;
		public bool Beyond => Location.Y < Form1.box.ClientSize.Height;
		public Food(int x)
		{
			points[0] = new Point(x - Side, 0);
			for (int i = 1; i < points.Length; ++i)
				points[i] = new Point(Form1.random.Next(Location.X, Location.X + Side), Form1.random.Next(Side));
			Form1.box.Paint += Back_Paint;
		}

		private void Back_Paint(object sender, PaintEventArgs e)
		{
			for (int i = 1; i < points.Length; ++i)
				e.Graphics.FillEllipse(Brushes.Orange, points[i].X, points[i].Y, 7, 7);
		}
		public void Move()
		{
			for (int i = 0; i < points.Length; ++i)
				++points[i].Y;
		}

		public void Dispose()
		{
			Form1.box.Paint -= Back_Paint;
		}
	}
}
