/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 9/20/2017
 * Time: 2:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace StudioCCS
{
	/// <summary>
	/// Description of TexturePreview.
	/// </summary>
	public static class TexturePreview
	{
		public static bool WasInit = false;
		
		[StructLayout(LayoutKind.Sequential)]
		public struct TextureVertex
		{
			public Vector3 Position;
			public Vector2 TexCoord;
		}
		
		public static int VertexCount = 4;
		public static int ProgramID = -1;
		public static int AttribPosition = -1;
		public static int AttribTexCoord = -1;
		public static int UniformMatrix = -1;
		public static int UniformTexture = -1;
		public static int UniformTextureSize = -1;
		
		public static float Scale = 0.01f;
		public static int ArrayID = -1;
		public static int BufferID = -1;
		
		public static TextureVertex[] Vertices = null;
		
		public static bool Init()
		{
			if(WasInit) return true;
			
			Vertices = new TextureVertex[4];
			//Bottom Left
			Vertices[0] = new TextureVertex()
			{
				Position = new Vector3(-0.5f, -0.5f, 0.0f),
				TexCoord = new Vector2(0.0f, 0.0f)
			};
			
			//Top Left
			Vertices[1] = new TextureVertex()
			{
				Position = new Vector3(-0.5f, 0.5f, 0.0f),
				TexCoord = new Vector2(0.0f, 1.0f)
			};
			
			//Bottom Right
			Vertices[2] = new TextureVertex()
			{
				Position = new Vector3(0.5f, -0.5f, 0.0f),
				TexCoord = new Vector2(1.0f, 0.0f)
			};	
			
			//Top Right
			Vertices[3] = new TextureVertex()
			{
				Position = new Vector3(0.5f, 0.5f, 0.0f),
				TexCoord = new Vector2(1.0f, 1.0f)
			};
			
		
			
			ProgramID = Scene.LoadProgram("TexturePreview");
			if(ProgramID == -1) 
			{
				Logger.LogError("Error loading Texture Preview Shader Program");
				return false;
			}
			
			AttribPosition = GL.GetAttribLocation(ProgramID, "vPosition");
			AttribTexCoord = GL.GetAttribLocation(ProgramID, "vTexCoord");
			UniformMatrix = GL.GetUniformLocation(ProgramID, "mMatrix");
			UniformTexture = GL.GetUniformLocation(ProgramID, "fTexture");
			UniformTextureSize = GL.GetUniformLocation(ProgramID, "mSize");
			
			if(AttribPosition == -1 || AttribTexCoord == -1)
			{
				Logger.LogError("Error getting Texture Preview Shader Attributes:");
				Logger.LogError(string.Format("\tPosition: {0}, TexCoord: {1}", AttribPosition, AttribTexCoord));
				return false;
			}
			
			if(UniformMatrix == -1 || UniformTexture == -1 || UniformTextureSize == -1)
			{
				Logger.LogError("Error getting Texture Preview Shader Uniform Locations:");
				Logger.LogError(string.Format("\tMatrix: {0}, Texture: {1}, Size: {2}", UniformMatrix, UniformTexture, UniformTextureSize));
				return false;
			}
			
			ArrayID = GL.GenVertexArray();
			GL.BindVertexArray(ArrayID);
			
			BufferID = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
			Type vType = Vertices[0].GetType();
			int vSize = Marshal.SizeOf(vType);
			
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vSize * VertexCount), Vertices, BufferUsageHint.StaticDraw);
			
			GL.EnableVertexAttribArray(AttribPosition);
			GL.VertexAttribPointer(AttribPosition, 3, VertexAttribPointerType.Float, false, vSize, Marshal.OffsetOf(vType, "Position"));
			
			GL.EnableVertexAttribArray(AttribTexCoord);
			GL.VertexAttribPointer(AttribTexCoord, 2, VertexAttribPointerType.Float, false, vSize, Marshal.OffsetOf(vType, "TexCoord"));
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			
			WasInit = true;
			return WasInit;
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
		
		public static void Render(Matrix4 matrix, int textureID, float textureWidth, float textureHeight)
		{
			if(!WasInit || ProgramID == -1) return;
			PolygonMode currentMode = (PolygonMode)GL.GetInteger(GetPName.PolygonMode);
			
			GL.UseProgram(ProgramID);
			
			GL.BindVertexArray(ArrayID);
			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			GL.Uniform1(UniformTexture, 1);
			GL.Uniform3(UniformTextureSize, textureWidth, textureHeight, 0.1f);
			Matrix4 id = Matrix4.Identity; //* matrix;
			GL.UniformMatrix4(UniformMatrix, false, ref matrix);
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, textureID);
			
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
			
			GL.BindVertexArray(0);
			GL.UseProgram(0);
			GL.PolygonMode(MaterialFace.FrontAndBack, currentMode);
			
		}
	}
}
