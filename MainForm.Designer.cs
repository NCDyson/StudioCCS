/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/21/2017
 * Time: 9:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace StudioCCS
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.SplitContainer LogSplit;
		private System.Windows.Forms.SplitContainer viewportSplit;
		private System.Windows.Forms.SplitContainer treeSplit;
		private System.Windows.Forms.TreeView ccsTree;
		private System.Windows.Forms.PropertyGrid ccsPropertyGrid;
		private System.Windows.Forms.ToolStrip viewToolstrip;
		private System.Windows.Forms.ToolStripButton tbtnPreview;
		private System.Windows.Forms.ToolStripButton tbtnScene;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
		private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem smoothShadedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem texturedToolStripMenuItem;
		private System.Windows.Forms.RichTextBox logView;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.Timer renderTimer;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadCCSToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sceneToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip ccsFileContextMenu;
		private System.Windows.Forms.TreeView sceneTreeView;
		private System.Windows.Forms.ToolStripMenuItem unloadToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addAllToSceneToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip sceneContextMenu;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem backfaceCullingToolStripMenuItem;
		private System.Windows.Forms.ToolStripLabel tlblRenderMode;
		private System.Windows.Forms.ToolStripStatusLabel statusCameraLabel;
		private System.Windows.Forms.ToolStripMenuItem drawAxisMarkerInTopOfViewportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawAxisMarkerAtWorldCenterToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem iMOQToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadTownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawGridToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem sceneModeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawCollisionMeshesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawDummiesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawLightHelpersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem vertexColorsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewCCSReportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dumpToOBJToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dumpToSMDToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip ccsClumpContextMenu;
		private System.Windows.Forms.ToolStripMenuItem editBonesToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip ccsAnimeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem addToSceneToolStripMenuItem1;
		private System.Windows.Forms.ContextMenuStrip sceneAnimeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem sceneAnimeContext_RemoveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setPoseToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton tbtnAll;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defaultToAxisMovementToolStripMenuItem;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.LogSplit = new System.Windows.Forms.SplitContainer();
			this.viewportSplit = new System.Windows.Forms.SplitContainer();
			this.treeSplit = new System.Windows.Forms.SplitContainer();
			this.sceneTreeView = new System.Windows.Forms.TreeView();
			this.ccsTree = new System.Windows.Forms.TreeView();
			this.ccsPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.viewToolstrip = new System.Windows.Forms.ToolStrip();
			this.tbtnPreview = new System.Windows.Forms.ToolStripButton();
			this.tbtnScene = new System.Windows.Forms.ToolStripButton();
			this.tbtnAll = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
			this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.vertexColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.smoothShadedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.texturedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.backfaceCullingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.sceneModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawCollisionMeshesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawDummiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawLightHelpersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tlblRenderMode = new System.Windows.Forms.ToolStripLabel();
			this.logView = new System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusCameraLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.renderTimer = new System.Windows.Forms.Timer(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadCCSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.iMOQToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadTownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dumpToOBJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dumpToSMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ccsFileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.unloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addAllToSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewCCSReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sceneContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ccsClumpContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editBonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ccsAnimeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addToSceneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.setPoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sceneAnimeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.sceneAnimeContext_RemoveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.defaultToAxisMovementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.LogSplit)).BeginInit();
			this.LogSplit.Panel1.SuspendLayout();
			this.LogSplit.Panel2.SuspendLayout();
			this.LogSplit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewportSplit)).BeginInit();
			this.viewportSplit.Panel1.SuspendLayout();
			this.viewportSplit.Panel2.SuspendLayout();
			this.viewportSplit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.treeSplit)).BeginInit();
			this.treeSplit.Panel1.SuspendLayout();
			this.treeSplit.Panel2.SuspendLayout();
			this.treeSplit.SuspendLayout();
			this.viewToolstrip.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.ccsFileContextMenu.SuspendLayout();
			this.ccsClumpContextMenu.SuspendLayout();
			this.ccsAnimeContextMenu.SuspendLayout();
			this.sceneAnimeContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// LogSplit
			// 
			this.LogSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LogSplit.Location = new System.Drawing.Point(0, 24);
			this.LogSplit.Name = "LogSplit";
			this.LogSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// LogSplit.Panel1
			// 
			this.LogSplit.Panel1.Controls.Add(this.viewportSplit);
			// 
			// LogSplit.Panel2
			// 
			this.LogSplit.Panel2.Controls.Add(this.logView);
			this.LogSplit.Panel2.Controls.Add(this.statusStrip1);
			this.LogSplit.Size = new System.Drawing.Size(901, 559);
			this.LogSplit.SplitterDistance = 438;
			this.LogSplit.TabIndex = 0;
			// 
			// viewportSplit
			// 
			this.viewportSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewportSplit.Location = new System.Drawing.Point(0, 0);
			this.viewportSplit.Name = "viewportSplit";
			// 
			// viewportSplit.Panel1
			// 
			this.viewportSplit.Panel1.Controls.Add(this.treeSplit);
			// 
			// viewportSplit.Panel2
			// 
			this.viewportSplit.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.viewportSplit.Panel2.Controls.Add(this.viewToolstrip);
			this.viewportSplit.Size = new System.Drawing.Size(901, 438);
			this.viewportSplit.SplitterDistance = 183;
			this.viewportSplit.TabIndex = 0;
			// 
			// treeSplit
			// 
			this.treeSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeSplit.Location = new System.Drawing.Point(0, 0);
			this.treeSplit.Name = "treeSplit";
			this.treeSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// treeSplit.Panel1
			// 
			this.treeSplit.Panel1.Controls.Add(this.sceneTreeView);
			this.treeSplit.Panel1.Controls.Add(this.ccsTree);
			// 
			// treeSplit.Panel2
			// 
			this.treeSplit.Panel2.Controls.Add(this.ccsPropertyGrid);
			this.treeSplit.Size = new System.Drawing.Size(183, 438);
			this.treeSplit.SplitterDistance = 294;
			this.treeSplit.TabIndex = 0;
			// 
			// sceneTreeView
			// 
			this.sceneTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sceneTreeView.Location = new System.Drawing.Point(0, 0);
			this.sceneTreeView.Name = "sceneTreeView";
			this.sceneTreeView.Size = new System.Drawing.Size(183, 294);
			this.sceneTreeView.TabIndex = 1;
			this.sceneTreeView.Visible = false;
			this.sceneTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SceneTreeViewNodeMouseClick);
			// 
			// ccsTree
			// 
			this.ccsTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ccsTree.Location = new System.Drawing.Point(0, 0);
			this.ccsTree.Name = "ccsTree";
			this.ccsTree.Size = new System.Drawing.Size(183, 294);
			this.ccsTree.TabIndex = 0;
			this.ccsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CcsTreeAfterSelect);
			this.ccsTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CcsTreeNodeMouseClick);
			// 
			// ccsPropertyGrid
			// 
			this.ccsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ccsPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
			this.ccsPropertyGrid.Location = new System.Drawing.Point(0, 0);
			this.ccsPropertyGrid.Name = "ccsPropertyGrid";
			this.ccsPropertyGrid.Size = new System.Drawing.Size(183, 140);
			this.ccsPropertyGrid.TabIndex = 0;
			// 
			// viewToolstrip
			// 
			this.viewToolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.viewToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tbtnPreview,
			this.tbtnScene,
			this.tbtnAll,
			this.toolStripSeparator1,
			this.toolStripSplitButton1,
			this.tlblRenderMode});
			this.viewToolstrip.Location = new System.Drawing.Point(0, 0);
			this.viewToolstrip.Name = "viewToolstrip";
			this.viewToolstrip.Size = new System.Drawing.Size(714, 25);
			this.viewToolstrip.TabIndex = 0;
			this.viewToolstrip.Text = "toolStrip1";
			// 
			// tbtnPreview
			// 
			this.tbtnPreview.CheckOnClick = true;
			this.tbtnPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbtnPreview.Image = ((System.Drawing.Image)(resources.GetObject("tbtnPreview.Image")));
			this.tbtnPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbtnPreview.Name = "tbtnPreview";
			this.tbtnPreview.Size = new System.Drawing.Size(52, 22);
			this.tbtnPreview.Text = "Preview";
			this.tbtnPreview.CheckedChanged += new System.EventHandler(this.TbtnPreviewCheckedChanged);
			// 
			// tbtnScene
			// 
			this.tbtnScene.CheckOnClick = true;
			this.tbtnScene.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbtnScene.Image = ((System.Drawing.Image)(resources.GetObject("tbtnScene.Image")));
			this.tbtnScene.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbtnScene.Name = "tbtnScene";
			this.tbtnScene.Size = new System.Drawing.Size(42, 22);
			this.tbtnScene.Text = "Scene";
			this.tbtnScene.CheckedChanged += new System.EventHandler(this.TbtnSceneCheckedChanged);
			// 
			// tbtnAll
			// 
			this.tbtnAll.CheckOnClick = true;
			this.tbtnAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbtnAll.Image = ((System.Drawing.Image)(resources.GetObject("tbtnAll.Image")));
			this.tbtnAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbtnAll.Name = "tbtnAll";
			this.tbtnAll.Size = new System.Drawing.Size(25, 22);
			this.tbtnAll.Text = "All";
			this.tbtnAll.CheckedChanged += new System.EventHandler(this.TbtnAllCheckedChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSplitButton1
			// 
			this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.wireframeToolStripMenuItem,
			this.vertexColorsToolStripMenuItem,
			this.smoothShadedToolStripMenuItem,
			this.texturedToolStripMenuItem,
			this.toolStripSeparator3,
			this.backfaceCullingToolStripMenuItem,
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem,
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem,
			this.drawGridToolStripMenuItem,
			this.toolStripSeparator4,
			this.sceneModeToolStripMenuItem});
			this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
			this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new System.Drawing.Size(95, 22);
			this.toolStripSplitButton1.Text = "Draw Options";
			// 
			// wireframeToolStripMenuItem
			// 
			this.wireframeToolStripMenuItem.CheckOnClick = true;
			this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
			this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.wireframeToolStripMenuItem.Text = "Wireframe";
			this.wireframeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.WireframeToolStripMenuItemCheckedChanged);
			// 
			// vertexColorsToolStripMenuItem
			// 
			this.vertexColorsToolStripMenuItem.CheckOnClick = true;
			this.vertexColorsToolStripMenuItem.Name = "vertexColorsToolStripMenuItem";
			this.vertexColorsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.vertexColorsToolStripMenuItem.Text = "Vertex Colors";
			this.vertexColorsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.VertexColorsToolStripMenuItemCheckedChanged);
			// 
			// smoothShadedToolStripMenuItem
			// 
			this.smoothShadedToolStripMenuItem.CheckOnClick = true;
			this.smoothShadedToolStripMenuItem.Name = "smoothShadedToolStripMenuItem";
			this.smoothShadedToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.smoothShadedToolStripMenuItem.Text = "Smooth Shading";
			this.smoothShadedToolStripMenuItem.Visible = false;
			this.smoothShadedToolStripMenuItem.CheckedChanged += new System.EventHandler(this.SmoothShadedToolStripMenuItemCheckedChanged);
			// 
			// texturedToolStripMenuItem
			// 
			this.texturedToolStripMenuItem.CheckOnClick = true;
			this.texturedToolStripMenuItem.Name = "texturedToolStripMenuItem";
			this.texturedToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.texturedToolStripMenuItem.Text = "Texturing";
			this.texturedToolStripMenuItem.CheckedChanged += new System.EventHandler(this.TexturedToolStripMenuItemCheckedChanged);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
			// 
			// backfaceCullingToolStripMenuItem
			// 
			this.backfaceCullingToolStripMenuItem.CheckOnClick = true;
			this.backfaceCullingToolStripMenuItem.Name = "backfaceCullingToolStripMenuItem";
			this.backfaceCullingToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.backfaceCullingToolStripMenuItem.Text = "Backface Culling";
			this.backfaceCullingToolStripMenuItem.CheckedChanged += new System.EventHandler(this.BackfaceCullingToolStripMenuItemCheckedChanged);
			// 
			// drawAxisMarkerInTopOfViewportToolStripMenuItem
			// 
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Checked = true;
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.CheckOnClick = true;
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Name = "drawAxisMarkerInTopOfViewportToolStripMenuItem";
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Text = "Draw View Orientation Axis";
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.ToolTipText = "Draw Axis Marker for View Orientation in Top Right Corner of Viewport";
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DrawAxisMarkerInTopOfViewportToolStripMenuItemCheckedChanged);
			// 
			// drawAxisMarkerAtWorldCenterToolStripMenuItem
			// 
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Checked = true;
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.CheckOnClick = true;
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Name = "drawAxisMarkerAtWorldCenterToolStripMenuItem";
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Text = "Draw View Center";
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.ToolTipText = "Draw Axis Marker at Center of World";
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DrawAxisMarkerAtWorldCenterToolStripMenuItemCheckedChanged);
			// 
			// drawGridToolStripMenuItem
			// 
			this.drawGridToolStripMenuItem.Checked = true;
			this.drawGridToolStripMenuItem.CheckOnClick = true;
			this.drawGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.drawGridToolStripMenuItem.Name = "drawGridToolStripMenuItem";
			this.drawGridToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.drawGridToolStripMenuItem.Text = "Draw Grid";
			this.drawGridToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DrawGridToolStripMenuItemCheckedChanged);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(213, 6);
			// 
			// sceneModeToolStripMenuItem
			// 
			this.sceneModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.drawCollisionMeshesToolStripMenuItem,
			this.drawDummiesToolStripMenuItem,
			this.drawLightHelpersToolStripMenuItem});
			this.sceneModeToolStripMenuItem.Name = "sceneModeToolStripMenuItem";
			this.sceneModeToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.sceneModeToolStripMenuItem.Text = "Scene Mode";
			// 
			// drawCollisionMeshesToolStripMenuItem
			// 
			this.drawCollisionMeshesToolStripMenuItem.Checked = true;
			this.drawCollisionMeshesToolStripMenuItem.CheckOnClick = true;
			this.drawCollisionMeshesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.drawCollisionMeshesToolStripMenuItem.Name = "drawCollisionMeshesToolStripMenuItem";
			this.drawCollisionMeshesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.drawCollisionMeshesToolStripMenuItem.Text = "Draw Collision Meshes";
			this.drawCollisionMeshesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DrawCollisionMeshesToolStripMenuItemCheckedChanged);
			// 
			// drawDummiesToolStripMenuItem
			// 
			this.drawDummiesToolStripMenuItem.Checked = true;
			this.drawDummiesToolStripMenuItem.CheckOnClick = true;
			this.drawDummiesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.drawDummiesToolStripMenuItem.Name = "drawDummiesToolStripMenuItem";
			this.drawDummiesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.drawDummiesToolStripMenuItem.Text = "Draw Dummy Helpers";
			this.drawDummiesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DrawDummiesToolStripMenuItemCheckedChanged);
			// 
			// drawLightHelpersToolStripMenuItem
			// 
			this.drawLightHelpersToolStripMenuItem.Checked = true;
			this.drawLightHelpersToolStripMenuItem.CheckOnClick = true;
			this.drawLightHelpersToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.drawLightHelpersToolStripMenuItem.Name = "drawLightHelpersToolStripMenuItem";
			this.drawLightHelpersToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.drawLightHelpersToolStripMenuItem.Text = "Draw Light Helpers";
			this.drawLightHelpersToolStripMenuItem.Visible = false;
			this.drawLightHelpersToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DrawLightHelpersToolStripMenuItemCheckedChanged);
			// 
			// tlblRenderMode
			// 
			this.tlblRenderMode.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tlblRenderMode.Name = "tlblRenderMode";
			this.tlblRenderMode.Size = new System.Drawing.Size(0, 22);
			// 
			// logView
			// 
			this.logView.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.logView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logView.ForeColor = System.Drawing.SystemColors.Window;
			this.logView.HideSelection = false;
			this.logView.Location = new System.Drawing.Point(0, 0);
			this.logView.Name = "logView";
			this.logView.ReadOnly = true;
			this.logView.Size = new System.Drawing.Size(901, 95);
			this.logView.TabIndex = 1;
			this.logView.Text = "";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.statusCameraLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 95);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(901, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusCameraLabel
			// 
			this.statusCameraLabel.Name = "statusCameraLabel";
			this.statusCameraLabel.Size = new System.Drawing.Size(48, 17);
			this.statusCameraLabel.Text = "Camera";
			// 
			// renderTimer
			// 
			this.renderTimer.Interval = 20;
			this.renderTimer.Tick += new System.EventHandler(this.RenderTimerTick);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.fileToolStripMenuItem,
			this.sceneToolStripMenuItem,
			this.optionsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(901, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.loadCCSToolStripMenuItem,
			this.toolStripSeparator2,
			this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// loadCCSToolStripMenuItem
			// 
			this.loadCCSToolStripMenuItem.Name = "loadCCSToolStripMenuItem";
			this.loadCCSToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.loadCCSToolStripMenuItem.Text = "Load CCS";
			this.loadCCSToolStripMenuItem.Click += new System.EventHandler(this.LoadCCSToolStripMenuItemClick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(122, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
			// 
			// sceneToolStripMenuItem
			// 
			this.sceneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.iMOQToolStripMenuItem,
			this.dumpToOBJToolStripMenuItem,
			this.dumpToSMDToolStripMenuItem});
			this.sceneToolStripMenuItem.Name = "sceneToolStripMenuItem";
			this.sceneToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
			this.sceneToolStripMenuItem.Text = "Scene";
			// 
			// iMOQToolStripMenuItem
			// 
			this.iMOQToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.loadTownToolStripMenuItem});
			this.iMOQToolStripMenuItem.Name = "iMOQToolStripMenuItem";
			this.iMOQToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.iMOQToolStripMenuItem.Text = "IMOQ";
			this.iMOQToolStripMenuItem.Visible = false;
			// 
			// loadTownToolStripMenuItem
			// 
			this.loadTownToolStripMenuItem.Name = "loadTownToolStripMenuItem";
			this.loadTownToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
			this.loadTownToolStripMenuItem.Text = "Load Town";
			// 
			// dumpToOBJToolStripMenuItem
			// 
			this.dumpToOBJToolStripMenuItem.Name = "dumpToOBJToolStripMenuItem";
			this.dumpToOBJToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.dumpToOBJToolStripMenuItem.Text = "Dump to OBJ";
			this.dumpToOBJToolStripMenuItem.Click += new System.EventHandler(this.DumpToOBJToolStripMenuItemClick);
			// 
			// dumpToSMDToolStripMenuItem
			// 
			this.dumpToSMDToolStripMenuItem.Name = "dumpToSMDToolStripMenuItem";
			this.dumpToSMDToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.dumpToSMDToolStripMenuItem.Text = "Dump to SMD";
			this.dumpToSMDToolStripMenuItem.Visible = false;
			this.dumpToSMDToolStripMenuItem.Click += new System.EventHandler(this.DumpToSMDToolStripMenuItemClick);
			// 
			// ccsFileContextMenu
			// 
			this.ccsFileContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.unloadToolStripMenuItem,
			this.addAllToSceneToolStripMenuItem,
			this.viewCCSReportMenuItem});
			this.ccsFileContextMenu.Name = "ccsFileContextMenu";
			this.ccsFileContextMenu.Size = new System.Drawing.Size(164, 70);
			// 
			// unloadToolStripMenuItem
			// 
			this.unloadToolStripMenuItem.Name = "unloadToolStripMenuItem";
			this.unloadToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.unloadToolStripMenuItem.Text = "Unload";
			this.unloadToolStripMenuItem.Click += new System.EventHandler(this.UnloadToolStripMenuItemClick);
			// 
			// addAllToSceneToolStripMenuItem
			// 
			this.addAllToSceneToolStripMenuItem.Name = "addAllToSceneToolStripMenuItem";
			this.addAllToSceneToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.addAllToSceneToolStripMenuItem.Text = "Add All To Scene";
			this.addAllToSceneToolStripMenuItem.Visible = false;
			// 
			// viewCCSReportMenuItem
			// 
			this.viewCCSReportMenuItem.Name = "viewCCSReportMenuItem";
			this.viewCCSReportMenuItem.Size = new System.Drawing.Size(163, 22);
			this.viewCCSReportMenuItem.Text = "View Info Report";
			this.viewCCSReportMenuItem.Click += new System.EventHandler(this.ViewCCSReportMenuItemClick);
			// 
			// sceneContextMenu
			// 
			this.sceneContextMenu.Name = "sceneContextMenu";
			this.sceneContextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// ccsClumpContextMenu
			// 
			this.ccsClumpContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.editBonesToolStripMenuItem});
			this.ccsClumpContextMenu.Name = "ccsClumpContextMenu";
			this.ccsClumpContextMenu.Size = new System.Drawing.Size(130, 26);
			// 
			// editBonesToolStripMenuItem
			// 
			this.editBonesToolStripMenuItem.Name = "editBonesToolStripMenuItem";
			this.editBonesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.editBonesToolStripMenuItem.Text = "Edit Bones";
			this.editBonesToolStripMenuItem.Click += new System.EventHandler(this.EditBonesToolStripMenuItemClick);
			// 
			// ccsAnimeContextMenu
			// 
			this.ccsAnimeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.addToSceneToolStripMenuItem1,
			this.setPoseToolStripMenuItem});
			this.ccsAnimeContextMenu.Name = "ccsAnimeContextMenu";
			this.ccsAnimeContextMenu.Size = new System.Drawing.Size(145, 48);
			// 
			// addToSceneToolStripMenuItem1
			// 
			this.addToSceneToolStripMenuItem1.Name = "addToSceneToolStripMenuItem1";
			this.addToSceneToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.addToSceneToolStripMenuItem1.Text = "Add to Scene";
			this.addToSceneToolStripMenuItem1.Click += new System.EventHandler(this.AddToSceneToolStripMenuItem1Click);
			// 
			// setPoseToolStripMenuItem
			// 
			this.setPoseToolStripMenuItem.Name = "setPoseToolStripMenuItem";
			this.setPoseToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.setPoseToolStripMenuItem.Text = "Set Pose";
			this.setPoseToolStripMenuItem.Click += new System.EventHandler(this.SetPoseToolStripMenuItemClick);
			// 
			// sceneAnimeContextMenu
			// 
			this.sceneAnimeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.sceneAnimeContext_RemoveMenuItem});
			this.sceneAnimeContextMenu.Name = "sceneAnimeContextMenu";
			this.sceneAnimeContextMenu.Size = new System.Drawing.Size(118, 26);
			// 
			// sceneAnimeContext_RemoveMenuItem
			// 
			this.sceneAnimeContext_RemoveMenuItem.Name = "sceneAnimeContext_RemoveMenuItem";
			this.sceneAnimeContext_RemoveMenuItem.Size = new System.Drawing.Size(117, 22);
			this.sceneAnimeContext_RemoveMenuItem.Text = "Remove";
			this.sceneAnimeContext_RemoveMenuItem.Click += new System.EventHandler(this.SceneAnimeContext_RemoveMenuItemClick);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.defaultToAxisMovementToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// defaultToAxisMovementToolStripMenuItem
			// 
			this.defaultToAxisMovementToolStripMenuItem.CheckOnClick = true;
			this.defaultToAxisMovementToolStripMenuItem.Name = "defaultToAxisMovementToolStripMenuItem";
			this.defaultToAxisMovementToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			this.defaultToAxisMovementToolStripMenuItem.Text = "Default to Axis Movement";
			this.defaultToAxisMovementToolStripMenuItem.CheckedChanged += new System.EventHandler(this.DefaultToAxisMovementToolStripMenuItemCheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(901, 583);
			this.Controls.Add(this.LogSplit);
			this.Controls.Add(this.menuStrip1);
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Text = "StudioCCS";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
			this.LogSplit.Panel1.ResumeLayout(false);
			this.LogSplit.Panel2.ResumeLayout(false);
			this.LogSplit.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.LogSplit)).EndInit();
			this.LogSplit.ResumeLayout(false);
			this.viewportSplit.Panel1.ResumeLayout(false);
			this.viewportSplit.Panel2.ResumeLayout(false);
			this.viewportSplit.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.viewportSplit)).EndInit();
			this.viewportSplit.ResumeLayout(false);
			this.treeSplit.Panel1.ResumeLayout(false);
			this.treeSplit.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.treeSplit)).EndInit();
			this.treeSplit.ResumeLayout(false);
			this.viewToolstrip.ResumeLayout(false);
			this.viewToolstrip.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ccsFileContextMenu.ResumeLayout(false);
			this.ccsClumpContextMenu.ResumeLayout(false);
			this.ccsAnimeContextMenu.ResumeLayout(false);
			this.sceneAnimeContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
