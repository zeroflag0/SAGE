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

using Sage;
using Sage.Modules;
namespace SageApplication
{
	class CpuLoadModule : Module
	{
		static int Instance = 0;
		string _Name = "CpuLoad" + Instance++;
		public override string Name
		{
			get { return _Name; }
		}

		protected override DependencyList DependenciesInit
		{
			get
			{
				return new DependencyList(this, new Dependency<IModule>());
			}
		}

		public override long Interval
		{
			get
			{
				return 50;
			}
			set
			{
			}
		}

		protected override void DoInitialize()
		{
		}

		int Test = int.MinValue;
		protected override void DoUpdate(long timeSinceLastUpdate)
		{
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Reset();
			watch.Start();

			Random rand = new Random((int)DateTime.Now.Ticks);
			//this.Logging.Message("Faking work...");
			for (int i = 0; i < int.MaxValue / 200 && watch.ElapsedMilliseconds < this.Interval / 2; i++)
			{
				Test += rand.Next(0, 10); ;
				if (Test >= int.MaxValue - 10)
					Test = int.MinValue;
			}
			watch.Stop();
		}

		protected override void DoShutdown()
		{
			this.Log.Message("Shutting down...");
		}
	}
}
