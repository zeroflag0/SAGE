using System;
using System.Collections.Generic;
using System.Text;
using Sage.Editor;

namespace Sage.Editor.Actions
{
	public class ActionQueue
	{
		#region Queue
		LinkedList<Action> _Queue = new LinkedList<Action>();
		/// <summary>
		/// This queue contains the actions that will be executed next time Execute() or ExecuteOne() are called. (oldest first, newest last)
		/// </summary>
		protected LinkedList<Action> Queue
		{
			get { return _Queue; }
		}

		public ActionQueue Enqueue(Action action)
		{
			if (action != null)
				lock (this.Queue)
				{
					this.Queue.AddLast(action);
				}
			return this;
		}

		protected Action Dequeue()
		{
			if (this.AreActionsQueued)
			{
				lock (this.Queue)
				{
					if (this.Queue.First != null)
					{
						Action action = this.Queue.First.Value;
						this.Queue.RemoveFirst();
						return action;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Whether there are actions queued for execution.
		/// </summary>
		public bool AreActionsQueued
		{
			get
			{
				return this.QueueLength > 0;
			}
		}

		public int QueueLength
		{
			get
			{
				return this.Queue.Count;
			}
		}

		/// <summary>
		/// Execute the next action in the queue.
		/// This method will not throw any exception if the queue is empty, it will simply return. Use <seealso cref="AreActionsQueued"/> for a way to check when the queue is empty.
		/// </summary>
		/// <returns></returns>
		public ActionQueue ExecuteOne()
		{
			Action action = this.Dequeue();
			if (action != null)
			{
				action.PerformDo();
			}
			return this;
		}



		#endregion

		#region History
		LinkedList<Action> _History = new LinkedList<Action>();
		/// <summary>
		/// This list contains previously executed actions. (newest first, oldest last)
		/// </summary>
		public LinkedList<Action> History
		{
			get { return _History; }
		}

		public int HistoryLength
		{
			get
			{
				return this.History.Count;
			}
		}

		#region HistoryLimit
		private int? _HistoryLimit;

		/// <summary>
		/// The number of items to be kept in history.
		/// </summary>
		public int HistoryLimit
		{
			get { return _HistoryLimit ?? (int)(_HistoryLimit = this.HistoryLimitCreate); }
			set { this._HistoryLimit = value; }
		}

		/// <summary>
		/// Creates the default/initial value for HistoryLimit.
		/// The number of items to be kept in history.
		/// </summary>
		protected virtual int HistoryLimitCreate
		{
			get { return 100; }
		}

		#endregion HistoryLimit


		public Action LastAction
		{
			get
			{
				if (this.History.Count > 0)
					lock (this.History)
					{
						if (this.History.First != null)
							return this.History.First.Value;
					}
				return null;
			}
		}

		#endregion

		#region Previous
		LinkedList<Action> _Previous = new LinkedList<Action>();
		/// <summary>
		/// This list contains previously executed actions. (newest first, oldest last)
		/// </summary>
		public LinkedList<Action> Previous
		{
			get { return _Previous; }
		}

		public int PreviousLength
		{
			get
			{
				return this.Previous.Count;
			}
		}

		public Action PreviousAction
		{
			get
			{
				if (this.Previous.Count > 0)
					lock (this.Previous)
					{
						if (this.Previous.First != null)
							return this.Previous.First.Value;
					}
				return null;
			}
		}

		#endregion
	}
}
