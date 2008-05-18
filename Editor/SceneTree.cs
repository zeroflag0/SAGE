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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sage.Editor
{
	public partial class SceneTree : TreeView, ISynchronizable
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

		public SceneTree()
		{
			InitializeComponent();
		}

		Dictionary<TreeNode, Sage.World.Node> _SageNodes = new Dictionary<TreeNode, Sage.World.Node>();
		[Category("Sage")]
		public Dictionary<TreeNode, Sage.World.Node> SageNodes
		{
			get { return _SageNodes; }
		}

		Dictionary<Sage.World.Node, TreeNode> _ViewNodes = new Dictionary<Sage.World.Node, TreeNode>();
		[Category("Sage")]
		public Dictionary<Sage.World.Node, TreeNode> ViewNodes
		{
			get { return _ViewNodes; }
		}

		public void Synchronize()
		{
			this.SynchronizeNode(this.World.Core.World.Root);
		}

		protected void SynchronizeNode(Sage.World.Node node)
		{
			TreeNode view;
			if (!this.ViewNodes.ContainsKey(node))
			{
				// create view node...
				this.ViewNodes.Add(node, new TreeNode(node.Name));
				view = this.ViewNodes[node];

				if (node.Parent == null)
					this.Nodes.Add(view);
				else
				{
					if (!this.ViewNodes.ContainsKey(node.Parent))
						this.SynchronizeNode(node.Parent);
					this.ViewNodes[node.Parent].Nodes.Add(view);
				}
			}
			else
				view = this.ViewNodes[node];

			if (!this.SageNodes.ContainsKey(view))
				this.SageNodes.Add(view, node);
			else
				this.SageNodes[view] = node;

			view.Text = node.ToString();

			foreach (Sage.World.Node sub in node.Children)
			{
				this.SynchronizeNode(sub);
			}
		}

		protected void CleanupNodes()
		{
			//TODO: remove nodes that haven't been updated in a while (probably because they don't exist anymore)...
		}

		private void SceneTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{

		}

		private void SceneTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node != null && this.SageNodes.ContainsKey(e.Node))
				this.World.SelectedNode = this.SageNodes[e.Node];
			else
				this.World.SelectedNode = null;
		}
	}
}
