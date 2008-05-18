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
using Sage.World;

namespace Sage.Modules
{
	public abstract class FeatureFactory<Mod, Feature> : IFeatureFactory<Feature>, IFeatureFactory
		where Mod : Module
		where Feature : class, IFeature
	{
		#region IFeatureFactory<Feature> Members

		public virtual Type FeatureType
		{
			get { return typeof(Feature); }
		}

		public abstract Feature CreateFeature(Node target);

		public abstract void DestroyFeature(Feature feature);

		#endregion

		#region IFeatureFactory Members


		IFeature IFeatureFactory.CreateFeature(Node target)
		{
			return this.CreateFeature(target);
		}

		public void DestroyFeature(IFeature feature)
		{
			this.DestroyFeature((Feature)feature);
		}

		#endregion

		#region Module

		private Mod _Module;

		public Mod Module
		{
			get { return _Module; }
			set
			{
				if (_Module != value)
				{
					_Module = value;
				}
			}
		}
		#endregion Module
	}
}