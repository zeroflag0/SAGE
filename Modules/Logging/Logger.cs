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

namespace Sage.Modules.Logging
{
	public abstract class Logger : ILogger, Sage.Basics.IPrototype<ILogger>
	{
		string m_Owner;

		public virtual string Owner
		{
			get { return m_Owner; }
			set { m_Owner = value; }
		}

		bool m_Quiet = false;
		/// <summary>
		/// If quiet is true, messages won't be logged. Has no effect on errors and warnings.
		/// </summary>
		public bool Quiet
		{
			get { return m_Quiet; }
			set { m_Quiet = value; }
		}

		static ILogger s_Prototype = new FileLogger();

		public static ILogger Prototype
		{
			get { return Logger.s_Prototype; }
			set { Logger.s_Prototype = value; }
		}

		public static ILogger CreateLog(string name)
		{
			ILogger log = Prototype.Create(name);
			return log;
		}

		#region IPrototype<Logger> Members

		public abstract ILogger Create();
		public abstract ILogger Create(string name);

		#endregion


		#region Indent
		string m_Indent = "\t";

		public virtual string Indent
		{
			get { return m_Indent; }
			set { m_Indent = value; }
		}

		protected virtual string CreateIndent(int count)
		{
			StringBuilder builder = new StringBuilder(/*this.Indent.Length * count*/);

			for (int i = 0; i < count; i++)
			{
				builder.Append(this.Indent);
				//for(int j = 0; j < this.Indent.Length; j++)
				//{
				//    builder[i + j] = this.Indent[j];
				//}
			}

			return builder.ToString();
		}
		#endregion Indent

		#region Message
		public virtual void Message(object value)
		{
			this.Verbose(value);
			if (!this.Quiet)
			{
				if (value == null)
				{
					this.Message("<null>");
				}
				else
				{
					this.Message(value.ToString());
				}
			}
		}

		string m_MessageIndentBuffer = null;
		int m_MessageIndentBufferCount = -1;

		protected virtual string MessageIndentBuffer
		{
			get
			{
				if (this.m_MessageIndentBuffer == null || this.m_MessageIndentBufferCount != this.MessageIndent)
				{
					this.m_MessageIndentBuffer = this.CreateIndent(this.MessageIndent);
				}
				return m_MessageIndentBuffer;
			}
		}

		public virtual void Message(string value)
		{
			this.Verbose(value);
			if (!this.Quiet)
			{
				this.WriteMessage(this.MessageIndentBuffer + this.MessagePrefix + value + this.MessagePostfix);
			}
		}

		protected abstract void WriteMessage(string value);

		int m_MessageIndent = 0;

		public virtual int MessageIndent
		{
			get { return m_MessageIndent; }
			set { m_MessageIndent = value; }
		}

		string m_MessagePrefix = "";

		public virtual string MessagePrefix
		{
			get { return m_MessagePrefix; }
			set { m_MessagePrefix = value; }
		}

		string m_MessagePostfix = "";

		public virtual string MessagePostfix
		{
			get { return m_MessagePostfix; }
			set { m_MessagePostfix = value; }
		}

		#endregion Message

		#region Warning

		public virtual void Warning(object value)
		{
			this.Verbose(value);
			if (value == null)
			{
				this.Warning("<null>");
			}
			else
			{
				this.Warning(value.ToString());
			}
		}

		string m_WarningIndentBuffer = null;
		int m_WarningIndentBufferCount = -1;

		protected virtual string WarningIndentBuffer
		{
			get
			{
				if (this.m_WarningIndentBuffer == null || this.m_WarningIndentBufferCount != this.WarningIndent)
				{
					this.m_WarningIndentBuffer = this.CreateIndent(this.WarningIndent);
				}
				return m_WarningIndentBuffer;
			}
		}

		public virtual void Warning(string value)
		{
			this.Verbose(value);
			this.WriteWarning(this.WarningIndentBuffer + this.WarningPrefix + value + this.WarningPostfix);
		}

		protected abstract void WriteWarning(string value);


		int m_WarningIndent = 0;

		public virtual int WarningIndent
		{
			get { return m_WarningIndent; }
			set { m_WarningIndent = value; }
		}

		string m_WarningPrefix = "[Warning] ";

		public virtual string WarningPrefix
		{
			get { return m_WarningPrefix; }
			set { m_WarningPrefix = value; }
		}

		string m_WarningPostfix = "";

		public string WarningPostfix
		{
			get { return m_WarningPostfix; }
			set { m_WarningPostfix = value; }
		}

		#endregion Warning

		#region Error

		public virtual void Error(object value)
		{
			this.Verbose(value);
			if (value == null)
			{
				this.Error("<null>");
			}
			else
			{
				this.Error(value.ToString());
			}
		}

		string m_ErrorIndentBuffer = null;
		int m_ErrorIndentBufferCount = -1;

		protected virtual string ErrorIndentBuffer
		{
			get
			{
				if (this.m_ErrorIndentBuffer == null || this.m_ErrorIndentBufferCount != this.ErrorIndent)
				{
					this.m_ErrorIndentBuffer = this.CreateIndent(this.ErrorIndent);
				}
				return m_ErrorIndentBuffer;
			}
		}

		public virtual void Error(string value)
		{
			this.Verbose(value);
			this.WriteError(this.ErrorIndentBuffer + this.ErrorPrefix + value + this.ErrorPostfix);
		}

		protected abstract void WriteError(string value);

		int m_ErrorIndent = 0;

		public int ErrorIndent
		{
			get { return m_ErrorIndent; }
			set { m_ErrorIndent = value; }
		}

		string m_ErrorPrefix = "[ERROR] ";

		public string ErrorPrefix
		{
			get { return m_ErrorPrefix; }
			set { m_ErrorPrefix = value; }
		}


		string m_ErrorPostfix = "";

		public string ErrorPostfix
		{
			get { return m_ErrorPostfix; }
			set { m_ErrorPostfix = value; }
		}

		#endregion Error

		#region Verbose
		[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		public virtual void Verbose(object value)
		{
			if (value == null)
			{
				this.Verbose("<null>");
			}
			else
			{
				this.Verbose(value.ToString());
			}
		}

		[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		public virtual void Verbose(string value)
		{
			lock (Console.Out)
			{
				Console.WriteLine(value);
			}
		}
		#endregion
	}
}
