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
	public class Task : ITask
	{
		public delegate void CallHandler();

		#region event Callback

		private event CallHandler _Call;

		/// <summary>
		/// The call for this message.
		/// </summary>
		public event CallHandler Call
		{
			add
			{
				this._Call += value;
				this.LastCall = value;
			}
			remove { this._Call -= value; }
		}

		#endregion event Callback

		public Task()
		{
		}

		public Task(CallHandler call)
		{
			this.Call += call;
		}

		public virtual void Run()
		{
			// if there are event subscribers...
			if (this._Call != null)
			{
				// call them...
				this._Call();
			}
		}

		public override string ToString()
		{
			if (toString == null)
			{
				this.toString = LastCall.Target.ToString() + "." + LastCall.Method.Name + " [" + LastCall.Method.ToString() + "]";
			}
			return toString;
		}

		private string toString = null;

		private CallHandler LastCall;
	}
}
