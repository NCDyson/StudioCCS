/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 1:18 AM
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
	/// Description of CCSExt.
	/// </summary>
	public class CCSExt : CCSBaseObject
	{
		public int ReferencedParentID;
		public int ReferencedObjectID;
		
		public CCSExt(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_EXTERNAL;
		}
		
		public override bool Init()
		{
			//Currently nothing to be done for CCSExt::Init()
			return true;
		}
		
		public override bool DeInit()
		{
			//Currently nothing to be done for CCSExt::DeInit()
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			ReferencedParentID = bStream.ReadInt32();
			ReferencedObjectID = bStream.ReadInt32();
			
			return true;
		}
	}
}
