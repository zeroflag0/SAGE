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

using Sage.Basics;
using Sage.Threading;

namespace Sage.Modules
{
	public class Scheduler : ScheduledTask
	{
		#region Properties
		IScheduleTarget _Target;
		/// <summary>
		/// The target which should be scheduled.
		/// </summary>
		public IScheduleTarget Target
		{
			get { return _Target; }
			set { _Target = value; }
		}

		long _LastUpdate = 0;

		protected long LastUpdate
		{
			get { return _LastUpdate; }
			set { _LastUpdate = value; }
		}

		public override Thread Thread
		{
			get { return base.Thread ?? this.Core.Threads[this.ThreadType]; }
			set { base.Thread = value; }
		}

		Core _Core;

		public Core Core
		{
			get { return _Core; }
			set { _Core = value; }
		}

		public TimingMode TimingMode
		{
			get { return this.Target.TimingMode; }
			set { this.Target.TimingMode = value; }
		}

		/// <summary>
		/// The interval between updates in miliseconds.
		/// </summary>
		public long Interval
		{
			get { return this.Target.Interval; }
			set { this.Target.Interval = value; }
		}

		#endregion Properties
		#region Constructor
		public Scheduler(IScheduleTarget target, Core core)
			: base((ITask)null)
		{
			this.Target = target;
			this.Core = core;

			base.Message = this;
		}
		#endregion Constructor

		#region Schedule
		/// <summary>
		/// Run the scheduler.
		/// </summary>
		public override void Run()
		{
			if (this.Thread == null)
				return;
			if (!this.Thread.Pool.Running && this.Target.ScheduleState != ScheduleState.Disposed && this.Target.ScheduleState != ScheduleState.Shutdown)
				this.Target.ScheduleState = ScheduleState.Shutdown;

			long callTime = Pool.Now;
			long timeSinceLastUpdate = callTime - this.LastUpdate;

			//((Module)this.Target).Logging.Message("Update: now=" + callTime + ", last=" + this.LastUpdate + ", delta=" + timeSinceLastUpdate);

			this.LastUpdate = callTime;

			if (this.TimingMode == TimingMode.BruteForce && this.Target.ScheduleState != ScheduleState.Shutdown)
			// if we aren't waiting for the update to finish, set time right away...
			{
				this.Time = callTime + this.Interval;

				// in brute force mode, reschedule right away...
				this.Reschedule();
			}

			if (!this.Update(timeSinceLastUpdate))
				// if we're not supposed to reschedule...
				return;

			if (this.TimingMode == TimingMode.Constant)
			{
				this.Time = callTime + this.Interval;
#if SAGE_DEBUG_THREADING
				if (this.Time - Pool.Now < this.Interval * Thread.DelayToleranceFactor)
				{
					((Module)this.Target).Log.Warning("Rescheduling late... schedule=" + this.Time + ", now=" + Pool.Now + ", delta=" + (this.Time - Pool.Now) + "ms");
				}
#endif
			}
			else if (this.TimingMode == TimingMode.Wait)
			// if we are supposed to wait for the update, get the time now...
			{
				this.Time = Pool.Now + this.Interval;
			}


			if (this.TimingMode != TimingMode.BruteForce)
			// if we're not in bruteforce mode, we still need to reschedule the scheduler...
			{
				this.Reschedule();
			}
		}

		/// <summary>
		/// Schedules the scheduler on a thread.
		/// </summary>
		protected internal void Reschedule()
		{
			if (this.ThreadID < 0 &&
				this.ThreadType == SpecialThread.Unspecified
				||
				this.ThreadType == SpecialThread.AutoBalance)
			{
				this.Thread = this.Thread.Pool.SelectBalancedThread();
			}
			this.Thread.Queue(this);
		}

		/// <summary>
		/// Perform the next scheduled step.
		/// </summary>
		/// <param name="timeSinceLastUpdate">The time since update was called last.</param>
		/// <returns>Whether to reschedule or to exit.</returns>
		protected bool Update(long timeSinceLastUpdate)
		{
			switch (this.Target.ScheduleState)
			{
				case ScheduleState.Initialization:
					try
					{
						if (this.Target.Initialize())
						{
							this.Target.ScheduleState = ScheduleState.Running;
						}
					}
					catch (Exception exc)
					{
						((Module)this.Target).Log.Error(exc);
						this.Target.ScheduleState = ScheduleState.Initialization;
					}
					return true;
				case ScheduleState.Running:
					this.Target.Update(timeSinceLastUpdate);
					return true;
				case ScheduleState.Paused:
					return true;
				case ScheduleState.Shutdown:
					this.Target.Shutdown();
					if (this.Target.ScheduleState == ScheduleState.Shutdown)
						this.Target.ScheduleState = ScheduleState.Disposed;
					else
						this.Reschedule();
					return false;
				default:
					return false;
			}
		}

		//TODO: schedule state management.

		public void Start()
		{
			this.Time = Pool.Now;
			if (this.Thread != null)
				this.Thread.Queue(this);
			else
				return;
		}

		public void Stop()
		{
			this.Target.ScheduleState = ScheduleState.Shutdown;
		}
		#endregion Schedule

		public override string ToString()
		{
			return "Time=" + this.Time + " " + "Eta=" + (this.Time - Pool.Now) + "ms " + "[" + this.Target.ToString() + ", " + this.Target.ScheduleState + ", " + this.Interval + "]";
			//return "Scheduler [" + this.Target.ToString() + ", " + this.Target.ScheduleState + ", " + this.Interval + "]";
		}
	}
}
