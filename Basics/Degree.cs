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

using Sage.Basics;

//using BaseType = org.ogre.OgreDotNet.Degree;
using real = System.Single;

namespace Sage
{
	public class Degree
		: Convertible<Degree>
	{
		#region Creation
		/// <summary>
		/// Create a radian from a value.
		/// </summary>
		/// <param name="value">A floating point value for the Radian.</param>
		public Degree(real value)
		{
			this.Value = value;
		}

		/// <summary>
		/// Create a radian from a value.
		/// </summary>
		/// <param name="radian">A floating point value for the Radian.</param>
		public Degree(Radian radian)
			: this(radian.ValueDegree)
		{
		}

		public Degree(object from)
			: base(from)
		{
		}

		public Degree()
			: this((real)0)
		{
		}

//		public static implicit operator Degree(BaseType from)
//		{
//			return new Degree(from);
//		}
//		public static implicit operator BaseType(Degree from)
//		{
//			return from.Base;
//		}

		public static implicit operator Degree(real from)
		{
			return new Degree(from);
		}
		public static implicit operator real(Degree from)
		{
			return from.ValueDegree;
		}

		#endregion

		#region Value
		real _Value = 0;

		[System.ComponentModel.Category("Value")]
		public real Value
		{
			get { return _Value; }
			set
			{
				if (_Value != value)
				{
					_Value = value;
				}
			}
		}
		#endregion

		public real ValueDegree
		{
			get
			{
				return this.Value;
			}
		}

		public real ValueRadian
		{
			get
			{
				return this.Value / 180f;
			}
		}
	}
}
