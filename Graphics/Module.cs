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
using Sage.Modules;

namespace Sage.Graphics
{
	public abstract class Module : Sage.Modules.Module, Modules.IGraphicsModule
	{
		public override string Name
		{
			get { return "Graphics"; }
		}

		#region Scheduling setup
		public override Sage.Threading.SpecialThread ThreadType
		{
			get
			{
				return Sage.Threading.SpecialThread.MainThread;
			}
		}

		long? _Interval = null;
		public override long Interval
		{
			get
			{
				return (_Interval ?? 20);		// 20ms intervals gives us 50 fps.
			}
			set
			{
				_Interval = value;
			}
		}
		#endregion Scheduling setup

		protected override DependencyList DependenciesInit
		{
			get
			{
				return new DependencyList(this, new Dependency<Sage.World.Manager>(true));
			}
		}

		/// <summary>
		/// A template to use for the filenames screenshots use.
		/// Include the file extension (e.g. '.jpg') and a * for where the current date/time should be inserted.
		/// </summary>
		protected virtual string TemplateScreenshotName
		{
			get
			{
				return "screenshot*.jpg";
			}
		}

		public void TakeScreenshot()
		{
			string fileName = this.TemplateScreenshotName.Replace("*", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

			TakeScreenshot(fileName);
		}

		public abstract void TakeScreenshot(string fileName);

		IntPtr _RenderingTarget = IntPtr.Zero;
		/// <summary>
		/// Allows setting a rendering target (like a winforms control) for the rendering engine to use. It depends on the rendering backend whether it's actually used.
		/// </summary>
		public IntPtr RenderingTarget
		{
			get { return _RenderingTarget; }
			set { _RenderingTarget = value; }
		}

		protected virtual void CreateScene()
		{
			//TODO: create the scene from scene data...
		}

		public virtual void OnResize()
		{
		}

		public virtual void TestRotate(float d)
		{
		}
	}
}