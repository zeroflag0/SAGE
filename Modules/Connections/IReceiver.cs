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
namespace Sage.Modules.Connections
{
	public interface IReceiver
	{

		/// <summary>
		/// Gets and removes the first message 
		/// </summary>
		/// <returns></returns>
		Sage.Threading.ITask Pop();

		/// <summary>
		/// Accepts and opens the connection.
		/// </summary>
		void Accept(Connection connection);

		/// <summary>
		/// Closes and disconnects the connection.
		/// </summary>
		void Close(Connection connection);

		/// <summary>
		/// Closes and disconnects the connection.
		/// </summary>
		void Close(ISender target);

		/// <summary>
		/// Discards ALL messages from all senders.
		/// </summary>
		void DiscardAll();

		/// <summary>
		/// Discards all messages from the target sender.
		/// </summary>
		/// <param name="target"></param>
		void Discard(ISender target);
	
		Connection this[ISender sender] { get; }

		/// <summary>
		/// Whether messages are available or not.
		/// </summary>
		bool MessagesAvailable { get; }
	}
}
