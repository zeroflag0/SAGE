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
using Sage.Modules.Connections;
using Sage.Threading;

namespace SageApplication
{
	class GraphicsTest : Module
	{
		static int Instance = 0;
		string _Name = "GraphicsTest" + Instance++;
		public override string Name
		{
			get { return _Name; }
		}

		protected override DependencyList DependenciesInit
		{
			get
			{
				return new DependencyList(this, new Dependency<IGraphicsModule>(true));
			}
		}
		public override long Interval
		{
			get
			{
				return 20;
			}
			set
			{
				base.Interval = value;
			}
		}

		Sage.Graphics.Module _Graphics = null;

		public Sage.Graphics.Module Graphics
		{
			get { return _Graphics; }
			set { _Graphics = value; }
		}

		//Sage.World.Manager _World = null;

		protected override void DoInitialize()
		{
			this.Graphics = this.Core.Modules.Get<Sage.Graphics.Module>();

			this.Connect(Graphics);
		}

		protected override void DoUpdate(long timeSinceLastUpdate)
		{
			//this.Sender[this.Graphics].Push(new Message<float>(new Message<float>.Call(this.Graphics.TestRotate), timeSinceLastUpdate / 10f));
			this.Graphics.Socket.Push<float>(this.Graphics.TestRotate, timeSinceLastUpdate / 10f);
		}

		protected override void DoShutdown()
		{
			this.Log.Message("Shutting down...");
		}
	}
}
