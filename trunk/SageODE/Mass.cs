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

/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Daniel Koppers
 * Datum: 02.08.2007
 * Zeit: 13:43
 * 
 * Sie k�nnen diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader �ndern.
 */

using System;
using System.Collections.Generic;
using Sage;
using Sage.Basics;
using Tao.Ode;

namespace Sage.Physics.ODE
{
	/// <summary>
	/// ODE Implementation for Sage.Physics
	/// </summary>
	public class Mass : Sage.Physics.Mass
	{
		Ode.dMass odemass;


		public Mass()
		{
			odemass = new Ode.dMass();
		}

		public IntPtr GetMass()
		{
			return (IntPtr)odemass;
		}

		public override float Weight
		{
			get { return odemass.mass; }
			set { odemass.mass = value; }
		}

		public override Vector CenterOfGravity
		{
			get
			{
				return odemass.c;
			}
			set
			{
				odemass.c = value;
			}
		}

		public bool SetMassSphere(float density, float radius)
		{
			de.dMassSetSphere(ref odemass, density, radius);
		}
	}
}
