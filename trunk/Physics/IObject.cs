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

namespace Sage.Physics
{
	/// <summary>
	/// This interface should be used by Physics' features and World.Nodes.
	/// </summary>
    public interface IObject
    {
        /// <summary>
        /// Enable / Disable Physics on this Object
        /// </summary>
		bool IsPhysical { get; set; }

        /// <summary>
        /// The weight of an object in kilogram.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// The Physic Body
        /// </summary>
        Body PhysicsBody { get; set; }


        /// <summary>
        /// Holds all Force Descriptors
        /// </summary>
		ICollection<Force> Forces { get; }
        
        /// <summary>
        /// Add force to a body using absolute coordinates.
        /// </summary>
        /// <param name="forcevec">coords</param>
        /// <returns></returns>
        bool AddForce(Vector forcevec);
        
        /// <summary>
        /// Add force to a body using absolute coordinates.
        /// </summary>
        /// <param name="forcevec">coords</param>
        /// <returns></returns>
        bool AddForce(int id, Vector forcevec);
        
        /// <summary>
        /// Add force to a body using absolute coordinates.
        /// </summary>
        /// <param name="forcevec">coords</param>
        /// <returns></returns>
        bool AddForce(string desc, Vector forcevec);
        
        /// <summary>
        /// Add force to a body using absolute coordinates.
        /// </summary>
        /// <param name="forcevec">coords</param>
        /// <returns></returns>
        bool AddForce(int id, string desc, Vector forcevec);

		/// <summary>
		/// Adds a Force into vec Direction
		/// </summary>
		/// <param name="force">Direction vector</param>
		void AddForce(float X, float Y, float Z);

		/// <summary>
		/// Adds a Force into vec Direction, at vecPos position
		/// </summary>
		/// <param name="force">force Vector</param>
		/// <param name="pos">position Vector</param>
		void AddForceAtPos(Vector force, Vector pos);
		/// <summary>
		/// Adds a Force into vec Direction, at vecPos position
		/// </summary>
		/// <param name="force">force Vector</param>
		/// <param name="pos">position Vector</param>
		void AddForceAtPos(float fX, float fY, float fZ, float pX, float pY, float pZ);
    }
}
