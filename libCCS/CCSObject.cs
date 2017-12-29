/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 12:41 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using System.Diagnostics;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSObject.
	/// </summary>
	public class CCSObject : CCSBaseObject
	{
		public int ParentObjectID = 0;
		public int ModelID = 0;
		public int ShadowID = 0;
		public float Alpha = 0.1f;
		public CCSClump ParentClump = null;
		public CCSModel ChildModel = null;
		public CCSModel ShadowModel = null;
		public Matrix4 AnimationMatrix = Matrix4.Identity;
		public int NodeID;
		
		public CCSObject(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_OBJECT;
		}

		public override bool Init()
		{
			if(ChildModel != null) ChildModel.Init();
			if(ShadowModel != null) ShadowModel.Init();
			return true;
		}
		public override bool DeInit()
		{
			if(ChildModel != null) ChildModel.DeInit();
			if(ShadowModel != null) ShadowModel.DeInit();
			return true;
		}
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			ParentObjectID = bStream.ReadInt32();
			ModelID = bStream.ReadInt32();
			ShadowID = bStream.ReadInt32();
			
			//TODO: CCSObject::Read(): Gen2 Extra Data
			if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1)
			{
				bStream.ReadUInt32();
			}
			
			return true;
		}
		
		public override TreeNode ToNode()
		{
			//TODO: CCSObject::ToNode(): Make it Heirarchal or Heirarchical or an actual tree.
			var retNode = base.ToNode();
			if(ModelID == 0) retNode.Nodes.Add("Model: <NONE>");
			else
			{
				if(ChildModel == null)
				{
					var modelNode = Util.NonExistantNode(ParentFile, ModelID);
					modelNode.Text = "Model: " + modelNode.Text;
					retNode.Nodes.Add(modelNode);
				}
				else
				{
					var ModelNode = ChildModel.ToNode();
					ModelNode.Text = "Model: " + ModelNode.Text;
					retNode.Nodes.Add(ModelNode);
				}
			}
			
			if(ShadowID == 0) retNode.Nodes.Add("Shadow: <NONE>");
			else
			{
				if(ShadowModel == null)
				{
					var shadowNode = Util.NonExistantNode(ParentFile, ShadowID);
					shadowNode.Text = "Shadow: " + shadowNode.Text;
					retNode.Nodes.Add(shadowNode);
				}
				else
				{
					var shadowNode = ShadowModel.ToNode();
					shadowNode.Text = "Shadow: " + shadowNode.Text;
					retNode.Nodes.Add(shadowNode);
				}
			}
			return retNode;
		}

		public void SetParentClump(CCSClump _parentClump, int _nodeID)
		{
			ParentClump = _parentClump;
			NodeID = _nodeID;
			if(ModelID != 0)
			{
				ChildModel = ParentFile.GetObject<CCSModel>(ModelID);
				if(ChildModel != null) ChildModel.SetClump(_parentClump, this);
			}
			
			if(ShadowID != 0)
			{
				ShadowModel = ParentFile.GetObject<CCSModel>(ShadowID);
				if(ShadowModel != null) ShadowModel.SetClump(_parentClump, this);
			}
		
		}
		
		public void FrameForward()
		{
			Matrix4 parentMatrix = Matrix4.Identity;
			if(ParentObjectID != 0)
			{
				CCSObject tmpObj = ParentFile.GetObject<CCSObject>(ParentObjectID);
				if(tmpObj != null)
				{
					parentMatrix = tmpObj.GetFinalMatrix();
					//Vector3 parentPosition = parentMatrix.ExtractTranslation();
					//Vector3 parentScale = parentMatrix.ExtractScale();
					//parentMatrix = Matrix4.CreateTranslation(parentPosition) * Matrix4.CreateScale(parentScale);
				}
			}
			SetFinalMatrix(GetPoseMatrix() * parentMatrix);
			//SetFinalMatrix(parentMatrix * GetPoseMatrix());
			//SetFinalMatrix(GetPoseMatrix());
		}
		
		public void Render(Matrix4 mtx, int extraOptions = 0)
		{
			//FrameForward();
			
			if(ChildModel != null)
			{
				//Debug.WriteLine("Rendering Obj {0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
				ChildModel.Render(mtx, extraOptions);
			}
		}
		
		public string GetReport(int level = 0)
		{
			string retVal = Util.Indent(level) + string.Format("Object 0x{0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			if(ParentObjectID == 0)
			{
				retVal += Util.Indent(level + 1) + "Parent: None\n";
			}
			else
			{
				retVal += Util.Indent(level + 1) + string.Format("Parent: 0x{0:X}: {1}\n", ParentObjectID, ParentFile.GetSubObjectName(ParentObjectID));
			}
			retVal += Util.Indent(level + 1) + "Model:\n";
			string modelReport = Util.Indent(level + 2) + "<None>\n";
			if(ChildModel != null)
			{
				modelReport = ChildModel.GetReport(level + 2);
			}
			retVal += modelReport;
			
			retVal += Util.Indent(level + 1) + "Shadow Model:\n";
			string shadowReport = Util.Indent(level + 2) + "<None>\n";
			if(ShadowModel != null)
			{
				shadowReport = ShadowModel.GetReport(level + 2);
			}
			retVal += shadowReport;
			
			return retVal;
		}
		
		public Matrix4 GetPoseMatrix()
		{
			return ParentClump.GetPoseMatrix(NodeID);
		}
		
		/*
		public void SetPoseMatrix(Matrix4 newMatrix)
		{
			ParentClump.SetPoseMatrix(NodeID, newMatrix);
		}
		*/
		
		public void SetPose(Vector3 Position, Vector3 Rotation, Vector3 Scale)
		{
			ParentClump.SetPose(NodeID, Position, Rotation, Scale);
		}
		
		public void SetPose(Vector3 Position, Quaternion Rotation, Vector3 Scale)
		{
			ParentClump.SetPose(NodeID, Position, Rotation, Scale);
		}
		
		public Matrix4 GetFinalMatrix()
		{
			return ParentClump.GetFinalMatrix(NodeID);
		}
		
		public void SetFinalMatrix(Matrix4 newMatrix)
		{
			ParentClump.SetFinalMatrix(NodeID, newMatrix);
		}
		
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split, bool withNormals)
		{
			if(ChildModel != null)
			{
				return ChildModel.DumpToObj(fStream, vOffset, split, withNormals);
			}
			
			return vOffset;
		}
		
		public void DumpToSMD(StreamWriter outf, bool withNormals)
		{
			if(ChildModel != null) ChildModel.DumpToSMD(outf, withNormals);
		}
		
	}
}
