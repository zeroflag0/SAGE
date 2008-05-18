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
// <file><id value="$Id$"/></file>

using System;
using System.Collections.Generic;
using System.Text;

using Sage.Modules;

namespace Sage.World
{
	public class Node : IEnumerable<Node>
	// : Sage.Data.Base<Node>
	{
		#region Features
		private FeatureContainer _Features;

		/// <summary>
		/// Features applied to this Node.
		/// </summary>
		public FeatureContainer Features
		{
			get { return _Features ?? (_Features = this.FeaturesCreate); }
		}

		/// <summary>
		/// Creates the default/initial value for Features.
		/// Features applied to this Node.
		/// </summary>
		protected virtual FeatureContainer FeaturesCreate
		{
			get { return new FeatureContainer(this); }
		}

		#endregion Features


		#region World Tree
		public virtual Tile TileNode
		{
			get { return this.Parent.TileNode; }
		}


		#region Parent

		private Node _Parent;

		/// <summary>
		/// This node's parent node.
		/// </summary>
		public Node Parent
		{
			get { return _Parent; }
			set
			{
				if (_Parent != value)
				{
					if (this._Parent != null)
					{
						this._Parent._Children.Remove(this);
					}
					this.OnParentChanged(_Parent, _Parent = value);

					if (this._Parent != null)
					{
						this._Parent._Children.Add(this);
					}
				}
			}
		}

		#region ParentChanged event
		public delegate void ParentChangedHandler(object sender, Node oldvalue, Node newvalue);

		private event ParentChangedHandler _ParentChanged;
		/// <summary>
		/// Occurs when Parent changes.
		/// </summary>
		public event ParentChangedHandler ParentChanged
		{
			add { this._ParentChanged += value; }
			remove { this._ParentChanged -= value; }
		}

		/// <summary>
		/// Raises the ParentChanged event.
		/// </summary>
		protected virtual void OnParentChanged(Node oldvalue, Node newvalue)
		{
			// if there are event subscribers...
			if (this._ParentChanged != null)
			{
				// call them...
				this._ParentChanged(this, oldvalue, newvalue);
			}
		}
		#endregion ParentChanged event
		#endregion Parent


		protected List<Node> _Children = new List<Node>();

		public IList<Node> Children
		{
			get
			{
				return this._Children;
			}
		}

		public virtual IEnumerator<Node> GetEnumerator()
		{
			return this.Children.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion World Tree

		#region AllowedChildren
		List<Type> _AllowedChildren;
		[System.ComponentModel.Browsable(false)]
		public List<Type> AllowedChildren
		{
			get { return _AllowedChildren ?? (_AllowedChildren = new List<Type>(this.AllowedChildrenCreate)); }
		}

		protected virtual Type[] AllowedChildrenCreate
		{
			get { return new Type[] { typeof(Node) }; }
		}
		#endregion AllowedChildren

		#region Name

		private string _Name = null;

		public string Name
		{
			get { return _Name; }
			set
			{
				if (_Name != value)
				{
					_Name = value;
				}
			}
		}

		static int _AutoCreateCount = 0;
		protected virtual string AutoCreateName
		{
			get { return this.GetType().Name + System.Threading.Interlocked.Increment(ref _AutoCreateCount); }
		}
		#endregion Name

		public Node()
			: base()
		{
		}

		#region Location

		private Vector _Location = new Vector();
		[System.ComponentModel.TypeConverter(typeof(Sage.Basics.VectorConverter))]
		[System.ComponentModel.Category("Position")]
		public Vector Location
		{
			get { return _Location; }
			set
			{
				if (_Location != value)
				{
					_Location = value;
				}
			}
		}
		#endregion Location

		#region Orientation

		private Quaternion _Orientation = new Quaternion();
		[System.ComponentModel.TypeConverter(typeof(Sage.Basics.QuaternionConverter))]
		[System.ComponentModel.Category("Position")]
		public Quaternion Orientation
		{
			get { return _Orientation; }
			set
			{
				if (_Orientation != value)
				{
					_Orientation = value;
				}
			}
		}
		#endregion Orientation

		public override string ToString()
		{
			return new StringBuilder(this.Name ?? this.GetType().Name).Append(" ").Append(this.Location).Append(this.Orientation).ToString();
		}
	}
}
