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
	public class TaskGroup : ScheduledTask
	{
		private List<ScheduledTask> _Tasks;

		/// <summary>
		/// The tasks in the group. All of these will be queued to the threadpool before the group's task is run.
		/// </summary>
		public List<ScheduledTask> Tasks
		{
			get { return _Tasks; }
			set { _Tasks = value; }
		}

		#region Construction
		public TaskGroup(ITask msg, long time, int threadID)
			: base(msg, time, threadID)
		{
		}

		public TaskGroup(ITask msg, long time, SpecialThread type)
			: base(msg, time, type)
		{
		}

		public TaskGroup(ITask msg, long time)
			: base(msg, time)
		{
		}

		public TaskGroup(ITask msg, int threadID)
			: base(msg, threadID)
		{
		}

		public TaskGroup(ITask msg)
			: base(msg)
		{
		}

		public TaskGroup()
			: this(null)
		{
		}
		#endregion Construction

		public override void Run()
		{
			foreach (ScheduledTask sub in this.Tasks)
			{
				this.Thread.QueuePool(sub);
			}
			base.Run();
		}
	}
}
