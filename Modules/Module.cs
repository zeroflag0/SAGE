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

using Sage.Modules.Logging;

namespace Sage.Modules
{
	/// <summary>
	/// Modules provide major and minor components. Modules can be scheduled into the threading system and are designed as simple finite state machines (runtime states). They also provide a dependency system that controls initialization orders.
	/// </summary>
	public abstract partial class Module : IScheduleTarget, Sage.Modules.IModule
	{
		#region Logging

		//DONE: implement logging... (Sage.Modules.Logging)

		private Log _Log;
		[System.ComponentModel.ReadOnly(true)]
		public Log Log
		{
			get { return _Log; }
			set { _Log = value; }
		}

		#endregion Logging

		#region Properties

		public virtual string Name { get { return this.GetType().ToString(); } }

		private Core _Core;
		[System.ComponentModel.ReadOnly(true)]
		public Core Core
		{
			get { return _Core; }
			set
			{
				if (_Core != value)
				{
					_Core = value;
					if (_Core != null)
					{
						#region Scheduling
						this.Scheduler = new Scheduler(this, this.Core);
						#endregion Scheduling

						this.Log.Module = this.Core.LogModule;

						if (!this.Core.Modules.Contains(this))
						{
							this.Log.Verbose("Implicitely registering with core.");
							this.Core.Modules.Add(this);
						}
					}
				}
			}
		}

		#endregion Properties

		#region Constructor

		protected Module()
		{
			#region Logging

			this.Log = new Log(this.Name);

			#endregion

			this.PrepareCommunication();
		}

		#endregion Constructor

		#region Dependencies

		#endregion Dependencies

		#region Features
		private zeroflag.Collections.List<IFeatureFactory> _Features;

		/// <summary>
		/// Factories for the Features provided by this module.
		/// </summary>
		public zeroflag.Collections.List<IFeatureFactory> Features
		{
			get { return _Features ?? (_Features = this.FeaturesCreate); }
		}

		/// <summary>
		/// Creates the default/initial value for Features.
		/// Factories for the Features provided by this module.
		/// </summary>
		protected virtual zeroflag.Collections.List<IFeatureFactory> FeaturesCreate
		{
			get { return new zeroflag.Collections.List<IFeatureFactory>(); }
		}

		#endregion Features

		public virtual Threading.SpecialThread ThreadType
		{
			get { return Sage.Threading.SpecialThread.AutoBalance; }
		}

		#region IScheduleTarget Members

		long _Interval = 1000;
		/// <summary>
		/// The interval between updates in miliseconds.
		/// </summary>
		public virtual long Interval
		{
			get { return _Interval; }
			set { _Interval = value; }
		}

		TimingMode _TimingMode = TimingMode.Constant;
		[System.ComponentModel.ReadOnly(true)]
		public TimingMode TimingMode
		{
			get { return _TimingMode; }
			set { _TimingMode = value; }
		}

		Scheduler _Scheduler;
		[System.ComponentModel.ReadOnly(true)]
		public Scheduler Scheduler
		{
			get { return _Scheduler; }
			protected set { _Scheduler = value; }
		}

		ScheduleState _ScheduleState = ScheduleState.Initialization;
		[System.ComponentModel.ReadOnly(true)]
		public virtual ScheduleState ScheduleState
		{
			get { return _ScheduleState; }
			set
			{
				if (_ScheduleState != value)
				{
					this.Log.Verbose("Schedule state changing from " + _ScheduleState + " to " + value);
#if SAGE_VERBOSE
					if (_ScheduleState == ScheduleState.Disposed && value == ScheduleState.Shutdown)
					    this.Log.Warning("Trying to shut down again!");
#endif//SAGE_VERBOSE
					_ScheduleState = value;
				}
			}
		}

		bool _InitWaiting = false;
		public bool Initialize()
		{
			IModule mod = this.Dependencies.Ready(ScheduleState.Running);

			if (mod != null)
			{
				if (!_InitWaiting)
				{
					this.Log.Message("Dependencies (" + mod + ") not ready, waiting...");
					mod.Socket.Push(delegate
					{
						this.Log.Verbose("Dependencies (" + mod + ") is ready, resuming...");
						this.Scheduler.Reschedule();
					});
					this.ScheduleState = ScheduleState.Initialization;
					_InitWaiting = true;
				}
				return false;
			}

			this.Log.Message("Dependencies (" + mod + ") ready, initializing...");

			this.DoInitialize();
			return true;
		}

		public void Shutdown()
		{
			this.DoShutdown();
		}

		public void Update(long timeSinceLastUpdate)
		{
			this.ProcessMessages();

			if (timeSinceLastUpdate > this.Interval * Threading.Thread.DelayToleranceFactor + Threading.Thread.DelayTolerance)
			{
				if (timeSinceLastUpdate > this.Interval * Threading.Thread.DelayToleranceFactor * 2 + Threading.Thread.DelayTolerance * 2)
					this.Log.Warning("Update delayed. [ Expected Delay: " + this.Interval + "ms ][ Actual Delay: " + timeSinceLastUpdate + "ms ][ Thread Utilization: " + (int)(this.Scheduler.Thread.Utilization * 100) + "% ]");
				else
					this.Log.Verbose("Update delayed. [ Expected Delay: " + this.Interval + "ms ][ Actual Delay: " + timeSinceLastUpdate + "ms ][ Thread Utilization: " + (int)(this.Scheduler.Thread.Utilization * 100) + "% ]");
			}
			this.DoUpdate(timeSinceLastUpdate);
		}

		protected abstract void DoInitialize();

		protected abstract void DoUpdate(long timeSinceLastUpdate);

		protected abstract void DoShutdown();

		#endregion


		#region Dependencies
		/// <summary>
		/// Creates a new DependencyList. Is only called once, the first time Dependency is accessed.
		/// </summary>
		protected virtual DependencyList DependenciesInit
		{
			get
			{
				return new DependencyList(this);
			}
		}

		DependencyList _Dependencies = null;

		public DependencyList Dependencies
		{
			get
			{
				if (this._Dependencies == null)
				{
					this._Dependencies = this.DependenciesInit;
				}
				return this._Dependencies;
			}
		}

		#endregion

		public override string ToString()
		{
			return this.Name + "[" + base.ToString() + "]";
		}
	}
}