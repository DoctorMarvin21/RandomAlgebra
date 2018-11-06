using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gtk;

namespace TableBinder
{
	public partial class Binder<T>
	{
		TreeStore _store;
		List<T> _data = new List<T>();

		public Binder(TreeView view)
		{
			_store = new TreeStore(typeof(T));
			View = view;


			InitStore();
			InitColumns();

			View.Model = _store;
		}

		public TreeView View
		{
			get;
		}

		public void AddValue(T value)
		{
			_store.AppendValues(value);
			_data.Add(value);
		}

		public T[] GetData()
		{
			return _data.ToArray();
		}

		public void RemoveCurrent()
		{
			//TODO:
			throw new NotImplementedException();
		}

		public void Clear()
		{
			_store.Clear();
		}
	}
}
