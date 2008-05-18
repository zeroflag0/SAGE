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

// QuaternionBridge.cs created by zeroflag at 5:49 PM 12/4/2007

using System;
using System.ComponentModel;

using real = System.Single;

namespace Sage.Basics
{

	public abstract class QuaternionConverter<ConvertibleType>
		: Converter<Quaternion, ConvertibleType>
	{
		public QuaternionConverter()
		{
		}
	}

	public class QuaternionConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(Quaternion) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (value != null && destinationType == typeof(string) && value is Quaternion)
			{
				Quaternion quat = (Quaternion)value;
				return new System.Text.StringBuilder().Append(quat.W).Append(',').Append(quat.X).Append(',').Append(quat.Y).Append(',').Append(quat.Z).ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value != null)
			{
				string[] values = value.ToString().Split(',');
				return new Quaternion(real.Parse(values[0]), real.Parse(values[1]), real.Parse(values[2]), real.Parse(values[3]));
			}
			return base.ConvertFrom(context, culture, value);
		}
	}

}
