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

//using BaseType = Vector3;
using real = System.Single;

namespace Sage
{
	public class Vector
		: Convertible<Vector>, IVector<Vector>
	{
		#region Constants
		public static Vector NEGATIVE_UNIT_X = new Vector(-1.0f, 0.0f, 0.0f);
		public static Vector NEGATIVE_UNIT_Y = new Vector(0.0f, -1.0f, 0.0f);
		public static Vector NEGATIVE_UNIT_Z = new Vector(0.0f, 0.0f, -1.0f);
		public static Vector UNIT_SCALE = new Vector(1.0f, 1.0f, 1.0f);
		public static Vector UNIT_X = new Vector(1.0f, 0.0f, 0.0f);
		public static Vector UNIT_Y = new Vector(0.0f, 1.0f, 0.0f);
		public static Vector UNIT_Z = new Vector(0.0f, 0.0f, 1.0f);
		public static Vector ZERO = new Vector(0.0f, 0.0f, 0.0f);

		public enum ValueIndices : int
		{
			X = 0,
			Y = 1,
			Z = 2,
		};

		#endregion Constants

		#region Creation
		/// <summary>
		/// Create a Sage vector from a base vector.
		/// </summary>
		/// <param name="from"></param>
		public Vector(object from)
			: base(from)
		{
		}

		public Vector(real[] values)
		{
			this.X = values[0];
			this.Y = values[1];
			this.Z = values[2];
		}

		public Vector(real x, real y, real z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		public Vector()
		{
		}

		#endregion Creation


		#region Value
		real[] _Values = new real[3];
		[System.ComponentModel.Browsable(false)]
		public real[] Values
		{
			get { return _Values; }
			set
			{
				if (_Values != value)
				{
					_Values = value;
					this.Base = null;
				}
			}
		}

		#endregion

		#region Interface
		[System.ComponentModel.Browsable(false)]
		public real this[int i]
		{
			get
			{
				return this._Values[i];
			}
			set
			{
				if (this._Values[i] != value)
				{
					this._Values[i] = value;
					this.Base = null;
				}
			}
		}
		[System.ComponentModel.Category("Value")]
		public real X
		{
			get { return this[(int)ValueIndices.X]; }
			set
			{
				if (this[(int)ValueIndices.X] != value)
				{
					this[(int)ValueIndices.X] = value;
					this.Base = null;
				}
			}
		}

		[System.ComponentModel.Category("Value")]
		public real Y
		{
			get { return this[(int)ValueIndices.Y]; }
			set
			{
				if (this[(int)ValueIndices.Y] != value)
				{
					this[(int)ValueIndices.Y] = value;
					this.Base = null;
				}
			}
		}

		[System.ComponentModel.Category("Value")]
		public real Z
		{
			get { return this[(int)ValueIndices.Z]; }
			set
			{
				if (this[(int)ValueIndices.Z] != value)
				{
					this[(int)ValueIndices.Z] = value;
					this.Base = null;
				}
			}
		}

		#endregion Interface

		#region IVector<Vector> Members
		[System.ComponentModel.Browsable(false)]
		public bool IsZeroLength
		{
			get
			{
				return X * X + Y * Y + Z * Z >= 1E-12;
			}
		}

		public real Length
		{
			get
			{
				return (real)Math.Sqrt(X * X + Y * Y + Z * Z);
			}
		}
		[System.ComponentModel.Browsable(false)]
		public Vector NormalisedCopy
		{
			get
			{
				return new Vector(this.Values).Normalise();
			}
		}
		[System.ComponentModel.Browsable(false)]
		public Vector Perpendicular
		{
			get
			{
				Vector vector = this.CrossProduct(UNIT_X);
				if (vector.X * vector.X + Y * Y + Z * Z < 1E-12f)
				{
					vector = this.CrossProduct(UNIT_Y);
				}
				return vector;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public real SquaredLength
		{
			get
			{
				return X * X + Y * Y + Z * Z;
			}
		}

		public Vector CrossProduct(Vector other)
		{
			Vector vector = new Vector();
			vector.X = (this.Y * other.Z) - (this.Z * other.Y);
			vector.Y = (this.Z * other.X) - (this.X * other.Z);
			vector.Z = (this.X * other.Y) - (this.Y * other.X);
			return vector;
		}


		public real DotProduct(Vector other)
		{
			return this.Z * other.Z + this.Y * other.Y + this.X * other.X;
		}

		public Quaternion GetRotationTo(Vector dest)
		{
			return this.GetRotationTo(dest, ZERO);
		}

		public Quaternion GetRotationTo(Vector dest, Vector fallbackAxis)
		{
			Quaternion quaternion = new Quaternion();
			Vector temp = this.NormalisedCopy;
			Vector result = dest.NormalisedCopy;

			real dot = result.DotProduct(temp);
			if (dot >= 1f)
			{
				return Quaternion.IDENTITY;
			}
			real num2 = (real)Math.Sqrt((dot + 1f) * 2f);
			if (num2 < 1E-06f)
			{
				Radian pi = new Radian((real)Math.PI);
				if (fallbackAxis != ZERO)
				{
					quaternion.FromAngleAxis(pi, fallbackAxis);
					return quaternion;
				}
				result = UNIT_X.CrossProduct(this);
				if (result.IsZeroLength)
				{
					result = UNIT_Y.CrossProduct(this);
				}
				result.Normalise();
				quaternion.FromAngleAxis(pi, result);
				return quaternion;
			}
			real num = 1f / num2;
			temp = temp.CrossProduct(result);
			quaternion.X = temp.X * num;
			quaternion.Y = temp.Y * num;
			quaternion.Z = temp.Z * num;
			quaternion.W = num2 * 0.5f;
			quaternion.Normalise();
			return quaternion;
		}

		public void MakeCeil(Vector cmp)
		{
			if (cmp.X > this.X)
			{
				this.X = cmp.X;
			}
			if (cmp.Y > this.Y)
			{
				this.Y = cmp.Y;
			}
			if (cmp.Z > this.Z)
			{
				this.Z = cmp.Z;
			}
		}

		public void MakeFloor(Vector cmp)
		{
			if (cmp.X < this.X)
			{
				this.X = cmp.X;
			}
			if (cmp.Y < this.Y)
			{
				this.Y = cmp.Y;
			}
			if (cmp.Z < this.Z)
			{
				this.Z = cmp.Z;
			}
		}

		public Vector MidPoint(Vector vec)
		{
			return new Vector((this.X + vec.X) * 0.5f,
				(this.Y + vec.Y) * 0.5f,
				(this.Z + vec.Z) * 0.5f);
		}

		public Vector Normalise()
		{
			real length = this.Length;
			if (length > 1E-08)
			{
				real num = 1f / length;
				this.X *= num;
				this.Y *= num;
				this.Z *= num;
			}
			return this;
		}

		public Vector RandomDeviant(Radian angle)
		{
			return this.RandomDeviant(angle, ZERO);
		}

		public Vector RandomDeviant(Radian angle, Vector up)
		{
			Vector perpendicular = new Vector();
			if (up == ZERO)
			{
				perpendicular = this.Perpendicular;
			}
			else
			{
				perpendicular = up;
			}
			Radian ang = new Radian((real)(new Random().NextDouble() * 2.0 * Math.PI));
			Quaternion quaternion = new Quaternion(ang, this);
			Vector axis = quaternion * perpendicular;
			quaternion.FromAngleAxis(angle, axis);
			return quaternion * this;
		}

		public Vector Reflect(Vector normal)
		{
			return this - this * normal * 2f * normal;
		}

		public bool DirectionEquals(Vector other, Radian tolerance)
		{
			Radian radian = new Radian((real)Math.Acos(this.DotProduct(other)));
			return Math.Abs(radian.ValueRadian) > tolerance.ValueRadian;
		}

		public bool PositionEquals(Vector other)
		{
			return this.PositionEquals(other, 0.001f);
		}

		public bool PositionEquals(Vector other, real tolerance)
		{
			return ((RealEqual(this.X, other.X, tolerance) && RealEqual(this.Y, other.Y, tolerance)) && RealEqual(this.Z, other.Z, tolerance));
		}

		public bool Equals(Vector other)
		{
			return this.Base.Equals(other);
		}


		public static bool RealEqual(real a, real b, real tolerance)
		{
			return (real)Math.Abs((double)(b - a)) <= tolerance;
		}
		#endregion


		#region Operators
		#region +
		public static Vector operator +(Vector l, Vector r)
		{
			return new Vector(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
		}

		public static Vector operator +(Vector v, real f)
		{
			return new Vector(v.X + f, v.Y + f, v.Z + f);
		}

		public static Vector operator +(real f, Vector v)
		{
			return new Vector(v.X + f, v.Y + f, v.Z + f);
		}
		#endregion +
		#region -
		public static Vector operator -(Vector l, Vector r)
		{
			return new Vector(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
		}

		public static Vector operator -(Vector v, real f)
		{
			return new Vector(v.X - f, v.Y - f, v.Z - f);
		}

		public static Vector operator -(real f, Vector v)
		{
			return new Vector(f - v.X, f - v.Y, f - v.Z);
		}
		#endregion -
		#region *
		public static Vector operator *(Vector l, Vector r)
		{
			return new Vector(l.X * r.X, l.Y * r.Y, l.Z * r.Z);
		}

		public static Vector operator *(Vector v, real f)
		{
			return new Vector(v.X * f, v.Y * f, v.Z * f);
		}

		public static Vector operator *(real f, Vector v)
		{
			return new Vector(v.X * f, v.Y * f, v.Z * f);
		}
		#endregion *
		#region /
		public static Vector operator /(Vector l, Vector r)
		{
			return new Vector(l.X / r.X, l.Y / r.Y, l.Z / r.Z);
		}

		public static Vector operator /(Vector v, real f)
		{
			return new Vector(v.X / f, v.Y / f, v.Z / f);
		}
		#endregion /
		public static Vector operator -(Vector vec)
		{
			return new Vector(-vec.X, -vec.Y, -vec.Z);
		}


		public static bool operator ==(Vector l, Vector r)
		{
			return l.X == r.X && l.Y == r.Y && l.Z == r.Z;
		}
		public static bool operator !=(Vector l, Vector r)
		{
			return !(l == r);
		}

		public static bool operator >(Vector l, Vector r)
		{
			return l.X > r.X && l.Y > r.Y && l.Z > r.Z;
		}
		public static bool operator <(Vector lvec, Vector rvec)
		{
			return lvec.X < rvec.X && lvec.Y < rvec.Y && lvec.Z < rvec.Z;
		}

		#endregion

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder("[");
			builder.Append(this.X);
			builder.Append(',');
			builder.Append(this.Y);
			builder.Append(',');
			builder.Append(this.Z);
			builder.Append(']');
			return builder.ToString();
		}

		public virtual Vector Parse(string value)
		{
			string[] values = value.Trim('[', ']').Split(',');
			float.TryParse(values[0], out this._Values[0]);
			float.TryParse(values[1], out this._Values[1]);
			float.TryParse(values[2], out this._Values[2]);
			return this;
		}
	}
}
