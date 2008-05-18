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

using Sage;
using Sage.Basics;
using Sage.Modules;
using Sage.Threading;

namespace Sage.Modules.Connections
{
	public class Connection : MessageQueue, IMessageQueueReader<ITask>, IMessageQueueWriter<ITask>
	{
		private IReceiver _Receiver;

		public IReceiver Receiver
		{
			get { return _Receiver; }
			protected set { _Receiver = value; }
		}
		private ISender _Sender;

		public ISender Sender
		{
			get { return _Sender; }
			protected set { _Sender = value; }
		}

		bool _Accepted = false;

		public bool Accepted
		{
			get { return _Accepted; }
			protected set { _Accepted = value; }
		}

		public bool IsConnected
		{
			get { return this.Accepted && this.Receiver != null && this.Sender != null; }
		}

		public void Open(IReceiver receiver)
		{
			this.Receiver = receiver;

			receiver.Accept(this);
		}

		public void Request(ISender sender)
		{
			this.Sender = sender;
		}

		public void Accept()
		{
			this.Accepted = true;
		}

		public Connection()
		{
		}

		public Connection(ISender sender)
			: this()
		{
			this.Request(sender);
		}

		public new void Clear()
		{
			base.Clear();
		}


		public override string ToString()
		{
			StringBuilder text = new StringBuilder();
			text.Append(base.ToString());

			text.Append("[");
			try
			{
				text.Append(this.Sender);
				text.Append(" -> ");
				text.Append(this.Receiver);
				//text.Append(" ");
				text.Append(this.Accepted ? ", accepted" : "");
				text.Append(this.IsConnected ? ", connected" : ", disconnected");
			}
			catch (Exception)
			{
				text.Append("<invalid>");
			}
			text.Append("]");

			return text.ToString();
		}
	}
}
