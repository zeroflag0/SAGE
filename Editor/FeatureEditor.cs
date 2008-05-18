using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sage.Editor
{
	public partial class FeatureEditor : UserControl, Sage.Editor.ISynchronizable
	{
		#region World

		private World m_World = default(World);
		[Category("Sage")]
		public World World
		{
			get { return m_World; }
			set
			{
				if (m_World != value)
				{
					m_World = value;
				}
			}
		}
		#endregion World

		public FeatureEditor()
		{
			InitializeComponent();
		}

		public void Synchronize()
		{
		}
	}
}
