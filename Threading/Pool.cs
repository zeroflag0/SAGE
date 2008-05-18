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

using System.Collections.Generic;

namespace Sage.Threading
{
	public class Pool : Thread, System.ComponentModel.IComponent
	{
		public const int PoolStopJoin = 1000;
		#region Properties
		private List<Thread> _Threads = new List<Thread>();
		/// <summary>
		/// The threads in the order in which they were created. (sage internal thread ID)
		/// </summary>
		protected List<Thread> Threads
		{
			get { return _Threads; }
		}

		private List<int> _ThreadIDs = new List<int>();
		/// <summary>
		/// The managed(.NET) thread IDs for the threads contained in the pool - in order of creation.
		/// </summary>
		protected List<int> ThreadIDs
		{
			get { return this._ThreadIDs; }
		}

		private Dictionary<SpecialThread, int> _SpecialThreads = new Dictionary<SpecialThread, int>();

		protected Dictionary<SpecialThread, int> SpecialThreads
		{
			get { return this._SpecialThreads; }
		}

		private int _ThreadCount = 4;

		public int ThreadCount
		{
			get { return this._ThreadCount; }
			set { this._ThreadCount = value; }
		}

		public static int CurrentThreadID
		{
			get { return System.Threading.Thread.CurrentThread.ManagedThreadId; }
		}
		#endregion Properties

		#region Constructor

		public Pool()
			: base(null)
		{
			this.Pool = this;
			this.Logging.Owner = "ThreadPool";
		}

		#endregion Constructor

		#region Control Methods
		private bool _Initialized = false;
		public void Initialize()
		{
			if (this._Initialized)
			{
				this.Logging.Message("Pool already initialized.");
				return;
			}
			this.Logging.Message("Initializing ThreadPool...");

			System.AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			// register a pool thread...
			Thread main = new Thread(this, true);
			main.Worker.Priority = System.Threading.ThreadPriority.Highest;
			main.SpecialThreadType = SpecialThread.MainThread;
			this.Threads.Add(main);
			this.ThreadIDs.Add(main.ID);
			this.SpecialThreads.Add(SpecialThread.MainThread, this.ThreadIDs.IndexOf(main.ID));

			int specialThread = 1;
			while (this.Threads.Count < this.ThreadCount)
			{
				// create a new thread in the pool...
				Thread thread = new Thread(this);
				// add it...
				this.Threads.Add(thread);

				int i = this.Threads.IndexOf(thread);
				// make sure the threadIDs collection is large enough...
				while (this.ThreadIDs.Count <= i)
					this.ThreadIDs.Add(-1);
				// remember it's ID...
				this.ThreadIDs[i] = thread.ID;


				if (specialThread < 3)
				{
					thread.SpecialThreadType = (SpecialThread)specialThread;
					this.SpecialThreads.Add(thread.SpecialThreadType, i);
				}
				specialThread++;
			}

			// register a pool thread...
			this.ThreadIDs.Add(this.ID);
			this.Threads.Add(this);
			this.SpecialThreads.Add(SpecialThread.PoolThread, this.ThreadIDs.IndexOf(this.ID));
			this.SpecialThreadType = SpecialThread.PoolThread;

			this._Initialized = true;
			this.Logging.Message("Initialized ThreadPool.");
		}

		void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
		{
			this.Logging.Error("Unhandled exception:\n" + e.ExceptionObject.ToString());
		}

		public override void Dispose()
		{
			//if (this.ID != CurrentThreadID)
			//{
			//    this.Logging.Message("Threadpool(" + this.ID + ") dispose on wrong thread(" + CurrentThreadID + "), requeuing...");
			//    this.Queue(new Task(new Message(this.Dispose), this[SpecialThread.PoolThread].ID));
			//}
			//    foreach (Thread thread in this.Threads)
			//    {
			//        try
			//        {
			//            thread.Join();
			//        }
			//        catch { }
			//    }
			//    return;
			//}
			if (this.IsDisposed)
			{
				this.Logging.Warning("Threadpool already disposed.");
				return;
			}
			this.IsDisposed = true;

			this.Logging.Message("Threadpool disposing...");
			this.Logging.Message("Requesting threads to stop...");
			foreach (Thread thread in this.Threads)
			{
				thread.Running = false;
			}
			try
			{
				Thread current = this[CurrentThreadID, true];
				while (current.RunOne())
				{
					this.Logging.Message("Processing messages for thread " + CurrentThreadID + "...");
				}
			}
			catch (System.Exception exc)
			{
				this.Logging.Error("Processing messages for thread " + CurrentThreadID + ": " + exc);
			}

			this.Logging.Message("Joining for " + PoolStopJoin + "ms...");
			foreach (Thread thread in this.Threads)
			{
				thread.Join(PoolStopJoin);
			}
			Thread main = this[SpecialThread.MainThread];
			this.Logging.Message("Scanning for live threads...");
			foreach (Thread thread in this.Threads)
			{
				try
				{
					if (thread != this && thread != main && thread.ID != CurrentThreadID)
					{
						this.Logging.Message("Joining thread " + thread.ID + " for " + PoolStopJoin + "ms...");
						thread.Join(PoolStopJoin);
						thread.Logging.Warning("Thread " + thread.ID + " disposing...");
						thread.Dispose();
						thread.Logging.Warning("Thread " + thread.ID + " disposed.");
					}
				}
				catch (System.Threading.ThreadAbortException)
				{
					thread.Logging.Warning("Thread " + thread.ID + " had to be killed.");
				}
			}
			//this.Threads.Clear();
			this.Logging.Message("Threadpool disposed.");

			if (this != main)
				base.Dispose();

			this.OnDisposed(this, new System.EventArgs());
		}

		public override void Run()
		{
			this.Initialize();
			this.Logging.Message("Threadpool starting... ");

			foreach (Thread thread in this.Threads)
			{
				if (this.ThreadState != System.Threading.ThreadState.Unstarted)
					continue;
				if (thread.SpecialThreadType != SpecialThread.MainThread)
					thread.Start();
			}
			if (Current.SpecialThreadType == SpecialThread.MainThread)
			{
				Thread main = this[SpecialThread.MainThread];
				this.Logging.Message("Passing mainthread to " + main);
				main.Run();
				this.Logging.Message("Mainthread returned from " + main);
			}
			else if (Current.SpecialThreadType == SpecialThread.PoolThread)
			{
				this.Logging.Message("Starting base thread message pump.");
				base.Run();
				this.Logging.Message("Base thread message pump returned.");
			}

			try
			{
				if (this.Worker.IsAlive)
				{
					this.Worker.Join(2000);
					//if (this.Worker.IsAlive)
					//    this.Worker.Abort();
				}
			}
			catch (System.Threading.ThreadAbortException exc)
			{
				this.Logging.Warning(exc);
			}
		}
		#endregion Control Methods

		#region Queueing

		public new void Queue(ScheduledTask task)
		{
			if (task == null)
			{
				this.Logging.Warning("Attempt to add null as task.\n" + (new System.Diagnostics.StackTrace()).ToString());
				return;
			}
			if (task.ThreadID >= 0)
			{
				this.Threads[task.ThreadID].Queue(task);
			}
			else if (task.ThreadType != SpecialThread.Unspecified)
			{
				this[task.ThreadType].Queue(task);
			}
			//else if (CurrentThreadID == this.ID)
			//{
			//    this.PriorityQueue.Push(task);
			//}
			else if (this.ThreadIDs.Contains(CurrentThreadID))
			{
				this.Threads[this.ThreadIDs.IndexOf(CurrentThreadID)].Queue(task);
			}
			else
			{
				this.PriorityQueue.Push(task);
			}
		}

		//public void Queue(IMessage msg)
		//{
		//    this.Queue(msg, Now);
		//}

		//public void Queue(IMessage msg, long time)
		//{
		//    this.Queue(new Task(msg, time));
		//}

		//public void Queue(IMessage msg, int threadID)
		//{
		//    this.Queue(new Task(msg, threadID));
		//}

		#endregion Queueing

		#region Processing

		#endregion Processing

		#region Thread Access

		public Thread Current
		{
			get { return this[CurrentThreadID, true]; }
		}

		public Thread this[int threadID, bool managedID]
		{
			get
			{
				if (managedID)
					return this[this.ThreadIDs.IndexOf(threadID)];
				else
					return this[threadID];
			}
		}

		public Thread this[int threadID]
		{
			get
			{
				return this.Threads[threadID];
			}
		}

		public Thread this[SpecialThread type]
		{
			get
			{
				if (type != SpecialThread.AutoBalance &&
					type != SpecialThread.Unspecified &&
					this.SpecialThreads.ContainsKey(type))
					return this.Threads[this.SpecialThreads[type]];
				else
				{
					return this.SelectBalancedThread();
				}
			}
		}

		SpecialThread _BalanceThreshold = SpecialThread.PrimaryWorker;
		/// <summary>
		/// The threshold after which load balancing will be effective.
		/// e.g. if BalanceThreshold is MainThread, only the MainThread will NOT be used for balancing.
		/// </summary>
		public SpecialThread BalanceThreshold
		{
			get { return _BalanceThreshold; }
			set { _BalanceThreshold = value; }
		}

		//TODO: make scheduling it more random/balanced...
		/// <summary>
		/// Last thread auto-selected. just to make sure we don't always bother the same thread...
		/// </summary>
		Thread _Last = null;

		long _LastAutoSelect = 0;

		/// <summary>
		/// Checks all threads whether they are busy and how their utilization is. Picks the one that has nothing to do or the least utilization. Ignores threads below the BalanceThreshold.
		/// </summary>
		/// <returns>The least busy thread.</returns>
		/// <seealso cref="BalanceThreshold"/>
		public virtual Thread SelectBalancedThread()
		{
			//TODO: implement proper load balancing...
			//FIXED: hiccups. seem to be caused by the auto-balancer. maybe implement a TimingType (to keep fast and slow threads seperated).

			if (_Last != null && Pool.Now - _LastAutoSelect > 100)
				_Last = null;

			Thread selected = null;
			for (int i = this.Threads.Count - 1; i >= 0; i--)
			{
				Thread thread = this.Threads[i];
				if (
					(// the thread is not the pool's thread and is above the balance threashold...
					thread.SpecialThreadType != SpecialThread.PoolThread &&
					thread.SpecialThreadType > this.BalanceThreshold
					) && ((
					// and we currently have no thread selected...
					selected == null &&
					thread.SpecialThreadType != SpecialThread.PoolThread &&
					thread.SpecialThreadType > this.BalanceThreshold
					) || (
					// or the selected thread is more busy than the current one...
					(
					(!thread.Work && selected.Work) ||
					thread.Utilization <= selected.Utilization)
					&&
					thread != _Last
					))
					)
					selected = thread;
			}
			//if (selected == null)
			//// something's wrong...
			//{
			//    this.Logging.Error("No thread autoselected.");

			//    // try to select a thread below the balance threshold...
			//    for (int i = (int)this.BalanceThreshold; selected == null && i >= 0; i--)
			//    {
			//        selected = this[(SpecialThread)i];
			//    }
			//}
#if SAGE_DEBUG_THREADING
			this.Logging.Message("Auto-selected thread " + selected);
#endif
			_Last = selected;
			return selected;
		}

		#endregion Thread Access

		#region IComponent Members

		public event System.EventHandler _Disposed;

		public event System.EventHandler Disposed
		{
			add { _Disposed += value; }
			remove { _Disposed -= value; }
		}

		protected virtual void OnDisposed(object sender, System.EventArgs e)
		{
			if (this._Disposed != null)
			{
				this._Disposed(sender, e);
			}
		}

		System.ComponentModel.ISite _Site;

		public System.ComponentModel.ISite Site
		{
			get { return _Site; }
			set { _Site = value; }
		}

		#endregion
	}
}