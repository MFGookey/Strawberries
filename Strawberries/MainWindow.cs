using System;
using Gtk;

public partial class MainWindow : Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		
		this.DrawRectangle();
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	private void DrawRectangle()
	{
		Gdk.Color RectangleColor = new Gdk.Color(128, 128, 255);
		this.ModifyBg(StateType.Normal, RectangleColor);
		//To modify the size of the rectangle use the following.
		this.HeightRequest = 10;
		this.WidthRequest = 10;
	}
}
