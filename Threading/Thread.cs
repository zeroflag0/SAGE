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
using System.Threading;
#if SAGE_LOGGING
using Sage.Modules.Logging;
#endif //SAGE_LOGGING

namespace Sage.Threading
{
	public class Thread : IDisposable
	{
#if WIN32
		public const int Sleep0Tolerance = 16;
		public const int Sleep10Tolerance = 25;
		public const int DelayTolerance = 10;
		public const float DelayToleranceFactor = 1.4f;
#else
		public const int Sleep0Tolerance = 18;
		public const int Sleep10Tolerance = 25;
		public const int DelayTolerance = 10;
		public const float DelayToleranceFactor = 1.4f;
#endif

		#region Logging
#if SAGE_LOGGING
		private Log _Logging;

		public Log Logging
		{
			get { return _Logging; }
			protected set { _Logging = value; }
		}
#else
		public ConsoleLogging Logging = new ConsoleLogging();
		public class ConsoleLogging
		{
			public string Owner = "<thread>";
			public void Message(object value)
			{
				lock (Console.Out)
				{
					Console.Write(DateTime.Now.ToString() + "[" + System.Threading.Thread.CurrentThread.ManagedThreadId + "]; \t");
					Console.Write(this.Owner);
					Console.WriteLine(value);
				}
			}
			public void Warning(object value)
			{
				lock (Console.Out)
				{
					Console.Write(DateTime.Now.ToString() + "[" + System.Threading.Thread.CurrentThread.ManagedThreadId + "] Warning; \t");
					Console.Write(this.Owner); 
					Console.WriteLine(value);
				}
			}
			public void Error(object value)
			{
				lock (Console.Out)
				{
					Console.Write(DateTime.Now.ToString() + "[" + System.Threading.Thread.CurrentThread.ManagedThreadId + "] Error; \t");
					Console.Write(this.Owner); 
					Console.WriteLine(value);
				}
			}
		}
#endif //SAGE_LOGGING
		#endregion Logging

		#region Fields/Properties

		static Thread()
		{
			s_TimeOffset = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			s_TimeStopwatch.Start();
		}

		static System.Diagnostics.Stopwatch s_TimeStopwatch = new System.Diagnostics.Stopwatch();
		static long s_TimeOffset;

		public static long Now
		{
			//get { return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; }
			get { return s_TimeOffset + s_TimeStopwatch.ElapsedMilliseconds; }
		}

		public static DateTime NowTime
		{
			get { return new DateTime(Now * TimeSpan.TicksPerMillisecond); }
		}

		private Pool _Pool;

		public Pool Pool
		{
			get { return _Pool; }
			protected set { _Pool = value; }
		}

		private System.Threading.Thread _Worker;

		public System.Threading.Thread Worker
		{
			get { return _Worker; }
			protected set { _Worker = value; }
		}

		public int ID
		{
			get { return this.Worker.ManagedThreadId; }
		}

		private SpecialThread _SpecialThreadType = SpecialThread.Unspecified;

		public SpecialThread SpecialThreadType
		{
			get { return this._SpecialThreadType; }
			set
			{
				if (this._SpecialThreadType != value)
				{
#if SAGE_LOGGING
					this.Logging.Message("SpecialThreadType changed from " + this.SpecialThreadType + " to " + value.ToString());
#endif//SAGE_LOGGING
					this._SpecialThreadType = value;
				}
			}
		}

		private LockfreeQueue<ScheduledTask> _MessageQueue = new LockfreeQueue<ScheduledTask>();

		protected LockfreeQueue<ScheduledTask> MessageQueue
		{
			get { return _MessageQueue; }
		}

		private PriorityQueue<ScheduledTask> _PriorityQueue = new PriorityQueue<ScheduledTask>();

		protected PriorityQueue<ScheduledTask> PriorityQueue
		{
			get { return _PriorityQueue; }
		}

		private bool _Running = true;

		public virtual bool Running
		{
			get { return _Running; }
			set { _Running = value; }
		}

		public virtual System.Threading.ThreadState ThreadState
		{
			get { return this.Worker != null ? this.Worker.ThreadState : ThreadState.Unstarted; }
		}

		private UtilizationCounter _Counter = new UtilizationCounter();

		protected UtilizationCounter Counter
		{
			get { return _Counter; }
		}

		public double Utilization
		{
			get { return this.Counter.Utilization; }
		}

		#endregion Fields/Properties

		#region Constructor

		public Thread(Pool manager)
			: this(manager, false)
		{
		}

		public Thread(Pool manager, bool keepThread)
		{
			if (keepThread)
				this.Worker = System.Threading.Thread.CurrentThread;
			else
				this.Worker = new System.Threading.Thread(this.Run);

			//this.Worker.TrySetApartmentState(ApartmentState.STA);

			this.Pool = manager;
			this.PriorityQueue.Comparer = delegate (ScheduledTask x, ScheduledTask y) { return (int)(x.Time - y.Time); };
#if SAGE_LOGGING
			this.Logging = new Log("Thread" + this.ID.ToString());
#endif//SAGE_LOGGING
			this.Logging.Message("Created thread.");
		}

		#endregion Constructor

		#region Task Processing

		public void Start()
		{
			this.Worker.Start();
		}

		[System.STAThread]
		public virtual void Run()
		{
			this.Logging.Message("Starting thread.");

			try
			{
				while (this.RunOne() || this.Running || this.PriorityQueue.Count > 0)
				{
					if (this.IsDisposed)
						break;
				}
			}
			catch (ThreadAbortException)
			{
				this.Logging.Warning("Thread aborted...");
			}
			this.Logging.Message("Thread finished.");
		}

		bool _Work = false;
		/// <summary>
		/// Whether the thread is currently working or not.
		/// </summary>
		public bool Work
		{
			get { return _Work; }
			protected set { _Work = value; }
		}

#if SAGE_DEBUG_THREADING
		System.Diagnostics.Stopwatch RunIntervalWatch = new System.Diagnostics.Stopwatch();
#endif

		int _CountWork = 0;
		protected internal bool RunOne()
		{
#if SAGE_DEBUG_THREADING
			// stop and check actual interval...
			RunIntervalWatch.Stop();
			if (RunIntervalWatch.ElapsedMilliseconds > DelayTolerance)
			{
				this.Logging.Warning("Thread polling slower than "+DelayTolerance+"ms: " + RunIntervalWatch.ElapsedMilliseconds + "ms");
			}
#endif

#if SAGE_DEBUG_THREADING
			RunIntervalWatch.Reset();
			RunIntervalWatch.Start();
#endif
			ScheduledTask task = null;

			// start timing...
			this.Counter.Start();

			// grab new tasks...
			this.ProcessMessageQueue();

			this.Work = false;

			// check for work...
			if (this.PriorityQueue.Count > 0)
			{
				// task found...
				task = this.PriorityQueue.Peek();

				long timestamp = Now;
				// compare timestamps...
				if (timestamp >= task.Time)
				{
#if SAGE_VERBOSE_THREADING
					//this.Logging.Message("Direct run task: " + task);
#endif
					this.Work = true;
				}
				else if (timestamp > task.Time - DelayTolerance)
				{
#if SAGE_VERBOSE_THREADING
					//this.Logging.Message("Active wait for task: " + task);
#endif
					while (Now < task.Time) ;
					this.Work = true;
				}
#if SAGE_VERBOSE_THREADING
				if (timestamp - task.Time > DelayTolerance * DelayToleranceFactor)
				{
					this.Logging.Warning("Task delayed[" + task.ToString() + "][ Thread Utilization: " + (int)(this.Utilization * 100.0) + "% ]");
					// [ Expected/Actual: " + task.Time + "/" + timestamp + " ][ Delay: " + (timestamp - task.Time) + "ms ]
					//this.Logging.Warning("Task delayed: desired=" + task.Time + ", actual=" + timestamp + ", delay=" + (timestamp - task.Time) + "ms");
				}
#endif
			}

			if (task != null && this.Work)
			{
				this.PriorityQueue.Pop();

				// work...
				this.Process(task);

				this.Work = false;

				// stop timing...
				this.Counter.Stop(UtilizationState.Work);
				this._CountWork++;
				if (this._CountWork > 5)
				{
					this.Logging.Verbose("Force sleep.");
					SleepShort();
					this._CountWork = 0;
				}
				return true;
			}
			else
			{
#if SAGE_VERBOSE_THREADING
				long start = Pool.Now;
#endif
				this._CountWork = 0;

				// idle...
				if (this.Worker.Priority == ThreadPriority.Highest || this.Worker.Priority == ThreadPriority.AboveNormal)
				{
					SleepShort();
				}
				else
				{
					Sleep();
				}

				// stop timing...
				this.Counter.Stop(UtilizationState.Idle);

#if SAGE_VERBOSE_THREADING
				long delta = Pool.Now - start;

				if (delta > ((this.Worker.Priority == ThreadPriority.Highest || this.Worker.Priority == ThreadPriority.AboveNormal) ? Sleep0Tolerance : Sleep10Tolerance))
				{
					this.Logging.Warning("Timing precision warning: Sleep(" + ((this.Worker.Priority == ThreadPriority.Highest || this.Worker.Priority == ThreadPriority.AboveNormal) ? 0 : 10) + ") took " + delta + "ms");
				}
#endif
				return false;
			}
		}

		public static void Sleep()
		{
			System.Threading.WaitHandle.WaitAny(handles, 10, false);
			//System.Threading.Thread.Sleep(10);
		}
		static WaitHandle[] handles = new WaitHandle[] { new AutoResetEvent(true) };

		public static void SleepShort()
		{
			System.Threading.WaitHandle.WaitAny(handles, 1, false);
			//System.Threading.Thread.Sleep(0);
		}

		protected virtual void ProcessMessageQueue()
		{
			ScheduledTask value;
			while ((value = this.MessageQueue.Pop()) != null)
				this.PriorityQueue.Push(value);
		}

		protected virtual void Process(ScheduledTask task)
		{
#if SAGE_DEBUG_THREADING
			this.Logging.Message("Executing " + task.Message.ToString() + ". [" + this.PriorityQueue.Count + (this.MessageQueue.IsEmpty ? "+" : "") + " remaining , " + task.Time + "-" + Pool.Now + "=" + (task.Time - Pool.Now) + "ms]");
#endif
			//try
			//{
			task.Run(this);
#if SAGE_DEBUG_THREADING
				this.Logging.Message("Executed " + task.Message.ToString());
#endif
			//	_FailCount = 0;
			//}
			//catch (Exception exc)
			//{
			//	this.Logging.Error("Exception from task: " + _FailCount + " " + task.Message.ToString() + "\n" + exc.ToString());
			//	if (_FailCount++ > 3)
			//		throw;
			//}
		}

		#endregion Task Processing

		#region Queueing
		public void QueuePool(Task.CallHandler call)
		{
			this.Queue(new ScheduledTask(call));
		}
		public void QueuePool(Task msg)
		{
			this.Queue(new ScheduledTask(msg));
		}
		public virtual void QueuePool(ScheduledTask task)
		{
			if (this.Pool == null || this.Pool == this)
			{
				this.Queue(task);
			}
			else
			{
				this.Pool.Queue(task);
			}
		}

		public virtual void Queue(ScheduledTask task)
		{
			if (!this.Running)
				this.Logging.Warning("Task " + task + " queued to stopped thread.");
			if (Pool.CurrentThreadID == this.ID)
			{
#if SAGE_DEBUG_THREADING
				this.Logging.Message("Queueing " + task.Message.ToString() + " at " + task.Time + " (direct). (in " + (task.Time - Pool.Now) + "ms)");
#endif
				this.PriorityQueue.Push(task);
			}
			else
			{
				lock (this.MessageQueue)
				{
#if SAGE_DEBUG_THREADING
					this.Logging.Message("Queueing " + task.Message.ToString() + " at " + task.Time + ".");
#endif
					this.MessageQueue.Push(task);
				}
			}
		}

		public void Queue(ITask msg, long time)
		{
			this.Queue(new ScheduledTask(msg, time, this.ID));
		}

		public void Queue(ITask msg)
		{
			this.Queue(msg, Pool.Now);
		}
		#endregion Queueing

		#region IDisposable Members

		private bool _IsDisposed = false;

		public bool IsDisposed
		{
			get { return _IsDisposed; }
			protected set { _IsDisposed = value; }
		}

		public virtual void Dispose()
		{
			if (this.IsDisposed)
				return;
			try
			{
				if (this.Worker.IsAlive)
					this.Worker.Abort();
			}
			catch (System.Threading.ThreadAbortException)
			{
				this.Logging.Error("Thread " + this.ID + " had to kill itself.");
			}
			this.IsDisposed = true;
		}

		#endregion

		#region Thread Interface

		public void Join(int timeout)
		{
			if (this.Worker.IsAlive)
				this.Worker.Join(timeout);
		}

		public void Join()
		{
			if (this.Worker.IsAlive)
				this.Worker.Join();
		}

		public bool IsAlive
		{
			get { return this.Worker.IsAlive; }
		}

		#endregion Thread Interface

		public override string ToString()
		{
			return base.ToString() + " " + this.ID + " " + this.SpecialThreadType;
		}

	}
}