/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/30/2017
 * Time: 4:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace StudioCCS
{
	/// <summary>
	/// Description of AxisMarker.
	/// </summary>
	public static class AxisMarker
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct AxisVertex
		{
			public Vector3 Position;
			public uint Color;
		}
		
		private static AxisVertex[] Vertices = null;
		private static int VertexCount = 0;
		private static bool WasInit = false;
		
		private static int ArrayID = -1;
		private static int BufferID = -1;
		private static int ProgramID = -1;
		private static int AttribPosition = -1;
		private static int AttribColor = -1;
		private static int UniMatrix = -1;
		private static int UniScale = -1;
		
		
		public static bool Init()
		{
			if(WasInit) return true;
			//Load Vertices
			ReadBin();
			
			//Load Shader Program
			ProgramID = Scene.LoadProgram("AxisMarker");
			if(ProgramID == -1)
			{
				Logger.LogError("Error loading AxisMarker shader program");
				return false;
			}
			
			AttribPosition = GL.GetAttribLocation(ProgramID, "Position");
			AttribColor = GL.GetAttribLocation(ProgramID, "Color");
			UniMatrix = GL.GetUniformLocation(ProgramID, "Matrix");
			UniScale = GL.GetUniformLocation(ProgramID, "Scale");
			
			if(AttribPosition == -1 || AttribColor == -1 || UniMatrix == -1 || UniScale == -1)
			{
				Logger.LogError("Error getting AxisVertex Shader Attribute/Uniform Locations:\n");
				Logger.LogError(string.Format("\tPosition: {0}, Color: {1}, Matrix: {2}, Scale: {3}\n", AttribPosition, AttribColor, UniMatrix, UniScale));
				return false;
			}
			//Deb
			
			ArrayID = GL.GenVertexArray();
			GL.BindVertexArray(ArrayID);
			
			BufferID = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
			
			Type vertexType = Vertices[0].GetType();
			int vertexSize = Marshal.SizeOf(vertexType);
			
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexCount * vertexSize), Vertices, BufferUsageHint.StaticDraw);
			
			GL.EnableVertexAttribArray(AttribPosition);
			GL.VertexAttribPointer(AttribPosition, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf(vertexType, "Position"));
			
			GL.EnableVertexAttribArray(AttribColor);
			GL.VertexAttribPointer(AttribColor, 4, VertexAttribPointerType.UnsignedByte, true, vertexSize, Marshal.OffsetOf(vertexType, "Color"));
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			
			WasInit = true;
			return WasInit;
		}
		
		private static void ReadBin()
		{
			using(var fs = new FileStream("data/bin/AxisMarker.bin", FileMode.Open))
			{
				using(var bs = new BinaryReader(fs))
				{
					VertexCount = bs.ReadInt32();
					bs.ReadInt32();
					bs.ReadInt32();
					bs.ReadInt32();
					
					if(Vertices == null || Vertices.Length != VertexCount)
					{
					   Vertices = new AxisVertex[VertexCount];
					}
					
					for(int i = 0; i < VertexCount; i++)
					{
						float x, y, z;
						uint col;
						x = bs.ReadSingle();
						y = bs.ReadSingle();
						z = bs.ReadSingle();
						col = bs.ReadUInt32();

						Vertices[i] = new AxisVertex()
						{
							Position = new Vector3(x, y, z),
							Color = col
						};
					}
				}
			}
		}
		
		public static void Reload()
		{
			ReadBin();
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
			Type vertexType = Vertices[0].GetType();
			int vertexSize = Marshal.SizeOf(vertexType);
			
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexCount * vertexSize), Vertices, BufferUsageHint.StaticDraw); 
		}
		
		public static void DeInit()
		{
			if(ProgramID != -1) GL.DeleteProgram(ProgramID);
			ProgramID = -1;
			
			
			if(ArrayID != -1) GL.DeleteVertexArray(ArrayID);
			ArrayID = -1;
			if(BufferID != -1) GL.DeleteBuffer(BufferID);
			BufferID = -1;
		}
		
		public static void Render(Matrix4 matrix, float scale)
		{
			if(!WasInit || ProgramID == -1) return;
			
			var currentMode = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			//Matrix4 FinalMtx = matrix * Matrix4.Identity;
			
			GL.UseProgram(ProgramID);
			
			GL.BindVertexArray(ArrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
			
			GL.UniformMatrix4(UniMatrix, false, ref matrix);
			GL.Uniform1(UniScale, scale);
			
			//GL.EnableVertexAttribArray(AttribPosition);
			GL.DrawArrays(PrimitiveType.Triangles, 0, VertexCount);
			
			//GL.DisableVertexAttribArray(AttribPosition);
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.UseProgram(0);
			
			GL.PolygonMode(MaterialFace.FrontAndBack, currentMode);
		}
	}
}
