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
using Sage.Threading;
namespace Sage.Modules
{
	public class MessageSocket : IMessageQueue<ITask>
	{
		public MessageSocket()
			//: this(null)
		{ }

		//public MessageSocket(IModule owner)
		//{
		//    this.Module = owner;
		//}

		//IModule _Module;

		//public IModule Module
		//{
		//    get { return _Module; }
		//    set { _Module = value; }
		//}

		Dictionary<int, MessageQueue> _Queues = new Dictionary<int, MessageQueue>();

		protected Dictionary<int, MessageQueue> Queues
		{
			get { return _Queues; }
		}

		public MessageQueue Queue
		{
			get
			{
				return this[Sage.Threading.Pool.CurrentThreadID];
			}
		}
		public MessageQueue this[int id]
		{
			get
			{
				if (!this.Queues.ContainsKey(id) || this.Queues[id] == null)
				{
					this.Queues[id] = new MessageQueue();
				}
				return this.Queues[id];
			}
		}

		#region IMessageQueueReader<IMessage> Members

		public bool IsEmpty
		{
			get
			{
				foreach (MessageQueue queue in this.Queues.Values)
				{
					if (!queue.IsEmpty)
						return false;
				}
				return true;
			}
		}

		public ITask Pop()
		{
			ITask value = null;
			foreach (MessageQueue queue in this.Queues.Values)
			{
				if (!queue.IsEmpty)
					if ((value = queue.Pop()) != null)
						break;
			}
			return value;
		}

		#endregion

		#region IMessageQueueWriter<IMessage> Members

		public void Push(ITask msg)
		{
			this.Queue.Push(msg);
		}

		public void Push(Task.CallHandler call)
		{
			this.Push(new Task(call));
		}

		public void Push<T1>(Task<T1>.CallHandler call, T1 p1)
		{
			this.Push(new Task<T1>(call, p1));
		}
		public void Push<T1, T2>(Task<T1, T2>.CallHandler call, T1 p1, T2 p2)
		{
			this.Push(new Task<T1, T2>(call, p1, p2));
		}
		public void Push<T1, T2, T3>(Task<T1, T2, T3>.CallHandler call, T1 p1, T2 p2, T3 p3)
		{
			this.Push(new Task<T1, T2, T3>(call, p1, p2, p3));
		}
		public void Push<T1, T2, T3, T4>(Task<T1, T2, T3, T4>.CallHandler call, T1 p1, T2 p2, T3 p3, T4 p4)
		{
			this.Push(new Task<T1, T2, T3, T4>(call, p1, p2, p3, p4));
		}

		public void Update()
		{
			this.Queue.Update();
		}

		#endregion
	}
}
