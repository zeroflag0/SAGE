using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sage.Editor
{
	public partial class RenderingControl : UserControl
	{
		public RenderingControl()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			InitializeComponent();
		}

		#region Loading

		private bool _Loading = true;

		public bool Loading
		{
			get { return _Loading; }
			set
			{
				if (_Loading != value)
				{
					_Loading = value;
					this.Invalidate();
				}
			}
		}
		#endregion Loading

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.Loading)
			{
				for (int i = 0; i < 15; i += 1)
					e.Graphics.DrawString("initializing ...", this.Font, new SolidBrush(Color.FromArgb(0 + i * 10, 0 + i * 10, 0 + i * 10, 255)), i, i);
			}

			base.OnPaint(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//base.OnPaintBackground(e);
		}
	}
}
