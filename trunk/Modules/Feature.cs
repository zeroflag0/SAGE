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

namespace Sage.Modules
{
	/// <summary>
	/// A basic feature to be hooked into sage's world graph nodes.
	/// </summary>
	/// <typeparam name="T">The module that provides the feature.</typeparam>
	public abstract class Feature<T> : Sage.Modules.IFeature<T>, Sage.Modules.IFeature
		where T : Module
	{
		private Sage.World.Node _Node;
		/// <summary>
		/// The node to which the feature belongs.
		/// </summary>
		public Sage.World.Node Node
		{
			get { return _Node; }
			set
			{
				if (_Node != value)
				{
					if (_Node != null)
					{
						this._Node.Features.Remove(this);
					}
					_Node = value;
					if (_Node != null)
					{
						this._Node.Features.Add(this);
					}
				}
			}
		}

		private T _Module;
		/// <summary>
		/// The module that provided the feature.
		/// </summary>
		public T Module
		{
			get { return _Module; }
			set { _Module = value; }
		}

		#region IFeature Members

		/// <summary>
		/// The module that provided the feature.
		/// </summary>
		Module IFeature.Module
		{
			get { return this.Module; }
			set { this.Module = (T)value; }
		}

		#endregion

		public Feature()
		{
		}

		public Feature(Sage.World.Node owner)
		{
			this.Node = owner;
		}


		/// <summary>
		/// Notification when the feature's object was rotated.
		/// </summary>
		/// <param name="w">The object's new orientation.</param>
		/// <param name="x">The object's new orientation.</param>
		/// <param name="y">The object's new orientation.</param>
		/// <param name="z">The object's new orientation.</param>
		public virtual void OnRotate(double w, double x, double y, double z)
		{
		}

		/// <summary>
		/// Notification when the feature's object was rotated.
		/// </summary>
		/// <param name="q">The object's new orientation.</param>
		public virtual void OnRotate(Quaternion q)
		{
			this.OnRotate(q.W, q.X, q.Y, q.Z);
		}

		/// <summary>
		/// Notification when the feature's object was moved.
		/// </summary>
		/// <param name="x">The object's new location.</param>
		/// <param name="y">The object's new location.</param>
		/// <param name="z">The object's new location.</param>
		public virtual void OnMove(double x, double y, double z)
		{
		}

		/// <summary>
		/// Notification when the feature's object was moved.
		/// </summary>
		/// <param name="v">The object's new location.</param>
		public virtual void OnMove(Vector v)
		{
			this.OnMove(v.X, v.Y, v.Z);
		}

		/// <summary>
		/// Notification when the feature's object was scaled.
		/// </summary>
		/// <param name="x">The object's new scale.</param>
		/// <param name="y">The object's new scale.</param>
		/// <param name="z">The object's new scale.</param>
		public virtual void OnScale(double x, double y, double z)
		{
		}

		/// <summary>
		/// Notification when the feature's object was scaled.
		/// </summary>
		/// <param name="v">The object's new scale.</param>
		public virtual void OnScale(Vector v)
		{
			this.OnScale(v.X, v.Y, v.Z);
		}


		public Type ModuleType
		{
			get { return typeof(T); }
		}

	}
}
