/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 1:52 AM
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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSHitMesh.
	/// </summary>
	public class CCSHitMesh : CCSBaseObject
	{
		public class HitGroup
		{
			public Vector3[] Vertices = null;
			
			public int VertexCount = 0;
			public Vector4 Color = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
			public Vector3 Minimum = new Vector3(0.0f);
			public Vector3 Maximum = new Vector3(0.0f);
		
			public int VertexArrayID = -1;
			public int VertexBufferID = -1;
		}
		
		public HitGroup[] HitGroups = null;
		public int HitGroupCount = 0;
		public int UnkID = 0;
		public int VertexCount = 0;
		
		//OpenGL Stuff, so we can render the hit groups
		public static int ProgramID = -1;
		public static int ProgramRefs = 0;
		public static int AttribPosition = -1;
		public static int UniformMatrix = -1;
		public static int UniformColor = -1;
		public static int UniformDisplayColor = -1;
		public static int DerpArrayID = -1;
		
		public CCSHitMesh(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_HITMESH;
		}
		
		public override bool Init()
		{
			if(ProgramID == -1)
			{
				ProgramID = StudioCCS.Scene.LoadProgram("CCSHitMesh");
				if(ProgramID == -1) return false;
				
				AttribPosition = GL.GetAttribLocation(ProgramID, "vPosition");
				UniformMatrix = GL.GetUniformLocation(ProgramID, "mMatrix");
				UniformColor = GL.GetUniformLocation(ProgramID, "vColor");
				//UniformDisplayColor = GL.GetUniformLocation(ProgramID, "DisplayColor");
				
				if(AttribPosition == -1 || UniformMatrix == -1 || UniformColor == -1)
				{
					Logger.LogError("CCSHitMesh: Error Getting Shader Attributes/Uniforms:\n");
					Logger.LogError(string.Format("\tPosition: {0}, Matrix: {1}, Color: {2}\n", AttribPosition, UniformMatrix, UniformColor));
					return false;
				}
			}
			
			
			ProgramRefs += 1;
			
			for(int i = 0; i < HitGroupCount; i++)
			{
				HitGroup tmpGroup = HitGroups[i];
				if(tmpGroup.VertexCount > 0)
				{
					tmpGroup.VertexArrayID = GL.GenVertexArray();
					GL.BindVertexArray(tmpGroup.VertexArrayID);
					
					Type vertexType = tmpGroup.Vertices[0].GetType();
					int vertexSize = Marshal.SizeOf(vertexType);
					
					tmpGroup.VertexBufferID = GL.GenBuffer();
					GL.BindBuffer(BufferTarget.ArrayBuffer, tmpGroup.VertexBufferID);
					
					GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(tmpGroup.VertexCount * vertexSize), tmpGroup.Vertices, BufferUsageHint.StaticDraw);
					
					GL.EnableVertexAttribArray(AttribPosition);
					GL.VertexAttribPointer(AttribPosition, 3, VertexAttribPointerType.Float, false, vertexSize, IntPtr.Zero);
					
					GL.BindVertexArray(0);
					GL.BindBuffer(BufferTarget.ArrayBuffer, 0);				
				}
				else
				{
					Logger.LogWarning(string.Format("HitMesh {0}: HitGroup{1} has zero vertices.\n", ParentFile.GetSubObjectName(ObjectID), i));
				}
			}
			
			return true;
		}
		
		public void RenderAll(Matrix4 _matrix, int _selectedID = -1)
		{
			if(ProgramID == -1) return;
			Matrix4 FinalMtx = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f) * _matrix;
			GL.UseProgram(ProgramID);
			Console.WriteLine("Wtf 1");
			for(int i = 0; i < HitGroupCount; i++)
			{
				try
				{
					//Logger.LogInfo(string.Format("Has {0} of {1} hit groups...\n", i, HitGroupCount));
					var tmpGroup = HitGroups[i];
					Console.WriteLine("Wtf 1");
					if(tmpGroup == null) return;
					Console.WriteLine("Wtf 1");
					GL.BindVertexArray(tmpGroup.VertexArrayID);
					GL.UniformMatrix4(UniformMatrix, false, ref FinalMtx);
					/* We don't want to render lines twice if we're in line mode already, so check first
					*/
					var curMode = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
					if(curMode == PolygonMode.Fill)
					{
						GL.Uniform4(UniformColor, tmpGroup.Color);
						GL.DrawArrays(PrimitiveType.Triangles, 0, tmpGroup.VertexCount);
					}
					GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
					
					if(i == _selectedID) GL.Uniform4(UniformColor, 1.0f, 1.0f, 1.0f, 1.0f);
					else GL.Uniform4(UniformColor, 0.0f, 0.0f, 0.0f, 1.0f);
	
					
					GL.DrawArrays(PrimitiveType.Triangles, 0, tmpGroup.VertexCount);
					
					GL.PolygonMode(MaterialFace.FrontAndBack, curMode);
					
					GL.BindVertexArray(0);
				}
				catch (NullReferenceException e)
				{
					Logger.LogError(string.Format("Null Reference expetion caught when attempting render of Hitmesh {0} Submesh {1}...\n", ParentFile.GetSubObjectName(ObjectID), i), Logger.LogType.LogOnceValue);
				}
			}
			GL.UseProgram(0);
		}
		
		public void RenderOne(Matrix4 _matrix, int groupID)
		{
			if(ProgramID == -1) return;
			if(groupID < 0 || groupID > (HitGroupCount - 1)) return;
			
			var tmpGroup = HitGroups[groupID];
			GL.UseProgram(ProgramID);
			
			GL.BindVertexArray(tmpGroup.VertexArrayID);
			GL.UniformMatrix4(UniformMatrix, false, ref _matrix);
			GL.Uniform4(UniformColor, tmpGroup.Color);
			
			//Since we're rendering hitmeshes with wrireframes too, it'd be rather pointless to draw them twice
			//if we're in wireframe mode already...
			
 			var curMode = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
			if(curMode == PolygonMode.Fill)
			{
				GL.DrawArrays(PrimitiveType.Triangles, 0, tmpGroup.VertexCount);
			}
			
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			
			GL.Uniform4(UniformColor, 1.0f, 1.0f, 1.0f, 1.0f);
			
			GL.DrawArrays(PrimitiveType.Triangles, 0, tmpGroup.VertexCount);
			
			GL.PolygonMode(MaterialFace.FrontAndBack, curMode);
			
			GL.BindVertexArray(0);
			GL.UseProgram(0);
		}
		
		public override bool DeInit()
		{
			for(int i = 0; i < HitGroupCount; i++)
			{
				HitGroup tmpGroup = HitGroups[i];
				if(tmpGroup.VertexArrayID != -1) GL.DeleteVertexArray(tmpGroup.VertexArrayID);
				if(tmpGroup.VertexBufferID != -1) GL.DeleteBuffer(tmpGroup.VertexBufferID);
			}
			
			ProgramRefs -= 1;
			if(ProgramRefs <= 0)
			{
				if(ProgramID != -1)
				{
					GL.DeleteProgram(ProgramID);
					ProgramID = -1;
				}
			}
			
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			UnkID = bStream.ReadInt32();
			HitGroupCount = bStream.ReadInt32();
			HitGroups = new HitGroup[HitGroupCount];
			VertexCount = bStream.ReadInt32();
			
			for(int i = 0; i < HitGroupCount; i++)
			{
				HitGroup tmpGroup = new HitGroup();
				tmpGroup.VertexCount = bStream.ReadInt32() * 2;
				tmpGroup.Color = Util.ReadVec4RGBA32(bStream);
				//tmpGroup.Color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
				tmpGroup.Vertices = new Vector3[tmpGroup.VertexCount];
				
				for(int v = 0; v < tmpGroup.VertexCount; v++)
				{
					Vector3 tmpVert = Util.ReadVec3Position(bStream);
					tmpGroup.Vertices[v] = tmpVert;
					if(Util.Vector3LessThan(tmpVert, tmpGroup.Minimum))
					{
						tmpGroup.Minimum = tmpVert;
					}
					else if(Util.Vector3LessThan(tmpGroup.Maximum, tmpVert))
					{
						tmpGroup.Maximum = tmpVert;
					}
				}
				HitGroups[i] = tmpGroup;
				//using(var fs = new StringWriter(string.Format("Group_{0}", i), FileMode.OpenOrCreate))
				/*
				using(var fs = new FileStream(string.Format("Group_{0}", i), FileMode.OpenOrCreate))
				{
					using(var ss = new StreamWriter(fs))
					{
						for(int v = 0; v < tmpGroup.VertexCount; v++)
						{
							Vector3 tmpV = tmpGroup.Vertices[v];
							
							ss.WriteLine("{0}, {1}, {2}", tmpV.X, tmpV.Y, tmpV.Z);
						}
						
				    }
				}
				*/
			}
			return true;
		}
		
		public override TreeNode ToNode()
		{
			var retNode = base.ToNode();
			for(int i = 0; i < HitGroupCount; i++)
			{
				TreeNode tmpGroupNode = new TreeNode(string.Format("HitGroup {0}", i))
				{
					Tag = new TreeNodeTag(ParentFile, ObjectID, ObjectType, TreeNodeTag.NodeType.SubNode, i)
				};
				retNode.Nodes.Add(tmpGroupNode);
			}
			
			return retNode;
		}
		
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split = false)
		{
			int totalVertCount = vOffset;
			if(HitGroupCount > 0)
			{
				//Write Descriptor comment
				string hitName = ParentFile.GetSubObjectName(ObjectID);
				fStream.WriteLine(string.Format("# {0}, {1} Vertices, {2} triangles", hitName, VertexCount, VertexCount / 3));
				fStream.WriteLine(string.Format("o {0}", hitName));
				for(int i = 0; i < HitGroupCount; i++)
				{
					var tmpHitGroup = HitGroups[i];
					if(split)
					{
						//Write group line
						fStream.WriteLine(string.Format("g {0}_{1}", hitName, i));
					}
					
					for(int v = 0; v < tmpHitGroup.VertexCount; v++)
					{
						Vector3 vPos = tmpHitGroup.Vertices[v];
						fStream.WriteLine(string.Format("v\t{0}\t{1}\t{2}", vPos.X, vPos.Y, vPos.Z));
					}
					
					for(int v = 0; v < (tmpHitGroup.VertexCount / 3); v++)
					{
						int vDelta = v * 3;
						fStream.WriteLine(string.Format("f {0}\t{1}\t{2}", (vDelta) + totalVertCount, (vDelta + 1) + totalVertCount, (vDelta + 2) + totalVertCount));
					}
					
					totalVertCount += tmpHitGroup.VertexCount;
				}
			}
			
			return totalVertCount;
		}
	}
}
