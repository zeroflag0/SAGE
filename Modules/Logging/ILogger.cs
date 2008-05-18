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
using System.Text;

using Sage.Basics;

namespace Sage.Modules.Logging
{
	public interface ILogger : Sage.Basics.IPrototype<ILogger>
	{
		ILogger Create(string name);

		string Owner { get; set; }
		string Indent { get; set; }

		void Message(object value);
		void Message(string value);
		int MessageIndent { get; set; }
		string MessagePrefix { get; set; }
		string MessagePostfix { get; set; }

		void Warning(object value);
		void Warning(string value);
		int WarningIndent { get; set; }
		string WarningPrefix { get;set;}
		string WarningPostfix { get;set;}

		void Error(object value);
		void Error(string value);
		int ErrorIndent { get; set; }
		string ErrorPrefix { get;set;}
		string ErrorPostfix { get; set;}
	}
}
