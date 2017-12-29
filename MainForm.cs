/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/21/2017
 * Time: 9:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;
using StudioCCS.libCCS;

namespace StudioCCS
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{	
		//Helpful References
		TreeNode SceneAnimationNode = new TreeNode("Animations");
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Logger.SetLogControl(logView);
			tbtnPreview.Checked = true;
			//wireframeToolStripMenuItem.Checked = true;
			//smoothShadedToolStripMenuItem.Checked = true;
			texturedToolStripMenuItem.Checked = true;
			picViewport.MouseWheel += PicViewportMouseWheel;
			
			#if DEBUG
			dumpToSMDToolStripMenuItem.Visible = true;
			#endif
			Scene.Init(picViewport);
			
			sceneTreeView.Nodes.Add(SceneAnimationNode);
			
			renderTimer.Enabled = true;
			//sceneTreeView.Nodes.Add(SceneAnimationNode);
		}
		
		#region FileMenu
		void LoadCCSToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "CCS Files (*.ccs, *.tmp)|*.ccs;*.tmp|All Files(*.*)|*.*";
			dlg.Title = "Select CCS Files to load";
			dlg.Multiselect = true;
			
			if(dlg.ShowDialog() != DialogResult.OK) return;
			
			int loadedCount = 0;
			foreach (var fileName in dlg.FileNames)
			{
				if(Scene.LoadCCSFile(fileName)) loadedCount += 1;
			}
			
			if(loadedCount > 0)
			{
				//ccsTree.Nodes.Add(Scene.ToNode());
				ccsTree.Nodes.Clear();
				foreach(var tmpCCS in Scene.CCSFileList)
				{
					ccsTree.Nodes.Add(tmpCCS.ToNode());
				}
			}
				
		}
		
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		#endregion
		
		#region ViewportToolbar
		void TbtnSceneCheckedChanged(object sender, EventArgs e)
		{
			if(tbtnScene.Checked)
			{
				tbtnPreview.CheckedChanged -= TbtnPreviewCheckedChanged;
				tbtnAll.CheckedChanged -= TbtnAllCheckedChanged;
				tbtnPreview.Checked = false;
				tbtnAll.Checked = false;
				tbtnPreview.CheckedChanged += TbtnPreviewCheckedChanged;
				tbtnAll.CheckedChanged += TbtnAllCheckedChanged;
				
				Scene.SceneDisplay = Scene.SceneMode.Scene;
				ccsTree.Visible = false;
				sceneTreeView.Visible = true;
				ccsPropertyGrid.Visible = true;
				treeSplit.Panel2Collapsed = false;
				viewportSplit.Panel1Collapsed = false;
			}
			else
			{
				tbtnScene.Checked = true;
			}
		}
		
		void TbtnPreviewCheckedChanged(object sender, EventArgs e)
		{
			if(tbtnPreview.Checked)
			{
				tbtnScene.CheckedChanged -= TbtnSceneCheckedChanged;
				tbtnAll.CheckedChanged -= TbtnAllCheckedChanged;
				tbtnScene.Checked = false;
				tbtnAll.Checked = false;
				tbtnScene.CheckedChanged += TbtnSceneCheckedChanged;
				tbtnAll.CheckedChanged += TbtnAllCheckedChanged;
				
				sceneTreeView.Visible = false;
				ccsPropertyGrid.Visible = false;
				ccsTree.Visible = true;
				treeSplit.Panel2Collapsed = true;
				viewportSplit.Panel1Collapsed = false;
				Scene.SceneDisplay = Scene.SceneMode.Preview;
			}
			else
			{
				tbtnPreview.Checked = true;
			}
		}
		
		void TbtnAllCheckedChanged(object sender, EventArgs e)
		{
			if(tbtnAll.Checked)
			{
				tbtnScene.CheckedChanged -= TbtnSceneCheckedChanged;
				tbtnPreview.CheckedChanged -= TbtnPreviewCheckedChanged;
				tbtnScene.Checked = false;
				tbtnPreview.Checked = false;
				tbtnScene.CheckedChanged += TbtnSceneCheckedChanged;
				tbtnPreview.CheckedChanged += TbtnPreviewCheckedChanged;
				
				sceneTreeView.Visible = false;
				ccsPropertyGrid.Visible = false;
				ccsTree.Visible = false;
				treeSplit.Panel2Collapsed = true;
				viewportSplit.Panel1Collapsed = true;
				Scene.SceneDisplay = Scene.SceneMode.All;
			}
			else
			{
				tbtnAll.Checked = true;
			}
		}
		
		void WireframeToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			/*
			if(wireframeToolStripMenuItem.Checked)
			{
				Scene.DisplayMode = Scene.RenderMode.Wireframe;
				SmoothSetNoFire(false);
				TexturedSetNoFire(false);
				SetRenderModeLabel();
				WireframeOverlayToolStripMenuItem.Enabled = false;
			}
			else
			{
				//Check to make sure at least one item is checked.
				if(!smoothShadedToolStripMenuItem.Checked && !texturedToolStripMenuItem.Checked)
				{
					WireframeSetNoFire(true);
				}
			}
			*/
			Scene.DrawWireframe = wireframeToolStripMenuItem.Checked;
			SetRenderModeLabel();
		}
		
		void VertexColorsToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawVertexColors = vertexColorsToolStripMenuItem.Checked;
			SetRenderModeLabel();
		}
		
		void SmoothShadedToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			/*
			if(smoothShadedToolStripMenuItem.Checked)
			{
				Scene.DisplayMode = Scene.RenderMode.Smooth;
				WireframeSetNoFire(false);
				TexturedSetNoFire(false);
				SetRenderModeLabel();
				WireframeOverlayToolStripMenuItem.Enabled = true;
			}
			else
			{
				//check to make sure at least one item is checked;
				if(!wireframeToolStripMenuItem.Checked && !texturedToolStripMenuItem.Checked)
				{
					SmoothSetNoFire(true);
				}
			}
			*/
			Scene.DrawVertexNormals = smoothShadedToolStripMenuItem.Checked;
			SetRenderModeLabel();
		}
		
		void TexturedToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			/*
			if(texturedToolStripMenuItem.Checked)
			{
				Scene.DisplayMode = Scene.RenderMode.Textured;
				WireframeSetNoFire(false);
				SmoothSetNoFire(false);
				SetRenderModeLabel();
			}
			else
			{
				if(!wireframeToolStripMenuItem.Checked && !smoothShadedToolStripMenuItem.Checked)
				{
					TexturedSetNoFire(true);
				}
			}
			*/
			Scene.DrawTextures = texturedToolStripMenuItem.Checked;
			SetRenderModeLabel();
		}
		
		/*
		void WireframeSetNoFire(bool value)
		{
			wireframeToolStripMenuItem.CheckedChanged -= WireframeToolStripMenuItemCheckedChanged;
			wireframeToolStripMenuItem.Checked = value;
			wireframeToolStripMenuItem.CheckedChanged += WireframeToolStripMenuItemCheckedChanged;
			
		}
		
		void SmoothSetNoFire(bool value)
		{
			smoothShadedToolStripMenuItem.CheckedChanged -= SmoothShadedToolStripMenuItemCheckedChanged;
			smoothShadedToolStripMenuItem.Checked = value;
			smoothShadedToolStripMenuItem.CheckedChanged += SmoothShadedToolStripMenuItemCheckedChanged;
		}
		
		void TexturedSetNoFire(bool value)
		{
			texturedToolStripMenuItem.CheckedChanged -= TexturedToolStripMenuItemCheckedChanged;
			texturedToolStripMenuItem.Checked = value;
			texturedToolStripMenuItem.CheckedChanged += TexturedToolStripMenuItemCheckedChanged;
		}
		
		*/
		void BackfaceCullingToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
		/*
			if(backfaceCullingToolStripMenuItem.Checked)
			{
				Scene.BackfaceCull = true;
			}
			else
			{
				Scene.BackfaceCull = false;
			}
			*/
			Scene.BackfaceCull = backfaceCullingToolStripMenuItem.Checked;
			SetRenderModeLabel();
		}
		
		void SetRenderModeLabel()
		{
			/*
			switch(Scene.DisplayMode)
			{
				case Scene.RenderMode.Wireframe:
					tlblRenderMode.Text = "Wireframe";
					break;
				
				case Scene.RenderMode.Smooth:
					tlblRenderMode.Text = "Smooth Shaded";
					break;
				case Scene.RenderMode.Textured:
					tlblRenderMode.Text = "Textured";
					break;
			}
			*/
			tlblRenderMode.Text = "";
			if((Scene.GetRenderMode() & 15) == 0) tlblRenderMode.Text = "None";
			else
			{
				List<string> options = new List<string>();
				if(Scene.DrawWireframe) options.Add("Wireframe");
				if(Scene.DrawVertexColors) options.Add("Vertex Colors");
				if(Scene.DrawVertexNormals) options.Add("Vertex Normals");
				if(Scene.DrawTextures) options.Add("Textured");
			
				tlblRenderMode.Text = String.Join("/", options);
				if(backfaceCullingToolStripMenuItem.Checked) tlblRenderMode.Text += " (Backface Culling)";
				else tlblRenderMode.Text += " (No Backface Culling)";
			}
		}
		
		void DrawGridToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawViewGrid = drawGridToolStripMenuItem.Checked;
		}
		
		void DrawCollisionMeshesToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawCollisionMeshes = drawCollisionMeshesToolStripMenuItem.Checked;
		}
		
		void DrawDummiesToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawDummyHelpers = drawDummiesToolStripMenuItem.Checked;
		}
		
		void DrawLightHelpersToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawLightHelpers = drawLightHelpersToolStripMenuItem.Checked;
		}
		void DrawAxisMarkerInTopOfViewportToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawViewAxis = drawAxisMarkerInTopOfViewportToolStripMenuItem.Checked;
		}
		
		void DrawAxisMarkerAtWorldCenterToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawWorldCenter = drawAxisMarkerAtWorldCenterToolStripMenuItem.Checked;
		}
		#endregion
		
		#region ViewportAndForm
		void PicViewportMouseWheel(object sender, MouseEventArgs e)
		{
			Scene.MouseWheel(e);
		}
		
		void PicViewportMouseMove(object sender, MouseEventArgs e)
		{
			Scene.MouseMove(e);
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Scene.DeInit();
		}
		
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			Scene.KeyPress(e);
			//if(e.Shift) Debug.WriteLine("Shift Key Pressed!");
		}
		
		void MainFormKeyUp(object sender, KeyEventArgs e)
		{
			Scene.KeyRelease(e);
			//if(!e.Shift) Debug.WriteLine("Shift Key Released!");
		}
		
		void PicViewportResize(object sender, EventArgs e)
		{
			Scene.UpdateViewport(picViewport);
		}
		void RenderTimerTick(object sender, EventArgs e)
		{
			Scene.Render();
			ArcBallCamera cam = Scene.CurrentCamera();
			string camRotStr = string.Format("Rotation: {0}, {1}, {2}", cam.Rotation.X, cam.Rotation.Y, cam.Rotation.Z);
			string camTargetStr = string.Format("Target: {0}, {1}, {2}", cam.Target.X, cam.Target.Y, cam.Target.Z);
			string camDistStr = string.Format("Distance: {0}", cam.Distance);
			
			statusCameraLabel.Text = string.Format("Camera: {0}, {1}, {2}", camRotStr, camTargetStr, camDistStr);
		}
		#endregion

		#region CCSTree
		void CcsTreeNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				ccsTree.SelectedNode = e.Node;
				var node = e.Node;
				if(node.Tag != null)
				{
					var tag = (TreeNodeTag)node.Tag;// as TreeNodeTag;
					var file = tag.File;
					if(tag.Type == TreeNodeTag.NodeType.File)
					{

						ccsFileContextMenu.Show(ccsTree, e.X, e.Y);
					}
					else if(tag.Type == TreeNodeTag.NodeType.Main)
					{
						if(tag.ObjectType == libCCS.CCSFile.SECTION_CLUMP)
						{
							ccsClumpContextMenu.Show(ccsTree, e.X, e.Y);
						}
						else if(tag.ObjectType == libCCS.CCSFile.SECTION_ANIME)
						{
							ccsAnimeContextMenu.Show(ccsTree, e.X, e.Y);
						}
					}
				}
			}
		}
		void UnloadToolStripMenuItemClick(object sender, EventArgs e)
		{
			var node = ccsTree.SelectedNode;
			if(node.Tag != null)
			{
				var tag = (TreeNodeTag)node.Tag;
				Scene.UnloadCCSFile(tag.File);
				ccsTree.SelectedNode = null;
				ccsTree.Nodes.Remove(node);
			}
		}
		void CcsTreeAfterSelect(object sender, TreeViewEventArgs e)
		{
				var node = e.Node;
				Scene.SelectedPreviewItemTag = (TreeNodeTag)node.Tag;
				if(node.Tag != null)
				{
					var tag = (TreeNodeTag)node.Tag;
					Debug.WriteLine("Node: Type: {0}, ObjType: {1:X}, ID: {2:X}, SubID: {3}", tag.Type, tag.ObjectType, tag.ObjectID, tag.SubID);
					if(tag.ObjectType == CCSFile.SECTION_ANIME)
					{
						CCSAnime tmpAnime = tag.File.GetObject<CCSAnime>(tag.ObjectID);
						if(tmpAnime != null)
						{
							tmpAnime.HasEnded = false;
							tmpAnime.CurrentFrame = 0;
						}
					}
				}
		}
		void ViewCCSReportMenuItemClick(object sender, EventArgs e)
		{
			var node = ccsTree.SelectedNode;
			var nodeTag = (TreeNodeTag)node.Tag;
			if(nodeTag != null)
			{
				var reportForm = new frmInfo();
				var tmpFile = nodeTag.File;
				string reportText = tmpFile.GetReport();
				reportForm.SetReportText(reportText);
				reportForm.Show();
			}
		}
		
		#region CCSClump
		void LoadMatrixMenuItemClick(object sender, EventArgs e)
		{
			var node = ccsTree.SelectedNode;
			var nodeTag = (TreeNodeTag)node.Tag;
			if(nodeTag != null)
			{
				var dlg = new OpenFileDialog();
				dlg.Filter = "Bin Files (*.bin)|*.bin|All Files(*.*)|*.*";
				dlg.Title = "Select Binary File to load";
			
				if(dlg.ShowDialog() != DialogResult.OK) return;
				libCCS.CCSClump tmpClump = nodeTag.File.GetObject<libCCS.CCSClump>(nodeTag.ObjectID);
				if(tmpClump != null)
				{
					tmpClump.LoadMatrixList(dlg.FileName);
				}
				
			}
		}
		void EditBonesToolStripMenuItemClick(object sender, EventArgs e)
		{		
			var node = ccsTree.SelectedNode;
			var nodeTag = (TreeNodeTag)node.Tag;
			if(nodeTag != null)
			{
				var editFrm = new frmEditBone();
				editFrm.SetClump(nodeTag.File.GetObject<CCSClump>(nodeTag.ObjectID));
				editFrm.Show();
			}
			
		}
		
		#endregion
		
		#region CCSAnime
		void AddToSceneToolStripMenuItem1Click(object sender, EventArgs e)
		{
			var node = ccsTree.SelectedNode;
			var nodeTag = (TreeNodeTag)node.Tag;
			if(nodeTag != null)
			{
				var tmpAnime = nodeTag.File.GetObject<CCSAnime>(nodeTag.ObjectID);
				if(tmpAnime != null) 
				{
					Scene.AddAnime(tmpAnime);
					SceneAnimationNode.Nodes.Add(tmpAnime.ToNode());
				}
			}	
		}
		
		void SetPoseToolStripMenuItemClick(object sender, EventArgs e)
		{
			var node = ccsTree.SelectedNode;
			var nodeTag = (TreeNodeTag)node.Tag;
			if(nodeTag != null)
			{
				var tmpAnime = nodeTag.File.GetObject<CCSAnime>(nodeTag.ObjectID);
				if(tmpAnime != null)
				{
					tmpAnime.FrameForward();
				}
			}
		}
		#endregion
		#endregion
		
		#region SceneMenu
		void DumpToOBJToolStripMenuItemClick(object sender, EventArgs e)
		{	
			using(var vfd = new frmExportToOBJ())
			{
				DialogResult result = vfd.ShowDialog();
				if(result == DialogResult.OK)
				{
					string savePath = vfd.txtExportPath.Text;
					Scene.DumpToObj(savePath, vfd.chkExportCollision.Checked, vfd.chkSplitSubModels.Checked, vfd.chkSplitCollision.Checked, vfd.chkModelWithNormals.Checked, vfd.chkExportDummies.Checked, vfd.chkDumpAnime.Checked);
					
				}
			}
			
		}
		void DumpToSMDToolStripMenuItemClick(object sender, EventArgs e)
		{
			using(var vfd = new frmExportToOBJ())
			{
				vfd.chkSplitCollision.Enabled = false;
				vfd.chkSplitSubModels.Enabled = false;
				vfd.chkExportCollision.Enabled = false;
				vfd.chkExportDummies.Enabled = false;
				vfd.Text = "Export to SMD...";
				DialogResult result = vfd.ShowDialog();
				if(result == DialogResult.OK)
				{
					string savePath = vfd.txtExportPath.Text;
					Scene.DumpToSMD(savePath, vfd.chkModelWithNormals.Checked);
				}
			}
		}
		#endregion
		
		#region SceneTree

		void SceneTreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				sceneTreeView.SelectedNode = e.Node;
				var node = e.Node;
				if(node.Tag != null)
				{
					var tag = (TreeNodeTag)node.Tag;// as TreeNodeTag;
					var file = tag.File;
					if(tag.Type == TreeNodeTag.NodeType.Main)
					{
						if(tag.ObjectType == libCCS.CCSFile.SECTION_ANIME)
						{
							//ccsAnimeContextMenu.Show(ccsTree, e.X, e.Y);
							sceneAnimeContextMenu.Show(sceneTreeView, e.X, e.Y);
						}
					}
				}
			}
		}
		#region CCSAnime
		void SceneAnimeContext_RemoveMenuItemClick(object sender, EventArgs e)
		{
			var node = sceneTreeView.SelectedNode;
			var nodeTag = (TreeNodeTag)node.Tag;
			if(nodeTag != null)
			{
				sceneTreeView.Nodes.Remove(node);
				CCSAnime tmpAnime = nodeTag.File.GetObject<CCSAnime>(nodeTag.ObjectID);
				if(tmpAnime != null)
				{
					Scene.RemoveAnime(tmpAnime);
				}
			}
		}

		
		#endregion
		
		
		#endregion

	}
}
