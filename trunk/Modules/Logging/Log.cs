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

namespace Sage.Modules.Logging
{
	public class Log : Sage.Modules.Logging.ILog
	{
		public Log()
			: this("")
		{

		}

		public Log(string name)
			: this(null, name)
		{
		}

		public Log(LogModule module, string name)
		{
			this.Module = module;
			this.Owner = name;
		}

		MessageSocket _Socket = new MessageSocket();

		public MessageSocket Socket
		{
			get { return _Socket; }
			set { _Socket = value; }
		}

		LogModule _Module;

		public LogModule Module
		{
			get { return _Module; }
			set
			{
				if (_Module != value)
				{
					_Module = value;
					if (value != null)
						value.Logs.Add(this);
				}
			}
		}

		string _Owner;

		public virtual string Owner
		{
			get { return _Owner; }
			set { _Owner = value; }
		}

		bool _Quiet = false;
		/// <summary>
		/// If quiet is true, messages won't be logged. Has no effect on errors and warnings.
		/// </summary>
		public bool Quiet
		{
			get { return _Quiet; }
			set { _Quiet = value; }
		}

		#region Indent
		string _Indent = "\t";

		public virtual string Indent
		{
			get { return _Indent; }
			set { _Indent = value; }
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

		protected virtual void DoWrite(DateTime time, string value)
		{
			if (this.Module != null)
				this.Module.Write(time, this.Owner, value);
			else
				return;
		}

		#region Message
		public virtual void Message(object value)
		{
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

		string _MessageIndentBuffer = null;
		int _MessageIndentBufferCount = -1;

		protected virtual string MessageIndentBuffer
		{
			get
			{
				if (this._MessageIndentBuffer == null || this._MessageIndentBufferCount != this.MessageIndent)
				{
					this._MessageIndentBuffer = this.CreateIndent(this.MessageIndent);
				}
				return _MessageIndentBuffer;
			}
		}

		public virtual void Message(string value)
		{
			if (!this.Quiet)
			{
				this.WriteMessage(Threading.Thread.NowTime, new StringBuilder(this.MessageIndentBuffer).Append(this.MessagePrefix).Append(value).Append(this.MessagePostfix).ToString());
			}
		}

		protected virtual void WriteMessage(DateTime time, string value)
		{
			this.Socket.Push<DateTime, string>(this.DoWrite, time, value);
		}

		int _MessageIndent = 0;

		public virtual int MessageIndent
		{
			get { return _MessageIndent; }
			set { _MessageIndent = value; }
		}

		string _MessagePrefix = "";

		public virtual string MessagePrefix
		{
			get { return _MessagePrefix; }
			set { _MessagePrefix = value; }
		}

		string _MessagePostfix = "";

		public virtual string MessagePostfix
		{
			get { return _MessagePostfix; }
			set { _MessagePostfix = value; }
		}

		#endregion Message

		#region Warning

		public virtual void Warning(object value)
		{
			if (value == null)
			{
				this.Warning("<null>");
			}
			else
			{
				this.Warning(value.ToString());
			}
		}

		string _WarningIndentBuffer = null;
		int _WarningIndentBufferCount = -1;

		protected virtual string WarningIndentBuffer
		{
			get
			{
				if (this._WarningIndentBuffer == null || this._WarningIndentBufferCount != this.WarningIndent)
				{
					this._WarningIndentBuffer = this.CreateIndent(this.WarningIndent);
				}
				return _WarningIndentBuffer;
			}
		}

		public virtual void Warning(string value)
		{
			this.WriteWarning(Threading.Thread.NowTime, new StringBuilder(this.WarningIndentBuffer).Append(this.WarningPrefix).Append(value).Append(this.WarningPostfix).ToString());
		}

		protected virtual void WriteWarning(DateTime time, string value)
		{
			this.Socket.Push<DateTime, string>(this.DoWrite, time, value);
		}


		int _WarningIndent = 0;

		public virtual int WarningIndent
		{
			get { return _WarningIndent; }
			set { _WarningIndent = value; }
		}

		string _WarningPrefix = "[Warning] ";

		public virtual string WarningPrefix
		{
			get { return _WarningPrefix; }
			set { _WarningPrefix = value; }
		}

		string _WarningPostfix = "";

		public string WarningPostfix
		{
			get { return _WarningPostfix; }
			set { _WarningPostfix = value; }
		}

		#endregion Warning

		#region Error

		public virtual void Error(object value)
		{
			if (value == null)
			{
				this.Error("<null>");
			}
			else
			{
				this.Error(value.ToString());
			}
		}

		string _ErrorIndentBuffer = null;
		int _ErrorIndentBufferCount = -1;

		protected virtual string ErrorIndentBuffer
		{
			get
			{
				if (this._ErrorIndentBuffer == null || this._ErrorIndentBufferCount != this.ErrorIndent)
				{
					this._ErrorIndentBuffer = this.CreateIndent(this.ErrorIndent);
				}
				return _ErrorIndentBuffer;
			}
		}

		public virtual void Error(string value)
		{
			this.WriteError(Threading.Thread.NowTime, new StringBuilder(this.ErrorIndentBuffer).Append(this.ErrorPrefix).Append(value).Append(this.ErrorPostfix).ToString());
		}

		protected virtual void WriteError(DateTime time, string value)
		{
			this.Socket.Push<DateTime, string>(this.DoWrite, time, value);
		}

		int _ErrorIndent = 0;

		public int ErrorIndent
		{
			get { return _ErrorIndent; }
			set { _ErrorIndent = value; }
		}

		string _ErrorPrefix = "[ERROR] ";

		public string ErrorPrefix
		{
			get { return _ErrorPrefix; }
			set { _ErrorPrefix = value; }
		}


		string _ErrorPostfix = "";

		public string ErrorPostfix
		{
			get { return _ErrorPostfix; }
			set { _ErrorPostfix = value; }
		}

		#endregion Error

		#region Verbose
		[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		public virtual void Verbose(object value)
		{
			if (!this.Quiet)
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
		}

		string _VerboseIndentBuffer = null;
		int _VerboseIndentBufferCount = -1;

		protected virtual string VerboseIndentBuffer
		{
			get
			{
				if (this._VerboseIndentBuffer == null || this._VerboseIndentBufferCount != this.VerboseIndent)
				{
					this._VerboseIndentBuffer = this.CreateIndent(this.VerboseIndent);
				}
				return _VerboseIndentBuffer;
			}
		}
		[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		public virtual void Verbose(string value)
		{
			if (!this.Quiet)
			{
				this.WriteVerbose(Threading.Thread.NowTime, new StringBuilder(this.VerboseIndentBuffer).Append(this.VerbosePrefix).Append(value).Append(this.VerbosePostfix).ToString());
			}
		}
		[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		protected virtual void WriteVerbose(DateTime time, string value)
		{
			this.Socket.Push<DateTime, string>(this.DoWrite, time, value);
		}

		int _VerboseIndent = 0;

		public virtual int VerboseIndent
		{
			get { return _VerboseIndent; }
			set { _VerboseIndent = value; }
		}

		string _VerbosePrefix = "";

		public virtual string VerbosePrefix
		{
			get { return _VerbosePrefix; }
			set { _VerbosePrefix = value; }
		}

		string _VerbosePostfix = "";

		public virtual string VerbosePostfix
		{
			get { return _VerbosePostfix; }
			set { _VerbosePostfix = value; }
		}

		#endregion Verbose

		//#region Verbose
		//[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		//public virtual void Verbose(object value)
		//{
		//    if (value == null)
		//    {
		//        this.Verbose("<null>");
		//    }
		//    else
		//    {
		//        this.Verbose(value.ToString());
		//    }
		//}

		//[System.Diagnostics.Conditional("SAGE_VERBOSE")]
		//public virtual void Verbose(string value)
		//{
		//    lock (Console.Out)
		//    {
		//        Console.WriteLine(value);
		//    }
		//}
		//#endregion
	}
}
