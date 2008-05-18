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

//using BaseType = OgreDotNet.Radian;
using real = System.Single;

namespace Sage
{
	public class Radian
		: Convertible<Radian>
	{
		#region Creation
		/// <summary>
		/// Create a radian from a value.
		/// </summary>
		/// <param name="value">A floating point value for the Radian.</param>
		/// <param name="isDegree">Whether the value should be interpreted as degree(true) or as radian(false).</param>
		public Radian(float value, bool isDegree)
			: this((isDegree ? value / 180f : value))
		{
		}

		/// <summary>
		/// Create a radian from a value.
		/// </summary>
		/// <param name="radian">A floating point value for the Radian.</param>
		public Radian(float radian)
		{
			this.Value = radian;
		}

		public Radian(object from)
			: base(from)
		{
		}

		public Radian()
			: this(0)
		{
		}

		public static implicit operator Radian(real from)
		{
			return new Radian(from);
		}
		public static implicit operator real(Radian from)
		{
			return from.ValueRadian;
		}

		public static implicit operator Radian(Degree from)
		{
			return new Radian(from);
		}
		public static implicit operator Degree(Radian from)
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


		public static Radian operator -(Radian r)
		{
			return -r;
		}

		public static Radian operator -(Radian l, Radian r)
		{
			return l - r;
		}

		#region != & ==
		public static bool operator !=(Radian l, Radian r)
		{
			return l.ValueRadian != r.ValueRadian;
		}
		public static bool operator ==(Radian l, Radian r)
		{
			return l.ValueRadian == r.ValueRadian;
		}
		public static bool operator !=(Radian l, Degree r)
		{
			return l.ValueRadian != r.ValueRadian;
		}
		public static bool operator ==(Radian l, Degree r)
		{
			return l.ValueRadian == r.ValueRadian;
		}
		public static bool operator !=(Degree l, Radian r)
		{
			return l.ValueRadian != r.ValueRadian;
		}
		public static bool operator ==(Degree l, Radian r)
		{
			return l.ValueRadian == r.ValueRadian;
		}
		public static bool operator !=(float l, Radian r)
		{
			return l != r.ValueRadian;
		}
		public static bool operator ==(float l, Radian r)
		{
			return l == r.ValueRadian;
		}
		public static bool operator !=(Radian l, float r)
		{
			return l.ValueRadian != r;
		}
		public static bool operator ==(Radian l, float r)
		{
			return l.ValueRadian == r;
		}
		#endregion != & ==

		public static Radian operator *(Radian rad, float f)
		{
			return new Radian(rad.Value * f);
		}
		public static Radian operator *(float f, Radian rad)
		{
			return new Radian(rad.Value * f);
		}
		public static Radian operator *(Radian l, Radian f)
		{
			return new Radian(l.ValueRadian * f.ValueRadian);
		}
		public static Radian operator /(real f, Radian r)
		{
			return new Radian(f / r.ValueRadian);
		}
		public static Radian operator /(Radian r, real f)
		{
			return new Radian(r.ValueRadian * f);
		}

		public static Radian operator +(Radian l, Radian r)
		{
			return new Radian( l.ValueRadian + r.ValueRadian);
		}
		public static bool operator <(Radian l, Radian r)
		{
			return l.ValueRadian < r.ValueRadian;
		}
		public static bool operator <=(Radian l, Radian r)
		{
			return l.ValueRadian <= r.ValueRadian;
		}
		public static bool operator >(Radian l, Radian r)
		{
			return l.ValueRadian > r.ValueRadian;
		}
		public static bool operator >=(Radian l, Radian r)
		{
			return l.ValueRadian >= r.ValueRadian;
		}

//		public real ValueAngleUnits
//		{
//			get
//			{
//				return this;
//			}
//		}
		public real ValueDegree
		{
			get
			{
				return this.Value * 180f;
			}
		}

		public real ValueRadian
		{
			get
			{
				return this.Value;
			}
		}

		public int CompareTo(Radian other)
		{
			return this.ValueRadian.CompareTo(other.ValueRadian);
		}
		public bool Equals(Radian other)
		{
			return this.ValueRadian.Equals(other.ValueRadian);
		}

		#region Operators



		public static Radian operator -(Radian rad, Degree deg)
		{
			return new Radian(rad.Value - deg.ValueRadian);
		}


		#endregion
	}
}
