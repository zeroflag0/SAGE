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
 * Zeit: 14:07
 * 
 * Sie k�nnen diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader �ndern.
 */

using System;
using Sage;
using Sage.Basics;
using Tao.Ode;

namespace Sage.Physics.ODE
{
	/// <summary>
	/// ODE Implementation for Sage.Physics
	/// </summary>
	public class Body : Sage.Physics.Body
	{
		
		public Body(World world)
		{
			body = new Ode.dBodyCreate(world.GetWorld());
		}
		
		public bool SetPosition(Vector pos)
		{
			Ode.dBodySetPosition(body, pos.X, pos.Y, pos.Z);
			return true;
		}
		
        public Vector GetPosition()
        {
        	Ode.dVector3 vec = Ode.dBodyGetPosition(body);
        	return new Sage.Basics.Vector(vec.X, vec.Y, vec.Z);
        }

        public bool SetMass(Mass mass)
        {
        	Ode.dBodySetMass(body, ref (Ode.dMass)mass.GetMass());
        }
        
        public IntPtr GetMass()
        {
        	Ode.dMass mass;
        	Ode.dBodyGetMass(body, ref mass);
        	return (IntPtr)mass;
        }
        
        public bool AddForce(Vector force)
        {
        	Ode.dBodyAddForce(body, force.X, force.Y, force.Z);
        	return true;
        }
        
        public Vector GetLinearVelocity()
        {
        	Ode.dVector3 vec = Ode.dBodyGetLinearVel(body);
        	return new Vector(vec.X, vec.Y, vec.Z);
        }

		public override bool SetMass(Sage.Physics.Mass mass)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
