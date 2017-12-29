/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/21/2017
 * Time: 11:15 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of Util.
	/// </summary>
	public static class Util
	{
		//Helpful defines
		public const float UV_SCALE = 1.0f / 256.0f;
		public const float WEIGHT_SCALE = 1.0f / 256.0f;
		public const float COLOR_SCALE = 1.0f / 255.0f;
		public const float NORMAL_SCALE = 1.0f / 64.0f;
		//These numbers are finagled...
		public const float VTEX_SCALE = 0.0625f * 0.01f;
		public const float CCS_GLOBAL_SCALE = 0.0625f		* 0.1f;
		public const float NINETY_RADS = -90.0f * (float)Math.PI / 180.0f;
		
		
		//Utility Functions
		public static bool Vector3LessThan(Vector3 a, Vector3 b)
		{
			return (a.X < b.X) && (a.Y < b.Y) && (a.Z < b.Z);
		}
		
		public static Vector3 FixAxis(Vector3 input)
		{
			return new Vector3(input.X, input.Y, input.Z);
			//return new Vector3(input.Y, input.Z, input.X);
		}
		
		public static Vector3 FixAxisRotation(Vector3 input)
		{
			//return input;
			//Works pretty good:
			return new Vector3(input.Z, -input.Y, input.X);
			
			//return new Vector3(input.Z, input.Y, input.X);
		}
		
		public static Vector3 UnFixAxisRotation(Vector3 input)
		{
			return new Vector3(input.X, -input.Y, input.Z);
		}
		
		public static Vector4 FixAxisRotation4(Vector4 input)
		{
			//Quaternion derp = new Quaternion(input.X, input.Y, input.Z, input.W);
			return input;
			//return new Vector4(input.Z, input.X, input.Y, input.W);
		}
		
		public static Vector3 ReadVec3Position(BinaryReader bStream)
		{
			//Close enough? 
			float scaleVar = 1.6f;
			//-pX, pZ, pY
			//float pX = -bStream.ReadSingle() * scaleVar;
			//float pZ = bStream.ReadSingle() * scaleVar;
			//float pY = bStream.ReadSingle() * scaleVar;
			
			//float pY = -bStream.ReadSingle() * scaleVar;
			//float pZ = -bStream.ReadSingle() * scaleVar;
			//float pX = bStream.ReadSingle() * scaleVar;
			
			float pX = bStream.ReadSingle() * scaleVar;
			float pY = bStream.ReadSingle() * scaleVar;
			float pZ = bStream.ReadSingle() * scaleVar;
			
			return FixAxis(new Vector3(pX * CCS_GLOBAL_SCALE, pY * CCS_GLOBAL_SCALE, pZ * CCS_GLOBAL_SCALE));
		}
		
		public static Vector3 ReadVec3Half(BinaryReader bStream, float scale)
		{
			scale /= 256.0f;
			float vX = bStream.ReadInt16() * VTEX_SCALE;
			float vY = bStream.ReadInt16() * VTEX_SCALE;
			float vZ = bStream.ReadInt16() * VTEX_SCALE;
			return FixAxis(new Vector3(vX * scale, vY * scale, vZ * scale));
		}
		
		public static Vector3 ReadVec3Rotation(BinaryReader bStream)
		{
			//rx, rZ, rY
			//Actually: rz, rx, -ry
			float rX = bStream.ReadSingle();
			float rY = bStream.ReadSingle();
			float rZ = bStream.ReadSingle();
			
			//float rY = bStream.ReadSingle();
			//float rZ = bStream.ReadSingle();
			//float rX = bStream.ReadSingle();
			
			
			float pi = 3.141592653589793f;
			//float toRads = pi / 180.0f;
			//return new Vector3(-rZ * toRads, rY * toRads, -rX * toRads); //FixAxis(new Vector3(rX, rY, rZ));
			return FixAxisRotation(new Vector3((rX * pi) / 180.0f, (rY * pi) / 180.0f, (rZ * pi) / 180.0f));
		}
		
		public static Vector3 ReadVec3Scale(BinaryReader bStream)
		{
			float sX = bStream.ReadSingle();
			float sY = bStream.ReadSingle();
			float sZ = bStream.ReadSingle();
			
			return new Vector3(sX, sY, sZ);
		}
		
		public static Vector3 ReadVec3Normal8(BinaryReader bStream)
		{
			float nX = -bStream.ReadByte() * NORMAL_SCALE;
			float nY = bStream.ReadByte() * NORMAL_SCALE;
			float nZ = bStream.ReadByte() * NORMAL_SCALE;
			
			return new Vector3(nX, nY, nZ);
		}
		
		public static Vector2 ReadVec2UV(BinaryReader bStream)
		{
			float u = bStream.ReadInt16() * UV_SCALE;
			float v = bStream.ReadInt16() * UV_SCALE;
			return new Vector2(u, v);
		}
		
		public static Vector2 ReadVec2UV_Gen3(BinaryReader bStream)
		{
			float uvscale2 = 1.0f / 65535.0f;
			float u = (bStream.ReadInt32() * uvscale2);
			float v = (bStream.ReadInt32() * uvscale2);
			//float u = bStream.ReadSingle();
			//float v = bStream.ReadSingle();
			
			return new Vector2(u, v);
		}
		

		
		public static Vector4 ReadVec4RGBA32(BinaryReader bStream)
		{
			byte uR = bStream.ReadByte();
			byte uG = bStream.ReadByte();
			byte uB = bStream.ReadByte();
			byte uA = bStream.ReadByte();
			if(uA >= 0x7f) uA = 0xff;
			else uA = (byte)(uA << 1);
			
			return new Vector4(uR * COLOR_SCALE, uG * COLOR_SCALE, uB * COLOR_SCALE, uA * COLOR_SCALE);
		}
		
		public static Color ReadColorRGBA32(BinaryReader bStream)
		{
			byte b = bStream.ReadByte();
			byte g = bStream.ReadByte();
			byte r = bStream.ReadByte();
			byte a = bStream.ReadByte();
			if(a >= 0x7f) a = 0xff;
			else a *= 2;
			
			return Color.FromArgb(a, r, g, b);
		}
		
		public static void SkipSection(BinaryReader bStream, int sectionSize)
		{
			bStream.BaseStream.Seek(sectionSize * 4, SeekOrigin.Current);
		}
		
		public static string ReadString(BinaryReader bStream, int stringSize = 0x20)
		{
			byte[] sBytes = bStream.ReadBytes(stringSize);
			//return BitConverter.ToString(sBytes);
			//return System.Text.Encoding.Default.GetString(sBytes).Replace("\0", string.Empty);
			string retVal = System.Text.Encoding.Default.GetString(sBytes);
			int strL = retVal.IndexOf('\0');
			
			if(strL > 0)
			{
				return retVal.Substring(0,strL);	
			}
			
			return string.Empty;
			
		}
		
		public static TreeNode NonExistantNode(CCSFile _file, int _objectID)
		{
			TreeNode retNode = new TreeNode(string.Format("{0}: {1}", _objectID, _file.GetSubObjectName(_objectID)))
			{
				Tag = new TreeNodeTag(_file, _objectID, 0),
				ForeColor = Color.Red
			};
			
			return retNode;
		}
		
		public static string Indent(int count, bool treeIt = false)
		{
			
			if(treeIt && count > 1)
			{
				return "".PadLeft(((count - 1) * 4) - 1) + "|" + "".PadLeft(4, '-');
			}
			
			return "".PadLeft(count * 4);
		}
		
		public static Vector3 SkinVertex(Vector3 v, Matrix4 m1, Matrix4 m2, float w1, float w2)
		{
			Vector4 v1 = new Vector4(v, 1.0f);
			Vector4 v2 = new Vector4(v, 1.0f);
			
			v2 = (v2 * m2) * w2;
			v1 = (v1 * m1) * w1;
			
			Vector4 fp = v1 + v2;
			
			return new Vector3(fp.X, fp.Y, fp.Z);
			
		}
		
		public static Vector3 q2e(Vector4 q)
		{
			//q = q.Zxyw;
			//q = FixAxisRotation4(q);
			//q.W = -q.W;
			float test = q.X * q.Y + q.Z * q.W;
			float heading = 0.0f;
			float attitude = 0.0f;
			float bank = 0.0f;
			if(test > 0.499)
			{
				heading = (float)(2 * Math.Atan2(q.X, q.W));
				attitude = (float)(Math.PI * Math.Atan2(q.X, q.W));
				bank = 0;
				
				//return  FixAxisRotation( new Vector3(heading, attitude, bank));
				//return new Vector3(bank, heading, -attitude);
			//	return new Vector3(heading, -attitude, -bank);
			}
			else
			{
			
				float sqx = q.X * q.X;
				float sqy = q.Y * q.Y;
				float sqz = q.Z * q.Z;
				//heading = (float)(Math.Atan2(2*q.Y * q.W * q.X * q.Z, 1 - 2 * sqy - 2 * sqz));
				heading = (float)(Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z, 1 - 2 * sqy - 2 * sqz));
				attitude = (float)(Math.Asin(2 * test));
				bank = (float)(Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * sqx - 2 * sqz));
				//return FixAxisRotation(new Vector3(heading, attitude, bank));
				//return new Vector3(bank, heading, -attitude);
			}
			return new Vector3(heading, attitude, bank);
		}
		
		public static float Clampf(float a, float min, float max)
		{
			if(a < min) return min;
			if(a > max) return max;
			return a;
		}
		
		public static Vector3 V3Slerp(float t, Vector3 a, Vector3 b)
		{
			float dot = Vector3.Dot(a, b);
			t /= 2;
			
			float theta = (float)Math.Acos(dot);
			if(theta < 0.0f) theta = -theta;
			
			float st = (float)Math.Sin(theta);
			float coeff1 = (float)Math.Sin((1-t) * theta) / st;
			float coeff2 = (float)Math.Sin(t * theta) / st;
			//Vector3 cv1 = a * coeff1;
			//Vector3 cv2 = b * coeff2;
			
			return ((a * coeff1) + (b * coeff2));
		}
		
		public static Vector3 Slerp(float t, Vector3 a, Vector3 b)
		{
			float dot = Vector3.Dot(a,b);
			float theta = (float)Math.Acos(dot) * t;
			Vector3 relative = b - a * dot;
			relative.Normalize();
			
			float mCosTheta = (float)Math.Cos(theta);
			float mSinTheta = (float)Math.Sin(theta);
			
			return (a * mCosTheta) + (relative * mSinTheta);
		}
		
		
		
		public static int GetRangeOfFrames(int curKeyNum, int nextKeyNum, int frameCount)
		{
			int frameRange = nextKeyNum - curKeyNum;
			if(frameRange < 0)
			{
				frameRange = frameCount - curKeyNum;
			}
			
			return frameRange;
		}
	}
}
