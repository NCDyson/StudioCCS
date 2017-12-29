/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 1:22 AM
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
	/// Description of CCSFBPage.
	/// </summary>
	public class CCSFBPage : CCSBaseObject
	{
		public Byte[] Data;
		public int DataSize;
		
		public CCSFBPage(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_FBPAGE;
		}
		
		public override bool Init()
		{
			//Currently nothing to do for CCSFBPage::Init()
			return true;
		}
		
		public override bool DeInit()
		{
			//Currently nothing to do for CCSFBPage::DeInit()
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			DataSize = sectionSize * 4;
			Data = new byte[DataSize];
			bStream.Read(Data,0, DataSize);
			
			return true;
		}
	}
}
