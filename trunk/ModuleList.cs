#region LGPL License
//********************************************************************
//	author:         Thomas "zeroflag" Kraemer
//	author email:   zeroflag@zeroflag.de
//	
//	Copyright (C) 2006-2008  Thomas "zeroflag" Kraemer
//	
//	license:	(LGPL)
//	
//		This library is free software; you can redistribute it and/or
//		modify it under the terms of the GNU Lesser General Public
//		License as published by the Free Software Foundation; either
//		version 2.1 of the License, or (at your option) any later version.
//
//		This library is distributed in the hope that it will be useful,
//		but WITHOUT ANY WARRANTY; without even the implied warranty of
//		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//		Lesser General Public License for more details.
//
//		You should have received a copy of the GNU Lesser General Public
//		License along with this library; if not, write to the Free Software
//		Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//
//		http://www.gnu.org/licenses/lgpl.html#TOC1
//
//*********************************************************************
#endregion LGPL License
// <file><id value="$Id$"/></file>

using System;
using System.Collections.Generic;
using System.Text;

using Sage.Modules;
namespace Sage
{
	public class ModuleList : ICollection<Modules.Module>, IDisposable
	{
		List<Modules.Module> _Items = new List<Sage.Modules.Module>();

		protected List<Modules.Module> Items
		{
			get { return _Items; }
		}

		Core _Core;

		public Core Core
		{
			get { return _Core; }
			set { _Core = value; }
		}

		public Module this[int i]
		{
			get
			{
				return this.Items[i];
			}
		}

		public T Get<T>()
			where T : Module
		{
			T item = null;
			foreach (Module module in this)
			{
				item = module as T;
				if (item != null)
					return item;
			}
			return null;
		}


		#region ICollection<Module> Members

		public void AddRange(IEnumerable<Sage.Modules.Module> items)
		{
			foreach (Sage.Modules.Module item in items)
			{
				this.Add(item);
			}
		}

		public void Add(Sage.Modules.Module item)
		{
			this.Items.Add(item);
			item.Core = this.Core;
		}

		public void Clear()
		{
			this.Items.Clear();
		}

		public bool Contains(Sage.Modules.Module item)
		{
			return this.Items.Contains(item);
		}

		public void CopyTo(Sage.Modules.Module[] array, int arrayIndex)
		{
			this.Items.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this.Items.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(Sage.Modules.Module item)
		{
			return this.Items.Remove(item);
		}

		#endregion

		#region IEnumerable<Module> Members

		public IEnumerator<Sage.Modules.Module> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			foreach (Module module in this)
			{
				if (module.ScheduleState != ScheduleState.Shutdown && module.ScheduleState != ScheduleState.Disposed)
					module.ScheduleState = ScheduleState.Shutdown;
			}
		}

		#endregion
	}
}
