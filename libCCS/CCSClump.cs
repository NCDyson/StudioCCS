/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 12:49 AM
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
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSClump.
	/// </summary>
	public class CCSClump : CCSBaseObject
	{
		public Matrix4[] PoseMatrixList = null;
		public Matrix4[] FinalMatrixList = null;
		public int[] NodeIDs = null;
		public int NodeCount = 0;
		
		public Vector3[] BindPositions = null;
		public Vector3[] BindRotations = null;
		public Vector3[] BindScales = null;
	

		public Vector3[] PosePositions = null;
		public Vector3[] PoseRotations = null;
		public Quaternion[] PoseQuats = null;
		public Vector3[] PoseScales = null;
		
		public int MatrixBuffer = -1;
		public int MatrixTexture = -1;
		
		//BoneVis stuff
		public static int ProgramID = -1;
		public static int ProgramRefs = 0;
		public static int AttribEndpoints = -1;
		public static int UniformMatrix = -1;
		public static int UniformSelectionID = -1;
		public static int UniformMatrixList = -1;
		
		[StructLayout(LayoutKind.Sequential)]
		public struct BoneVis
		{
			public int HeadID;
			public int TailID;
		}
		
		
		public int BoneVisVAO = -1;
		public int BoneVisVBO = -1;
		public BoneVis[] BoneVisBones = null;
		public bool RenderBones = false;
		public int SelectedBoneID = -1;
		
		
		public CCSClump(int _objectID, CCSFile _parentFile)
		{
			ParentFile = _parentFile;
			ObjectID = _objectID;
			ObjectType = CCSFile.SECTION_CLUMP;
		}
		
		public override bool DeInit()
		{
			for(int i = 0; i < NodeCount; i++)
			{
				CCSObject tmpObj = GetObject(i);
				tmpObj.DeInit();
			}
			
			GL.DeleteBuffer(MatrixBuffer);
			MatrixBuffer = -1;
			GL.DeleteTexture(MatrixTexture);
			MatrixTexture = -1;
			
			GL.DeleteBuffer(BoneVisVBO);
			GL.DeleteVertexArray(BoneVisVAO);
			BoneVisVBO = -1;
			BoneVisVAO = -1;
			
			ProgramRefs -= 1;
			if(ProgramRefs <= 0)
			{
				if(ProgramID != -1) GL.DeleteProgram(ProgramID);
				ProgramID = -1;
			}
			
			return true;
		}
		
		public override bool Init()
		{
			/*
			for(int i = 0; i < NodeCount; i++)
			{
				CCSObject childObject = ParentFile.GetObject<CCSObject>(NodeIDs[i]);
				if(childObject != null)
				{
					childObject.SetParentClump(this, i);
				}
				else
				{
					Logger.LogError(string.Format("Clump {0} Init: could not get child Obj {1}({2}: {3})", ObjectID, i, NodeIDs[i], ParentFile.GetSubObjectName(NodeIDs[i])));
					return false;
				}
			}
			*/

			/*			
			for(int i = 0; i < NodeCount; i++)
			{
				int nodeType = ParentFile.GetSubObjectType(NodeIDs[i]);
				if(nodeType == CCSFile.SECTION_OBJECT)
				{
					var childObject = ParentFile.GetObject<CCSObject>(NodeIDs[i]);
					if(childObject != null)
					{
						childObject.SetParentClump(this, i);
					}
					else
					{
						Logger.LogError(string.Format("Clump {0:X}: {1} Init: could not get child Object {2:X} {3}", ObjectID, ParentFile.GetSubObjectName(ObjectID), NodeIDs[i], ParentFile.GetSubObjectName(NodeIDs[i])));
						return false;
					}
				}
				*/
				/*
				else if(nodeType == CCSFile.SECTION_EFFECT)
				{
					var childEffect = ParentFile.GetObject<CCSEffect>(NodeIDs[i]);
					if(childEffect != null)
					{
						
					}
					else
					{
						Logger.LogError(string.Format("Clump {0:X}: {1} Init: could not get child Effect {2:X} {3}", ObjectID, ParentFile.GetSubObjectName(ObjectID), NodeIDs[i], ParentFile.GetSubObjectName(NodeIDs[i])));
						return false;
					}
				}
				*/
			//}
			
			//Load shaders and stuff
			
			if(ProgramID == -1)
			{
				ProgramID = Scene.LoadProgram("CCSClump", true);
				if(ProgramID == -1)
				{
					Logger.LogError("Error loading Shader program for CCSClump\n");
					return false;
				}
				
				AttribEndpoints = GL.GetAttribLocation(ProgramID, "vEndpoints");
				
				if(AttribEndpoints == -1)
				{
					Logger.LogError("Error getting CCSClump Shader Attributes:\n");
					Logger.LogError(string.Format("\tEndpoints: {0}", AttribEndpoints));
					return false;
				}
				
				UniformMatrix = GL.GetUniformLocation(ProgramID, "uMatrix");
				UniformSelectionID = GL.GetUniformLocation(ProgramID, "selectedID");
				UniformMatrixList = GL.GetUniformLocation(ProgramID, "uMatrixList");
				
				if(UniformMatrix == -1 || UniformSelectionID == -1 || UniformMatrix == -1)
				{
					Logger.LogError("Error getting CCSClump Shader Uniforms:\n");
					Logger.LogError(string.Format("\tMatrix: {0}, SelectionID: {1}, UniformMatrix: {2}", UniformMatrix, UniformSelectionID, UniformMatrixList));
					return false;
				}
			}
			
			
			ProgramRefs += 1;
			BoneVisVAO = GL.GenVertexArray();
			GL.BindVertexArray(BoneVisVAO);
			
			BoneVisVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, BoneVisVBO);
			//GL.BufferData(BufferTarget.ArrayBuffer, 8 * NodeCount, BoneVisBones, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(AttribEndpoints);
			GL.VertexAttribIPointer(AttribEndpoints, 2, VertexAttribIntegerType.Int, Marshal.SizeOf(BoneVisBones[0].GetType()), Marshal.OffsetOf(BoneVisBones[0].GetType(), "HeadID"));
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			
			//Generate matrix list buffer...
			MatrixBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.TextureBuffer, MatrixBuffer);

		
			MatrixTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureBuffer, MatrixTexture);
			GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, MatrixBuffer);
			GL.BufferData(BufferTarget.TextureBuffer, Marshal.SizeOf(FinalMatrixList[0].GetType()) * NodeCount, FinalMatrixList, BufferUsageHint.DynamicDraw);
			
			GL.BindTexture(TextureTarget.TextureBuffer, 0);
			//GL.BindBuffer(BufferTarget.TextureBuffer, 0);
			
			
			//TODO: CCSClump::Init(): Can we condense this down to just 1 loop?
			for(int i = 0; i < NodeCount; i++)
			{
				int nodeType = ParentFile.GetSubObjectType(NodeIDs[i]);
				if(nodeType == CCSFile.SECTION_OBJECT)
				{
					var childObject = ParentFile.GetObject<CCSObject>(NodeIDs[i]);
					if(childObject != null)
					{
						childObject.SetParentClump(this, i);
						childObject.Init();
						
						
						if(childObject.ParentObjectID != 0)
						{
							var tmpParentObj = ParentFile.GetObject<CCSObject>(childObject.ParentObjectID);
							if(tmpParentObj != null)
							{
								var tmpVis = BoneVisBones[tmpParentObj.NodeID];
								if(tmpVis.TailID == tmpVis.HeadID)
								{
									BoneVisBones[tmpParentObj.NodeID].TailID = i;
								}
							}
						}
						
					}
				}
				else if(nodeType == CCSFile.SECTION_EFFECT)
				{
					var childEffect = ParentFile.GetObject<CCSEffect>(NodeIDs[i]);
					if(childEffect != null) childEffect.Init();
					else
					{
						Logger.LogError(string.Format("Clump {0:X}: {1} Init: could not get child Effect {2:X} {3}", ObjectID, ParentFile.GetSubObjectName(ObjectID), NodeIDs[i], ParentFile.GetSubObjectName(NodeIDs[i])));
						return false;
					}
				}
			}
			
			GL.BindVertexArray(BoneVisVAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, BoneVisVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, 8 * NodeCount, BoneVisBones, BufferUsageHint.StaticDraw);
			GL.BindVertexArray(0);
			
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			NodeCount = bStream.ReadInt32();
			NodeIDs = new int[NodeCount];
			PoseMatrixList = new Matrix4[NodeCount];
			FinalMatrixList = new Matrix4[NodeCount];
			BindPositions = new Vector3[NodeCount];
			BindRotations = new Vector3[NodeCount];
			BindScales = new Vector3[NodeCount];
			//TODO: CCSClump: Pose Vec3 arrays is kinda lame...
			PosePositions = new Vector3[NodeCount];
			
			BoneVisBones = new BoneVis[NodeCount];
			
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
			{
				PoseRotations = new Vector3[NodeCount];	
			}
			else
			{
				PoseQuats = new Quaternion[NodeCount];
			}
			
			PoseScales = new Vector3[NodeCount];
			
			for(int i = 0; i < NodeCount; i++)
			{
				NodeIDs[i] = bStream.ReadInt32();
			}
			
			//TODO: CCSClump::Read(): Gen2 Extra Data, is this the bind pose?
			if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1)
			{
				for(int i = 0; i < NodeCount; i++)
				{
					Vector3 pos = Util.ReadVec3Position(bStream);
					Vector3 rot = Util.ReadVec3Rotation(bStream);
					Vector3 scale = Util.ReadVec3Scale(bStream);
					
					BindPositions[i] = pos;
					BindRotations[i] = rot;
					BindScales[i] = scale;
					
					PosePositions[i] = Vector3.Zero;
					//PoseRotations[i] = Vector3.Zero;
					PoseQuats[i] = Quaternion.Identity; //Vector4.UnitW;
					//PoseQuats[i].W = -PoseQuats[i].W;
					PoseScales[i] = Vector3.One;
					BoneVisBones[i].HeadID = i;
					BoneVisBones[i].TailID = i;
					//var rotQuat = new Quaternion(rot);
					//var rotQuat = new Quaternion(Vector3.Zero);
			//		PoseMatrixList[i] = Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotQuat) * Matrix4.CreateTranslation(pos);
					//PoseMatrixList[i] = Matrix4.CreateTranslation(pos) * Matrix4.CreateFromQuaternion(rotQuat) * Matrix4.CreateScale(scale);
					//MatrixList[i] = Matrix4.CreateTranslation(pos) * Matrix4.CreateFromQuaternion(rotQuat) * Matrix4.CreateScale(scale);
					
					//PoseMatrixList[i] = Matrix4.CreateTranslation(pos);
				}
			}
			else
			{
				for(int i = 0; i < NodeCount; i++)
				{
					//PoseMatrixList[i] = Matrix4.Identity;
					BindPositions[i] = Vector3.Zero;
					PosePositions[i] = Vector3.Zero;
					
					BindRotations[i] = Vector3.Zero;
					PoseRotations[i] = Vector3.Zero;
					
					BindScales[i] = Vector3.One;
					PoseScales[i] = Vector3.One;
					
					BoneVisBones[i].HeadID = i;
					BoneVisBones[i].TailID = i;
				}
			}
			
			return true;
		}
		
		/*
		public void SetPoseMatrix(int _nodeID, Matrix4 _matrix)
		{
			if(_nodeID < NodeCount)
			{
				PoseMatrixList[_nodeID] = _matrix;
			}
		}
		
		public Matrix4 GetPoseMatrix(int _nodeID)
		{
			if(_nodeID < NodeCount) return PoseMatrixList[_nodeID];
			
			Logger.LogWarning(string.Format("Clump {0}: GetPoseMatrix({1}), Invalid NodeID", ParentFile.GetSubObjectName(ObjectID), _nodeID), Logger.LogType.LogOnceValue);
			return Matrix4.Identity;
		}
		*/
		
		public void SetPose(int _nodeID, Vector3 Positon, Vector3 Rotation, Vector3 Scale)
		{
			if(_nodeID < NodeCount)
			{
				PosePositions[_nodeID] = Positon;
				PoseRotations[_nodeID] = Rotation;
				PoseScales[_nodeID] = Scale;
			}
		}
		
		public void SetPose(int _nodeID, Vector3 Position, Quaternion Rotation, Vector3 Scale)
		{
			if(_nodeID < NodeCount)
			{
				PosePositions[_nodeID] = Position;
				PoseQuats[_nodeID] = Rotation;
				PoseScales[_nodeID] = Scale;
			}
		}
		
		public Matrix4 GetPoseMatrix(int _nodeID)
		{
			if(_nodeID < NodeCount)
			{
				if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
				{
					PoseMatrixList[_nodeID] = Matrix4.CreateScale(PoseScales[_nodeID]) * Matrix4.CreateFromQuaternion(new Quaternion(PoseRotations[_nodeID])) * Matrix4.CreateTranslation(PosePositions[_nodeID]);
					//PoseMatrixList[_nodeID] *= Matrix4.CreateScale(BindScales[_nodeID]) * Matrix4.CreateFromQuaternion(new Quaternion(PoseRotations[_nodeID])) * Matrix4.CreateTranslation(BindPositions[_nodeID]);
				
					return PoseMatrixList[_nodeID];
				}
				else
				{
					//Quaternion rotQuat = new Quaternion(PoseQuats[_nodeID].X, PoseQuats[_nodeID].Y,PoseQuats[_nodeID].Z,PoseQuats[_nodeID].W);
					PoseMatrixList[_nodeID] = Matrix4.CreateScale(PoseScales[_nodeID]) * Matrix4.CreateFromQuaternion(PoseQuats[_nodeID]) * Matrix4.CreateTranslation(PosePositions[_nodeID]);
					PoseMatrixList[_nodeID] *= Matrix4.CreateScale(BindScales[_nodeID]) * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(BindRotations[_nodeID])) * Matrix4.CreateTranslation(BindPositions[_nodeID]);
					//PoseMatrixList[_nodeID] *= Matrix4.CreateTranslation(BindPositions[_nodeID]);
					//PoseMatrixList[_nodeID] = Matrix4.CreateScale(BindScales[_nodeID]) * Matrix4.CreateFromQuaternion(new Quaternion(BindRotations[_nodeID])) * Matrix4.CreateTranslation(BindPositions[_nodeID]) * PoseMatrixList[_nodeID];
					return PoseMatrixList[_nodeID];
				}
			}
			return Matrix4.Identity;
		}
		
		
		/*
		public int GetPose(int nodeID, out Vector3 Pos, out Vector3 Rot, out Vector3 Scale)
		{
			if(nodeID < NodeCount)
			{
				Pos = BindPositions[nodeID];
				Rot = BindRotations[nodeID];
				Scale = BindScales[nodeID];
				
				return 1;
			}
			
			return 0;
		}
		*/
		
		public void SetFinalMatrix(int _nodeID, Matrix4 _matrix)
		{
			if(_nodeID < NodeCount)
			{
				FinalMatrixList[_nodeID] = _matrix;
			}
		}
		
		public Matrix4 GetFinalMatrix(int _nodeID)
		{
			if(_nodeID < NodeCount) return FinalMatrixList[_nodeID];
			
			Logger.LogWarning(string.Format("Clump {0}: GetFinalMatrix({1}), Invalid NodeID", ParentFile.GetSubObjectName(ObjectID), _nodeID), Logger.LogType.LogOnceValue);
			return Matrix4.Identity;
		}
		
		
		public CCSObject GetObject(int _nodeID)
		{
			if(_nodeID < NodeCount)
			{
				return ParentFile.GetObject<CCSObject>(NodeIDs[_nodeID]);
			}
			return null;
		}
		
		public void BindMatrixList()
		{
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.TextureBuffer, MatrixTexture);
			
			GL.BindBuffer(BufferTarget.TextureBuffer, MatrixBuffer);
			GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, MatrixBuffer);
			GL.BufferData(BufferTarget.TextureBuffer, Marshal.SizeOf(FinalMatrixList[0].GetType()) * NodeCount, FinalMatrixList, BufferUsageHint.DynamicDraw);
		}
		
		public void UpdateMatrixList()
		{
			GL.BufferData(BufferTarget.TextureBuffer, Marshal.SizeOf(FinalMatrixList[0].GetType()) * NodeCount, FinalMatrixList, BufferUsageHint.DynamicDraw);
		}
		
		public void Render(Matrix4 projView, int extraOptions = 0)
		{
			
			FrameForward();
			/*
			for(int i = 0; i < NodeCount; i++)
			{
				FinalMatrixList[i] = Matrix4.Identity;
			}
			*/
			/*
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.TextureBuffer, MatrixTexture); //(BufferTarget.TextureBuffer, MatrixBuffer);
			
			GL.BindBuffer(BufferTarget.TextureBuffer, MatrixBuffer);
			GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, MatrixBuffer);
			GL.BufferData(BufferTarget.TextureBuffer, Marshal.SizeOf(FinalMatrixList[0].GetType()) * NodeCount, FinalMatrixList, BufferUsageHint.DynamicDraw);
			*/
			BindMatrixList();
			//projView = Matrix4.Identity;
			
			for(int i = 0; i < NodeCount; i++)
			{
				int nodeType = ParentFile.GetSubObjectType(NodeIDs[i]);
				if(nodeType == CCSFile.SECTION_OBJECT)
				{
					CCSObject tmpObj = ParentFile.GetObject<CCSObject>(NodeIDs[i]);
					if(tmpObj != null) tmpObj.Render(projView, extraOptions);
				}
				/*
				else if(nodeType == CCSFile.SECTION_EFFECT)
				{
					
				}
				*/
			}
			
			if(RenderBones)
			{
				GL.UseProgram(ProgramID);
				GL.BindVertexArray(BoneVisVAO);
			
				GL.Uniform1(UniformSelectionID, SelectedBoneID);
				GL.UniformMatrix4(UniformMatrix, false, ref projView);
				GL.Uniform1(UniformMatrixList, 1);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				GL.DrawArrays(PrimitiveType.Points, 0, NodeCount);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				GL.UseProgram(0);
				GL.BindVertexArray(0);
			}
			//GL.ActiveTexture(TextureUnit.Texture1);
			//GL.BindTexture(TextureTarget.TextureBuffer, 0);
			//GL.BindBuffer(BufferTarget.TextureBuffer, 0);
			
		}
		
		public override TreeNode ToNode()
		{
			var retNode = base.ToNode();
			List<TreeNode> tNodes = new List<TreeNode>();
			//Add child nodes.
			for(int i = 0; i < NodeCount; i++)
			{
				CCSObject childObject = ParentFile.GetObject<CCSObject>(NodeIDs[i]);
				TreeNode tmpNode = null;
				if(childObject != null)
				{
					tmpNode = childObject.ToNode();
				}
				else
				{
					tmpNode = Util.NonExistantNode(ParentFile, NodeIDs[i]);
				}
				
				tNodes.Add(tmpNode);
				int parentNodeID = 0;
				if(childObject == null)
				{
					parentNodeID = -1;
				}
				else
				{
				 	parentNodeID = SearchNodeID(childObject.ParentObjectID);
				}
				
				if(parentNodeID == -1)
				{
					retNode.Nodes.Add(tmpNode);
				}
				else
				{
					tNodes[parentNodeID].Nodes.Add(tmpNode);
				}
				
			}
			return retNode;
		}
		
		public string GetReport(int level = 0)
		{
			string retVal = Util.Indent(level) + string.Format("Clump: 0x{0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			retVal += Util.Indent(level) + string.Format("0x{0:X}({0}) Nodes\n", NodeCount);
			for(int i = 0; i < NodeCount; i++)
			{
				int nodeType = ParentFile.GetSubObjectType(NodeIDs[i]);
				if(nodeType == CCSFile.SECTION_OBJECT)
				{
					if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1)
					{
						Vector3 p = BindPositions[i];
						Vector3 r = BindRotations[i];
						Vector3 s = BindScales[i];
						retVal += Util.Indent(level + 1) + String.Format("Node {0}\n", i);
						retVal += Util.Indent(level + 1) + string.Format("{0}, {1}, {2}\n", p.X, p.Y, p.Z);
						retVal += Util.Indent(level + 1) + string.Format("{0}, {1}, {2}\n", r.X, r.Y, r.Z);
						retVal += Util.Indent(level + 1) + string.Format("{0}, {1}, {2}\n", s.X, s.Y, s.Z);
						
					}
						retVal += GetObject(i).GetReport(level + 1);
					
				}
				else if(nodeType == CCSFile.SECTION_EFFECT)
				{
					retVal += Util.Indent(level + 2) + string.Format("Effect 0x{0:X}: {1}\n", NodeIDs[i], ParentFile.GetSubObjectName(NodeIDs[i]));
					retVal += Util.Indent(level + 3) + string.Format("No Information currently Available\n");
				}
			}
			
			return retVal + "\n";
		}
		
		public void FrameForward()
		{
			for(int i = 0; i < NodeCount; i++)
			{
				var tmpObj = GetObject(i);
				if(tmpObj != null) tmpObj.FrameForward();
			}
		}
		
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split, bool withNormals)
		{
			FrameForward();
			int totalVertCount = vOffset;
			for(int i = 0; i < NodeCount; i++)
			{
				var tmpObj = GetObject(i);
				if(tmpObj != null)
				{
					totalVertCount = tmpObj.DumpToObj(fStream, totalVertCount, split, withNormals);
				}
			}
			
			return totalVertCount;
		}
		
		public void DumpToSMD(string outputPath, bool withNormals)
		{
			FrameForward();
			string outputFileName = System.IO.Path.Combine(outputPath, ParentFile.GetSubObjectName(ObjectID)) + ".smd";
			using(var fs = new FileStream(outputFileName, FileMode.Truncate))
			{
				using(var outf = new StreamWriter(fs))
				{
					Logger.LogInfo(string.Format("Dumping {0} to {1}...\n", ParentFile.GetSubObjectName(ObjectID), outputFileName));
					outf.WriteLine("version 1");
					
					outf.WriteLine("nodes");
					for(int i = 0; i < NodeCount; i++)
					{
						string boneName = ParentFile.GetSubObjectName(NodeIDs[i]).Replace(" ", "_");
						CCSObject b = GetObject(i);
						int pId = SearchNodeID(b.ParentObjectID);
						outf.WriteLine(string.Format("{0} \"{1}\" {2}", i, boneName, pId));
					}
					outf.WriteLine("end");
					outf.WriteLine("skeleton");
					/*
					outf.WriteLine("time 0");
					for(int i = 0; i < NodeCount; i++)
					{
						outf.WriteLine(string.Format("{0} 0.0 0.0 0.0 0.0 0.0 0.0", i));
					}
					*/
					
					outf.WriteLine("time 0");
					for(int i = 0; i < NodeCount; i++)
					{
						//float rad = 0.0174533f;
						//Vector3 bRot = Util.FixAxisRotation(BindRotations[i]);
						string pStr = string.Format("{0} {1} {2}", BindPositions[i].X, BindPositions[i].Y, BindPositions[i].Z);
						string rStr = string.Format("{0} {1} {2}", BindRotations[i].X, BindRotations[i].Y, -BindRotations[i].Z);
						//string rStr = string.Format("{0} {1} {2}", bRot.X, bRot.Y, bRot.Z);
						//string rStr = string.Format("{2} {1} {0}", -(BindRotations[i].X * rad), -(BindRotations[i].Y * rad), -(BindRotations[i].Z * rad));
						//string pStr = "0.0 0.0 0.0";
						//string rStr = "0.0 0.0 0.0";
						outf.WriteLine(string.Format("{0} {1} {2}", i, pStr, rStr));
					}
					
					outf.WriteLine("end");
					
					
					outf.WriteLine("triangles");
					//Now, write out the models...
					for(int i = 0; i < NodeCount; i++)
					{
						CCSObject tmpObj = GetObject(i);
						tmpObj.DumpToSMD(outf, withNormals);
					}
					outf.WriteLine("end");
					
				}
			}
			/*
			outputFileName = System.IO.Path.Combine(outputPath, ParentFile.GetSubObjectName(ObjectID)) + "_bind.smd";
			Logger.LogInfo(string.Format("Dumping bind pose to {0} to {1}...\n", ParentFile.GetSubObjectName(ObjectID), outputFileName));
			using(var fs = new FileStream(outputFileName, FileMode.OpenOrCreate))
			{
				using(var outf = new StreamWriter(fs))
				{
					outf.WriteLine("version 1");
					
					outf.WriteLine("nodes");
					for(int i = 0; i < NodeCount; i++)
					{
						string boneName = ParentFile.GetSubObjectName(NodeIDs[i]);
						CCSObject b = GetObject(i);
						int pId = SearchNodeID(b.ParentObjectID);
						outf.WriteLine(string.Format("{0} \"{1}\" {2}", i, boneName, pId));
					}
					outf.WriteLine("end");
					outf.WriteLine("skeleton");
					outf.WriteLine("time 0");
					for(int i = 0; i < NodeCount; i++)
					{
						string pStr = string.Format("{0} {1} {2}", BindPositions[i].X, BindPositions[i].Y, BindPositions[i].Z);
						string rStr = string.Format("{0} {1} {2}", BindRotations[i].X, BindRotations[i].Y, BindRotations[i].Z);
						outf.WriteLine(string.Format("{0} {1} {2}", i, pStr, rStr));
					}
					outf.WriteLine("end");
				}
			}
			*/
		}
		
		public int SearchNodeID(int _objectID)
		{
			for(int i = 0; i < NodeCount; i++)
			{
				if(NodeIDs[i] == _objectID) return i;
			}
			return -1;
		}
		
		public void LoadMatrixList(string fileName)
		{
			using(var fs = new FileStream(fileName, FileMode.Open))
			{
				using(var bs = new BinaryReader(fs))
				{
					for(int i = 0; i < NodeCount; i++)
					{
						float mScale = 0.0625f * 0.1f;
						Vector4 v1 = new Vector4(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
						Vector4 v2 = new Vector4(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
						Vector4 v3 = new Vector4(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
						Vector4 v4 = new Vector4(bs.ReadSingle() * mScale, bs.ReadSingle() * mScale, bs.ReadSingle() * mScale, bs.ReadSingle());
						Matrix4 derp = new Matrix4(v1, v2, v3, v4);
						derp.Invert();
						FinalMatrixList[i] = derp;
						//return new Matrix4(v1, v2, v3, v4)
					}
				}
			}
		}
		
		public void LoadPose(string fileName)
		{
			using(var fs = new FileStream(fileName, FileMode.Open))
			{
				using(var bs = new BinaryReader(fs))
				{
					for(int i = 0; i < NodeCount; i++)
					{
						Vector3 p = new Vector3(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
						Vector3 r = new Vector3(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
						Vector3 s = new Vector3(bs.ReadSingle(), bs.ReadSingle(), bs.ReadSingle());
						
						if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
						{
							PosePositions[i] = p;
							PoseRotations[i] = r;
							PoseScales[i] = s;
						}
						else
						{
							BindPositions[i] = p;
							BindRotations[i] = r;
							BindScales[i] = s;
						}
					}
				}
			}
		}
		
		public void SavePose(string fileName)
		{
			using(var fs = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				using(var bs = new BinaryWriter(fs))
				{
					for(int i = 0; i < NodeCount; i++)
					{
						Vector3 p = BindPositions[i];
						Vector3 r = BindRotations[i];
						Vector3 s = BindScales[i];
					
						if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
						{
							p = PosePositions[i];
							r = PoseRotations[i];
							s = PoseScales[i];
						}
						
						bs.Write(p.X);
						bs.Write(p.Y);
						bs.Write(p.Z);
						bs.Write(r.X);
						bs.Write(r.Y);
						bs.Write(r.Z);
						bs.Write(s.X);
						bs.Write(s.Y);
						bs.Write(s.Z);
					}
				}
			}
		}
		
		
	}
}
