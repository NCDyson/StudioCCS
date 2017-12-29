/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 8/1/2017
 * Time: 2:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing.Drawing2D;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace StudioCCS
{
	/// <summary>
	/// Description of LightHelper.
	/// </summary>
	public static class WireHelper
	{
		public static bool WasInit = false;
		public static Vector3[] Vertices = null;
		public static int VertexCount = 0;
		public static int ProgramID = -1;
		public static int AttribPosition = -1;
		public static int UniformMatrix = -1;
		public static int UniformColor = -1;
		public static int UniformScale = -1;
		
		public static int ArrayID = -1;
		public static int BufferID = -1;
		
		//Helper Types
		public static int DirectionalHelperOffset = 0;
		public static int DirectionalHelperCount = 0;
		public static int OmniHelperOffset = 0;
		public static int OmniHelperCount = 0;
		public static int OmniRingHelperOffset = 0;
		public static int OmniRingHelperCount = 0;
		public static int DummyHelperOffset = 0;
		public static int DummyHelperCount = 0;
		
		public static bool Init()
		{
			if(WasInit) return true;
			
			//Load Vertices
			ReadBin();
			
			ProgramID = Scene.LoadProgram("WireHelper");
			if(ProgramID == -1)
			{
				Logger.LogError("Error loading LightHelper shader program");
				return false;
			}
			
			AttribPosition = GL.GetAttribLocation(ProgramID, "Position");
			UniformMatrix = GL.GetUniformLocation(ProgramID, "Matrix");
			UniformColor = GL.GetUniformLocation(ProgramID, "Color");
			UniformScale = GL.GetUniformLocation(ProgramID, "Scale");

			if(AttribPosition == -1 || UniformMatrix == -1 || UniformColor == -1 || UniformScale == -1 )
			{
				Logger.LogError("Error getting LightHelper Shader Attribute/Uniform Locations:\n");
				Logger.LogError(string.Format("\tPosition: {0}, Matrix: {1}, Color: {2}, Scale: {3}\n", AttribPosition, UniformMatrix, UniformColor, UniformScale));
				return false;
			}
			
			ArrayID = GL.GenVertexArray();
			GL.BindVertexArray(ArrayID);
			
			BufferID = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
			
			Type vertexType = Vertices[0].GetType();
			int vertexSize = Marshal.SizeOf(vertexType);
			
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexCount * vertexSize), Vertices, BufferUsageHint.StaticDraw);
			
			GL.EnableVertexAttribArray(AttribPosition);
			GL.VertexAttribPointer(AttribPosition, 3, VertexAttribPointerType.Float, false, vertexSize, IntPtr.Zero);
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			
			WasInit = true;
			return true;
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
		
		public static void RenderDirectionalHelper(Matrix4 matrix)
		{
			if(!WasInit || ProgramID == -1) return;
			GL.UseProgram(ProgramID);
			
			GL.BindVertexArray(ArrayID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
			
			GL.Uniform1(UniformScale, 1.0f);
			GL.Uniform4(UniformColor, 1.0f, 1.0f, 0.0f, 1.0f);
			GL.UniformMatrix4(UniformMatrix, false, ref matrix);
			
			GL.DrawArrays(PrimitiveType.Lines, DirectionalHelperOffset, DirectionalHelperCount);
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.UseProgram(0);
			
		}
		
		public static void RenderOmniHelper(Matrix4 matrix, float scale)
		{
			if(!WasInit || ProgramID == -1) return;
			
			GL.UseProgram(ProgramID);
			
			//Draw Main helper
			GL.BindVertexArray(ArrayID);
			GL.Uniform1(UniformScale, 1.0f);
			GL.Uniform4(UniformColor, 1.0f, 1.0f, 0.0f, 1.0f);
			GL.UniformMatrix4(UniformMatrix, false, ref matrix);
			GL.DrawArrays(PrimitiveType.Lines, OmniHelperOffset, OmniHelperCount);
			
			//Draw size ring
			GL.Uniform1(UniformScale, scale);
			GL.DrawArrays(PrimitiveType.Lines, OmniRingHelperOffset, OmniRingHelperCount);
			
			GL.BindVertexArray(0);
			GL.UseProgram(0);
		}
		
		public static void RenderDummyHelper(Matrix4 ProjView, libCCS.CCSDummy Dummy)
		{
			if(!WasInit || ProgramID == -1) return;
			
			GL.UseProgram(ProgramID);
			
			GL.BindVertexArray(ArrayID);
			GL.Uniform1(UniformScale, 0.5f);
			GL.Uniform4(UniformColor, 0.0f, 1.0f, 0.0f, 1.0f);
			
			Matrix4 FinalMtx = Dummy.Matrix() * ProjView;
			GL.UniformMatrix4(UniformMatrix, false, ref FinalMtx);
			
			GL.DrawArrays(PrimitiveType.Lines, DummyHelperOffset, DummyHelperCount);
			
			GL.BindVertexArray(0);
			GL.UseProgram(0);
		}
		
		public static void ReadBin()
		{
			using(var fs = new FileStream("data/bin/WireHelpers.bin", FileMode.Open))
			{
				using(var bs = new BinaryReader(fs))
				{
					VertexCount = bs.ReadInt32();
					DirectionalHelperOffset = 0;
					DirectionalHelperCount = bs.ReadInt32();
					OmniHelperOffset = DirectionalHelperCount;
					OmniHelperCount = bs.ReadInt32();
					OmniRingHelperOffset = OmniHelperOffset + OmniHelperCount;
					OmniRingHelperCount = bs.ReadInt32();
					DummyHelperOffset = OmniRingHelperOffset + OmniRingHelperCount;
					DummyHelperCount = bs.ReadInt32();
					
					if(Vertices == null || Vertices.Length != VertexCount)
					{
						Vertices = new Vector3[VertexCount];
					}
					
					for(int i = 0; i < VertexCount; i++)
					{
						float x, y, z;
						x = bs.ReadSingle();
						y = bs.ReadSingle();
						z = bs.ReadSingle();
						Vertices[i] = new Vector3(x, y, z);
					}
				}
			}
		}
		
	}
}
