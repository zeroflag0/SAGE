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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Sage;
using zeroflag;
using zeroflag.Serialization;

namespace Sage.Editor
{
	public partial class World : global::Sage.Modules.Module, IComponent
	{
		public World()
		{
			InitializeComponent();
		}

		public World(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		public virtual void Startup()
		{
			this.Sage = new Core();
			this.Sage.Modules.Add(new global::Sage.Graphics.Ogre.Module());
			//this.Sage.Modules.Add(new SageApplication.GraphicsTest());
			this.Sage.Modules.Add(this);
			this.Sage.Initialize();

			this.Graphics.Interval = 100;
		}

		public global::Sage.Graphics.Module Graphics
		{
			get { return this.Sage.Modules.Get<global::Sage.Graphics.Module>(); }
		}

		#region RenderingTarget

		private RenderingControl _RenderingTarget;

		public RenderingControl RenderingTarget
		{
			get { return _RenderingTarget; }
			set
			{
				if (_RenderingTarget != value)
				{
					if (_RenderingTarget != null)
						_RenderingTarget.Resize -= this.RenderingTargetResize;
					_RenderingTarget = value;

					if (value != null && this.Sage != null)
						this.Graphics.RenderingTarget = value.Handle;

					if (_RenderingTarget != null)
						_RenderingTarget.Resize += this.RenderingTargetResize;
				}
			}
		}

		void RenderingTargetResize(object sender, EventArgs e)
		{
			if (this.Sage != null && this.Graphics != null)
			{
				this.Graphics.OnResize();
			}
		}
		#endregion RenderingTarget
		public void Run(RenderingControl renderingTarget)
		{
			this.RenderingTarget = renderingTarget;
			if (this.Sage != null)
				this.Sage.Run();
		}

		#region Scheduling setup
		public override global::Sage.Threading.SpecialThread ThreadType
		{
			get
			{
				return global::Sage.Threading.SpecialThread.MainThread;
			}
		}

		public override long Interval
		{
			get
			{
				return 200;		// 20ms intervals gives us 5 fps.
			}
			set
			{
			}
		}
		#endregion Scheduling setup

		public override string Name
		{
			get
			{
				return "Editor";
			}
		}

		public override global::Sage.Modules.IFeatureFactory Features
		{
			get { return null; }
		}

		protected override void DoInitialize()
		{
		}

		protected override void DoUpdate(long timeSinceLastUpdate)
		{
			this.RenderingTarget.Loading = false;
			System.Windows.Forms.Application.DoEvents();
		}

		protected override void DoShutdown()
		{
			this.Sage.Threads.QueuePool(this.Sage.Dispose);
		}

		Core _Sage;
		[ReadOnly(true)]
		[Category("Sage")]
		public Core Sage
		{
			get { return _Sage; }
			protected set { _Sage = value; }
		}


		#region SelectedNode

		private Sage.World.Node _SelectedNode;

		/// <summary>
		/// The currently selected scene node.
		/// </summary>
		[DefaultValue(null)]
		[Category("Sage")]
		public Sage.World.Node SelectedNode
		{
			get { return _SelectedNode; }
			set
			{
				if (_SelectedNode != value)
				{
					this.OnSelectedNodeChanged(_SelectedNode, _SelectedNode = value);
				}
			}
		}

		#region SelectedNodeChanged event
		public delegate void SelectedNodeChangedHandler(object sender, Sage.World.Node oldvalue, Sage.World.Node newvalue);

		private event SelectedNodeChangedHandler _SelectedNodeChanged;
		/// <summary>
		/// Occurs when SelectedNode changes.
		/// </summary>
		[DefaultValue(null)]
		[Category("Sage")]
		public event SelectedNodeChangedHandler SelectedNodeChanged
		{
			add { this._SelectedNodeChanged += value; }
			remove { this._SelectedNodeChanged -= value; }
		}

		/// <summary>
		/// Raises the SelectedNodeChanged event.
		/// </summary>
		protected virtual void OnSelectedNodeChanged(Sage.World.Node oldvalue, Sage.World.Node newvalue)
		{
			// if there are event subscribers...
			if (this._SelectedNodeChanged != null)
			{
				// call them...
				this._SelectedNodeChanged(this, oldvalue, newvalue);
			}
		}
		#endregion SelectedNodeChanged event
		#endregion SelectedNode
		#region IComponent Members

		#region event Disposed
		private event EventHandler _Disposed;
		/// <summary>
		/// Disposed
		/// </summary>
		public event EventHandler Disposed
		{
			add { this._Disposed += value; }
			remove { this._Disposed -= value; }
		}
		/// <summary>
		/// Call to raise the Disposed event:
		/// Disposed
		/// </summary>
		protected virtual void OnDisposed(EventArgs e)
		{
			// if there are event subscribers...
			if (this._Disposed != null)
			{
				// call them...
				this._Disposed(this, e);
			}
		}
		#endregion event Disposed

		ISite _Site;

		public ISite Site
		{
			get { return _Site; }
			set { _Site = value; }
		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Shutdown();

			this.OnDisposed(new EventArgs());
		}

		#endregion

		#endregion
	}
}
