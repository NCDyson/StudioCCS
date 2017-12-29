/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 5:38 AM
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

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSMorpher.
	/// </summary>
	public class CCSMorpher : CCSBaseObject
	{
		public int BaseModelID = 0;
		
		//Helper Refs
		CCSModel BaseModelRef = null;
		
		public CCSMorpher(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_MORPHER;
		}
		
		public override bool Init()
		{
			BaseModelRef = ParentFile.GetObject<CCSModel>(BaseModelID);
			return true;
		}
		public override bool DeInit()
		{
			return true;
		}
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			BaseModelID = bStream.ReadInt32();
			return true;
		}
		
		public override TreeNode ToNode()
		{
			var retNode =  base.ToNode();
			retNode.Text += string.Format(" Base: {0}", ParentFile.GetSubObjectName(BaseModelID));
			return retNode;
		}
	}
}
