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

namespace Sage.Modules.Connections
{
	public class Receiver
		: Component,
		Sage.Modules.Connections.IReceiver
	{
		private Dictionary<ISender, Connection> _Connections = new Dictionary<ISender, Connection>();

		public Dictionary<ISender, Connection> Connections
		{
			get { return _Connections; }
		}

		public Connection this[ISender sender]
		{
			get
			{
				return this.Connections[sender];
			}
		}


		#region IReceiver Members

		public void Accept(Connection connection)
		{
			this.Module.Log.Message("Accepting connection " + connection + ".");

			connection.Accept();
			if (connection.IsConnected)
			{
				lock (this.Connections)
				{
					if (!this.Connections.ContainsKey(connection.Sender))
						this.Connections.Add(connection.Sender, connection);
					else
						this.Module.Log.Warning("Already connected.");
				}
			}
		}

		public void Close(Connection connection)
		{
			this.Close(connection.Sender);
		}

		public void Close(ISender target)
		{
			this.Module.Log.Message("Closing connection from " + target + ".");
			lock (this.Connections)
			{
				this.Connections.Remove(target);
			}
		}

		public void DiscardAll()
		{
			this.Module.Log.Message("Discarding all messages.");
			
			lock (this.Connections)
			{
				foreach (ISender sender in this.Connections.Keys)
				{
					this.Discard(sender);
				}
			}
		}

		public void Discard(Sage.Modules.Connections.ISender target)
		{
			this.Module.Log.Message("Discarding all messages from " + target + ".");

			this.Connections[target].Clear();
		}

		public bool MessagesAvailable
		{
			get
			{
				foreach (Connection connection in this.Connections.Values)
				{
					if (!connection.IsEmpty)
						return true;
				}
				return false;
			}
		}

		public Sage.Threading.ITask Pop()
		{
			Sage.Threading.ITask message = null;

			foreach (Connection connection in this.Connections.Values)
			{
				if (!connection.IsEmpty)
				{
					message = connection.Pop();

					if (message != null)
						return message;
				}
			}
			return null;
		}

		#endregion

		public Receiver(IModule module)
			: base(module)
		{
		}

		public override string ToString()
		{
			if (this.Module == null)
				return base.ToString();
			else
				return this.Module + "." + this.GetType().Name;
		}
	}
}
