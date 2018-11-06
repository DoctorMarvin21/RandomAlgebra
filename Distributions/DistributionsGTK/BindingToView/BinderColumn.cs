using System;
using System.Linq;
using Gtk;

namespace TableBinder
{
	public partial class Binder<T>
	{
		public void InitColumns()
		{
			for (int i = 0; i < _orderedVisible.Count; i++)
			{
				var data = _orderedVisible[i];
				var cell = data.GetCellRenderer();

				TreeViewColumn column = new TreeViewColumn();
				//column.Width = //TODO:widthattribute
				column.Resizable = true;
				column.Title = data.Title;
				column.PackStart(cell, true);
				column.SetCellDataFunc(cell, new TreeCellDataFunc(data.RenderValue));
				data.Renderer = cell;
				data.Column = column;
				View.AppendColumn(column);
			}
		}
	}
}
