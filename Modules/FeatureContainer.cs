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

namespace Sage.Modules
{
	public class FeatureContainer : IEnumerable<IFeature>
	{
		Sage.World.Node _Owner;

		public Sage.World.Node Owner
		{
			get { return _Owner; }
			set { _Owner = value; }
		}

		private List<IFeature> _Items = new List<IFeature>();

		protected List<IFeature> Items
		{
			get { return _Items; }
		}

		private Dictionary<Module, List<IFeature>> _ModuleLookup = new Dictionary<Module, List<IFeature>>();

		protected Dictionary<Module, List<IFeature>> ModuleLookup
		{
			get { return _ModuleLookup; }
		}

		/// <summary>
		/// Create a feature container for a node.
		/// </summary>
		/// <param name="owner"></param>
		public FeatureContainer(Sage.World.Node owner)
		{
			this.Owner = owner;
		}

		/// <summary>
		/// Adds a feature to the node.
		/// </summary>
		/// <param name="feature">The feature to add.</param>
		public void Add(IFeature feature)
		{
			if (feature != null && !this.Items.Contains(feature))
			{
				this.Items.Add(feature);

				if (feature is IMoveFeature)
				{
					this.MoveFeature = feature as IMoveFeature;
				}
			}
		}

		/// <summary>
		/// Removes a feature from the node.
		/// </summary>
		/// <param name="feature">The feature to remove.</param>
		public void Remove(IFeature feature)
		{
			if (feature == this.MoveFeature)
			{
				this.MoveFeature = null;
			}
			if (this.Items.Contains(feature))
			{
				this.Items.Remove(feature);
			}
			if (this.ModuleLookup.ContainsKey(feature.Module))
			{
				this.ModuleLookup[feature.Module].Remove(feature);
			}
		}

		/// <summary>
		/// Removes all features from the node.
		/// </summary>
		public void Clear()
		{
			foreach (IFeature feature in this.Items)
			{
				this.Remove(feature);
			}
			this.MoveFeature = null;
		}

		IMoveFeature _MoveFeature = null;
		/// <summary>
		/// The feature which is registered to handle movement request. (Usually the graphics feature)
		/// </summary>
		public IMoveFeature MoveFeature
		{
			get
			{
				return this._MoveFeature;
			}
			protected set
			{
				this._MoveFeature = value;
			}
		}


		/// <summary>
		/// Find a featuere by it's type. (performs an "as" conversion so derived classes also return)
		/// </summary>
		/// <typeparam name="T">The type to find.</typeparam>
		/// <returns>The feature found converted to T, or null if nothing was found.</returns>
		public T Find<T>()
			where T : class, IFeature
		{
			T value = null;
			foreach (IFeature feature in this.Items)
			{
				value = feature as T;

				if (value != null)
				{
					break;
				}
			}
			return value;
		}

		/// <summary>
		/// Find features by their providing module.
		/// </summary>
		/// <param name="module">The module which provided the features.</param>
		/// <returns>An array of features.</returns>
		public IFeature[] this[Module module]
		{
			get
			{
				if (!this.ModuleLookup.ContainsKey(module))
					return new IFeature[0];

				return this.ModuleLookup[module].ToArray();
			}
		}


		#region IEnumerable<IFeature> Members

		public IEnumerator<IFeature> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion

		#endregion

		public bool Contains(IFeature feature)
		{
			return this.Items.Contains(feature);
		}
	}
}
