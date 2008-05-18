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

namespace Sage.Basics
{
	public class Parameters : IDictionary<string, string>
	{
		Dictionary<string, string> _Items = new Dictionary<string, string>();

		public Dictionary<string, string> Items
		{
			get { return _Items; }
			set { _Items = value; }
		}

		#region IDictionary<string,string> Members

		public void Add(string key, string value)
		{
			this.Items.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return this.Items.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return this.Items.Keys; }
		}

		public bool Remove(string key)
		{
			return this.Items.Remove(key);
		}

		public bool TryGetValue(string key, out string value)
		{
			return this.Items.TryGetValue(key, out value);
		}

		public ICollection<string> Values
		{
			get { return this.Items.Values; }
		}

		public string this[string key]
		{
			get
			{
				return this.Items[key];
			}
			set
			{
				this.Items[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,string>> Members

		public void Add(KeyValuePair<string, string> item)
		{
			this.Items.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			this.Items.Clear();
		}

		public bool Contains(KeyValuePair<string, string> item)
		{
			return this.Items.ContainsKey(item.Key) && this.Items[item.Key] == item.Value;
		}

		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return this.Items.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<string, string> item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,string>> Members

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion


		public Parameters()
		{
		}

		public Parameters(params string[] parameters)
		{
			this.Add(parameters);
		}

		public Parameters(IEnumerable<string> parameters)
		{
			this.Add(parameters);
		}

		public Parameters(IEnumerable<KeyValuePair<string,string>> parameters)
		{
			this.Add(parameters);
		}

		public Parameters(string key, string value)
		{
			this.Add(key, value);
		}

		public void Add(IEnumerable<string> parameters)
		{
			int i = 0;
			string key = null;
			foreach (string param in parameters)
			{
				if (i % 2 == 0)
				{
					key = param;
				}
				else
				{
					this.Add(key, param);
				}
				i++;
			}
		}

		public void Add(IEnumerable<KeyValuePair<string,string>> parameters)
		{
			foreach(KeyValuePair<string,string> param in parameters)
			{
				this.Add(param.Key, param.Value);
			}
		}


		public static implicit operator Parameters(string[] parameters)
		{
			return new Parameters(parameters);
		}

		public static implicit operator Parameters(Parameters[] parameters)
		{
			Parameters value = new Parameters();
			foreach(Parameters param in parameters)
			{
				value.Add(param);
			}
			return value;
		}
	}
}
