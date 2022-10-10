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


namespace Sage.Graphics.Ogre.Converters
{
	public class Quaternion : Sage.Basics.QuaternionConverter<org.ogre.Quaternion>
	{
		protected override org.ogre.Quaternion ConvertFromSage(Sage.Quaternion from)
		{
			return new org.ogre.Quaternion(from.W, from.X, from.Y, from.Z);
		}

		protected override Sage.Quaternion ConvertToSage(org.ogre.Quaternion from)
		{
			return new Sage.Quaternion(from.y, from.x, from.y, from.z);
		}
	}
}
