/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 1:05 AM
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
	/// Description of CCSClut.
	/// </summary>
	public class CCSClut : CCSBaseObject
	{
		public int ColorCount;
		public int BlitGroup;
		public Color[] Palette = null;
		public bool HasAlpha = false;
		
		public CCSClut(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_CLUT;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			BlitGroup = bStream.ReadInt32();
			
			bStream.ReadInt32();
			bStream.ReadInt32();
			ColorCount = bStream.ReadInt32();
			
			Palette = new Color[ColorCount];
			for(int i = 0; i < ColorCount; i++)
			{
				Palette[i] = Util.ReadColorRGBA32(bStream);
				HasAlpha |= Palette[i].A != 0xff;
			}
			
			return true;
		}
		
		public override bool Init()
		{
			//Currently nothing to do for CCSClut::Init()
			return true;
		}
		
		public override bool DeInit()
		{
			//Currently nothing to do for CCSClut::DeInit()
			return true;
		}
		
		public override TreeNode ToNode()
		{
			var retNode = base.ToNode();
			retNode.Text += string.Format(" ({0} Colors)", ColorCount);
			return retNode;
		}
	}
}
