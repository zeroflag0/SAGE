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
using System.IO;

namespace Sage.Modules.Logging
{
	//TODO: rewrite logging to be faster and safer.
	//TODO: output from different threads seem to collide. locking? tracing implementation from .NET? do performance tests.
	public class FileLogger
		: Logger,
		  IDisposable
	{
		static FileLogger()
		{
			s_MasterLogger = new FileLogger("master");
		}

		private static FileLogger s_MasterLogger;

		public static FileLogger MasterLogger
		{
			get
			{
				return s_MasterLogger;
			}
			protected set { s_MasterLogger = value; }
		}

		public override ILogger Create()
		{
			return new FileLogger();
		}
		public override ILogger Create(string name)
		{
			return new FileLogger(name);
		}

		private StreamWriter m_Writer;

		public StreamWriter Writer
		{
			get { return m_Writer; }
			protected set { m_Writer = value; }
		}

		public override string Owner
		{
			get { return base.Owner; }
			set
			{
				base.Owner = value;

				if (value != null)
				{

					if (value != "")
					{
						this.CreateWriter(value);
					}
					else
					{
						this.CreateWriter("unknown");
					}
				}
			}
		}

		public FileLogger(string name)
		{
			this.Owner = name;
		}

		public FileLogger()
		{
			this.Owner = "";
		}

		public void Dispose()
		{
			this.CloseWriter();
		}


		~FileLogger()
		{
			this.CloseWriter();
		}

		protected void CreateWriter(string name)
		{
			if (this.Writer != null)
			{
				//this.CloseWriter();
				return;
			}

			string sufix = "";
			int count = -1;
			while (this.Writer == null)
			{
				try
				{
					this.Writer = new StreamWriter("log_" + name + sufix + ".txt");
					break;
				}
				catch (IOException)
				{
					count++;
					sufix = "_" + count;

					if (count > 10)
					{
						return;
						//throw;
					}
				}
			}

			this.Message("Log initialized.");
		}

		protected void CloseWriter()
		{
			if (this.Writer != null)
			{
				try
				{
					this.Message("Log closed.");
				}
				catch
				{
				}
				try
				{
					this.Writer.Flush();
				}
				catch
				{
				}
				try
				{
					this.Writer.Close();
				}
				catch
				{
				}
				try
				{
					this.Writer.Dispose();
				}
				catch
				{
				}
				this.Writer = null;
			}
		}

		protected virtual string Timestamp
		{
			get
			{
				DateTime now = DateTime.Now;

				return now.ToString("HH:mm:ss") + "." + now.Millisecond.ToString().PadLeft(3, '0');
			}
		}

		protected virtual void WriteMessage(string owner, string value)
		{
			this.Writer.WriteLine(Timestamp + " " + owner + "[" + Sage.Threading.Pool.CurrentThreadID + "]:\t" + value);
			this.Writer.Flush();
#if VERBOSE
			if (this.MasterLogger == null || this.MasterLogger == this)
			{
				lock (Console.Out)
				{
					Console.WriteLine(Timestamp + " " + owner + "[" + Sage.Threading.Pool.CurrentThreadID + "]:\t" + value);
				}
			}
#endif
		}

		protected override void WriteMessage(string value)
		{
			if (MasterLogger != null && MasterLogger != this)
			{
				MasterLogger.WriteMessage(this.Owner, value);
			}
			this.WriteMessage(this.Owner, value);
		}

		protected virtual void WriteWarning(string owner, string value)
		{
			this.Writer.WriteLine(Timestamp + " " + owner + "[" + Sage.Threading.Pool.CurrentThreadID + "]:\t" + value);
			this.Writer.Flush();
#if VERBOSE
			if (this.MasterLogger == null || this.MasterLogger == this)
			{
				lock (Console.Out)
				{
					Console.WriteLine(Timestamp + " " + owner + "[" + Sage.Threading.Pool.CurrentThreadID + "]:\t" + value);
				}
			}
#endif
		}

		protected override void WriteWarning(string value)
		{
			if (MasterLogger != null && MasterLogger != this)
			{
				MasterLogger.WriteWarning(this.Owner, value);
			}

			this.WriteWarning(this.Owner, value);
		}

		protected virtual void WriteError(string owner, string value)
		{
			this.Writer.WriteLine(Timestamp + " " + owner + "[" + Sage.Threading.Pool.CurrentThreadID + "]:\t" + value);
			this.Writer.Flush();
#if VERBOSE
			if (this.MasterLogger == null || this.MasterLogger == this)
			{
				lock (Console.Out)
				{
					Console.WriteLine(Timestamp + " " + owner + "[" + Sage.Threading.Pool.CurrentThreadID + "]:\t" + value);
				}
			}
#endif
		}

		protected override void WriteError(string value)
		{
			if (MasterLogger != null && MasterLogger != this)
			{
				MasterLogger.WriteError(this.Owner, value);
			}

			this.WriteError(this.Owner, value);
		}
	}
}