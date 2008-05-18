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

// Base.cs created by zeroflag at 5:07 PM 12/4/2007

using System;
using System.Collections.Generic;

namespace Sage.Basics
{
	public interface IConvertible
	{
		object ConvertTo(Type type);
		object SetFrom(object from);
	}

	public abstract class Convertible<SageType>
		: IConvertible
		where SageType : Convertible<SageType>, new()
	{
		public Convertible()
		{
		}

		public Convertible(object from)
		{
			this.SetFrom(from);
		}

		static Dictionary<Type, List<IConverter>> s_Converters = null;

		protected static Dictionary<Type, List<IConverter>> Converters
		{
			get
			{
				if (s_Converters == null)
					_InitializeConverterMap();
				return s_Converters;
			}
		}

		protected static void _InitializeConverterMap()
		{
			// create the container...
			s_Converters = new Dictionary<Type, List<IConverter>>();

			// fetch a list of all types derived from Converter, specialized for the current type...
			List<Type> converterTypes = zeroflag.TypeHelper.GetDerived(typeof(Converter<,>));

			// scan all converters...
			foreach (Type converterType in converterTypes)
			{
				if (converterType.ContainsGenericParameters)
					continue;	// don't even bother... doesn't work.
				try
				{
					// pull an instance...
					IConverter converter = (IConverter)zeroflag.TypeHelper.CreateInstance(converterType);

					if (typeof(SageType).IsAssignableFrom(converter.SageObjectType))
					// if the converter is built for the current type...
					{

						// get the target type...
						Type targetType = converter.TargetObjectType;

						if (!Converters.ContainsKey(targetType))
							Converters.Add(targetType, new List<IConverter>());

						Converters[targetType].Add(converter);
					}
				}
				catch (ArgumentException)
				{
					// can't create instance from generic type...
				}
				catch (Exception exc)
				{
#if DEBUG
					lock (Console.Out)
					{
						Console.WriteLine(exc);
					}
#endif
				}
			}

			//TODO: initialize converters and store in map...


		}

		public T ConvertTo<T>()
		{
			return (T)this.ConvertTo(typeof(T));
		}

		public virtual object ConvertTo(Type type)
		{
			if (Converters.ContainsKey(type))
			{
				List<IConverter> converters = Converters[type];

				foreach (IConverter conv in converters)
				{
					if (typeof(SageType).IsAssignableFrom(conv.SageObjectType))
					{
						// found a suitable converter...
						return conv.FromSageObject(this);
					}
				}
			}
			throw new NotImplementedException("Cannot convert from " + typeof(SageType) + " to " + type);
		}

		public static SageType ConvertFrom(object from)
		{
			return (SageType)new SageType().SetFrom(from);
		}

		public virtual object SetFrom(object from)
		{
			if (Converters.ContainsKey(from.GetType()))
			{
				List<IConverter> converters = Converters[from.GetType()];

				foreach (IConverter conv in converters)
				{
					if (typeof(SageType).IsAssignableFrom(conv.SageObjectType))
					{
						// found a suitable converter...
						return conv.FromSageObject(this);
					}
				}
			}
			throw new NotImplementedException("Cannot convert from " + from.GetType() + " to " + typeof(SageType));
		}

		private object _Base;
		protected object Base
		{
			get { return _Base; }
			set { _Base = value; }
		}

	}
}
