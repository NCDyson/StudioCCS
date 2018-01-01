/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 5:31 AM
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
	/// Description of CCSMaterial.
	/// </summary>
	public class CCSMaterial : CCSBaseObject
	{
		public int TextureID = 0;
		public float Alpha = 1.0f;
		public Vector2 TextureOffset = Vector2.Zero;
		
		//Helper Refs
		public CCSTexture TextureRef = null;
		
		public CCSMaterial(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_MATERIAL;
		}
		
		public override bool Init()
		{
			TextureRef = ParentFile.GetObject<CCSTexture>(TextureID);
			return true;
		}
		
		public override bool DeInit()
		{
			//Currently nothing to be done for CCSMaterial::DeInit()
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			TextureID = bStream.ReadInt32();
			Alpha = bStream.ReadSingle();
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
			{
				TextureOffset = Util.ReadVec2UV(bStream);	
			}
			else
			{
				float tmpX = bStream.ReadSingle();
				float tmpY = bStream.ReadSingle();
				TextureOffset = new Vector2(tmpX, tmpY);
			}
			//TODO: CCSMaterial::Read(): Gen2 Extra Data
			
			/*
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen2)
			{
				bStream.ReadUInt32();
			}
			else if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3)
			{
				if(sectionSize == 0x4)
				{
					bStream.ReadInt32();
				}
				else
				{
					bStream.BaseStream.Seek(0x4c, SeekOrigin.Current);	
				}
				
			}*/
			int restData = sectionSize - 3;
			if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1) restData -= 1;
			if(restData > 0) bStream.BaseStream.Seek(restData * 4, SeekOrigin.Current);
			
			return true;
		}
		
		public override TreeNode ToNode()
		{
			var retNode = base.ToNode();
			retNode.Text += string.Format(" Texture: {0}", ParentFile.GetSubObjectName(TextureID));
			return retNode;
		}
		
		
		public string GetReport(int level = 0)
		{
			string retVal = Util.Indent(level) + string.Format("Material 0x{0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			//string textureName = "<None>";
			if(TextureID != 0)
			{
				retVal += Util.Indent(level + 1) + string.Format("Texture: 0x{0:X}: {1}\n", TextureID, ParentFile.GetSubObjectName(TextureID));
			}
			else
			{
				retVal += Util.Indent(level + 1) + string.Format("Texture: None\n");
			}
			
			return retVal;
		}
		
	}
}
