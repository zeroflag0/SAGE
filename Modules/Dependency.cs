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

namespace Sage.Modules
{
	public class Dependency<T> : Sage.Modules.IDependency
		where T : class, IModule
	{

		public DependencyList List
		{
			get { return this.Module.Dependencies; }
		}

		IModule _Module;

		public IModule Module
		{
			get { return this._Module; }
			protected set { this._Module = value; }
		}

		protected Core Core
		{
			get { return this.Module.Core; }
		}

		public Type DependencyType
		{
			get { return typeof(T); }
		}

		List<IModule> _Links = new List<IModule>();

		public List<IModule> Links
		{
			get { return _Links; }
		}

		bool _Explicit = false;

		public bool Explicit
		{
			get { return _Explicit; }
			set { _Explicit = value; }
		}

		public Dependency()
		{
		}

		public Dependency(bool _explicit)
		{
			this.Explicit = _explicit;
		}

		/// <summary>
		/// Build the dependency trace for the specified owner.
		/// Break cyclic dependency if necessary.
		/// </summary>
		/// <param name="owner"></param>
		public void Build(IModule owner)
		{
			this.Module = owner;

			foreach (IModule module in this.Core.Modules)
			{
				if (module as T != null)
				{
					this.Links.Add(module);
				}
			}

			if (this.Links.Count <= 0)
				throw new DependencyNotFoundException("Cannot find dependency " + typeof(T));
		}

		List<IModule> _Traced = new List<IModule>();

		public IModule Traced
		{
			set { this._Traced.Add(value); }
		}

		protected bool IsTraced(IModule module)
		{
			return this._Traced.Contains(module);
		}

		public virtual void Break(IModule initiator)
		{
			this.Break(initiator, true);
		}

		protected virtual void Break(IModule initiator, bool recursive)
		{
			if (this.IsTraced(initiator))
				return;

			if (this.Links.Contains(this.Module))
			{
				// break cyclic links to self...
				this.Links.Remove(this.Module);
			}

			if (this.Links.Contains(initiator))
			{
				if (!this.Explicit)
				{
					// break cyclic dependencies...
					initiator.Log.Message("Breaking implicit cyclic reference in " + this.Module.Name + ".");
					this.Links.Remove(initiator);
				}
				else
				{
					// explicit cyclic reference...
					initiator.Log.Error("Cyclic reference found through " + this.Module.Name + ".");
				}
			}

			this.Traced = initiator;
			if (recursive)
			{
				foreach (IModule mod in this.Links)
				{
					mod.Dependencies.Break(initiator);
				}
			}
		}

		public IModule Ready(ScheduleState state)
		{
			if (this.Links.Count <= 0)
				return this.Module;

#if DEBUG_DEPENDENCIES
			IModule notready = null;	// additional variable to allow debugging of module states...

			string log = this.Module.Name + " checking dependencies [" + state + "] (" + this.Links.Count + "): [";
#endif
			foreach (IModule dep in this.Links)
			{
#if DEBUG_DEPENDENCIES
				log += "<" + dep.Name + "=";
#endif
				if (dep.ScheduleState < state)
				{
					// if any dependency is not ready yet...
#if DEBUG_DEPENDENCIES
					log += dep.ScheduleState + "=not ready>";
					if (notready == null)
						notready = dep;
#else
					return dep;
#endif
				}
#if DEBUG_DEPENDENCIES
				else
				{
					log += "OK>";
				}
#endif
			}
#if DEBUG_DEPENDENCIES
			this.Module.Logging.Message(log + ((notready == null) ? " <<all ready>> " : " <<WAIT>> ") + "]");
			return notready;
#else
			return null;
#endif
		}
	}

	public class DependencyNotFoundException : Exception
	{
		public DependencyNotFoundException(string message) : base(message) { }

		public DependencyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
	}
}
