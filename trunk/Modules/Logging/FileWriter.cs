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
using System.IO;

namespace Sage.Modules.Logging
{
	public class FileWriter : IWriter
	{
		Dictionary<string, StreamWriter> _Writers = new Dictionary<string, StreamWriter>();

		protected Dictionary<string, StreamWriter> Writers
		{
			get { return _Writers; }
		}

		public StreamWriter this[string name]
		{
			get
			{
				if (!this.Writers.ContainsKey(name) || this.Writers[name] == null)
				{
					this.Writers[name] = this.CreateWriter(name);
				}
				return this.Writers[name];
			}
		}

		StreamWriter _Master;

		public StreamWriter Master
		{
			get { return _Master ?? (_Master = this.CreateWriter("master")); }
			set { _Master = value; }
		}

		protected StreamWriter CreateWriter(string name)
		{
			StreamWriter writer = null;
			string sufix = "";
			int count = -1;
			while (writer == null)
			{
				try
				{
					writer = new StreamWriter("log_" + name + sufix + ".txt");
					break;
				}
				catch (IOException)
				{
					count++;
					sufix = "_" + count;

					if (count > 10)
					{
						return writer;
						//throw;
					}
				}
			}
			return writer;
		}



		public void Write(DateTime time, string owner, string text)
		{
			try
			{
				this.Master.WriteLine(new StringBuilder(time.ToString("HH:mm:ss.fff")).Append(" [").Append(owner.PadLeft(15)).Append("]: ").Append(text));
				this[owner].WriteLine(new StringBuilder(time.ToString("HH:mm:ss.fff")).Append(" [").Append(owner.PadLeft(15)).Append("]: ").Append(text));
			}
			catch (ObjectDisposedException) { }
		}

		public void Flush()
		{
			foreach (StreamWriter writer in this.Writers.Values)
				try
				{
					writer.Flush();
				}
				catch (ObjectDisposedException) { }
			try
			{
				this.Master.Flush();
			}
			catch (ObjectDisposedException) { }
		}

		public void Dispose()
		{
			foreach (StreamWriter writer in this.Writers.Values)
			{
				try
				{
					writer.WriteLine(new StringBuilder(Sage.Threading.Thread.NowTime.ToString("HH:mm:ss.fff")).Append(" closed."));
				}
				catch (ObjectDisposedException) { }

				writer.Dispose();
			}
			try
			{
				this.Master.WriteLine(new StringBuilder(Sage.Threading.Thread.NowTime.ToString("HH:mm:ss.fff")).Append(" closed."));
			}
			catch (ObjectDisposedException) { }

			this.Master.Dispose();
		}

	}
}
