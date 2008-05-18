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

namespace Sage.Physics
{
	/// <summary>
	/// Pysical Mass
	/// </summary>
	public abstract class Mass
	{
		//NOTE: public extern without references are a BAD idea! breaks reflection...
		//public extern Mass();

		/// <summary>
		/// Gets or sets the Weight in Kilograms.
		/// </summary>
		public abstract float Weight { get; set; }

		/// <summary>
		/// Gets or sets the center of gravity.
		/// </summary>
		public abstract Vector CenterOfGravity { get; set; }

		/// <summary>
		/// Set the mass parameters to represent a sphere of the given radius and density, with the center of mass at (0,0,0) relative to the body. 
		/// </summary>
		/// <param name="density">dansity</param>
		/// <param name="radius">radius</param>
		/// <returns></returns>
		//public abstract bool SetMassSphere(float density, float radius);
	}
}
