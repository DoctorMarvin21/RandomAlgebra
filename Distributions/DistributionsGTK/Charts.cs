using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;

namespace Charts
{

	public class ChartsDrawer
	{
		public ChartsDrawer(Gtk.DrawingArea owner)
		{
			Owner = owner;
			Owner.ExposeEvent += Owner_ExposeEvent;
			Pane = new ChartPane(this);
		}

		void Owner_ExposeEvent(object o, Gtk.ExposeEventArgs args)
		{
#pragma warning disable CS1701 // Assuming assembly reference matches identity
			using (Context g = Gdk.CairoHelper.Create(Owner.GdkWindow))
#pragma warning restore CS1701 // Assuming assembly reference matches identity
			{

				int offset = 10;
				var destination = new Rectangle(offset, offset, Owner.Allocation.Width - offset * 2, Owner.Allocation.Height - offset * 2);

				g.Rectangle(destination);
				g.SetSourceRGB(255, 255, 255);
				g.Fill();
				g.Rectangle(destination);
				g.SetSourceRGB(0, 0, 0);
				g.Stroke();

				if (Pane.Lines.Count > 0)
				{
					var source = Pane.GetSize();

					foreach (var line in Pane.Lines)
					{
						DrawLine(g, line, source, destination);
					}

					g.GetTarget().Dispose();
					g.Dispose();
				}
			}		}

		public void Redraw()
		{
			Owner.QueueDraw();
		}

		private void DrawLine(Context g, ChartLine line, Rectangle source, Rectangle destination)
		{
			var points = line.Points;


			double scaleX = destination.Width / source.Width;
			double scaleY = -destination.Height / source.Height;

			double offsetX = (destination.X) - source.X * scaleX;
			double offsetY = (destination.Y + destination.Height) + source.Y * scaleY;

			PointD prev = new PointD();
			for (int i = 0; i < points.Count; i++)
			{
				var pt = points[i];

				pt.X *= scaleX;
				pt.Y *= scaleY;
				pt.X += offsetX;
				pt.Y += offsetY;

				if (i == 0)
				{
					g.MoveTo(pt);
				}
				else
				{
					g.LineTo(pt);
				}

				prev = pt;
			}

			g.SetSourceRGB(line.Color.R, line.Color.G, line.Color.B);
			g.LineWidth = line.LineWidth;
			g.Stroke();
		}

		public ChartPane Pane
		{
			get;
		}

		public Gtk.DrawingArea Owner
		{
			get;
		}

		public class ChartPane
		{
			public ChartPane(ChartsDrawer graphics)
			{
				Owner = graphics;
				Lines = new List<ChartLine>();
			}

			public ChartsDrawer Owner
			{
				get;
			}

			public List<ChartLine> Lines
			{
				get;
			}

			public Rectangle GetSize()
			{
				var xPoints = Lines.SelectMany(x => x.Points.Select(y => y.X));
				var yPoints = Lines.SelectMany(x => x.Points.Select(y => y.Y));
				var minX = xPoints.Min();
				var minY = yPoints.Min();
				var maxX = xPoints.Max();
				var maxY = yPoints.Max();

				return new Rectangle(minX, minY, maxX - minX, maxY - minY);

			}
		}
	}


	public class ChartLine
	{
		public ChartLine()
		{
			Points = new List<PointD>();
		}

		public List<PointD> Points
		{
			get;
		}

		public double LineWidth
		{
			get;
			set;
		} = 1;

		public Color Color
		{
			get;
			set;
		}
	}
}
