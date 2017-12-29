/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 11/8/2017
 * Time: 5:17 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK;
using System.Diagnostics;
using StudioCCS.libCCS;


namespace StudioCCS
{
	/// <summary>
	/// Description of frmEditBone.
	/// </summary>
	public partial class frmEditBone : Form
	{
		public class BoneNodeTag
		{
			public libCCS.CCSObject Bone;
		}
		
		
		public libCCS.CCSFile OperatingFile = null;
		public libCCS.CCSClump OperatingClump = null;
		public libCCS.CCSObject OperatingObject = null;
		
		public List<TextBox> TextBoxes = new List<TextBox>();
		
		public frmEditBone()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			TextBoxes.Add(txtPosX);
			TextBoxes.Add(txtPosY);
			TextBoxes.Add(txtPosZ);
			
			TextBoxes.Add(txtRotX);
			TextBoxes.Add(txtRotY);
			TextBoxes.Add(txtRotZ);
			
			TextBoxes.Add(txtScaleX);
			TextBoxes.Add(txtScaleY);
			TextBoxes.Add(txtScaleZ);
			
			
			
		}
		
		public void SetClump(libCCS.CCSClump clump)
		{
			OperatingClump = clump;
			OperatingFile = clump.ParentFile;
			OperatingClump.RenderBones = true;
			/*
			listBones.Items.Clear();
			
			for(int i = 0; i < clump.NodeCount; i++)
			{
				listBones.Items.Add(OperatingFile.GetSubObjectName(clump.NodeIDs[i]));
			}
			
			listBones.SelectedIndex = 0;
			*/
			string clumpName = OperatingFile.GetSubObjectName(OperatingClump.ObjectID);
			
			TreeNode mainNode = new TreeNode(clumpName);
			
			List<TreeNode> nodes = new List<TreeNode>();
			for(int i = 0; i < OperatingClump.NodeCount; i++)
			{
				var tmpBone = OperatingClump.GetObject(i);
				var tmpNode = new TreeNode(OperatingFile.GetSubObjectName(tmpBone.ObjectID));
				
				var tmpTag = new BoneNodeTag()
				{
					Bone = tmpBone
				};
				tmpNode.Tag = tmpTag;
				nodes.Add(tmpNode);
				int parentObjectID = tmpBone.ParentObjectID;
				if(parentObjectID != 0)
				{
					int parentNodeID = OperatingClump.SearchNodeID(parentObjectID);
					if(parentNodeID == -1)
					{
						mainNode.Nodes.Add(tmpNode);
					}
					else
					{
						nodes[parentNodeID].Nodes.Add(tmpNode);
					}
				}
				else
				{
					mainNode.Nodes.Add(tmpNode);
				}
			}
			
			
			Debug.WriteLine(string.Format("BoneTree has {0} Nodes...", mainNode.Nodes.Count));
			foreach(var tmpNode in mainNode.Nodes)
			{
				treeBones.Nodes.Add((TreeNode)tmpNode);
			}
			
			Text = string.Format("Edit Bones for {0}...", clumpName);
		}
		
		
		
		void BtnUpdateClick(object sender, EventArgs e)
		{
			Single radTo = 0.0174533f;
			bool result = true;
			float[] vals = {0.0f, 0.0f, 0.0f,  0.0f, 0.0f, 0.0f,   0.0f, 0.0f, 0.0f};
			for(int i = 0; i < TextBoxes.Count; i++)
			{
				var tmpTextBox = TextBoxes[i];
				float tmpVal = 0.0f;
				try
				{
					float.TryParse(tmpTextBox.Text, out tmpVal);
				}
				catch
				{
					tmpTextBox.BackColor = Color.Red;
					result = false;
					continue;
				}
				tmpTextBox.BackColor = Color.White;
				vals[i] = tmpVal;
			}
			
			if(result)
			{
				Vector3 tmpPos = new Vector3(vals[0], vals[1], vals[2]);
				Vector3 tmpRot = new Vector3(vals[3] * radTo, vals[4] * radTo, vals[5] * radTo);
				Vector3 tmpScale= new Vector3(vals[6], vals[7], vals[8]);
				
				if(OperatingObject != null)
				{
					if(OperatingFile.GetVersion() == libCCS.CCSFileHeader.CCSVersion.Gen1)
					{
						OperatingClump.PosePositions[OperatingObject.NodeID] = tmpPos;
						OperatingClump.PoseRotations[OperatingObject.NodeID] = tmpRot;
						OperatingClump.PoseScales[OperatingObject.NodeID] = tmpScale;
					}
					else
					{
					//OperatingObject.SetPose(tmpPos, tmpRot, tmpScale);
						OperatingClump.BindPositions[OperatingObject.NodeID] = tmpPos;
						OperatingClump.BindRotations[OperatingObject.NodeID] = tmpRot;
						OperatingClump.BindScales[OperatingObject.NodeID] = tmpScale;
					}
				}
			}
		}
		
		void LoadPoseToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dlg = new OpenFileDialog();
			dlg.Filter = "Bin Files (*.bin)|*.bin|All Files (*.*)|*.*";
			dlg.Title = "Load Clump Pose";
			
			if(dlg.ShowDialog() != DialogResult.OK) return;
			
			OperatingClump.LoadPose(dlg.FileName);
			Logger.LogInfo(string.Format("Loaded pose for {0} from {1}.\n", OperatingFile.GetSubObjectName(OperatingClump.ObjectID), dlg.FileName));

		}
		
		void SavePoseToolStripMenuItemClick(object sender, EventArgs e)
		{
			var dlg = new SaveFileDialog();
			dlg.Filter = "Bin Files (*.bin)|*.bin|All Files (*.*)|*.*";
			dlg.Title = "Save Clump Pose";
			
			if(dlg.ShowDialog() != DialogResult.OK) return;
			
			OperatingClump.SavePose(dlg.FileName);
			Logger.LogInfo(string.Format("Saved pose for {0} to {1}.\n", OperatingFile.GetSubObjectName(OperatingClump.ObjectID), dlg.FileName));
		}
		void TreeBonesAfterSelect(object sender, TreeViewEventArgs e)
		{
			var tmpNode = e.Node;
			var tmpTag = (BoneNodeTag)tmpNode.Tag;
			if(tmpTag != null)
			{
				OperatingObject = tmpTag.Bone;
				OperatingClump.SelectedBoneID = OperatingObject.NodeID;
				
				Vector3 tmpPos = OperatingClump.BindPositions[OperatingObject.NodeID];
				Vector3 tmpRot = OperatingClump.BindRotations[OperatingObject.NodeID];
				Vector3 tmpScale = OperatingClump.BindScales[OperatingObject.NodeID];
				if(OperatingFile.GetVersion() == libCCS.CCSFileHeader.CCSVersion.Gen1)
				{
					tmpPos = OperatingClump.PosePositions[OperatingObject.NodeID];
					tmpRot = OperatingClump.PoseRotations[OperatingObject.NodeID];
					tmpScale = OperatingClump.PoseScales[OperatingObject.NodeID];
				}
				
				txtPosX.Text = tmpPos.X.ToString();
				txtPosY.Text = tmpPos.Y.ToString();
				txtPosZ.Text = tmpPos.Z.ToString();
				
				float pi = 3.14159265f;
				txtRotX.Text = (tmpRot.X * 180.0f / pi).ToString();
				txtRotY.Text = (tmpRot.Y * 180.0f / pi).ToString();
				txtRotZ.Text = (tmpRot.Z * 180.0f / pi).ToString();
				
				txtScaleX.Text = tmpScale.X.ToString();
				txtScaleY.Text = tmpScale.Y.ToString();
				txtScaleZ.Text = tmpScale.Z.ToString();
				
				lblBoneName.Text = OperatingFile.GetSubObjectName(OperatingObject.ObjectID);
			}
		}
		void ClearRotationValuesToolStripMenuItemClick(object sender, EventArgs e)
		{
			for(int i = 0; i < OperatingClump.NodeCount; i++)
			{
				if(OperatingFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
				{
					OperatingClump.PoseRotations[i] = Vector3.Zero;
				}
				else
				{
					OperatingClump.BindRotations[i] = Vector3.Zero;
					OperatingClump.PoseQuats[i] = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
				}
			}
		}
		void FrmEditBoneFormClosed(object sender, FormClosedEventArgs e)
		{
			OperatingClump.RenderBones = false;
			OperatingClump.SelectedBoneID = -1;
		}
		void ClearScaleValuesToolStripMenuItemClick(object sender, EventArgs e)
		{
			for(int i = 0; i < OperatingClump.NodeCount; i++)
			{
				OperatingClump.BindScales[i] = Vector3.One;
				OperatingClump.PoseScales[i] = Vector3.One;
			}
		}
		
		
	}
}
