using System;
using Gtk;

namespace DistributionsGTK
{
	public static class CommonInterface
	{

		public static void ShowException(Window owner, Exception ex)
		{
			MessageDialog dialog = new MessageDialog(owner, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, ex.Message);
			dialog.Title = "Ошибка";
			dialog.Run();
			dialog.Destroy();
		}
	}
}
