using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Editor.Actions
{
	public class Action
	{
		public delegate void Handle();

		#region Do

		private Handle m_Do = default(Handle);

		public Handle Do
		{
			get { return m_Do; }
			set
			{
				if (m_Do != value)
				{
					m_Do = value;
				}
			}
		}

		public void PerformDo()
		{
			if (this.Do != null)
				this.Do();
		}
		#endregion Do

		#region Undo

		private Handle m_Undo = default(Handle);

		public Handle Undo
		{
			get { return m_Undo; }
			set
			{
				if (m_Undo != value)
				{
					m_Undo = value;
				}
			}
		}

		public void PerformUndo()
		{
			if (this.Undo != null)
				this.Undo();
		}
		#endregion Undo

		#region Constructors
		public Action()
		{
		}

		public Action(Handle doAction)
			: this()
		{
			this.Do = doAction;
		}

		public Action(Handle doAction, Handle undoAction)
			: this(doAction)
		{
			this.Undo = undoAction;
		}
		#endregion

		#region Name

		private string _Name;
		[System.ComponentModel.Localizable(true)]
		public string Name
		{
			get { return _Name; }
			set
			{
				if (_Name != value)
				{
					_Name = value;
				}
			}
		}

		#endregion Name

		#region Enabled

		private bool _Enabled = default(bool);

		public bool Enabled
		{
			get { return _Enabled; }
			set
			{
				if (_Enabled != value)
				{
					_Enabled = value;
				}
			}
		}

		#endregion Enabled
	}
}
