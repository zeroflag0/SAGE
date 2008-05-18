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

//using BaseType = Quaternion;
using real = System.Single;

namespace Sage
{
	public class Quaternion
		: Sage.Basics.Convertible<Quaternion>,
		Sage.Basics.IQuaternion<Quaternion>
	{

		#region Constants
		public static Quaternion IDENTITY;
		public static Quaternion ZERO;

		public enum ValueIndices : int
		{
			W = 0,
			X = 1,
			Y = 2,
			Z = 3,
		};
		#endregion

		#region Creation
		/// <summary>
		/// Construct a quaternion.
		/// </summary>
		public Quaternion()
		{
		}
		/// <summary>
		/// Construct a quaternion.
		/// </summary>
		public Quaternion(float W)
			: this(W, 0.0f, 0.0f, 0.0f)
		{
		}
		/// <summary>
		/// Construct a quaternion.
		/// </summary>
		public Quaternion(Radian angle, Vector axis)
			: this()
		{
			this.FromAngleAxis(angle, axis);
		}
		public Quaternion(float W, float X, float Y, float Z)
			: this()
		{
			this.W = W;
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}
		public Quaternion(object from)
			: base(from)
		{
		}

		//		public static implicit operator Quaternion(BaseType from)
		//		{
		//			return new Quaternion(from);
		//		}
		//		public static implicit operator BaseType(Quaternion from)
		//		{
		//			return from.Base;
		//		}
		#endregion

		#region BaseBridge
		//		protected override BaseType CreateBase(SageType from)
		//		{
		//			return new BaseType(this.W, this.X, this.Y, this.Z);
		//		}
		//
		//		protected override SageType FromBase(BaseType from)
		//		{
		//			this.Set(from.w, from.x, from.y, from.z);
		//			this._Base = from;
		//		}

		private void Set(float W, float X, float Y, float Z)
		{
			this.W = W;
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}
		#endregion BaseBridge

		#region Value
		real[] _Values = new real[4];
		[System.ComponentModel.Browsable(false)]
		public real[] Values
		{
			get { return _Values; }
			set
			{
				if (_Values != value)
				{
					_Values = value;
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

		public real W
		{
			get { return this[(int)ValueIndices.W]; }
			set
			{
				if (this[(int)ValueIndices.W] != value)
				{
					this[(int)ValueIndices.W] = value;
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

		#region IQuaternion<Quaternion> Members

		[System.ComponentModel.Browsable(false)]
		public real Norm
		{
			get
			{
				return W * W + X * X + Y * Y + Z * Z;
			}
		}
		/// <summary>
		/// Calculate the local pitch element of this quaternion.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Sage.Basics.RadianConverter))]
		public Radian Pitch
		{
			get
			{
				return new Radian((real)Math.Atan2((this.Z * this.Y + this.X * this.W) * 2f, (W * W - X * X - Y * Y) + Z * Z));
			}
		}
		/// <summary>
		/// Calculate the local roll element of this quaternion.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Sage.Basics.RadianConverter))]
		public Radian Roll
		{
			get
			{
				return new Radian((real)Math.Atan2(((this.Y * this.X) + (this.Z * this.W)) * 2f, (((W * W) + (X * X)) - (Y * Y)) - (Z * Z)));
			}
		}
		/// <summary>
		/// Calculate the local yaw element of this quaternion.m
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(Sage.Basics.RadianConverter))]
		public Radian Yaw
		{
			get
			{
				return new Radian((float)Math.Asin((double)(((this.Z * this.X) - (this.Y * this.W)) * -2f)));
			}
		}
		/// <summary>
		/// Get the local x-axis.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public Vector XAxis
		{
			get
			{
				float z2 = Z * 2f;
				float y2 = Y * 2f;
				return new Vector(1f - (Z * z2 + Y * y2), X * y2 + W * z2, X * z2 - W * y2);
			}
		}
		/// <summary>
		/// Get the local y-axis.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public Vector YAxis
		{
			get
			{
				float x2 = X * 2f;
				float y2 = Y * 2f;
				float z2 = Z * 2f;
				return new Vector(X * y2 - W * z2, 1f - (Z * z2 + X * x2), Y * z2 + W * x2);
			}
		}
		/// <summary>
		/// Get the local z-axis. 
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public Vector ZAxis
		{
			get
			{
				float x2 = X * 2f;
				float y2 = Y * 2f;
				float z2 = Z * 2f;
				return new Vector(X * z2 + W * y2, Y * z2 - W * x2, 1f - (Y * y2 + X * x2));
			}
		}

		public real Dot(Quaternion other)
		{
			return ((((Z * other.Z) + (Y * other.Y)) + (X * other.X)) + (W * other.W));
		}

		public Quaternion Exp()
		{
			float z = this.Z;
			float y = this.Y;
			float x = this.X;
			Radian rad = new Radian((real)Math.Sqrt(x * x + y * y + z * z));
			float sin = (real)Math.Sin(rad);
			Quaternion quaternion = new Quaternion();
			quaternion.W = (real)Math.Cos(rad);
			if (Math.Abs(sin) >= float.Epsilon)
			{
				float num = sin / rad.ValueRadian;
				quaternion.X = x * num;
				quaternion.Y = y * num;
				quaternion.Z = z * num;
				return quaternion;
			}
			quaternion.X = x;
			quaternion.Y = y;
			quaternion.Z = z;
			return quaternion;
		}

		public Quaternion Inverse()
		{
			float z = this.Z;
			float y = this.Y;
			float x = this.X;
			float w = this.W;
			float length = w * w + x * x + y * y + z * z;
			if (length > 0f)
			{
				Quaternion quaternion = new Quaternion();
				float i = 1f / length;
				quaternion.W = w * i;
				quaternion.X = -x * i;
				quaternion.Y = -y * i;
				quaternion.Z = -z * i;
				return quaternion;
			}
			return ZERO;
		}

		public Quaternion Log()
		{
			float z = this.Z;
			float y = this.Y;
			float x = this.X;
			float w = this.W;
			Quaternion quaternion = new Quaternion();
			quaternion.W = 0f;
			if (Math.Abs(w) < 1f)
			{
				Radian acos = new Radian((float)Math.Acos(w));
				float sin = (real)Math.Sin(acos);
				if (Math.Abs(sin) >= float.Epsilon)
				{
					float num = acos.ValueRadian / sin;
					quaternion.X = x * num;
					quaternion.Y = y * num;
					quaternion.Z = z * num;
					return quaternion;
				}
			}
			quaternion.X = x;
			quaternion.Y = y;
			quaternion.Z = z;
			return quaternion;
		}
		/// <summary>
		/// Normalises this quaternion, and returns the previous length.
		/// </summary>
		/// <returns>The length before normalization.</returns>
		public real Normalise()
		{
			real num = (((this.W * this.W) + (X * X)) + (Y * Y)) + (Z * Z);
			real num2 = (real)(1.0 / Math.Sqrt((double)num));
			Quaternion quaternion = (Quaternion)(this * num2);
			this.Values = quaternion.Values;
			return num;
		}

		public Quaternion UnitInverse()
		{
			return new Quaternion(this.W, -this.X, -this.Y, -this.Z);
		}

		public bool Equals(Quaternion other)
		{
			return this.Base.Equals(other);
		}

		public bool Equals(Quaternion other, Radian tolerance)
		{
			Radian radian = new Radian((float)Math.Acos(this.Dot(other)));
			return
				((Math.Abs(radian.ValueRadian) < tolerance.ValueRadian)
				||
				(Math.Abs(radian.ValueRadian) - Math.PI < tolerance.ValueRadian));
		}

		#endregion

		#region Methods
		public void FromAngleAxis(Radian angle, Vector axis)
		{
			Radian fValue = 0.5f * angle;
			real num = (real)Math.Sin(fValue.ValueRadian);
			this.W = (real)Math.Cos(fValue.ValueRadian);
			this.X = axis.X * num;
			this.Y = axis.Y * num;
			this.Z = axis.Z * num;
		}

		public override string ToString()
		{
			return new StringBuilder("{").Append(this.ZAxis).Append(this.W).Append(',').Append(this.X).Append(',').Append(this.Y).Append(',').Append(this.Z).Append('}').ToString();
		}

		public virtual Quaternion Parse(string value)
		{
			string[] values = value.Substring(value.IndexOf(']')).Trim(']', '[', '{', '}').Split(',');
			float.TryParse(values[0], out this._Values[0]);
			float.TryParse(values[1], out this._Values[1]);
			float.TryParse(values[2], out this._Values[2]);
			float.TryParse(values[3], out this._Values[3]);
			return this;
		}
		#endregion

		#region Operators
		public static Quaternion operator +(Quaternion l, Quaternion r)
		{
			return new Quaternion(l.W + r.W, l.X + r.X, l.Y + r.Y, l.Z + r.Z);
		}

		public static Quaternion operator -(Quaternion l, Quaternion r)
		{
			return new Quaternion(
				l.W - r.W,
				l.X - r.X,
				l.Y - r.Y,
				l.Z - r.Z);
		}

		public static Quaternion operator *(Quaternion l, Quaternion r)
		{
			return new Quaternion(
				(((l.W * r.W) - (l.X * r.X)) - (l.Y * r.Y)) - (l.Z * r.Z),
				(((l.Y * r.Z) + (r.X * l.W)) + (l.X * r.W)) - (r.Y * l.Z),
				(((r.X * l.Z) + (r.Y * l.W)) + (l.Y * r.W)) - (l.X * r.Z),
				(((r.Y * l.X) + (r.Z * l.W)) + (l.Z * r.W)) - (l.Y * r.X));
		}

		public static Vector operator *(Quaternion quat, Vector vector)
		{
			Vector result = new Vector(quat.X, quat.Y, quat.Z);
			result = result.CrossProduct(vector);
			return vector + (vector * (quat.W * 2f)) + (result.CrossProduct(vector) * 2f);
		}

		public static Quaternion operator *(Quaternion q, float s)
		{
			return new Quaternion(q.W * s, q.X * s, q.Y * s, q.Z * s);
		}

		public static Quaternion operator *(float s, Quaternion q)
		{
			return new Quaternion(q.W * s, q.X * s, q.Y * s, q.Z * s);
		}


		public static Quaternion operator -(Quaternion rkQ)
		{
			return new Quaternion(-rkQ.W, -rkQ.X, -rkQ.Y, -rkQ.Z);
		}


		public static bool operator ==(Quaternion l, Quaternion r)
		{
			return ((r.X == l.X) && (r.Y == l.Y)) && ((r.Z == l.Z) && (r.W == l.W));
		}

		public static bool operator !=(Quaternion l, Quaternion r)
		{
			return !(l == r);
		}

		#endregion
	}
}
