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
	public class MessageQueue
		: LockfreeQueue<ITask>
	{
		public void Push<T1>(Task<T1>.CallHandler call, T1 p1)
		{
			this.Push(new Task<T1>(call, p1));
		}
		public void Push<T1, T2>(Task<T1, T2>.CallHandler call, T1 p1, T2 p2)
		{
			this.Push(new Task<T1, T2>(call, p1, p2));
		}
		public void Push<T1, T2, T3>(Task<T1, T2, T3>.CallHandler call, T1 p1, T2 p2, T3 p3)
		{
			this.Push(new Task<T1, T2, T3>(call, p1, p2, p3));
		}
		public void Push<T1, T2, T3, T4>(Task<T1, T2, T3, T4>.CallHandler call, T1 p1, T2 p2, T3 p3, T4 p4)
		{
			this.Push(new Task<T1, T2, T3, T4>(call, p1, p2, p3, p4));
		}
		//public void Push<T1, T2, T3, T4, T5>(Message<T1, T2, T3, T4, T5>.Call call, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
		//{
		//    this.Push(new Message<T1, T2, T3, T4,T5>(call, p1, p2, p3, p4, p5));
		//}
	}
}
