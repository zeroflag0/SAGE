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

using Sage.Modules;

namespace Sage
{
	public class SageException : Exception
	{
		IModule _Module = null;

		public IModule Module
		{
			get { return _Module; }
			protected set { _Module = value; }
		}

		public SageException()
			: base()
		{
		}

		public SageException(IModule module)
			: this(module, (Exception)null)
		{
		}

		//((module != null) ? module.Name : "unknown") + ": " + 
		public SageException(IModule module, string message)
			: this(module, message, null)
		{
		}

		public SageException(IModule module, Exception exception)
			: this(module, "Error in Sage.", exception)
		{
		}

		public SageException(IModule module, string message, Exception exception)
			: base(message, exception)
		{
			this.Module = module;
			module.Log.Error(this.ToString());
		}

		public override string ToString()
		{
			return "Exception in " + (Module != null?this.Module.ToString():"<unknown>") + ": " + PrintException(this);
		}


		static string PrintException(Exception exc)
		{
			if (exc == null) return "";
			else return exc.ToString() + PrintInnerException(exc.InnerException);
		}

		static string PrintInnerException(Exception exc)
		{
			if (exc == null) return "";
			else return "\nInnerException:\n" + exc.ToString() + PrintInnerException(exc.InnerException);
		}
	}
}
