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

namespace Sage.Modules.Logging
{
	public class LogModule : Sage.Modules.Module
	{
		List<IWriter> _Writers = new List<IWriter>();

		public List<IWriter> Writers
		{
			get { return _Writers; }
			protected set { _Writers = value; }
		}

		List<ILog> _Logs = new List<ILog>();

		public List<ILog> Logs
		{
			get { return _Logs; }
			protected set { _Logs = value; }
		}

		public override string Name
		{
			get { return "Logging"; }
		}

		public override long Interval
		{
			get
			{
				return 200;
			}
			set
			{
				base.Interval = value;
			}
		}

		protected override void DoInitialize()
		{
			if (this.Writers.Count <= 0)
				// add a default writer...
				this.Writers.Add(new ConsoleWriter());
		}

		protected override void DoUpdate(long timeSinceLastUpdate)
		{
			Sage.Threading.ITask msg;
			foreach (ILog log in this.Logs)
			{
				while ((msg = log.Socket.Pop()) != null)
					msg.Run();
			}
			foreach (IWriter writer in this.Writers)
				writer.Flush();
		}

		public void Write(DateTime time, string owner, string value)
		{
			foreach (IWriter writer in this.Writers)
			{
				writer.Write(time, owner, value);
			}
		}

		int _ShutdownWait = 0;
		Module _ShutdownWaiting = null;

		protected override void DoShutdown()
		{
			if (!this._Explicit)
			{
				if (this.ScheduleState != ScheduleState.Running)
				{
					//this.Write(Threading.Thread.NowTime, "Log", "refusing shutdown.");
					this.ScheduleState = ScheduleState.Running;
				}
			}
			else
			{
				this.Write(Threading.Thread.NowTime, "Log", "shutting down...");

				this.DoUpdate(0);
				//System.Threading.Thread.Sleep(100);
				foreach (Module mod in this.Core.Modules)
				{
					if (mod != this && mod.ScheduleState != ScheduleState.Disposed)
					{
						if (mod != _ShutdownWaiting || (_ShutdownWait % 10) == 0)
							this.Write(Threading.Thread.NowTime, "Log", "waiting for " + mod.Name + " to shut down...");
						_ShutdownWaiting = mod;
						break;
					}
					else if (mod == _ShutdownWaiting)
					{
						this.Write(Threading.Thread.NowTime, "Log", mod.Name + " has shut down.");
						_ShutdownWaiting = null;
					}
				}

				if (this._ShutdownWait++ > 100 || this._ShutdownWaiting == null)
				{
					this.ScheduleState = ScheduleState.Disposed;

					this.Write(Threading.Thread.NowTime, "Log", "shutdown.");
					foreach (IWriter writer in this.Writers)
						writer.Dispose();
				}
			}
		}

		bool _Explicit = false;
		public void ExplicitShutdown()
		{
			this.Write(Threading.Thread.NowTime, "Log", "explicit shutdown.");

			this._Explicit = true;
			this._ShutdownWaiting = null;
			this._ShutdownWait = 0;
			this.ScheduleState = ScheduleState.Shutdown;
			if (this.Scheduler != null)
			{
				this.Scheduler.TimingMode = TimingMode.BruteForce;
				this.Scheduler.Run();
			}
		}
	}
}
