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

using Sage.Modules.Connections;
using Sage.Threading;

namespace Sage.Modules
{
	/// <summary>
	/// This part of module provides a bridge into the connection system.
	/// </summary>
	partial class Module : Sage.Modules.Connections.ISender, Sage.Modules.Connections.IReceiver
	{
		protected virtual void PrepareCommunication()
		{
			this.Sender = new Sage.Modules.Connections.Sender(this);
			this.Receiver = new Sage.Modules.Connections.Receiver(this);
			//this.Socket = new MessageSocket(this);
		}

		MessageSocket _Socket = new MessageSocket();

		public MessageSocket Socket
		{
			get { return _Socket; }
			protected set { _Socket = value; }
		}

		#region Message Processing
		/// <summary>
		/// Processes messages received. Timelimit depends on the TimingMode.
		/// </summary>
		/// <remarks>Must only be called from the module's thread!</remarks>
		protected void ProcessMessages()
		{
			this.ProcessMessages(this.TimingMode == TimingMode.Constant);
		}

		/// <summary>
		/// Processes messages until the timelimit is reached. The timelimit is equal to the module's Interval.
		/// </summary>
		/// <param name="timelimit">Whether to use the Module's Interval as timelimit.</param>
		/// <remarks>Must only be called from the module's thread!</remarks>
		protected void ProcessMessages(bool timelimit)
		{
			this.ProcessMessages(this.Interval);
		}

		System.Diagnostics.Stopwatch _MessageProcessingClock = new System.Diagnostics.Stopwatch();
		protected System.Diagnostics.Stopwatch MessageProcessingClock
		{
			get { return _MessageProcessingClock; }
		}

		/// <summary>
		/// Processes messages until the timelimit is reached.
		/// </summary>
		/// <param name="timelimit">A timelimit in miliseconds.</param>
		/// <remarks>Must only be called from the module's thread!</remarks>
		protected virtual void ProcessMessages(long timelimit)
		{
			this.MessageProcessingClock.Reset();
			this.MessageProcessingClock.Start();
#if DEBUG_MODULE_COMMUNICATION
			int processed = 0;
#endif
			do
			{
				//TODO: process messages...
				//IMessage message = this.Receiver.Pop();
				ITask message = this.Socket.Pop();

				if (message == null)
					continue;

				try
				{
					message.Run();

#if DEBUG_MODULE_COMMUNICATION
					processed++;
#endif
				}
				catch (Exception exc)
				{
					new SageException(this, "Task " + message + " failed.", exc);
				}
			}
			while (this.Receiver.MessagesAvailable && (timelimit < 0 || this.MessageProcessingClock.ElapsedMilliseconds < timelimit));

			this.MessageProcessingClock.Stop();
#if DEBUG_MODULE_COMMUNICATION
			if (processed > 0)
			{
				this.Logging.Message("Processed " + processed + " messages.");
			}
#endif
		}

		#endregion Message Processing

		#region Sender
		Sage.Modules.Connections.Sender _Sender;

		public Sage.Modules.Connections.Sender Sender
		{
			get { return _Sender; }
			protected set { _Sender = value; }
		}
		#endregion Sender

		#region Receiver
		Sage.Modules.Connections.Receiver _Receiver;

		public Sage.Modules.Connections.Receiver Receiver
		{
			get { return _Receiver; }
			protected set { _Receiver = value; }
		}
		#endregion Receiver

		#region ISender Members

		public void Connect(Sage.Modules.Connections.IReceiver target)
		{
			this.Sender.Connect(target);
		}

		public void Disconnect(Sage.Modules.Connections.IReceiver target)
		{
			this.Sender.Disconnect(target);
		}

		public void Push(Sage.Modules.Connections.IReceiver receiver, Sage.Threading.ITask message)
		{
			this.Sender.Push(receiver, message);
		}

		public Sage.Modules.Connections.Connection this[Sage.Modules.Connections.IReceiver receiver]
		{
			get { return this.Sender[receiver]; }
		}

		#endregion

		#region IReceiver Members

		public Sage.Threading.ITask Pop()
		{
			return this.Receiver.Pop();
		}

		public void Accept(Sage.Modules.Connections.Connection connection)
		{
			this.Receiver.Accept(connection);
		}

		public void Close(Sage.Modules.Connections.Connection connection)
		{
			this.Receiver.Close(connection);
		}

		public void Close(Sage.Modules.Connections.ISender target)
		{
			this.Receiver.Close(target);
		}

		public void DiscardAll()
		{
			this.Receiver.DiscardAll();
		}

		public void Discard(Sage.Modules.Connections.ISender target)
		{
			this.Receiver.Discard(target);
		}

		public Sage.Modules.Connections.Connection this[Sage.Modules.Connections.ISender sender]
		{
			get { return this.Receiver[sender]; }
		}

		public bool MessagesAvailable
		{
			get { return this.Receiver.MessagesAvailable; }
		}

		#endregion
	}
}
