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
	/// <summary>
	/// MoveFeatures provide functionality for moving/transforming a scene node.
	/// This functionality is usually provided by the graphics engine's scene graph.
	/// </summary>
	/// <typeparam name="ModuleType">The module that provides the feature.</typeparam>
	public abstract class MoveFeature<ModuleType>
		:
		Feature<ModuleType>,
		IMoveFeature
		where ModuleType : Module
	{
		/// <summary>
		/// Perform a rotation.
		/// </summary>
		/// <param name="w">The object's new orientation.</param>
		/// <param name="x">The object's new orientation.</param>
		/// <param name="y">The object's new orientation.</param>
		/// <param name="z">The object's new orientation.</param>
		public virtual void Rotate(double w, double x, double y, double z)
		{
		}

		/// <summary>
		/// Perform a rotation.
		/// </summary>
		/// <param name="q">The object's new orientation.</param>
		public abstract void Rotate(Quaternion q);

		/// <summary>
		/// Perform a translation/move.
		/// </summary>
		/// <param name="x">The object's new location.</param>
		/// <param name="y">The object's new location.</param>
		/// <param name="z">The object's new location.</param>
		public virtual void Translate(double x, double y, double z)
		{
		}

		/// <summary>
		/// Perform a translation/move.
		/// </summary>
		/// <param name="v">The object's new location.</param>
		public abstract void Translate(Vector v);

		/// <summary>
		/// Scale.
		/// </summary>
		/// <param name="x">The object's new scale.</param>
		/// <param name="y">The object's new scale.</param>
		/// <param name="z">The object's new scale.</param>
		public virtual void Scale(double x, double y, double z)
		{
		}

		/// <summary>
		/// Scale.
		/// </summary>
		/// <param name="v">The object's new scale.</param>
		public abstract void Scale(Vector v);
	}
}
