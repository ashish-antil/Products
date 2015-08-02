using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Collections;


namespace RecipeGUI
{

#if used
	
	/// <summary>
	/// Utilities to make working with menus easier
	/// </summary>
	public static class MenuHandling
	{

		public static List<ToolStripMenuItem> GetMenuItems(ToolStripItemCollection menu, ToolStripItem start)
		{
			if (start == null) start = menu[0];
			int startIndex = menu.IndexOf(start);

			var list = new List<ToolStripMenuItem>();
			for (int i = startIndex; i < menu.Count && !(menu[i] is ToolStripSeparator); i++)
			{
				var mi = menu[i] as ToolStripMenuItem;
				if (mi != null) list.Add(mi);
			}
			return list;
		}
	}

	/// <summary>
	/// Controls a number of radio-button (i.e. checkable) menu items and makes sure that only
	/// one is checked at any time.
	/// </summary>
	public class OptionMenuController
	{
		/// <summary>
		/// Only called for the new checked item each time the check changes.
		/// </summary>
		public EventHandler Selected;

		/// <summary>
		/// Checkable menu items managed by an instance of this class.
		/// </summary>
		private ToolStripMenuItem[] _Items;


		/// <summary>
		/// Create a controller for the menu items. Skip any items that are not of type ToolStripMenuItem
		/// and stop at the first ToolStripSeparator equal to or after 'start'.
		/// </summary>
		/// <param name="menu">The collection of menu items</param>
		/// <param name="start">item to start considering for inclusion in the radio button set</param>
		/// <example><code>
		/// new OptionMenuController(_TestMnu.DropDownItems, _StartMni).Selected += new EventHandler(Hello);
		/// </code></example>
		public OptionMenuController(ToolStripMenuItem menu, ToolStripMenuItem start)
		{
			ToolStripItemCollection menuItems = menu.DropDownItems;
			var list = MenuHandling.GetMenuItems(menuItems, start);
			list.ForEach(item => item.Click += new EventHandler(ItemClick));
			_Items = list.ToArray();
		}

		private void ItemClick(object sender, EventArgs e)
		{
			Checked = (ToolStripMenuItem)sender;
		}

		public void SetChecked(string tag)
		{
			foreach (ToolStripMenuItem item in _Items)
			{
				if (tag.Equals(item.Tag))
				{
					item.Checked = true;
					break;
				}
			}
		}

		public ToolStripMenuItem Checked
		{
			get
			{
				return _Items.First(item => item.Checked);
			}
			set
			{
				foreach (ToolStripMenuItem item in _Items)
				{
					item.Checked = (item == value);
					if (item.Checked && Selected != null) Selected(item, EventArgs.Empty);
				}
			}
		}
	}
#endif

	public class MostRecentlyUsedHandler : IEnumerable<MostRecentlyUsedHandler.MruItem>
	{
		/// <summary>
		/// Only called for the new selected item.
		/// </summary>
		public EventHandler Do;

		private int _Maximum;
		private ToolStripMenuItem _Menu;
		private int _StartIndex;

		public MostRecentlyUsedHandler(ToolStripMenuItem menu, ToolStripItem start, int max, EventHandler eh)
		{
			_Menu = menu;
			int startIndex = start == null ? 0 : _Menu.DropDownItems.IndexOf(start);
			if (startIndex < _Menu.DropDownItems.Count && _Menu.DropDownItems[startIndex] is ToolStripSeparator) startIndex++;
			_StartIndex = startIndex;
			_Maximum = max;
			Do = eh;
		}

		private void ItemClick(object sender, EventArgs e)
		{
			var item = (MruItem)sender;
			SetMruItem(DateTime.UtcNow, item.Text, item.Tag).Font = item.Font;
			Do(item, e);
		}

		public MruItem SetMruItem(DateTime utcLastUsed, string text, object tag)
		{
			var newItem = new MruItem(this, utcLastUsed, text, tag);
			ToolStripItemCollection items = _Menu.DropDownItems;
			if (_StartIndex >= items.Count || items[_StartIndex] is ToolStripSeparator)
			{
				items.Insert(_StartIndex, newItem);
			}
			else
			{
				bool inserted = false;
				int endIndex = _StartIndex + _Maximum;
				for (int index = _StartIndex; index < items.Count && !(items[index] is ToolStripSeparator); index++)
				{
					var item = (MruItem)items[index];
					if (item.Text == text)
					{
						items.RemoveAt(index);
						newItem = SetMruItem(utcLastUsed, item.Text, item.Tag);
						break;
					}
					if (!inserted && utcLastUsed > item.LastUsed)
					{
						items.Insert(index, newItem);
						inserted = true;
					}
					if (index == endIndex)
					{
						((MruItem)items[index]).Click -= ItemClick;
						items.RemoveAt(index);
						break;
					}
				}
			}
			return newItem;
		}



		public class MruItem : ToolStripMenuItem
		{
			private MostRecentlyUsedHandler _Handler;
			internal DateTime LastUsed;

			internal MruItem(MostRecentlyUsedHandler handler, DateTime lastUsed, string name, object obj)
				: base(name, null, handler.ItemClick)
			{
				_Handler = handler;
				LastUsed = lastUsed;
				Tag = obj;
			}
		}


		#region IEnumerable<MruItem> Members

		public IEnumerator<MostRecentlyUsedHandler.MruItem> GetEnumerator()
		{
			int i = _StartIndex;
			MostRecentlyUsedHandler.MruItem item;
			while (i < _Menu.DropDownItems.Count && (item = _Menu.DropDownItems[i] as MostRecentlyUsedHandler.MruItem) != null)
			{
				yield return item;
				i++;
			}

		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}

#if USED
	public class TriStateButtonController
	{
		public TriStateButtonController(params ToolStripButton[] buttons)
		{
			foreach (ToolStripButton button in buttons)
			{
				button.Click += new EventHandler(Button_Click);
			}
		}

		private void Button_Click(object sender, EventArgs e)
		{
			var button = (ToolStripButton)sender;
			if (button.CheckState == CheckState.Checked)
			{
				button.CheckState = CheckState.Unchecked;
				button.Image = DataAdminTool.Properties.Resources.open_minus_;
			}
			else if (button.CheckState == CheckState.Unchecked)
			{
				button.CheckState = CheckState.Indeterminate;
				button.Image = DataAdminTool.Properties.Resources.pressed_dot_;
			}
			else
			{
				button.CheckState = CheckState.Checked;
				button.Image = DataAdminTool.Properties.Resources.closed_plus_;
			}
		}
	}


	public class ToolStripComboController
	{
		public ToolStripComboController(params ToolStripComboBox[] boxes)
		{
			foreach (ToolStripComboBox box in boxes) box.MouseDown += new MouseEventHandler(MouseDown);
		}

		private void MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right) ((ToolStripComboBox)sender).SelectedIndex = -1;
		}

	}
#endif
}
