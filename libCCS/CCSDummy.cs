/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 1:10 AM
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
using System.Drawing;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSDummy.
	/// </summary>
	public class CCSDummy : CCSBaseObject
	{
		public Vector3 Position = new Vector3(0.0f, 0.0f, 0.0f);
		public Vector3 Rotation = new Vector3(0.0f, 0.0f, 0.0f);
		
		public CCSDummy(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_DUMMYPOS;
		}
		
		public CCSDummy(int _objectID, CCSFile _parentFile, int _objType)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = _objType;
		}
		
		public override bool Init()
		{
			//Currently nothing to be done for CCSDummy::Init()
			return true;
		}
		
		public override bool DeInit()
		{
			//Currently nothing to be done for CCSDummy::DeInit()
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			Position = Util.ReadVec3Position(bStream);
			
			if(ObjectType == CCSFile.SECTION_DUMMYPOSROT) Rotation = Util.ReadVec3Rotation(bStream);
			
			return true;
		}
		
		public Matrix4 Matrix()
		{
			var rotQuat = new Quaternion(Rotation);
			return Matrix4.CreateFromQuaternion(rotQuat) * Matrix4.CreateTranslation(Position);
		}
		
		public void DumpToTxt(StreamWriter fStream)
		{
			fStream.WriteLine(ParentFile.GetSubObjectName(ObjectID));
			fStream.WriteLine(string.Format("{0}\t{1}\t{2}", Position.X, Position.Y, Position.Z));
			fStream.WriteLine(string.Format("{0}\t{1}\t{2}", Rotation.X, Rotation.Y, Rotation.Z));
		}
		
		
	}
}
