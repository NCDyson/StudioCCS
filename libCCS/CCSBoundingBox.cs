/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 4:31 AM
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

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSBoundingBox.
	/// </summary>
	public class CCSBoundingBox : CCSBaseObject
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct BBox
		{
			public Vector3 Minimum;// = Vector3.Zero;
			public Vector3 Maximum; // = Vector3.Zero;
			public Vector4 Color; // = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
		}
		
		public int ModelID = 0;
		public BBox[] Box = new BBox[1];
		
		//OpenGL Stuff
		public static int ProgramID = -1;
		public static int ProgramRefs = 0;
		public static int AttribVMin = -1;
		public static int AttribVMax = -1;
		public static int AttribVColor = -1;
		public static int UniMatrix = -1;
		
		public int VertexArrayID = -1;
		public int VertexBufferID = -1;
		
		public CCSBoundingBox(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_BBOX;
		}
		
		public override bool Init()
		{
			if(ProgramID == -1)
			{
				ProgramID = Scene.LoadProgram("BBox", true);
				if(ProgramID == -1) return false;
				
				AttribVMin = GL.GetAttribLocation(ProgramID, "VMin");
				AttribVMax = GL.GetAttribLocation(ProgramID, "VMax");
				AttribVColor = GL.GetAttribLocation(ProgramID, "VColor");
				UniMatrix = GL.GetUniformLocation(ProgramID, "UMatrix");
				
				if(AttribVMin == -1 || AttribVMax == -1 || AttribVColor == -1 || UniMatrix == -1)
				{
					Logger.LogError("CCSBBox: Error Getting Shader Attributes/Uniforms:\n");
					Logger.LogError(string.Format("\tVMin: {0}, VMax: {1}, VColor: {2}, UMatrix: {3}", AttribVMin, AttribVMax, AttribVColor, UniMatrix));
					return false;
				}
			}
			
			ProgramRefs += 1;
			Box[0] = new BBox();
			VertexArrayID = GL.GenVertexArray();
			GL.BindVertexArray(VertexArrayID);
			
			VertexBufferID = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferID);
			
			Type VertexType = Box[0].GetType();
			int VertexSize = Marshal.SizeOf(VertexType);
			
			GL.BufferData(BufferTarget.ArrayBuffer, VertexSize, Box, BufferUsageHint.DynamicDraw);
			
			GL.EnableVertexAttribArray(AttribVMin);
			GL.VertexAttribPointer(AttribVMin, 3, VertexAttribPointerType.Float, false, VertexSize, Marshal.OffsetOf(VertexType, "Min"));
			
			GL.EnableVertexAttribArray(AttribVMax);
			GL.VertexAttribPointer(AttribVMax, 3, VertexAttribPointerType.Float, false, VertexSize, Marshal.OffsetOf(VertexType, "Max"));
			
			GL.EnableVertexAttribArray(AttribVColor);
			GL.VertexAttribPointer(AttribVColor, 4, VertexAttribPointerType.Float, false, VertexSize, Marshal.OffsetOf(VertexType, "Color"));
			
			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			
			return true;
		}
		
		public override bool DeInit()
		{
			if(VertexArrayID != -1) GL.DeleteVertexArray(VertexArrayID);
			if(VertexBufferID != -1) GL.DeleteBuffer(VertexBufferID);
			
			ProgramRefs -= 1;
			if(ProgramRefs <= 0)
			{
				if(ProgramID != -1) GL.DeleteProgram(ProgramID);
				ProgramID = -1;
			}
			
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			ModelID = bStream.ReadInt32();
			Box[0].Minimum = Util.ReadVec3Position(bStream);
			Box[0].Maximum = Util.ReadVec3Position(bStream);
			
			return true;
		}
		
		public override TreeNode ToNode()
		{
			return base.ToNode();
		}
	}
}
