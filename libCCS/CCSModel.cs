/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 5:42 AM
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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSModel.
	/// </summary>
	public class CCSModel : CCSBaseObject
	{
		//Useful Defines
		public const int CCS_MODEL_DEFORMABLE = 0x4;
		public const int CCS_MODEL_SHADOW = 0x8;
		public const int CCS_MODEL_MORPHTARGET = 0x600;
		
		public const int CCS_MODEL_DEFORMABLE_GEN2 = 0x1004;
		
		public const int CCS_MODEL_RIGID_GEN2_NO_COLOR = 0x0200;
		public const int CCS_MODEL_RIGID_GEN2_COLOR = 0x1000;
		public const int CCS_MODEL_RIGID_GEN2_NO_COLOR2 = 0x1200;
		
		public const int CCS_MODEL_MORPHTARGET_GEN2 = 0x400;
		
		#region Sub Structures
		[StructLayout(LayoutKind.Sequential)]
		public struct BoneID
		{
			public int Bone1; // = 0;
			public int Bone2; // = 0;
			public int Bone3;
			public int Bone4;
			
			public BoneID(int id1, int id2, int id3 = 0, int id4 = 0)
			{
				Bone1 = id1;
				Bone2 = id2;
				Bone3 = id3;
				Bone4 = id4;
			}
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct ModelVertex
		{
			public Vector3 Position;
			public Vector3 Position2;
			public Vector3 Position3;	//Last Recode Compatibility
			public Vector3 Position4;	//Last Recode Compatibility
			public Vector2 TexCoords;
			public Vector4 Color;
			public Vector3 Normal;
			public BoneID BoneIDs;
			public Vector4 Weights;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct ModelTriangle
		{
			public int ID1; // = 0;
			public int ID2; // = 1;
			public int ID3; // = 2;
			
			public ModelTriangle(int _id1, int _id2, int _id3)
			{
				ID1 = _id1;
				ID2 = _id2;
				ID3 = _id3;
			}
		}
		
		public class SubModel
		{
			public int ParentID = 0;
			public int VertexCount = 0;
			public int TriangleCount = 0;
			public int MatTexID = 0;
			public ModelVertex[] Vertices = null;
			public ModelTriangle[] Triangles = null;
			
			//OpenGL Stuff
			public int VertexArrayID = -1;
			public int VertexBufferID = -1;
			public int ElementArrayID = -1;
			
			//Helper Refs
			public CCSObject ParentObjectRef = null;
			public CCSTexture ParentTextureRef = null;
			public CCSMaterial ParentMaterialRef = null;
		}
		
		//TODO: CCS Model, Can we just use one shader? 
		/*
		private struct RigidProgram
		{
			public static int ProgramID = -1;
			public static int ProgramRefs = 0;
			public static int AttribPosition = -1;
			public static int AttribTexCoord = -1;
			public static int AttribNormal = -1;
			public static int AttribColor = -1;
			public static int UniformMatrix = -1;
			public static int UniformAlpha = -1;
			public static int UniformTextureOffset = -1;
			public static int UniformTexture = -1;
			public static int UniformDrawOptions = -1;
			public static int UniformSelectionColor = -1;
			public static int UniformRenderMode = -1;
		}
		*/
		public static int ProgramID = -1;
		public static int ProgramRefs = 0;
		public static int AttribPosition = -1;
		public static int AttribPosition1 = -1;
		public static int AttribPosition2 = -1;
		public static int AttribPosition3 = -1;
		public static int AttribTexCoord = -1;
		public static int AttribNormal = -1;
		public static int AttribColor = -1;
		public static int AttribBoneIDs = -1;
		public static int AttribBoneWeights = -1;
		public static int UniformMatrix = -1;
		public static int UniformAlpha = -1;
		public static int UniformTextureOffset = -1;
		public static int UniformTexture = -1;
		public static int UniformDrawOptions = -1;
		public static int UniformSelectionColor = -1;
		public static int UniformRenderMode = -1;
		public static int UniformMatrixList = -1;
		
		#endregion
		
		//Fields
		public float VertexScale = 512.0f;
		public short ModelType = 0;
		public short ActualModelType = 0;
		public short SubModelCount = 0;
		public int VertexCount = 0;
		public int TriangleCount = 0;
		public short DrawFlags = 0;
		public short UnkFlags = 0;
		public int Unk2 = 0;
		public SubModel[] SubModels = null;
		
		//Helper Refs
		public CCSClump ClumpRef;
		public CCSObject ObjectRef;
		
		
		
		
		public CCSModel(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_MODEL;
		}

		public override bool Init()
		{
			LoadShaders();
			ProgramRefs += 1;
			
			//Currently, we don't want to bother with Shadow or MorphTarget rendering...
			if((ModelType & CCS_MODEL_SHADOW) == 0)
			{
				for(int i = 0; i < SubModelCount; i++)
				{
					//Skip the bendy parts for now.
					//if((ModelType & CCS_MODEL_DEFORMABLE) == 1 && i == (SubModelCount - 1)) break;
					//if((ModelType & CCS_MODEL_DEFORMABLE_GEN2) == 1 && i == (SubModelCount - 1)) break;
					SubModel tmpSubModel = SubModels[i];
					
					//Set Convenience refs
					int materialType = ParentFile.GetSubObjectType(tmpSubModel.MatTexID);
					if(materialType == CCSFile.SECTION_MATERIAL)
					{
						tmpSubModel.ParentMaterialRef = ParentFile.GetObject<CCSMaterial>(tmpSubModel.MatTexID);
						if(tmpSubModel.ParentMaterialRef != null)
						{
							tmpSubModel.ParentTextureRef = ParentFile.GetObject<CCSTexture>(tmpSubModel.ParentMaterialRef.TextureID);
						}
						/*
						else
						{
							Logger.LogError(string.Format("Error initializing Model {0}, SubModel({1}), Non-Existent Material referenced: 0x{2:X}\n", ObjectID, i, tmpSubModel.ParentMaterialRef));
							return false;
						}
						*/
					}
					else if(materialType == CCSFile.SECTION_TEXTURE)
					{
						tmpSubModel.ParentMaterialRef = null;
						tmpSubModel.ParentTextureRef = ParentFile.GetObject<CCSTexture>(tmpSubModel.MatTexID);
					}
					
					//if((ModelType < CCS_MODEL_DEFORMABLE) || (ModelType & CCS_MODEL_MORPHTARGET) == CCS_MODEL_MORPHTARGET || (ModelType & CCS_MODEL_RIGID_GEN2_NO_COLOR) == CCS_MODEL_RIGID_GEN2_NO_COLOR)
					if(ModelType != CCS_MODEL_DEFORMABLE && ModelType != CCS_MODEL_DEFORMABLE_GEN2)
					{
						//TODO: CCSModel: Abuse/Take for granted the fact that a model's parent clump *SHOULD* appear in the file before it and 
						//convert the global object ID's to clump node indices like DEFORMABLE models use, so that we can use one shader for static
						//and deformable models?
						//Maybe not, Deformable models don't use morph targets.
						tmpSubModel.ParentObjectRef = ParentFile.GetObject<CCSObject>(tmpSubModel.ParentID);
					}
					//else if((ModelType & CCS_MODEL_DEFORMABLE) == CCS_MODEL_DEFORMABLE || (ModelType & CCS_MODEL_RIGID_GEN2_NO_COLOR) == CCS_MODEL_RIGID_GEN2_NO_COLOR)
					else if(ModelType == CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_DEFORMABLE_GEN2)
					{
						tmpSubModel.ParentObjectRef = ClumpRef.GetObject(tmpSubModel.ParentID);
						if(tmpSubModel.ParentObjectRef == null)
						{
							tmpSubModel.ParentObjectRef = ObjectRef;
						}
					}
					
					
					//GL Init
					tmpSubModel.VertexArrayID = GL.GenVertexArray();
					GL.BindVertexArray(tmpSubModel.VertexArrayID);
					
					tmpSubModel.VertexBufferID = GL.GenBuffer();
					Type vertType = tmpSubModel.Vertices[0].GetType();
					int vertSize = Marshal.SizeOf(vertType);
					
					GL.BindBuffer(BufferTarget.ArrayBuffer, tmpSubModel.VertexBufferID);
					GL.BufferData(BufferTarget.ArrayBuffer, tmpSubModel.VertexCount * vertSize, tmpSubModel.Vertices, BufferUsageHint.StaticDraw);
					
					/*
					
					GL.EnableVertexAttribArray(RigidProgram.AttribPosition);
					GL.VertexAttribPointer(RigidProgram.AttribPosition, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Position"));
					
					GL.EnableVertexAttribArray(RigidProgram.AttribTexCoord);
					GL.VertexAttribPointer(RigidProgram.AttribTexCoord, 2, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "TexCoords"));
					
					GL.EnableVertexAttribArray(RigidProgram.AttribNormal);
					GL.VertexAttribPointer(RigidProgram.AttribNormal, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Normal"));
					
					GL.EnableVertexAttribArray(RigidProgram.AttribColor);
					GL.VertexAttribPointer(RigidProgram.AttribColor, 4, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Color"));
					*/
					
					if(AttribPosition != -1)
					{
						GL.EnableVertexAttribArray(AttribPosition);
						GL.VertexAttribPointer(AttribPosition, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Position"));
					}
					
					if(AttribPosition1 != -1)
					{
						GL.EnableVertexAttribArray(AttribPosition1);
						GL.VertexAttribPointer(AttribPosition1, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Position2"));
					}
						
					if(AttribPosition2 != -1)
					{
						GL.EnableVertexAttribArray(AttribPosition2);
						GL.VertexAttribPointer(AttribPosition2, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Position3"));
					}
						
					if(AttribPosition3 != -1)
					{
						GL.EnableVertexAttribArray(AttribPosition3);
						GL.VertexAttribPointer(AttribPosition3, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Position4"));
					}
					
					GL.EnableVertexAttribArray(AttribTexCoord);
					GL.VertexAttribPointer(AttribTexCoord, 2, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "TexCoords"));
					
					if(AttribNormal != -1)
					{
						GL.EnableVertexAttribArray(AttribNormal);
						GL.VertexAttribPointer(AttribNormal, 3, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Normal"));
					}
						
					GL.EnableVertexAttribArray(AttribColor);
					GL.VertexAttribPointer(AttribColor, 4, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Color"));
					
					if(AttribBoneIDs != -1)
					{
						GL.EnableVertexAttribArray(AttribBoneIDs);
						//GL.VertexAttribPointer(AttribBoneIDs, 2, VertexAttribPointerType.Int, false, vertSize, Marshal.OffsetOf(vertType, "BoneIDs"));
						GL.VertexAttribIPointer(AttribBoneIDs, 4, VertexAttribIntegerType.Int, vertSize, Marshal.OffsetOf(vertType, "BoneIDs"));
					}
					
					if(AttribBoneWeights != -1)
					{
						GL.EnableVertexAttribArray(AttribBoneWeights);
						GL.VertexAttribPointer(AttribBoneWeights, 4, VertexAttribPointerType.Float, false, vertSize, Marshal.OffsetOf(vertType, "Weights"));
					}
						
					tmpSubModel.ElementArrayID = GL.GenBuffer();
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, tmpSubModel.ElementArrayID);
					
					GL.BufferData(BufferTarget.ElementArrayBuffer, 12 * tmpSubModel.TriangleCount, tmpSubModel.Triangles, BufferUsageHint.StaticDraw);
					
					GL.BindVertexArray(0);
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
					GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
					//Debug.WriteLine("Initialized Model {0:X} SubModel {1}, type: {2}\n", ObjectID, i, ModelType);
					
				}
			}
			return true;
		}
		
		/*
		private bool LoadShaders()
		{
			//Check and Load Rigid Shader
			if(RigidProgram.ProgramID == -1)
			{
				RigidProgram.ProgramID = Scene.LoadProgram("CCSRigidModel");
				if(RigidProgram.ProgramID == -1) return false;

				int rp = RigidProgram.ProgramID;
				//Get Attribs
				RigidProgram.AttribPosition = GL.GetAttribLocation(rp, "vPosition");
				RigidProgram.AttribTexCoord = GL.GetAttribLocation(rp, "vTexCoords");
				RigidProgram.AttribNormal = GL.GetAttribLocation(rp, "vNormal");
				RigidProgram.AttribColor = GL.GetAttribLocation(rp, "vColor");
				
				if(RigidProgram.AttribPosition == -1 || RigidProgram.AttribTexCoord == -1 RigidProgram.AttribColor == -1)
				{
					Logger.LogError("Error getting CCSModel Rigid Shader Attributes:");
					Logger.LogError(string.Format("\tPosition: {0}, TexCoord: {1}, Normal: {2}, Color: {3}", RigidProgram.AttribPosition, RigidProgram.AttribTexCoord, RigidProgram.AttribNormal, RigidProgram.AttribColor));
					return false;
				}
				
				//Get Uniforms
				RigidProgram.UniformMatrix = GL.GetUniformLocation(rp, "mMatrix");
				RigidProgram.UniformAlpha = GL.GetUniformLocation(rp, "mAlpha");
				RigidProgram.UniformTextureOffset = GL.GetUniformLocation(rp, "mTextureOffset");
				RigidProgram.UniformTexture = GL.GetUniformLocation(rp, "fTexture");
				RigidProgram.UniformDrawOptions = GL.GetUniformLocation(rp, "mDrawOptions");
				RigidProgram.UniformSelectionColor = GL.GetUniformLocation(rp, "mSelectionColor");
				RigidProgram.UniformRenderMode = GL.GetUniformLocation(rp, "mRenderMode");
				if(RigidProgram.UniformMatrix == -1 || RigidProgram.UniformAlpha == -1 || RigidProgram.UniformTextureOffset == -1 || RigidProgram.UniformDrawOptions == -1 || RigidProgram.UniformRenderMode == -1)
				{
					Logger.LogError("Error getting CCSModel Rigid Shader Uniforms:");
					Logger.LogError(string.Format("\tMatrix: {0}, Alpha: {1}, TextureOffset: {2}, DrawOptions: {3}, RenderMode: {4}", RigidProgram.UniformMatrix, RigidProgram.UniformAlpha, RigidProgram.UniformTextureOffset, RigidProgram.UniformDrawOptions, RigidProgram.UniformRenderMode));
					return false;
				}
			}
			
			//TODO: CCSModel::LoadShaders() Check and Load Deformable Shader 
			return true;
		}*/
		
		public bool LoadShaders()
		{
			if(ProgramID == -1)
			{
				ProgramID = Scene.LoadProgram("CCSModel");
				if(ProgramID == -1) return false;
				
				AttribPosition = GL.GetAttribLocation(ProgramID, "vPosition");
				AttribPosition1 = GL.GetAttribLocation(ProgramID, "vPosition1");
				AttribPosition2 = GL.GetAttribLocation(ProgramID, "vPosition2");
				AttribPosition3 = GL.GetAttribLocation(ProgramID, "vPosition3");
				AttribTexCoord = GL.GetAttribLocation(ProgramID, "vTexCoords");
				AttribColor = GL.GetAttribLocation(ProgramID, "vColor");
				AttribNormal = GL.GetAttribLocation(ProgramID, "vNormal");
				AttribBoneIDs = GL.GetAttribLocation(ProgramID, "vBoneIDs");
				AttribBoneWeights = GL.GetAttribLocation(ProgramID, "vBoneWeights");
				
				if(AttribPosition == -1 || AttribTexCoord == -1 || AttribColor == -1 || AttribBoneIDs == -1 || AttribBoneWeights == -1)
				{
					Logger.LogError("Error getting CCSModel Shader Attributes:\n");
					Logger.LogError(string.Format("\tPosition: {0}, TexCoord: {1}, Normal: {2}, Color: {3}, BoneIDs: {4}, BoneWeights: {5}\n", AttribPosition, AttribTexCoord, AttribNormal, AttribColor, AttribBoneIDs, AttribBoneWeights));
					//return false;
				}
				
				UniformMatrix = GL.GetUniformLocation(ProgramID, "uMatrix");
				UniformAlpha = GL.GetUniformLocation(ProgramID, "uAlpha");
				UniformTextureOffset = GL.GetUniformLocation(ProgramID, "uTextureOffset");
				UniformDrawOptions = GL.GetUniformLocation(ProgramID, "uDrawOptions");
				UniformSelectionColor = GL.GetUniformLocation(ProgramID, "uSelectionColor");
				UniformRenderMode = GL.GetUniformLocation(ProgramID, "uRenderMode");
				UniformTexture = GL.GetUniformLocation(ProgramID, "uTexture");
				UniformMatrixList = GL.GetUniformLocation(ProgramID, "uMatrixList");
				
				if(UniformMatrix == -1 || UniformAlpha == -1 || UniformTextureOffset == -1 || UniformDrawOptions == -1 || UniformSelectionColor == -1 || UniformRenderMode == -1 || UniformTexture == -1 || UniformMatrixList == -1)
				{
					Logger.LogError("Error getting CCSModel Shader Uniforms:\n");
					Logger.LogError(string.Format("\tMatrix: {0}, Alpha: {1}, TextureOffset: {2}, DrawOptions: {3}, Selection Color: {4}, Render Mode: {5}, Texture: {6}, Matrix List: {7}\n", UniformMatrix, UniformAlpha, UniformTextureOffset, UniformDrawOptions, UniformSelectionColor, UniformRenderMode, UniformTexture, UniformMatrixList));
				}
			}
			
			return true;
		}
		

		public override bool DeInit()
		{
			for(uint i = 0; i < SubModelCount; i++)
			{
				SubModel tmpSubModel = SubModels[i];
				if(tmpSubModel.VertexArrayID != -1) GL.DeleteVertexArray(tmpSubModel.VertexArrayID);
				tmpSubModel.VertexArrayID = -1;
				if(tmpSubModel.VertexBufferID != -1) GL.DeleteBuffer(tmpSubModel.VertexBufferID);
				tmpSubModel.VertexBufferID = -1;
				if(tmpSubModel.ElementArrayID != -1) GL.DeleteBuffer(tmpSubModel.ElementArrayID);
				tmpSubModel.ElementArrayID = -1;
			}
			
			
			ProgramRefs -= 1;
			if(ProgramRefs <= 0)
			{
				if(ProgramID != -1) GL.DeleteProgram(ProgramID);
				ProgramID = -1;
			}
			
			return true;
		}
		
		/*
		private const int MODEL_SUBTYPE_RIGID = 0x0;
		private const int MODEL_SUBTYPE_DEFORM = 0x1;
		private const int MODEL_SUBTYPE_MORPH = 0x2;
		private const int MODEL_SUBTYPE_SHADOW = 0x3;
		
		private int GetSubType()
		{
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
			{
				if(ModelType < CCS_MODEL_DEFORMABLE) return MODEL_SUBTYPE_RIGID;
				
				if((ModelType & CCS_MODEL_DEFORMABLE) == CCS_MODEL_DEFORMABLE) return MODEL_SUBTYPE_DEFORM;
				
				if((ModelType & CCS_MODEL_SHADOW) == CCS_MODEL_SHADOW) return MODEL_SUBTYPE_SHADOW;
			
				if((ModelType & CCS_MODEL_MORPHTARGET) == CCS_MODEL_MORPHTARGET) return MODEL_SUBTYPE_MORPH;
			
			}
			else
			{
				if(ModelType < CCS_MODEL_DEFORMABLE) return MODEL_SUBTYPE_RIGID;
				
				if((ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2) return MODEL_SUBTYPE_DEFORM;
				
				if((ModelType & CCS_MODEL_RIGID_GEN2_COLOR) == CCS_MODEL_RIGID_GEN2_COLOR) return MODEL_SUBTYPE_RIGID;
				
				if((ModelType & CCS_MODEL_RIGID_GEN2_NO_COLOR) == CCS_MODEL_RIGID_GEN2_NO_COLOR) return MODEL_SUBTYPE_RIGID;
				
				if((ModelType & CCS_MODEL_MORPHTARGET_GEN2) == CCS_MODEL_MORPHTARGET_GEN2) return MODEL_SUBTYPE_MORPH;
			}
			return 0;
		}
		
		private bool HasTexCoords()
		{
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
			{
				if(GetSubType() == MODEL_SUBTYPE_RIGID) return true;
				if(GetSubType() == MODEL_SUBTYPE_DEFORM) return true;
			}
			else
			{
				if(ModelType < CCS_MODEL_DEFORMABLE) return true;
				if((ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2) return true;
				if((ModelType & CCS_MODEL_RIGID_GEN2_COLOR) == CCS_MODEL_RIGID_GEN2_COLOR) return true;
				if((ModelType & CCS_MODEL_RIGID_GEN2_NO_COLOR) == CCS_MODEL_RIGID_GEN2_NO_COLOR) return true;
			}
			return false;
		}
		
		private bool HasColors()
		{
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
			{
				if(ModelType < CCS_MODEL_DEFORMABLE) return true;
			}
			else
			{
				if(ModelType < CCS_MODEL_DEFORMABLE) return true;
				if((ModelType & CCS_MODEL_RIGID_GEN2_COLOR) == CCS_MODEL_RIGID_GEN2_COLOR) return true;
				if((ModelType & CCS_MODEL_MORPHTARGET_GEN2) == CCS_MODEL_MORPHTARGET_GEN2) return true;
			}
			return false;
		}
		*/
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			Debug.WriteLine("Parsing CCSModel 0x{0:X} at 0x{1:X}", ObjectID, bStream.BaseStream.Position);
			VertexScale = bStream.ReadSingle();
			
			ActualModelType = bStream.ReadInt16();
			ModelType = (short)(ActualModelType & 0xFFFE);
			//ModelType = (short)(bStream.ReadInt16() & 0xFFFE);
			SubModelCount = bStream.ReadInt16();
			
			DrawFlags = bStream.ReadInt16();
			UnkFlags = bStream.ReadInt16();
			
			int gifShit = bStream.ReadInt32();
			Unk2 = gifShit; //Hacks for Gen3
			//CCSModel::Read(): Gen2 Extra Data for CCSModel
			bool gen2 = (ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1);
			if(gen2)
			{
				bStream.ReadSingle();
				bStream.ReadSingle();
			}
			

			
			if(SubModelCount > 0)
			{
				SubModels = new SubModel[SubModelCount];
				if(ModelType < CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_MORPHTARGET || ModelType == CCS_MODEL_RIGID_GEN2_NO_COLOR || ModelType == CCS_MODEL_RIGID_GEN2_COLOR || ModelType  == CCS_MODEL_MORPHTARGET_GEN2 || ModelType == CCS_MODEL_RIGID_GEN2_NO_COLOR2)
				{
					for(int i = 0; i < SubModelCount; i++)
					{
						SubModel tmpSubModel = new SubModel();
						tmpSubModel.ParentID = bStream.ReadInt32();
						tmpSubModel.MatTexID = bStream.ReadInt32();
						tmpSubModel.VertexCount = bStream.ReadInt32();
						ReadRigidSubModel(bStream, tmpSubModel, tmpSubModel.VertexCount);
						SubModels[i] = tmpSubModel;
					}
				}
				//else if(((ModelType & CCS_MODEL_DEFORMABLE) == CCS_MODEL_DEFORMABLE) || ((ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2))
				else if(ModelType == CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_DEFORMABLE_GEN2)
				{
					//Gen2 has a lookup list...
					int[] lookupList = null;
					if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1)
					{
						//TODO: CCSModel::Read(): Gen2 read bone indice list thingy.
						//bStream.BaseStream.Seek(gifShit, SeekOrigin.Current);
						lookupList = new int[gifShit];
						for(int i = 0; i < gifShit; i++)
						{
							lookupList[i] = bStream.ReadByte();
						}
						
						long pos = bStream.BaseStream.Position;
						if((pos % 4) != 0)
						{
							long padSize = 4 - (pos % 4);
							bStream.BaseStream.Seek(padSize, SeekOrigin.Current);
						}
					}
					
					for(int i = 0; i < (SubModelCount - 1); i++)
					{
						Debug.WriteLine("Reading SubModel {0} of {1} at 0x{2:X}", i, SubModelCount + 1, bStream.BaseStream.Position);
						SubModel tmpSubModel = new SubModel();
						tmpSubModel.MatTexID = bStream.ReadInt32();
						tmpSubModel.VertexCount = bStream.ReadInt32();
						bStream.ReadInt32();
						tmpSubModel.ParentID = bStream.ReadInt32();
						if(gen2)
						{
							if(tmpSubModel.ParentID > gifShit)
							{
								Logger.LogError(string.Format("Model 0x{0}: {1}, Sub Model {2}, Lookup table indice out of bounds.", ObjectID, ParentFile.GetSubObjectName(ObjectID), i));
							}
							else
							{
								//TODO: CCSModel: Gen2 Bendy parts need to use the lookup list...
								tmpSubModel.ParentID = lookupList[tmpSubModel.ParentID];
							}
						}
						//ReadRigidSubModel(bStream, tmpSubModel, tmpSubModel.VertexCount);
						ReadDeform_RigidSubModel(bStream, tmpSubModel, tmpSubModel.VertexCount, lookupList);
						SubModels[i] = tmpSubModel;
					}
					
					SubModel tmpDeformModel = new SubModel();
					tmpDeformModel.MatTexID = bStream.ReadInt32();
					tmpDeformModel.VertexCount = bStream.ReadInt32();
					int numVertsPresent = bStream.ReadInt32();
					if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen3)
					{
						ReadDeform_DeformSubModel(bStream, tmpDeformModel, tmpDeformModel.VertexCount, numVertsPresent, lookupList);
					}
					else
					{
						ReadDeform_DeformSubModel_Gen3(bStream, tmpDeformModel, tmpDeformModel.VertexCount, numVertsPresent, lookupList);
					}
					SubModels[SubModelCount - 1] = tmpDeformModel;
				}
				else if(ModelType  == CCS_MODEL_SHADOW)
				{
					for(int i = 0; i < SubModelCount; i++)
					{
						SubModel tmpSubModel = new SubModel();
						tmpSubModel.VertexCount = bStream.ReadInt32();
						var tVertCount = bStream.ReadInt32();
						if((tVertCount % 3) != 0)
						{
							Logger.LogWarning(string.Format("Shadow Model {0:X}:{1} has an bad number of triangle verts!", ObjectID, ParentFile.GetSubObjectName(ObjectID)));
							//return false;
						}
						tmpSubModel.TriangleCount = tVertCount / 3;
						ReadShadowModel(bStream, tmpSubModel, tmpSubModel.VertexCount, tmpSubModel.TriangleCount);
						SubModels[i] = tmpSubModel;
					}
				}
				else
				{
					Logger.LogError(string.Format("Model {0}: Unknown model type: 0x{1:X}\n", ParentFile.GetSubObjectName(ObjectID), ModelType));
					return false;
				}
			}
			
			return true;
		}
		
		public string GetModelTypeStr()
		{
			/*
			string modelTypeStr = "";
			if(ModelType == 0 && SubModelCount == 0)
			{
				modelTypeStr = "None";
			}
			else if(ModelType < CCS_MODEL_DEFORMABLE)
			{
				modelTypeStr = "Rigid";
			}
			else if((ModelType & CCS_MODEL_DEFORMABLE) == CCS_MODEL_DEFORMABLE)
			{
				modelTypeStr = "Deformable";
			}
			else if((ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2)
			{
				modelTypeStr = "Deformable (Gen2)";
			}
			else if((ModelType & CCS_MODEL_SHADOW) == CCS_MODEL_SHADOW)
			{
				modelTypeStr = "Shadow";
			}
			else if((ModelType & CCS_MODEL_MORPHTARGET) == CCS_MODEL_MORPHTARGET)
			{
				modelTypeStr = "Morph Target";
			}
			else if((ModelType & CCS_MODEL_RIGID_GEN2_NO_COLOR) == CCS_MODEL_RIGID_GEN2_NO_COLOR)
			{
				modelTypeStr = "Rigid(No Color) (Gen2)";
			}
			else if((ModelType & CCS_MODEL_RIGID_GEN2_COLOR) == CCS_MODEL_RIGID_GEN2_COLOR)
			{
				modelTypeStr = "Rigid(Color) (Gen2)";
			}
			else if((ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_MORPHTARGET_GEN2)
			{
				modelTypeStr = "Morph Target (Gen2)";
			}
			else
			{
				modelTypeStr = string.Format("Unknown Model Type: 0x{0:X}", ModelType);
			}
			
			return modelTypeStr;
			*/
			string modelTypeStr = "";
			if(ModelType == 0 && SubModelCount == 0)
			{
				modelTypeStr = "None";
			}
			else if(ModelType < CCS_MODEL_DEFORMABLE)
			{
				modelTypeStr = "Rigid";
			}
			else if(ModelType == CCS_MODEL_DEFORMABLE)
			{
				modelTypeStr = "Deformable";
			}
			else if(ModelType == CCS_MODEL_SHADOW)
			{
				modelTypeStr = "Shadow";
			}
			else if(ModelType == CCS_MODEL_MORPHTARGET)
			{
				modelTypeStr = "Morph Target";
			}
			else if(ModelType == CCS_MODEL_RIGID_GEN2_COLOR)
			{
				modelTypeStr = "Rigid (Gen2)";
			}
			else if(ModelType == CCS_MODEL_RIGID_GEN2_NO_COLOR)
			{
				modelTypeStr = "Rigid(No Color) (Gen2)";
			}
			else if(ModelType == CCS_MODEL_DEFORMABLE_GEN2)
			{
				modelTypeStr = "Deformable (Gen2)";
			}
			else if(ModelType == CCS_MODEL_MORPHTARGET_GEN2)
			{
				modelTypeStr = "Morph Target (Gen2)";
			}
			else
			{
				modelTypeStr = string.Format("Unknown Model Type: 0x{0:X}", ActualModelType);
			}
			return modelTypeStr;
		}
		
		public override TreeNode ToNode()
		{
			var retNode = base.ToNode();
			string modelTypeStr = string.Format(" Type: {0}", GetModelTypeStr());
			
			retNode.Text += modelTypeStr;
			for(int i = 0; i < SubModelCount; i++)
			{
				if((ModelType & CCS_MODEL_SHADOW) == CCS_MODEL_SHADOW)
				{
					TreeNode tmpSubNode = new TreeNode(string.Format("Sub Model {0}", i))
					{
						Tag = new StudioCCS.TreeNodeTag(ParentFile, ObjectID, ObjectType, TreeNodeTag.NodeType.SubNode, i)
					};
					retNode.Nodes.Add(tmpSubNode);
				}
				//else if((ModelType & CCS_MODEL_DEFORMABLE) == CCS_MODEL_DEFORMABLE || (ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2 )
				else if(ModelType == CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_DEFORMABLE_GEN2) 
				{
					SubModel tmpSubModel = SubModels[i];
					//TODO: Why is this failing?
					//string subModelName = ParentFile.GetSubObjectName(tmpSubModel.ParentObjectRef.ModelID);
					CCSObject tmpObject = ClumpRef.GetObject(tmpSubModel.ParentID);
					string subModelName = ParentFile.GetSubObjectName(ClumpRef.GetObject(tmpSubModel.ParentID).ObjectID);
					TreeNode tmpSubNode = new TreeNode(string.Format("Sub Model {0}: {1}", i, subModelName))
					{
						Tag = new StudioCCS.TreeNodeTag(ParentFile, ObjectID, ObjectType, TreeNodeTag.NodeType.SubNode, i)
					};
					
					retNode.Nodes.Add(tmpSubNode);
				}
				else
				{
					var tmpSubModel = SubModels[i];
					string subModelName = ParentFile.GetSubObjectName(tmpSubModel.ParentID);
					var tmpSubNode = new TreeNode(string.Format("Sub Model {0}: {1}", i, subModelName))
					{
						Tag = new StudioCCS.TreeNodeTag(ParentFile, ObjectID, ObjectType, TreeNodeTag.NodeType.SubNode, i)
					};
					
					retNode.Nodes.Add(tmpSubNode);
				}
			}
			return retNode;
		}
		
		public void Render(Matrix4 mtx, int extraOptions = 0, int single = -1)
		{
			if((ModelType & CCS_MODEL_SHADOW) == 0)
			{
				//Debug.WriteLine("Rendering Model {0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
				if(ProgramID == -1 || SubModelCount == 0) return;
				GL.UseProgram(ProgramID);
				
				if(single > -1)
				{
					if(single < SubModelCount) RenderSubModel(mtx, single, extraOptions);
				}
				else
				{
					for(int i = 0; i < SubModelCount; i++)
					{
						RenderSubModel(mtx, i, extraOptions);
					}
				}
				GL.UseProgram(0);
			}
			//else if((ModelType & CCS_MODEL_DEFORMABLE) != 0)
			//{
				
			//}
		}
		
		private void RenderSubModel(Matrix4 mtx, int subModelID,int extraOptions = 0)
		{
			var tmpSubModel = SubModels[subModelID];
			GL.BindVertexArray(tmpSubModel.VertexArrayID);
			
			Matrix4 finalMtx = mtx;// * Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
			
			Vector2 matTexOffset = Vector2.Zero;
			float matAlpha = 1.0f;
			GL.ActiveTexture(TextureUnit.Texture0);
			if(tmpSubModel.ParentMaterialRef != null)
			{
				CCSMaterial tmpMaterial = tmpSubModel.ParentMaterialRef;
				matTexOffset = tmpMaterial.TextureOffset;
				matAlpha = tmpMaterial.Alpha;
			}
			
			CCSTexture tmpTexture = tmpSubModel.ParentTextureRef;
			if(tmpTexture != null)
			{
				GL.BindTexture(TextureTarget.Texture2D, tmpTexture.TextureID);
				if(tmpTexture.TextureType == CCSTexture.CCS_TEXTURE_DXT1 || tmpTexture.TextureType == CCSTexture.CCS_TEXTURE_DXT5)
				{
					extraOptions |= Scene.SCENE_DRAW_FLIP_TEXCOORDS;
				}
			}
			else GL.BindTexture(TextureTarget.Texture2D, 0);
			
			
			//finalMtx = Matrix4.Identity * Matrix4.CreateTranslation(0.5f, 1.0f, 1.5f);
			GL.UniformMatrix4(UniformMatrix, false, ref finalMtx);
			GL.Uniform1(UniformAlpha, matAlpha);
			GL.Uniform2(UniformTextureOffset, ref matTexOffset);
			if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1)
			{
				GL.Uniform1(UniformDrawOptions, 0);
			}
			else
			{
				GL.Uniform1(UniformDrawOptions, DrawFlags);	
			}
			//if((DrawFlags & 8) != 0) GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
			//else GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			int fillMode = extraOptions & -2;
			if(fillMode != 0)
			{
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
				Vector4 SelectionColor = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
				
				if((fillMode & Scene.SCENE_DRAW_VERTEX_COLORS) == 0 && (fillMode & Scene.SCENE_DRAW_SMOOTH) == 0)
				{
					SelectionColor = new Vector4(0.5f, 0.5f, 0.5f, 0.0f);
				}
				
				GL.Uniform4(UniformSelectionColor, ref SelectionColor);
				
				//GL.DrawArrays(PrimitiveType.Triangles, 0, tmpSubModel.VertexCount);
				GL.Uniform1(UniformRenderMode, fillMode);
				GL.Uniform1(UniformMatrixList, 1);
				GL.Uniform1(UniformTexture, 0);
				GL.BindBuffer(BufferTarget.ElementArrayBuffer, tmpSubModel.ElementArrayID);
				GL.DrawElements(PrimitiveType.Triangles, tmpSubModel.TriangleCount * 3, DrawElementsType.UnsignedInt, 0);
			}
			
			if((extraOptions & Scene.SCENE_DRAW_LINES) != 0)
			{
				//var texturingEnabled = GL.GetBoolean(GetPName.Texture2D);
				//GL.Disable(EnableCap.Texture2D);
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
				
				if((extraOptions & Scene.SCENE_DRAW_SELECTION) != 0)
				{
					GL.Uniform4(UniformSelectionColor, 1.0f, 1.0f, 0.0f, 1.0f);
				}
				else
				{
					GL.Uniform4(UniformSelectionColor, 1.0f, 1.0f, 1.0f, 1.0f);	
				}
				
				int lineMode = 1;
				GL.Uniform1(UniformRenderMode, lineMode);
				
				GL.DrawElements(PrimitiveType.Triangles, tmpSubModel.TriangleCount * 3, DrawElementsType.UnsignedInt, 0);
			}
			
			
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			GL.DepthMask(true);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				
		}
		
		public void SetClump(CCSClump _parentClump, CCSObject _parentObject)
		{
			ClumpRef = _parentClump;
			ObjectRef = _parentObject;
			
			/*
			foreach(var s in SubModels)
			{
				if((ModelType & CCS_MODEL_DEFORMABLE) == 1)
				{
					s.ParentObjectRef = ClumpRef.GetObject(s.ParentID);
				}
				else
				{
					s.ParentObjectRef = ParentFile.GetObject<CCSObject>(s.ParentID);
				}
			}
			*/
			for(int i = 0; i < SubModelCount; i++)
			{
				/*
				var tmpSubModel = SubModels[i];
				if(tmpSubModel == null) continue;
				if((ModelType & CCS_MODEL_DEFORMABLE) == 1)
				{
					tmpSubModel.ParentObjectRef = ClumpRef.GetObject(tmpSubModel.ParentID);
				}
				else
				{
					tmpSubModel.ParentObjectRef = ParentFile.GetObject<CCSObject>(tmpSubModel.ParentID);
				}
				*/
				var tmpSubModel = SubModels[i];
				if(tmpSubModel == null) continue;
				if(ModelType == CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_DEFORMABLE_GEN2)
				{
					tmpSubModel.ParentObjectRef = ClumpRef.GetObject(tmpSubModel.ParentID);
				}
				else
				{
					tmpSubModel.ParentObjectRef = ParentFile.GetObject<CCSObject>(tmpSubModel.ParentID);
				}
			}
		}
		
		private bool ReadRigidSubModel(BinaryReader bStream, SubModel _subModel, int _vertexCount)
		{
			long subModelPosition = bStream.BaseStream.Position;
			//for Rigid Sub Models, we can just read everything sequentially.
			_subModel.VertexCount = _vertexCount;
			_subModel.Vertices = new ModelVertex[_vertexCount];
			
			//int boneID = ParentFile.LastClump.SearchNodeID(ParentFile.LastObject.ObjectID);
			int boneID = ParentFile.SearchClump(ParentFile.LastObject.ObjectID);
			if(boneID == -1)
			{
				Logger.LogError(string.Format("CCSModel::Read(): Model {0:X} at 0x{1:X}, Last Bone object ID is not part of a clump that have been read yet...\n", ObjectID, subModelPosition));
				boneID = 0;
			}
			
			for(int i = 0; i < _vertexCount; i++)
			{
				ModelVertex tmpVert = new ModelVertex()
				{
					Color = new Vector4(0.5f, 0.5f, 0.5f, 1.0f)
				};
				
				tmpVert.Position = Util.ReadVec3Half(bStream, VertexScale);
				//if(ModelType >= CCSModel.CCS_MODEL_DEFORMABLE)
				//{
				//	tmpVert.Color = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
				//}
				
				//Rigid and MorphTarget models use the global object ID of their bone's object's.
				//whereas DEFORM type use their Clump's local index. We're just going to have to deal with that.
				

				tmpVert.BoneIDs = new BoneID(boneID, 0);
				tmpVert.Weights = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
				
				_subModel.Vertices[i] = tmpVert;
			}
			
			if((bStream.BaseStream.Position % 4) == 2) bStream.ReadUInt16();
			int triOffs = (int)bStream.BaseStream.Position;
			
			int tCount = 0;
			for(uint i = 0; i < _vertexCount; i++)
			{
				int triFlag = (bStream.ReadInt32() >> 24) & 0xff;
				if(triFlag == 0) tCount += 1;
			}
			
			//TODO: CCSModel::ReadRigidSubModel(): should we keep triangle strips?
			bStream.BaseStream.Seek(triOffs, SeekOrigin.Begin);
			//Debug.WriteLine("Reading Triangles for Model {0:X}, at {1:X}", ObjectID, triOffs);
			_subModel.TriangleCount = tCount;
			_subModel.Triangles = new ModelTriangle[tCount];
			
			tCount = 0;
			byte lastFlag = 1;
			int sCount = 0;
			for(int i = 0; i < _vertexCount; i++)
			{
				_subModel.Vertices[i].Normal = Util.ReadVec3Normal8(bStream);
				byte triFlag = bStream.ReadByte();
				//TODO: CCSModel: Gen1 CCS Files don't care about vertex winding order (everything is drawn double sided)
				// Can we derive proper order from normal direction? Probably not.
				if(triFlag == 0)
				{
					if((sCount % 2) == 0)
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i, i - 1, i - 2);	
					}
					else
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i - 2, i - 1, i);	
					}
					
					tCount += 1;
					sCount += 1;
					lastFlag = triFlag;
				}
				else
				{
					if(lastFlag == 0)
					{
						sCount = 0;
					}
				}
			}
			
			//Read vertex colors, if applicable (Rigid SubModel)
			//if(ModelType < CCS_MODEL_DEFORMABLE || (ModelType & CCS_MODEL_RIGID_GEN2_COLOR) == CCS_MODEL_RIGID_GEN2_COLOR || (ModelType & CCS_MODEL_MORPHTARGET_GEN2) == CCS_MODEL_MORPHTARGET_GEN2)
			if(ModelType < CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_RIGID_GEN2_COLOR || ModelType == CCS_MODEL_MORPHTARGET_GEN2 )
			{
				for(int i = 0; i < _vertexCount; i++)
				{
					_subModel.Vertices[i].Color = Util.ReadVec4RGBA32(bStream);
				}
			}
			
			//Read UV's if applicable (not a MorphTarget or Shadow), or a GEN2 Rigid Model
			//if((ModelType < CCS_MODEL_SHADOW) || (ModelType & CCS_MODEL_RIGID_GEN2_NO_COLOR) == CCS_MODEL_RIGID_GEN2_NO_COLOR || (ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2)
			//Fuck it, if it's not a Morph Target, since shadow models should be caught and redirected in parent function
			//if((ModelType & CCS_MODEL_MORPHTARGET) != CCS_MODEL_MORPHTARGET && (ModelType & CCS_MODEL_MORPHTARGET_GEN2) != CCS_MODEL_MORPHTARGET_GEN2)
			if(ModelType != CCS_MODEL_MORPHTARGET && ModelType != CCS_MODEL_MORPHTARGET_GEN2)
			{
				for(int i = 0; i < _vertexCount; i++)
				{
					if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3)
					{
						_subModel.Vertices[i].TexCoords = Util.ReadVec2UV_Gen3(bStream);
					}
					else
					{
						_subModel.Vertices[i].TexCoords = Util.ReadVec2UV(bStream);	
					}
				}
			}
			
			if(ModelType == 0 && ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3 && Unk2 != 0)
			{
				bStream.BaseStream.Seek(_vertexCount * 8, SeekOrigin.Current);
			}
			
			return true;
		}
		
		private bool ReadDeform_RigidSubModel(BinaryReader bStream, SubModel _subModel, int _vertexCount, int[] lookupTbl)
		{
			long subModelPosition = bStream.BaseStream.Position;
			
			_subModel.VertexCount = _vertexCount;
			_subModel.Vertices = new ModelVertex[_vertexCount];
			//For deform_rigid sub models, we can read everything sequentially. easy.
			for(int i = 0; i < _vertexCount; i++)
			{
				ModelVertex tmpVert = new ModelVertex()
				{
					Color = new Vector4(0.5f, 0.5f, 0.5f, 1.0f),
					Position = Util.ReadVec3Half(bStream, VertexScale)
				};
				
				//Gen1 DEFORM sub models use the local index in parent clump.
				/*
				if(lookupTbl == null)
				{
					tmpVert.BoneIDs = new BoneID(_subModel.ParentID, 0);	
				}
				//Gen2 use local index in parent clump, from lookup table.
				else
				{
					tmpVert.BoneIDs = new BoneID(lookupTbl[_subModel.ParentID], 0);
				}
				*/
				tmpVert.BoneIDs = new BoneID(_subModel.ParentID, 0);
				tmpVert.Weights = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
			
				_subModel.Vertices[i] = tmpVert;
			}
			
			if((bStream.BaseStream.Position % 4) == 2) bStream.ReadInt16();
			
			int triOffs = (int)bStream.BaseStream.Position;
			//Count the triangles first.
			int tCount = 0;
			for(uint i = 0; i < _vertexCount; i++)
			{
				int triFlag = (bStream.ReadInt32() >> 24) & 0xFF;
				if(triFlag == 0) tCount += 1;
			}
			
			bStream.BaseStream.Seek(triOffs, SeekOrigin.Begin);
			_subModel.TriangleCount = tCount;
			_subModel.Triangles = new ModelTriangle[tCount];
			tCount = 0;
			byte lastFlag = 1;
			int sCount = 0;
			for(int i = 0; i < _vertexCount; i++)
			{
				_subModel.Vertices[i].Normal = Util.ReadVec3Normal8(bStream);
				byte triFlag = bStream.ReadByte();
				if(triFlag == 0)
				{
					if((sCount % 2) == 0)
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i, i - 1, i - 2);	
					}
					else
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i - 2, i - 1, i);	
					}
					
					tCount += 1;
					sCount += 1;
					lastFlag = triFlag;
				}
				else
				{
					if(lastFlag == 0)
					{
						sCount = 0;
					}
				}
			}
			
			//Deform sub models don't have colors
			
			for(int i = 0; i < _vertexCount; i++)
			{
				if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3)
				{
					_subModel.Vertices[i].TexCoords = Util.ReadVec2UV_Gen3(bStream);
				}
				else
				{
					_subModel.Vertices[i].TexCoords = Util.ReadVec2UV(bStream);	
				}
			}
			 
			
			return true;
		}
		
		private bool ReadDeform_DeformSubModel(BinaryReader bStream, SubModel _subModel, int _vertexCount, int _weightedCount, int[] lookupTbl)
		{
			//Easiest method for reading these is a jump-around approach like the games use.
			//Unfortunately, this method is not nearly efficient enough when reading out of files
			//as opposed to a ring buffer, or some other in-memory buffer, but oh well. 
			int smDelta = (int)bStream.BaseStream.Position;
			int vBaseOffset = smDelta;
			int tBaseOffset = vBaseOffset + (_weightedCount * 8);
			int uBaseOffset = tBaseOffset + (_weightedCount * 4);
			
			_subModel.Vertices = new ModelVertex[_vertexCount];
			_subModel.Triangles = new ModelTriangle[_vertexCount];
			
			int vCount = 0;
			int tCount = 0;
			byte lastFlag = 1;
			int sCount = 0;
			for(int i = 0; i < _vertexCount; i++)
			{
				int vOffs = vBaseOffset + (vCount * 8);
				int tOffs = tBaseOffset + (vCount * 4);
				int uOffs = uBaseOffset + (i * 4);
				
				//Read Vertex Position
				bStream.BaseStream.Seek(vOffs, SeekOrigin.Begin);
				ModelVertex tmpVert = new ModelVertex();
				tmpVert.Position = Util.ReadVec3Half(bStream, VertexScale);
				ushort vertParams = bStream.ReadUInt16();
				
				int boneID1 = vertParams >> 10;
				int boneID2 = 0;
				float weight1 = (vertParams & 0x1ff) * Util.WEIGHT_SCALE;
				//tmpVert.Position *= weight1;
				float weight2 = 0;
				bool dualFlag = ((vertParams >> 9) & 0x1) == 0;
				
				if(dualFlag)
				{
					//tmpVert.Position = -tmpVert.Position;
					vCount += 1;
					//Skip this vertex position
					//tmpVert.Position = -Util.ReadVec3Half(bStream, VertexScale);
					tmpVert.Position2 = Util.ReadVec3Half(bStream, VertexScale);
					ushort secondParams = bStream.ReadUInt16();
					weight2 = (secondParams & 0x1ff) * Util.WEIGHT_SCALE;
					boneID2 = (secondParams >> 10);
					//tmpVert.Position *= weight2;
				}
				if(lookupTbl != null)
				{
					tmpVert.BoneIDs = new BoneID(lookupTbl[boneID1], lookupTbl[boneID2]);
				}
				else
				{
					tmpVert.BoneIDs = new BoneID(boneID1, boneID2);
				}
				tmpVert.Weights = new Vector4(weight1, weight2, 0.0f, 0.0f);
				
				//Read Vertex Normals, and Triangle Info
				bStream.BaseStream.Seek(tOffs, SeekOrigin.Begin);
				tmpVert.Normal = Util.ReadVec3Normal8(bStream);
				
				byte triFlag = bStream.ReadByte();
				if(triFlag == 0)
				{
					if((sCount % 2) == 0)
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i, i - 1, i - 2);	
					}
					else
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i - 2, i - 1, i);	
					}
					
					tCount += 1;
					sCount += 1;
					lastFlag = triFlag;
				}
				else
				{
					if(lastFlag == 0)
					{
						sCount = 0;
					}
				}
				
				//Read UV Coordinates
				bStream.BaseStream.Seek(uOffs, SeekOrigin.Begin);
				if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3)
				{
					tmpVert.TexCoords = Util.ReadVec2UV_Gen3(bStream);
				}
				else
				{
					tmpVert.TexCoords = Util.ReadVec2UV(bStream);	
				}
				
				
				//Set default vertex color
				tmpVert.Color = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
				_subModel.Vertices[i] = tmpVert;
				vCount += 1;
			}
			
			//Shrink the Triangle Array down.
			_subModel.TriangleCount = tCount;
			ModelTriangle[] subModelTris = _subModel.Triangles;
			_subModel.Triangles = new ModelTriangle[tCount];
			Array.Copy(subModelTris, _subModel.Triangles, tCount);
			return true;
		}

		private bool ReadDeform_DeformSubModel_Gen3(BinaryReader bStream, SubModel _subModel, int _vertexCount, int _weightedCount, int[] lookupTbl)
		{
			//Easiest method for reading these is a jump-around approach like the games use.
			//Unfortunately, this method is not nearly efficient enough when reading out of files
			//as opposed to a ring buffer, or some other in-memory buffer, but oh well. 
			int smDelta = (int)bStream.BaseStream.Position;
			int vBaseOffset = smDelta;
			int tBaseOffset = vBaseOffset + (_weightedCount * 12);
			int uBaseOffset = tBaseOffset + (_weightedCount * 4);
			
			_subModel.Vertices = new ModelVertex[_vertexCount];
			_subModel.Triangles = new ModelTriangle[_vertexCount];
			
			int vCount = 0;
			int tCount = 0;
			byte lastFlag = 1;
			int sCount = 0;
			for(int i = 0; i < _vertexCount; i++)
			{
				int vOffs = vBaseOffset + (vCount * 12);
				int tOffs = tBaseOffset + (vCount * 4);
				int uOffs = uBaseOffset + (i * 8);
				
				//Read Vertex Position
				bStream.BaseStream.Seek(vOffs, SeekOrigin.Begin);
				ModelVertex tmpVert = new ModelVertex();
				
				Vector3[] vertexPositions = {Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero};
				int[] BoneIDs = {0, 0, 0, 0};
				float[] Weights = {0.0f, 0.0f, 0.0f, 0.0f};
				
				
				for(int w = 0; w < 4; w++)
				{
					vertexPositions[w] = Util.ReadVec3Half(bStream, VertexScale);
					Weights[w] = bStream.ReadInt16() * (1.0f / 256.0f);
					int stopBit = bStream.ReadInt16();
					BoneIDs[w] = bStream.ReadInt16();
					
					if(stopBit != 0)
					{
						vCount += w;
						break;
					}
				}
				
				/*
				tmpVert.Position = Util.ReadVec3Half(bStream, VertexScale);
				ushort vertParams = bStream.ReadUInt16();
				int boneID1 = vertParams >> 10;
				int boneID2 = 0;
				float weight1 = (vertParams & 0x1ff) * Util.WEIGHT_SCALE;
				//tmpVert.Position *= weight1;
				float weight2 = 0;
				bool dualFlag = ((vertParams >> 9) & 0x1) == 0;
				
				if(dualFlag)
				{
					//tmpVert.Position = -tmpVert.Position;
					vCount += 1;
					//Skip this vertex position
					//tmpVert.Position = -Util.ReadVec3Half(bStream, VertexScale);
					Util.ReadVec3Half(bStream, VertexScale);
					ushort secondParams = bStream.ReadUInt16();
					weight2 = (secondParams & 0x1ff) * Util.WEIGHT_SCALE;
					boneID2 = (secondParams >> 10);
					tmpVert.Position *= weight2;
				}
				*/
				
				
				if(lookupTbl != null)
				{
					//tmpVert.BoneIDs = new BoneID(lookupTbl[boneID1], lookupTbl[boneID2]);
					tmpVert.BoneIDs = new BoneID(lookupTbl[BoneIDs[0]], lookupTbl[BoneIDs[1]], lookupTbl[BoneIDs[2]], lookupTbl[BoneIDs[3]]);
				}
				else
				{
					//tmpVert.BoneIDs = new BoneID(boneID1, boneID2);
					tmpVert.BoneIDs = new BoneID(BoneIDs[0], BoneIDs[1], BoneIDs[2], BoneIDs[3]);
				}
				
				tmpVert.Weights = new Vector4(Weights[0], Weights[1], Weights[2], Weights[3]);
				tmpVert.Position = vertexPositions[0];
				tmpVert.Position2 = vertexPositions[1];
				tmpVert.Position3 = vertexPositions[2];
				tmpVert.Position4 = vertexPositions[3];
				//Read Vertex Normals, and Triangle Info
				bStream.BaseStream.Seek(tOffs, SeekOrigin.Begin);
				tmpVert.Normal = Util.ReadVec3Normal8(bStream);
				
				byte triFlag = bStream.ReadByte();
				if(triFlag == 0)
				{
					if((sCount % 2) == 0)
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i, i - 1, i - 2);	
					}
					else
					{
						_subModel.Triangles[tCount] = new ModelTriangle(i - 2, i - 1, i);	
					}
					
					tCount += 1;
					sCount += 1;
					lastFlag = triFlag;
				}
				else
				{
					if(lastFlag == 0)
					{
						sCount = 0;
					}
				}
				
				//Read UV Coordinates
				bStream.BaseStream.Seek(uOffs, SeekOrigin.Begin);
				tmpVert.TexCoords = Util.ReadVec2UV_Gen3(bStream);
				
				
				
				//Set default vertex color
				tmpVert.Color = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
				_subModel.Vertices[i] = tmpVert;
				vCount += 1;
			}
			
			//Shrink the Triangle Array down.
			_subModel.TriangleCount = tCount;
			ModelTriangle[] subModelTris = _subModel.Triangles;
			_subModel.Triangles = new ModelTriangle[tCount];
			Array.Copy(subModelTris, _subModel.Triangles, tCount);
			return true;
		}

		
		private bool ReadShadowModel(BinaryReader bStream, SubModel _subModel, int _vertexCount, int _triangleCount)
		{
			
			_subModel.Vertices = new ModelVertex[_vertexCount];
			_subModel.Triangles = new ModelTriangle[_triangleCount];
			for(int i = 0; i < _vertexCount; i++)
			{
				ModelVertex tmpVertex = new ModelVertex();
				tmpVertex.Position = Util.ReadVec3Half(bStream, VertexScale);
				_subModel.Vertices[i] = tmpVertex;
			}
			if((bStream.BaseStream.Position % 4) == 2) bStream.ReadInt16();
			
			for(int i = 0; i < _triangleCount; i++)
			{
				ModelTriangle tmpTri = new ModelTriangle()
				{
					ID1 = bStream.ReadInt32(),
					ID2 = bStream.ReadInt32(),
					ID3 = bStream.ReadInt32()
				};
				_subModel.Triangles[i] = tmpTri;
			}
			return true;
		}
		
		public string GetReport(int level = 0)
		{
			string retVal = Util.Indent(level) + string.Format("Model 0x{0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			retVal += Util.Indent(level + 1) + string.Format("Type: 0x{0:X}: {1}, {2} Vertices, {3} Triangles, {4} Sub Models\n", ActualModelType, GetModelTypeStr(), VertexCount, TriangleCount, SubModelCount);
			
			for(int i = 0; i < SubModelCount; i++)
			{
				var tmpSubModel = SubModels[i];
				
				string modelNameStr = "";
				if((ModelType & CCS_MODEL_SHADOW) == CCS_MODEL_SHADOW)
				{
					modelNameStr = string.Format("Shadow Sub Model {0}", i);
				}
				else if((ModelType & CCS_MODEL_DEFORMABLE) == CCS_MODEL_DEFORMABLE || (ModelType & CCS_MODEL_DEFORMABLE_GEN2) == CCS_MODEL_DEFORMABLE_GEN2)
				{
					CCSObject tmpObject = ClumpRef.GetObject(tmpSubModel.ParentID);
					modelNameStr = ParentFile.GetSubObjectName(ClumpRef.GetObject(tmpSubModel.ParentID).ObjectID);
				}
				else
				{
					modelNameStr = ParentFile.GetSubObjectName(tmpSubModel.ParentID);
				}
				
				retVal += Util.Indent(level + 1) + string.Format("Sub Model {0}: {1}\n", i, modelNameStr);
				retVal += Util.Indent(level + 2) + string.Format("{0} Vertices, {1} Triangles\n", tmpSubModel.VertexCount, tmpSubModel.TriangleCount);
				int texMatType = ParentFile.GetSubObjectType(tmpSubModel.MatTexID);
				string texMatDescString = Util.Indent(level + 2) + "No Texture or Matrial applied.\n";
				if(texMatType == CCSFile.SECTION_TEXTURE)
				{
					texMatDescString = Util.Indent(level + 2) + string.Format("Texture: 0x{0}: {1}\n", tmpSubModel.MatTexID, ParentFile.GetSubObjectName(tmpSubModel.MatTexID));
				}
				else if(texMatType == CCSFile.SECTION_MATERIAL)
				{
					texMatDescString = Util.Indent(level + 2) + string.Format("Material: 0x{0}: {1}\n", tmpSubModel.MatTexID, ParentFile.GetSubObjectName(tmpSubModel.MatTexID));
				}
				retVal += texMatDescString;
			}
			
			
			return retVal;
		}
		
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split, bool withNormals = false)
		{
			int totalVertCount = vOffset;
			if(SubModelCount > 0 && ModelType != CCS_MODEL_SHADOW)
			{
				//if(!split)
				//{
					//If we're not splitting, we'll just use the main Model's name...
					fStream.WriteLine(string.Format("o {0}", ParentFile.GetSubObjectName(ObjectID)));
				//}
				
				for(int i = 0; i < SubModelCount; i++)
				{
					var tmpSubModel = SubModels[i];
					if(tmpSubModel.VertexCount > 0)
					{
						Matrix4 finalMtx = Matrix4.Identity;
						/*
						if(tmpSubModel.ParentObjectRef != null)
						{
							finalMtx = tmpSubModel.ParentObjectRef.GetFinalMatrix();
						}
						else
						{
							finalMtx = ObjectRef.GetFinalMatrix();
						}
						*/
						
						Matrix4 firstMatrix = Matrix4.Identity;
						Matrix4 secondMatrix = Matrix4.Identity;
						
						string tmpSubModelName = "";
						if(ModelType == CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_DEFORMABLE_GEN2)
						{
							tmpSubModelName = ParentFile.GetSubObjectName(ClumpRef.GetObject(tmpSubModel.ParentID).ObjectID);
						}
						else
						{
							tmpSubModelName = ParentFile.GetSubObjectName(tmpSubModel.ParentID);
						}
						
						//Write descriptor comment
						fStream.WriteLine(string.Format("# {0}, {1} Vertices, {2} triangles", tmpSubModelName, tmpSubModel.VertexCount, tmpSubModel.TriangleCount));
						
						if(split)
						{
							//Write Group line
							fStream.WriteLine(string.Format("g {0}", tmpSubModelName));
						}
						//write material line
						if(tmpSubModel.ParentTextureRef != null)
						{
							fStream.WriteLine(string.Format("usemtl {0}", ParentFile.GetSubObjectName(tmpSubModel.ParentTextureRef.ObjectID)));
						}
						/*
						if(ModelType == CCS_MODEL_DEFORMABLE || ModelType == CCS_MODEL_DEFORMABLE_GEN2)
						{
							
						}
						else
						{
						*/
							//Write Vertices
							for(int v = 0; v < tmpSubModel.VertexCount; v++)
							{
								ModelVertex tmpVert = tmpSubModel.Vertices[v];
								//Vector3 vPos = Vector3.TransformPosition(tmpVert.Position, finalMtx);
								
								/*
								firstMatrix = ClumpRef.GetFinalMatrix(tmpVert.BoneIDs.Bone1);
								secondMatrix = ClumpRef.GetFinalMatrix(tmpVert.BoneIDs.Bone2);
								
								Vector3 vPos = Util.SkinVertex(tmpVert.Position, firstMatrix, secondMatrix, tmpVert.Weights.X, tmpVert.Weights.Y);
								*/
								
								Vector3 vPos = SoftSkinVertex(tmpVert);
								//write Position
								fStream.WriteLine(string.Format("v\t{0}\t{1}\t{2}", -vPos.X, vPos.Y, -vPos.Z));
								//write texture coordinates
								//TODO: CCSModel::DumpToObj() Export texture coordinates with material's texture coordinate offset.
								CCSTexture tmpTexture = tmpSubModel.ParentTextureRef;
								if(tmpTexture != null)
								{
									if(tmpTexture.TextureType == CCSTexture.CCS_TEXTURE_DXT1 || tmpTexture.TextureType == CCSTexture.CCS_TEXTURE_DXT5)
									{
										fStream.WriteLine(string.Format("vt\t{0}\t{1}", tmpVert.TexCoords.X, tmpVert.TexCoords.Y));
									}
									else
									{
										fStream.WriteLine(string.Format("vt\t{0}\t{1}", tmpVert.TexCoords.X, 1.0 - tmpVert.TexCoords.Y));
									}
								}
								else
								{
									fStream.WriteLine(string.Format("vt\t{0}\t{1}", tmpVert.TexCoords.X, 1.0 - tmpVert.TexCoords.Y));
								}
								
								//write normal
								fStream.WriteLine(string.Format("vn\t{0}\t{1}\t{2}", tmpVert.Normal.X, tmpVert.Normal.Y, tmpVert.Normal.Z));
							}
						/*	
						}
						*/
						//bool writeNormals = true;
						//Write triangles
						for(int t = 0; t < tmpSubModel.TriangleCount; t++)
						{
							ModelTriangle tmpTri = tmpSubModel.Triangles[t];
							//write triangle line
							if(withNormals)
							{
								fStream.WriteLine(string.Format("f {0}/{0}/{0}\t{1}/{1}/{1}\t{2}/{2}/{2}", tmpTri.ID1 + totalVertCount, tmpTri.ID2 + totalVertCount, tmpTri.ID3 + totalVertCount));
							}
							else
							{
								fStream.WriteLine(string.Format("f {0}/{0}\t{1}/{1}\t{2}/{2}", tmpTri.ID1 + totalVertCount, tmpTri.ID2 + totalVertCount, tmpTri.ID3 + totalVertCount));
							}
						}
						
						totalVertCount += tmpSubModel.VertexCount;
					}
				}
			}
			
			return totalVertCount;
		}
		
		public void DumpToSMD(StreamWriter outf, bool withNormals)
		{
			if(SubModelCount == 0) return;
			foreach(var tmpSm in SubModels)
			{
				string texName = "None";
				if(tmpSm.ParentTextureRef != null) texName = ParentFile.GetSubObjectName(tmpSm.ParentTextureRef.ObjectID);
				
				for(int t = 0; t < tmpSm.TriangleCount; t++)
				{
					ModelTriangle tmpT = tmpSm.Triangles[t];
					outf.WriteLine(texName);
					int[] vIDs = {tmpT.ID1, tmpT.ID2, tmpT.ID3};
					for(int v = 0; v < 3; v++)
					{
						ModelVertex tmpV = tmpSm.Vertices[vIDs[v]];
						int pbID = tmpSm.ParentID;
						//Vector3 tmpVp = (tmpV.Position * tmpV.Weights.X) + (tmpV.Position2 * tmpV.Weights.Y) + (tmpV.Position3 * tmpV.Weights.Z) + (tmpV.Position4 * tmpV.Weights.W);
						Vector3 tmpVp = SoftSkinVertex(tmpV);
						string posStr = string.Format("{0} {1} {2}", tmpVp.X, tmpVp.Y, tmpVp.Z);
						//string posStr = string.Format("{0} {1} {2}", tmpV.Position.X, tmpV.Position.Y, tmpV.Position.Z);
						string normalStr = "0.0 0.0 0.0";
						if(withNormals) normalStr = string.Format("{0} {1} {2}", tmpV.Normal.X, tmpV.Normal.Y, tmpV.Normal.Z);
						string uvStr = string.Format("{0} {1}", tmpV.TexCoords.X, tmpV.TexCoords.Y);
						
						
						int[] tmpB = {tmpV.BoneIDs.Bone1, tmpV.BoneIDs.Bone2, tmpV.BoneIDs.Bone3, tmpV.BoneIDs.Bone4};
						float[] tmpW = {tmpV.Weights.X, tmpV.Weights.Y, tmpV.Weights.Z, tmpV.Weights.W};
						string weightStr = "";
						int wCount = 0;
						for(int w = 0; w < 4; w++)
						{
							if(tmpW[w] != 0.0f)
							{
								wCount += 1;
								weightStr += string.Format(" {0} {1}", tmpB[w], tmpW[w]);
							}
						}
						weightStr = wCount.ToString() + weightStr;
						outf.WriteLine(string.Format("{0} {1} {2} {3} {4}", pbID, posStr, normalStr, uvStr, weightStr));
						
					}
				}
			}
		}
		
		public Vector3 SoftSkinVertex(ModelVertex v)
		{
			Matrix4 m1 = ClumpRef.GetFinalMatrix(v.BoneIDs.Bone1);
			Matrix4 m2 = ClumpRef.GetFinalMatrix(v.BoneIDs.Bone2);
			Matrix4 m3 = ClumpRef.GetFinalMatrix(v.BoneIDs.Bone3);
			Matrix4 m4 = ClumpRef.GetFinalMatrix(v.BoneIDs.Bone4);
			
			Vector4 v1 = new Vector4(v.Position.X, v.Position.Y, v.Position.Z, 1.0f) * m1;
			Vector4 v2 = new Vector4(v.Position2.X, v.Position2.Y, v.Position2.Z, 1.0f) * m2;
			Vector4 v3 = new Vector4(v.Position3.X, v.Position3.Y, v.Position3.Z, 1.0f) * m3;
			Vector4 v4 = new Vector4(v.Position4.X, v.Position4.Y, v.Position4.Z, 1.0f) * m4;
			
			Vector3 final = new Vector3(v1.X * v.Weights.X, v1.Y * v.Weights.X, v1.Z * v.Weights.X);
			final += new 		Vector3(v2.X * v.Weights.Y, v2.Y * v.Weights.Y, v2.Z * v.Weights.Y);
			final += new 		Vector3(v3.X * v.Weights.Z, v3.Y * v.Weights.Z, v3.Z * v.Weights.Z);
			final += new 		Vector3(v4.X * v.Weights.W, v4.Y * v.Weights.W, v4.Z * v.Weights.W);
			return final;
		}
		
	}
}
