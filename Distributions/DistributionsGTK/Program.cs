using System;
using Gtk;

namespace DistributionsGTK
{
	class MainClass
	{


		public static void Main(string[] args)
		{
			GLib.ExceptionManager.UnhandledException += ExceptionManager_UnhandledException;
			Application.Init();
			TransformWindow win = new TransformWindow();
			//MainWindow win = new MainWindow();
			win.Show();
			Application.Run();
		}


		static void ExceptionManager_UnhandledException(GLib.UnhandledExceptionArgs args)
		{
			if (args.ExceptionObject is Exception)
			{
				CommonInterface.ShowException(null, (Exception)args.ExceptionObject);
			}
			args.ExitApplication = true;
		}
	}
}
