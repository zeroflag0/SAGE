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

namespace Sage.Threading
{
	public class CyclicTask : ScheduledTask
	{
		private Thread _TargetThread;

		public Thread TargetThread
		{
			get { return this._TargetThread; }
			set { this._TargetThread = value; }
		}

		#region Constructor
		public CyclicTask(Thread target, ITask msg, long time, int threadID)
			: base(msg, time, threadID)
		{
			this.TargetThread = target;
		}

		public CyclicTask(Thread target, ITask msg, long time)
			: this(target, msg, time, -1)
		{
		}

		public CyclicTask(Thread target, ITask msg, int threadID)
			: this(target, msg, 0, threadID)
		{
		}

		public CyclicTask(Thread target, ITask msg)
			: this(target, msg, 0)
		{
		}
		#endregion Constructor

		public override void Run()
		{
			base.Run();
		}
	}
}
