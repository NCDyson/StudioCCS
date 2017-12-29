/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 5:20 AM
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
	/// Description of CCSLight.
	/// </summary>
	public class CCSLight : CCSBaseObject
	{
		//Useful Defines
		public const int CCS_LIGHT_DIRECTIONAL = 0x1;
		public const int CCS_LIGHT_OMNI = 0x2;
		
		//Fields
		public int LightType = 0;
		
		//Not defined in Setup, but in Animation
		public Vector4 Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
		public Vector3 Position = Vector3.Zero;
		public Vector3 Rotation = Vector3.Zero;
		public float Unk1 = 0.0f;
		public float Unk2 = 0.0f;
		
		
		public CCSLight(int _objectID, CCSFile _parentFile)
		{
			ObjectType = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_LIGHT;
		}
		
		public override bool Init()
		{
			//Currently nothing to be done for CCSLight::Init()
			//TODO: CCSLight: Rendering Visual Aids for light types.
			return true;
		}
		
		public override bool DeInit()
		{
			//Currently nothing to be done for CCSLight::DeInit()
			return true;
		}
		
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			LightType = bStream.ReadInt32();
			return true;
		}
		
		public override TreeNode ToNode()
		{
			var retNode = base.ToNode();
			string lightTypeStr = "";
			switch (LightType)
			{
				case 0:
					lightTypeStr = " Type: None";
					break;
				case CCS_LIGHT_DIRECTIONAL:
					lightTypeStr = " Type: Directional";
					break;
				case CCS_LIGHT_OMNI:
					lightTypeStr = " Type: Omni";
					break;
				default:
					lightTypeStr = string.Format(" Type: Unknown {0}", LightType);
					break;
			}
			retNode.Text += lightTypeStr;
			
			return retNode;
		}
	}
}
