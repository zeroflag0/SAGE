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

using real = System.Single;

namespace Sage.Basics
{
	public interface IVector<Vector>
		where Vector : IVector<Vector>
	{
		bool IsZeroLength { get; }
		real Length { get; }
		Vector NormalisedCopy { get; }
		Vector Perpendicular { get; }
		real SquaredLength { get; }

		real this[int i] { get; set; }

		Vector CrossProduct(Vector other);
		real DotProduct(Vector other);
		Quaternion GetRotationTo(Vector dest);
		Quaternion GetRotationTo(Vector dest, Vector fallbackAxis);
		void MakeCeil(Vector cmp);
		void MakeFloor(Vector cmp);
		Vector MidPoint(Vector vec);
		Vector Normalise();
		Vector RandomDeviant(Radian angle);
		Vector RandomDeviant(Radian angle, Vector up);
		Vector Reflect(Vector normal);

		bool DirectionEquals(Vector other, Radian tolerance);
		bool PositionEquals(Vector other);
		bool PositionEquals(Vector other, real tolerance);

		bool Equals(Vector other);
	}
}
