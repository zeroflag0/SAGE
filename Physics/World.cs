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

namespace Sage.Physics
{
	/// <summary>
	/// Description of World.
	/// </summary>
	public abstract class World
	{
		//NOTE: public extern without references are a BAD idea! breaks reflection...
		//public extern World();

		/// <summary>
		/// Destroys the World ;D
		/// </summary>
		/// <returns></returns>
		public abstract bool DestroyWorld();

		/// <summary>
		/// Returns the World object
		/// </summary>
		/// <returns></returns>
		public abstract IntPtr GetWorld();

		/// <summary>
		/// Sets the World Gravity
		/// </summary>
		/// <param name="gravity">Gravity strength</param>
		public abstract bool SetGravity(Vector gravity);

		/// <summary>
		/// Sets the World Gravity
		/// </summary>
		/// <param name="X">force in X direction</param>
		/// <param name="Y">force in Y direction</param>
		/// <param name="Z">force in Z direction</param>
		/// <returns></returns>
		public abstract bool SetGravity(float X, float Y, float Z);

		/// <summary>
		/// Get the Gravity of this World Object
		/// </summary>
		/// <returns>Vector3 of Gravity</returns>
		public abstract Vector GetGravity();

		/// <summary>
		/// Step the world.
		/// </summary>
		/// <param name="deltaT">Time</param>
		/// <returns></returns>
		public abstract bool Step(float deltaT);

		/// <summary>
		/// Step the world (faster but less acurate!)
		/// </summary>
		/// <param name="step">step size</param>
		/// <returns></returns>
		public abstract bool QuickStep(float step);
	}
}
