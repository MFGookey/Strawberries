using System;
using System.Collections.Generic;
using Gtk;
using System.Drawing;
namespace Strawberries
{
	class ColourExample
	{
		Window win;
		DrawingArea da;

		static void Main()
		{
			Application.Init();
			new ColourExample();
			Application.Run();
		}

		ColourExample()
		{
			FindColors();
			win = new Window("Strawberry Reds");
			win.SetDefaultSize(610, 70);
			win.DeleteEvent += OnWinDelete;

			da = new DrawingArea();
			da.ExposeEvent += OnExposed;

			Gdk.Color col = new Gdk.Color();
			Gdk.Color.Parse("red", ref col);
			win.ModifyBg(StateType.Normal, col);
			da.ModifyBg(StateType.Normal, new Gdk.Color(128, 128, 128));

			win.Add(da);
			win.ShowAll();
		}

		private IList<DoubleColor> highestReds;

		void OnExposed(object o, ExposeEventArgs args)
		{
			DrawSquares(highestReds, 0);
		}

		private void FindColors()
		{
			var image = new Bitmap(System.Drawing.Image.FromFile("./Media/strawberries.jpg"));
			highestReds = FindHighestReds(image);
		}

		private DoubleColor PixelToColor(System.Drawing.Color pixel)
		{
			var result = new DoubleColor();
			result.DrawColor = pixel;
			result.GdkColor = new Gdk.Color(pixel.R, pixel.G, pixel.B);
			return result;
		}

		private IList<DoubleColor> FindHighestReds(Bitmap toSearch)
		{
			var results = new List<DoubleColor>();
			var intermediate = new List<DoubleColor>();
			for (int x = 0; x < toSearch.Width; x++)
			{
				for (int y = 0; y < toSearch.Height; y++)
				{
					var pixel = toSearch.GetPixel(x, y);
					intermediate.Add(PixelToColor(pixel));
				}
			}

			intermediate.Sort((DoubleColor a, DoubleColor b) => (FindDistance(a).CompareTo(FindDistance(b))));
			for (var i = 0; i < 10 && i < intermediate.Count; i++)
			{
				results.Add(intermediate[i]);
			}

			results.Sort((DoubleColor a, DoubleColor b) => (FindDistance(a).CompareTo(FindDistance(b))));

			for (var i = 0; i < results.Count; i++)
			{
				Console.WriteLine(string.Format("{0}: R: {1}\tG: {2}\tB: {3}\tDist:\t{4}", i + 1, results[i].DrawColor.R, results[i].DrawColor.G, results[i].DrawColor.B, FindDistance(results[i])));
			}
			return results;
		}

		private double FindDistance(DoubleColor color)
		{
			var r = color.DrawColor.R;
			var rAdjusted = 255 - r;
			var g = color.DrawColor.G;
			var b = color.DrawColor.B;
			var rAdjustedSquared = rAdjusted * rAdjusted;
			var gSquared = g * g;
			var bSquared = b * b;

			var raw = rAdjustedSquared + gSquared + bSquared;
			return Math.Sqrt(raw);
		}

		int rowGap = 10;
		int cellHeight = 50;
		int cellWidth = 50;
		int columnGap = 10;

		void DrawSquares(IList<DoubleColor> colors, int row)
		{
			var gc = da.Style.BaseGC(StateType.Normal);
			for (var i = 0; i < colors.Count; i++)
			{
				gc.RgbFgColor = colors[i].GdkColor;
				da.GdkWindow.DrawRectangle(gc, true, columnGap + (i * (columnGap + cellWidth)), row * (rowGap + cellHeight) + rowGap, cellWidth, cellHeight);
			}
		}

		void OnWinDelete(object o, DeleteEventArgs args)
		{
			Application.Quit();
		}
	}
}