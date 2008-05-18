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
	public class DependencyList : IDependencyTracer, IEnumerable<IDependency>
	{
		IModule _Owner;

		public IModule Owner
		{
			get { return _Owner; }
			protected set { _Owner = value; }
		}

		List<IDependency> _Items = new List<IDependency>();

		protected List<IDependency> Items
		{
			get { return _Items; }
		}

		public DependencyList(IModule owner)
		{
			this.Owner = owner;
		}

		public DependencyList(IModule owner, params IDependency[] dependencies)
			: this(owner)
		{
			foreach (IDependency dep in dependencies)
			{
				this.Add(dep);
			}
		}

		public void Add(IDependency dep)
		{
			this.Items.Add(dep);
		}

		public void Remove(IDependency dep)
		{
			this.Remove(dep);
		}


		#region IDependencyTracer Members

		public void Build(IModule owner)
		{
			foreach (IDependency dep in this.Items)
			{
				dep.Build(owner);
			}
		}


		List<IModule> _Traced = new List<IModule>();

		protected IModule Traced
		{
			set { this._Traced.Add(value); }
		}

		protected bool IsTraced(IModule module)
		{
			return this._Traced.Contains(module);
		}

		public virtual void Break(IModule initiator)
		{
			if (this.IsTraced(initiator))
				return;

			this.Traced = initiator;

			foreach (IDependency dep in this)
			{
				dep.Break(initiator);
			}

		}

		public IModule Ready(ScheduleState state)
		{
			IModule mod = null;
#if DEBUG_DEPENDENCIES
			IModule notready = null;	// additional variable to allow debugging of module states...
			string log = this.Owner.Name + " checking dependencies(" + this.Items.Count + "): ["; 
#endif

			foreach (IDependency dep in this)
			{
#if DEBUG_DEPENDENCIES
				log += dep.Module.Name + "=";
#endif
				mod = dep.Ready(state);

				if (mod != null)
				{
#if DEBUG_DEPENDENCIES
					if (notready == null)
						notready = mod;
					log += "not ready(" + mod + ") ";
#else
					return mod;
#endif
				}
#if DEBUG_DEPENDENCIES
				else
				{
					log += "ready, ";
				}
#endif
			}
#if DEBUG_DEPENDENCIES
			this.Owner.Logging.Message(log + ((notready == null) ? "all ready" : "") + "]");
			return notready;
#else
			return null;
#endif
		}

		#endregion

		#region IEnumerable<IDependency> Members

		public IEnumerator<IDependency> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		#endregion
	}
}
