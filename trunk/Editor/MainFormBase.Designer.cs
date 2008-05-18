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

namespace Sage.Editor
{
	partial class MainFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormBase));
			this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
			this.splitContainerLeftBottom = new System.Windows.Forms.SplitContainer();
			this.sceneTree = new Sage.Editor.SceneTree();
			this.World = new Sage.Editor.World(this.components);
			this.tabNodeEdit = new System.Windows.Forms.TabControl();
			this.tabProperties = new System.Windows.Forms.TabPage();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.tabNodeFeatures = new System.Windows.Forms.TabPage();
			this.featureEditor = new Sage.Editor.FeatureEditor();
			this.splitContainerBottom = new System.Windows.Forms.SplitContainer();
			this.panelSageView = new System.Windows.Forms.Panel();
			this.sageView = new RenderingControl();
			this.timerSynchronize = new System.Windows.Forms.Timer(this.components);
			this.splitContainerLeft.Panel1.SuspendLayout();
			this.splitContainerLeft.Panel2.SuspendLayout();
			this.splitContainerLeft.SuspendLayout();
			this.splitContainerLeftBottom.Panel1.SuspendLayout();
			this.splitContainerLeftBottom.Panel2.SuspendLayout();
			this.splitContainerLeftBottom.SuspendLayout();
			this.tabNodeEdit.SuspendLayout();
			this.tabProperties.SuspendLayout();
			this.tabNodeFeatures.SuspendLayout();
			this.splitContainerBottom.Panel1.SuspendLayout();
			this.splitContainerBottom.Panel2.SuspendLayout();
			this.splitContainerBottom.SuspendLayout();
			this.panelSageView.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainerLeft
			// 
			resources.ApplyResources(this.splitContainerLeft, "splitContainerLeft");
			this.splitContainerLeft.Name = "splitContainerLeft";
			// 
			// splitContainerLeft.Panel1
			// 
			this.splitContainerLeft.Panel1.Controls.Add(this.splitContainerLeftBottom);
			// 
			// splitContainerLeft.Panel2
			// 
			this.splitContainerLeft.Panel2.Controls.Add(this.splitContainerBottom);
			// 
			// splitContainerLeftBottom
			// 
			resources.ApplyResources(this.splitContainerLeftBottom, "splitContainerLeftBottom");
			this.splitContainerLeftBottom.Name = "splitContainerLeftBottom";
			// 
			// splitContainerLeftBottom.Panel1
			// 
			this.splitContainerLeftBottom.Panel1.Controls.Add(this.sceneTree);
			// 
			// splitContainerLeftBottom.Panel2
			// 
			this.splitContainerLeftBottom.Panel2.Controls.Add(this.tabNodeEdit);
			// 
			// sceneTree
			// 
			this.sceneTree.AllowDrop = true;
			resources.ApplyResources(this.sceneTree, "sceneTree");
			this.sceneTree.FullRowSelect = true;
			this.sceneTree.HideSelection = false;
			this.sceneTree.HotTracking = true;
			this.sceneTree.Name = "sceneTree";
			this.sceneTree.PathSeparator = "/";
			this.sceneTree.ShowNodeToolTips = true;
			this.sceneTree.World = this.World;
			// 
			// World
			// 
			this.World.Interval = ((long)(20));
			this.World.RenderingTarget = null;
			this.World.SelectedNodeChanged += new Sage.Editor.World.SelectedNodeChangedHandler(this.World_SelectedNodeChanged);
			// 
			// tabNodeEdit
			// 
			resources.ApplyResources(this.tabNodeEdit, "tabNodeEdit");
			this.tabNodeEdit.Controls.Add(this.tabProperties);
			this.tabNodeEdit.Controls.Add(this.tabNodeFeatures);
			this.tabNodeEdit.HotTrack = true;
			this.tabNodeEdit.Multiline = true;
			this.tabNodeEdit.Name = "tabNodeEdit";
			this.tabNodeEdit.SelectedIndex = 0;
			// 
			// tabProperties
			// 
			this.tabProperties.Controls.Add(this.propertyGrid);
			resources.ApplyResources(this.tabProperties, "tabProperties");
			this.tabProperties.Name = "tabProperties";
			this.tabProperties.UseVisualStyleBackColor = true;
			// 
			// propertyGrid
			// 
			resources.ApplyResources(this.propertyGrid, "propertyGrid");
			this.propertyGrid.Name = "propertyGrid";
			// 
			// tabNodeFeatures
			// 
			this.tabNodeFeatures.Controls.Add(this.featureEditor);
			resources.ApplyResources(this.tabNodeFeatures, "tabNodeFeatures");
			this.tabNodeFeatures.Name = "tabNodeFeatures";
			this.tabNodeFeatures.UseVisualStyleBackColor = true;
			// 
			// featureEditor
			// 
			resources.ApplyResources(this.featureEditor, "featureEditor");
			this.featureEditor.Name = "featureEditor";
			this.featureEditor.World = this.World;
			// 
			// splitContainerBottom
			// 
			resources.ApplyResources(this.splitContainerBottom, "splitContainerBottom");
			this.splitContainerBottom.Name = "splitContainerBottom";
			// 
			// splitContainerBottom.Panel1
			// 
			this.splitContainerBottom.Panel1.Controls.Add(this.panelSageView);
			// 
			// panelSageView
			// 
			this.panelSageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelSageView.Controls.Add(this.sageView);
			resources.ApplyResources(this.panelSageView, "panelSageView");
			this.panelSageView.Name = "panelSageView";
			// 
			// sageView
			// 
			resources.ApplyResources(this.sageView, "sageView");
			this.sageView.Name = "sageView";
			// 
			// timerSynchronize
			// 
			this.timerSynchronize.Enabled = true;
			this.timerSynchronize.Interval = 2000;
			this.timerSynchronize.Tick += new System.EventHandler(this.timerSynchronize_Tick);
			// 
			// MainFormBase
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainerLeft);
			this.Name = "MainFormBase";
			this.splitContainerLeft.Panel1.ResumeLayout(false);
			this.splitContainerLeft.Panel2.ResumeLayout(false);
			this.splitContainerLeft.ResumeLayout(false);
			this.splitContainerLeftBottom.Panel1.ResumeLayout(false);
			this.splitContainerLeftBottom.Panel2.ResumeLayout(false);
			this.splitContainerLeftBottom.ResumeLayout(false);
			this.tabNodeEdit.ResumeLayout(false);
			this.tabProperties.ResumeLayout(false);
			this.tabNodeFeatures.ResumeLayout(false);
			this.splitContainerBottom.Panel1.ResumeLayout(false);
			this.splitContainerBottom.Panel2.ResumeLayout(false);
			this.splitContainerBottom.ResumeLayout(false);
			this.panelSageView.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainerLeft;
		private System.Windows.Forms.SplitContainer splitContainerBottom;
		private System.Windows.Forms.Panel panelSageView;
		private RenderingControl sageView;
		private System.Windows.Forms.SplitContainer splitContainerLeftBottom;
		private System.Windows.Forms.TabControl tabNodeEdit;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.TabPage tabNodeFeatures;
		private World World;
		private SceneTree sceneTree;
		private FeatureEditor featureEditor;
		private System.Windows.Forms.Timer timerSynchronize;
	}
}

