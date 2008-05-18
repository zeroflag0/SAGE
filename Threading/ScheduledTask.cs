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

namespace Sage.Threading
{
	public class ScheduledTask
		: ITask,
		  IComparable<ScheduledTask>,
		  IComparable<long>,
		  IComparable<double>,
		  IComparable<int>
	{
		private long _Time;

		public long Time
		{
			get { return _Time; }
			set { _Time = value; }
		}

		private ITask _Message;

		public ITask Message
		{
			get { return _Message; }
			set { _Message = value; }
		}

		private int _ThreadID = -1;

		/// <summary>
		/// ID of the thread on which the task should be executed.
		/// -1 means random thread.
		/// </summary>
		public int ThreadID
		{
			get { return _ThreadID; }
			set { _ThreadID = value; }
		}

		SpecialThread _ThreadType = SpecialThread.Unspecified;

		public SpecialThread ThreadType
		{
			get { return _ThreadType; }
			set { _ThreadType = value; }
		}

		int _Priority = 0;

		public int Priority
		{
			get { return _Priority; }
			set { _Priority = value; }
		}

		Thread _Thread = null;

		public virtual Thread Thread
		{
			get { return _Thread; }
			set { _Thread = value; }
		}

		#region Constructor

		public ScheduledTask(ITask msg, long time, int threadID)
		{
			this.Message = msg;
			this.Time = time;
			this.ThreadID = threadID;
		}

		public ScheduledTask(ITask msg, long time, SpecialThread type)
		{
			this.Message = msg;
			this.Time = time;
			this.ThreadType = type;
		}

		public ScheduledTask(ITask msg, long time)
			: this(msg, time, SpecialThread.AutoBalance)
		{
		}

		public ScheduledTask(ITask msg, int threadID)
			: this(msg, Pool.Now, threadID)
		{
		}

		public ScheduledTask(ITask msg)
			: this(msg, Pool.Now, -1)
		{
		}

		public static implicit operator ScheduledTask(Task msg)
		{
			return new ScheduledTask(msg);
		}

		public ScheduledTask(Task.CallHandler call)
			: this(new Task(call))
		{
		}

		public static implicit operator ScheduledTask(Task.CallHandler call)
		{
			return new ScheduledTask(call);
		}

		#endregion Constructor

		#region IMessage Members

		public virtual void Run()
		{
			if (this.Message != null)
			{
				this.Message.Run();
			}
			if (this.Next != null)
			{
				if (this.Thread == null)
					this.Next.Run();
				else
					this.Thread.QueuePool(this.Next);
			}
		}

		public virtual void Run(Thread thread)
		{
			this.Thread = thread;
			this.Run();
		}

		#endregion

		#region IComparable<Task> Members

		public int CompareTo(ScheduledTask other)
		{
			if (other != null)
			{
				return this.CompareTo(other.Time);
			}
			else
			{
				return this.CompareTo((long)0);
			}
		}

		#endregion

		#region IComparable<double> Members

		public int CompareTo(double other)
		{
			return this.CompareTo((long)other);
		}

		#endregion

		#region IComparable<long> Members

		public int CompareTo(long other)
		{
			// I know this comparison is "wrong". but: lower time => higher priority.
			//return (int)(other - this.Time);

			return (int)(this.Time - Priority - other);
		}

		#endregion


		#region IComparable<int> Members

		public int CompareTo(int other)
		{
			return this.CompareTo((long)other);
			//return (int)this.Time - other;
		}

		#endregion

		public override string ToString()
		{
			if (this.Message == null || this.Message == this)
				return base.ToString();
			else
				return this.ToString(this.Message);
		}

		public string ToString(ITask message)
		{
			return "Time=" + this.Time + " " + "Eta=" + (this.Time - Pool.Now) + "ms " + this.Message.ToString();
		}

		private ScheduledTask _Next;

		/// <summary>
		/// A followup task to be executed after the task finished. It will NOT be executed imediately but enqueued in the threadpool.
		/// </summary>
		public ScheduledTask Next
		{
			get { return _Next; }
			set { _Next = value; }
		}

		private long _NextDelay;

		/// <summary>
		/// If a Next task is specified, this adds a delay after the task finished before the Next task is called. [milliseconds]
		/// </summary>
		public long NextDelay
		{
			get { return _NextDelay; }
			set { _NextDelay = value; }
		}
	}
}