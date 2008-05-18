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

#define OPTIMIZE_TIMING

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Sage.Threading
{
	public class UtilizationCounter
	{
		Queue<UtilizationItem> _Items = new Queue<UtilizationItem>();

		protected Queue<UtilizationItem> Items
		{
			get { return _Items; }
		}

		public UtilizationCounter()
		{
			//this.Items.Enqueue(new UtilizationItem(UtilizationState.Idle, 1));
		}

		int _HistorySize = 50;

		/// <summary>
		/// How many values should be remembered for the average calculation?
		/// </summary>
		public int HistorySize
		{
			get { return _HistorySize; }
			set { _HistorySize = value; }
		}

		Stopwatch _Watch = new Stopwatch();

		protected Stopwatch Watch
		{
			get { return _Watch; }
		}

		UtilizationState _State = UtilizationState.Idle;

		/// <summary>
		/// The current state of the thread.
		/// </summary>
		public UtilizationState State
		{
			get { return _State; }
			set { _State = value; }
		}

		/// <summary>
		/// Start timing a utilization item.
		/// </summary>
		/// <param name="state">The current thread state.</param>
		public void Start(UtilizationState state)
		{
			this.State = state;
			this.Start();
		}

		/// <summary>
		/// Start timing a utilization item.
		/// </summary>
		public virtual void Start()
		{
			this.Watch.Stop();
			this.Watch.Reset();
			this.Watch.Start();
		}

		/// <summary>
		/// Stop timing for the current item and push it to the history.
		/// </summary>
		/// <param name="state">The current thread state.</param>
		public virtual void Stop(UtilizationState state)
		{
			this.Watch.Stop();
			this.State = state;
			this.Push();
		}

		/// <summary>
		/// Stop timing for the current item and push it to the history.
		/// </summary>
		public virtual void Stop()
		{
			this.Watch.Stop();
			this.Push();
		}

		protected virtual void Push()
		{
			this.Items.Enqueue(new UtilizationItem(this.State, this.Watch.ElapsedMilliseconds));

			// bring down the memory size to the specified limit...
			while (this.Items.Count > this.HistorySize)
			{
				this.Items.Dequeue();
			}

#if !OPTIMIZE_TIMING
			double work = 0.0;
			double all = 0.0;

			foreach (UtilizationItem item in this.Items)
			{
				if (item.State == UtilizationState.Idle)
				{
					// add up idle times...
					all += item.Duration;
				}
				else
				{
					// add up work times...
					work += item.Duration;
				}
			}

			// complete duration of all items...
			all += work;

			// utilization...
			System.Threading.Interlocked.Exchange(ref this._Utilization, work / all);
#endif
		}

#if OPTIMIZE_TIMING

		/// <summary>
		/// The utilization calculated by this counter.
		/// </summary>
		public double Utilization
		{
			get
			{
				if (this.Items.Count > 0)
				{
					double work = 0.0;
					double all = 0.0;
					UtilizationItem[] items = this.Items.ToArray();

					foreach (UtilizationItem item in items)
					{
						if (item.State == UtilizationState.Idle)
						{
							// add up idle times...
							all += item.Duration;
						}
						else
						{
							// add up work times...
							work += item.Duration;
						}
					}

					// complete duration of all items...
					all += work;

					// utilization...
					return work / all;
				}
				else
				{
					return 1;
				}
			}
		}
#else
		double _Utilization = 0.0;

		/// <summary>
		/// The utilization calculated by this counter.
		/// </summary>
		public double Utilization
		{
			get { return _Utilization; }
			protected set { _Utilization = value; }
		}
#endif
	}
}
