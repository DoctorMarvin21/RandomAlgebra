using System;
using Gtk;

namespace TableBinder
{
	public class BinderReadonlyAttribute : Attribute
	{
		public BinderReadonlyAttribute()
		{
		}
	}

	public class BinderHiddenAttribute : Attribute
	{
		public BinderHiddenAttribute()
		{
		}

	}

	public class BinderOrdinalAttribute : Attribute
	{
		public BinderOrdinalAttribute(int order)
		{
			Ordinal = order;
		}

		public int Ordinal
		{
			get;
		}
	}

	public class BinderTitleAttribute : Attribute
	{
		public BinderTitleAttribute(string text)
		{
			Title = text;
		}

		public string Title
		{
			get;
		}
	}

	public abstract class BinderComboBoxAttribute : Attribute
	{
		public BinderComboBoxAttribute()
		{
			
		}

		public abstract string[] GetValues();
	}
}
