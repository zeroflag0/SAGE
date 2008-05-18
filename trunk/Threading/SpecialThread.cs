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

namespace Sage.Threading
{
	public  enum SpecialThread : short
	{
		/// <summary>
		/// The thread which processes the thread pool queue.
		/// </summary>
		PoolThread = -1,
		/// <summary>
		/// The mainthread which started the engine/pool.
		/// </summary>
		MainThread = 0,
		/// <summary>
		/// The first worker thread. Maps to MainThread if no additional thread was created.
		/// </summary>
		PrimaryWorker = 1,
		/// <summary>
		/// The second worker thread. Maps to PrimaryWorker if only one or less additional threads were created.
		/// </summary>
		SecondaryWorker = 2,
		/// <summary>
		/// The thread is not "special".
		/// </summary>
		Unspecified = short.MaxValue - 1,
		/// <summary>
		/// The thread Pool automatically selects a thread with the lowest utilization.
		/// </summary>
		AutoBalance = short.MaxValue,
	}
}