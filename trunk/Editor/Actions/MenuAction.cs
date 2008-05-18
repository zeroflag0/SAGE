using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Editor.Actions
{
	public class MenuAction : System.Windows.Forms.MenuItem
	{
		#region Action

		private Action _Action;

		public Action Action
		{
			get { return _Action; }
			set
			{
				if (_Action != value)
				{
					_Action = value;
				}
			}
		}
		#endregion Action

		protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
		{
			if (this.Action == null)
			{
				this.Enabled = false;
				this.Text = "<null>";
			}
			else
			{
				this.Enabled = this.Action.Enabled;
				this.Text = this.Action.Name;
			}

			base.OnDrawItem(e);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			//this.GetContextMenu().SourceControl.
			this.Action.PerformDo();
		}
	}
}
