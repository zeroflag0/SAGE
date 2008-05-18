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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sage.Editor
{
	public partial class MainFormBase : Form
	{
		public MainFormBase()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!this.DesignMode)
			{
				this.Show();
				new System.Threading.Thread(()
				=>
				{
					this.World.Startup();
					this.World.Run(this.sageView);
				}).Start();
			}
		}

		int _Synchronizing = 0;
		private void timerSynchronize_Tick(object sender, EventArgs e)
		{
			try
			{
				if (System.Threading.Interlocked.Increment(ref _Synchronizing) < 2)
				{
					this.World.Log.Verbose("Synchronizing...");
					this.sceneTree.Synchronize();
					this.World.Log.Verbose("Synchronized.");
				}
			}
			finally
			{
				System.Threading.Interlocked.Decrement(ref _Synchronizing);
			}
		}

		private void World_SelectedNodeChanged(object sender, Sage.World.Node oldvalue, Sage.World.Node newvalue)
		{
			if (newvalue != null)
				this.propertyGrid.SelectedObject = newvalue;
		}
	}
}