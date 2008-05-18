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
	public class Sender
		: Component,
		Sage.Modules.Connections.ISender
	{
		private Dictionary<IReceiver, Connection> _Connections = new Dictionary<IReceiver, Connection>();

		public Dictionary<IReceiver, Connection> Connections
		{
			get { return _Connections; }
		}

		public Connection this[IReceiver receiver]
		{
			get
			{
				return this.Connections[receiver];
			}
		}

		#region ISender Members

		public void Connect(IReceiver target)
		{
			this.Module.Log.Message("Connecting to " + target + ".");

			Connection connection = new Connection(this);

			connection.Open(target);

			if (connection.IsConnected)
			{
				lock (this.Connections)
				{
					this.Module.Log.Message("Established connection " + connection + ".");
					if (!this.Connections.ContainsKey(connection.Receiver))
						this.Connections.Add(connection.Receiver, connection);
					else
						this.Module.Log.Warning("Already connected.");
				}
			}
		}

		public void Disconnect(IReceiver target)
		{
			if (this.Connections.ContainsKey(target))
			{
				this.Module.Log.Message("Disconnecting from " + target + ".");
				lock (this.Connections)
				{
					this.Connections.Remove(target);
				}
			}
		}

		public void Push(IReceiver receiver, Sage.Threading.ITask message)
		{
			try
			{
				Connection connection = this.Connections[receiver];

			}
			catch (Exception exc)
			{
				throw new SageException(this.Module, "Failed to push message (" + message + ").", exc);
			}
		}

		#endregion

		public Sender(IModule module)
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
