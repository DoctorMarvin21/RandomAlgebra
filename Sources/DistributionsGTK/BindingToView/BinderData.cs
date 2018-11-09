using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gtk;

namespace TableBinder
{
	public partial class Binder<T>
	{
		List<ParsedData> _orderedVisible;


		public void InitStore()
		{

			var type = typeof(T);

			var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			                .Where(x => x.CanRead).ToList();

			List<ParsedData> parsed = new List<ParsedData>();

			foreach (var prop in props)
			{
				var data = new ParsedData(prop, _store, View);
				parsed.Add(data);
			}

			var ordered = parsed.Where(x => x.Ordinal >= 0 && x.Visible).OrderBy(x => x.Ordinal).ToList();
			ordered.AddRange(parsed.Where(x => x.Visible && x.Ordinal < 0));

			_orderedVisible = ordered;
		}

		private class ParsedData
		{
			string[] _values = null;
			TreeView _view = null;

			public ParsedData(PropertyInfo info, TreeStore store, TreeView owner)
			{
				Store = store;
				Info = info;
				_view = owner;
				ValueType = info.PropertyType;
				_values = info.GetCustomAttribute<BinderComboBoxAttribute>()?.GetValues();
				Visible = info.GetCustomAttribute<BinderHiddenAttribute>() == null;
				ReadOnly = !info.CanWrite || info.GetCustomAttribute<BinderReadonlyAttribute>(true) != null;
				Title = info.GetCustomAttribute<BinderTitleAttribute>(true)?.Title ?? info.Name;
				Ordinal = info.GetCustomAttribute<BinderOrdinalAttribute>(true)?.Ordinal ?? -1;
			}

			public string GetValue(T data)
			{
				return Info.GetValue(data)?.ToString();
			}

			public void SetValue(T data, string value)
			{
				try
				{
					var converted = Convert.ChangeType(value, ValueType);
					Info.SetValue(data, converted);
				}
				catch
				{
					
				}
			}

			public bool Visible
			{
				get;
			}

			public int Ordinal
			{
				get;
			}

			public bool ReadOnly
			{
				get;
			}

			public string Title
			{
				get;
			}

			public PropertyInfo Info
			{
				get;
			}

			public TreeStore Store
			{
				get;
			}

			public CellRendererText GetCellRenderer()
			{
				CellRendererText renderer = null;
				if (_values != null)
				{
					var combo = new CellRendererCombo();
					combo.TextColumn = 0;
					ListStore store = new ListStore(typeof(string));
					for (int i = 0; i < _values.Length; i++)
					{
						store.AppendValues(_values[i]);
					}

					combo.Model = store;
					renderer = combo;
				}
				else
				{
					renderer = new CellRendererText();
				}
				renderer.Editable = !ReadOnly;
				renderer.Edited += Edited;

				return renderer;
			}

			public void Edited(object sender, EditedArgs e)
			{
				TreeIter iter;

				if (Store.GetIterFromString(out iter, e.Path))
				{
					var obj = (T)Store.GetValue(iter, 0);
					SetValue(obj, e.NewText);
				}
			}


			public void RenderValue(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
			{
				var obj = (T)model.GetValue(iter, 0);
				string value = GetValue(obj) ?? string.Empty;

				Renderer.Text = value;
			}

			//TODO:Converter
			public Type ValueType
			{
				get;
			}

			public CellRendererText Renderer
			{
				get;
				set;
			}
			public TreeViewColumn Column
			{
				get;
				set;
			}
		}
	}
}
