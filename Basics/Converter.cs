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

namespace Sage.Basics
{
	public interface IConverter
	{
		object ToSageObject(object from);
		object FromSageObject(object from);
		Type TargetObjectType { get; }
		Type SageObjectType { get; }
	}

	public abstract class Converter<SageType, TargetType>
		: IConverter
		where SageType : IConvertible
	//		where BaseType : class
	{
		protected abstract TargetType ConvertFromSage(SageType from);

		protected abstract SageType ConvertToSage(TargetType from);

		//public Converter(TargetType from)
		//{
		//    this.ConvertFrom(from);
		//}

		//#region Internal Value
		//protected BaseType _Base = default(BaseType);

		//public BaseType Base
		//{
		//    get
		//    {
		//        if (_Base == null)// default(BaseType))
		//        {
		//            _Base = this.CreateBase(this);
		//        }
		//        return (BaseType)_Base;
		//    }
		//    set
		//    {
		//        if (_Base == null || !_Base.Equals(value))
		//        {
		//            _Base = value;
		//            this.FromBase(value);
		//        }
		//    }
		//}
		//#endregion Internal Value

		//public int CompareTo(BaseType other)
		//{
		//    if (other != null && other is IComparable<BaseType>)
		//        return ((IComparable<BaseType>)other).CompareTo(this.Base);
		//    else
		//        return -1;
		//}
		//public bool Equals(BaseType other)
		//{
		//    return this.Base.Equals(other);
		//}

		//public static implicit operator TargetType(Converter<SageType, TargetType> from)
		//{
		//    throw new NotImplementedException();
		//    //TODO: return a base type...
		//}


		public virtual object ToSageObject(object from)
		{
			return this.ConvertToSage((TargetType)from);
		}

		public virtual object FromSageObject(object from)
		{
			return this.ConvertFromSage((SageType)from);
		}

		public Type TargetObjectType
		{
			get
			{
				return typeof(TargetType);
			}
		}

		public Type SageObjectType
		{
			get
			{
				return typeof(SageType);
			}
		}
	}

}
