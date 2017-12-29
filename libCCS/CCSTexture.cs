/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 3:33 AM
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
using System.Diagnostics;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSTexture.
	/// </summary>
	public class CCSTexture : CCSBaseObject
	{
		//Known Texture Types
		public const int CCS_TEXTURE_I4 = 0x14;
		public const int CCS_TEXTURE_I8 = 0x13;
		public const int CCS_TEXTURE_RGBA32 = 0x0;
		public const int CCS_TEXTURE_DXT1 = 0x87; //0x41; //or 0x8741
		public const int CCS_TEXTURE_DXT5 = 0x89; //0x45; //or 0x8945
		
		//Fields
		public byte[] TextureIndices = null;
		public int CLUTID = 0;
		public int MipCount = 0;
		public int TextureType = 0;
		public int Width = 0;
		public int Height = 0;
		public int BlitGroupID = 0;
		public int TextureFlags = 0;
		public bool NonP2 = false;
		public bool HasAlpha = false;
		//For preview/scene purposes
		public int CurrentCLUTID = 0;
		
		//OpenGL Related things
		public int TextureID;
		
		public CCSTexture(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_TEXTURE;
		}
		
		public override bool Init()
		{
			//We need to be able to re-init texture with different clut, and I'm not writing the code twice.
			return InitTexture(CLUTID);
		}
		
		public override bool DeInit()
		{
			//Remove Texture
			if(TextureID != -1) GL.DeleteTexture(TextureID);
			TextureID = -1;
			
			return true;
		}
		
		public override bool Read(System.IO.BinaryReader bStream, int sectionSize)
		{
			//TODO: CCSTexture::Read() Redo so it's not so ugly handling Gen3 textures...
			long textureOffset = bStream.BaseStream.Position;
			Debug.WriteLine(string.Format("Reading texture {0:X}: {1} at 0x{2:X}...\n", ObjectID, ParentFile.GetSubObjectName(ObjectID), textureOffset));
			CLUTID = bStream.ReadInt32();
			BlitGroupID = bStream.ReadInt32();
			
			TextureFlags = bStream.ReadByte();
			TextureType = bStream.ReadByte();
			if(TextureType != CCS_TEXTURE_I4 && TextureType != CCS_TEXTURE_I8 && TextureType != CCS_TEXTURE_RGBA32 && TextureType != CCS_TEXTURE_DXT1 && TextureType != CCS_TEXTURE_DXT5)
			{
				Logger.LogError(string.Format("CCSTexture::Read(): Unknown texture type {0:X} at 0x{1:X}\n", TextureType, textureOffset));
				return false;
			}
			MipCount = bStream.ReadByte();
			bStream.ReadByte();
			
			this.Width = bStream.ReadByte();
			this.Height = bStream.ReadByte();
			bStream.ReadInt16();
			//Handle Non-Power of Two texture sizes in Gen2(Link)
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
			{
				Width = 1 << Width;
				Height = 1 << Height;
				bStream.ReadInt32();
			}
			else
			{
				if(Width == 0xff || Height == 0xff)
				{
					NonP2 = true;
					if(TextureType == CCS_TEXTURE_DXT1 || TextureType == CCS_TEXTURE_DXT5)
					{
						Logger.LogError(string.Format("Error, CCSTexture 0x{0:X}: {1} has non-power of two width or height", ObjectID, ParentFile.GetSubObjectName(ObjectID)));
						return false;
					}
					else
					{
						Width = bStream.ReadInt16();
						Height = bStream.ReadInt16();
					}
				}
				else
				{
					Width = 1 << Width;
					Height = 1 << Height;
					if(TextureType == CCS_TEXTURE_DXT1 || TextureType == CCS_TEXTURE_DXT5)
					{
						bStream.BaseStream.Seek(0x10, SeekOrigin.Current);
						int texWidth = bStream.ReadInt16();
						int texHeight = bStream.ReadInt16();
						if(texWidth != Width || texHeight != Height)
						{
							Logger.LogWarning(string.Format("Warning, CCSTexture 0x{0:X}: {1} has mismatched Width/Height values..", ObjectID, ParentFile.GetSubObjectName(ObjectID)));
							//return false;
						}
						bStream.BaseStream.Seek(0x14, SeekOrigin.Current);
					}
					else
					{
						bStream.ReadInt32();
					}
				}
			}
			
			//TODO: CCSTexture::Read(): Handle Swizzling in PSP Version (CCSF_VERSION: 0x124)
			
			//TODO: CCSTexture::Read(): Investigate/Handle other texture types.
			//TODO: CCSTexture::Read(): Throw a bitch fit if Gen1/2 CCS Files contain DXT textures...
			int textureDataSize = bStream.ReadInt32();
			if(TextureType == CCS_TEXTURE_DXT1 || TextureType == CCS_TEXTURE_DXT5)
			{
				textureDataSize -= 0x40;
				bStream.BaseStream.Seek(0x1c, SeekOrigin.Current);
			}
			else
			{
				textureDataSize = textureDataSize << 2;
			}
			TextureIndices = new byte[textureDataSize];
			if(TextureType == CCS_TEXTURE_RGBA32)
			{
				for(int i = 0; i < (textureDataSize / 4); i++)
				{
					byte b = bStream.ReadByte();
					byte g = bStream.ReadByte();
					byte r = bStream.ReadByte();
					byte a = bStream.ReadByte();
					if(a >= 0x7f) a = 0xff;
					else a *= 2;
					
					int tDelta = i * 4;
					TextureIndices[tDelta] = b;
					TextureIndices[tDelta + 1] = g;
					TextureIndices[tDelta + 2] = r;
					TextureIndices[tDelta + 3] = a;
				}
			}
			else
			{
				bStream.Read(TextureIndices, 0, textureDataSize);	
			}
			if(TextureType == CCS_TEXTURE_DXT1 || TextureType == CCS_TEXTURE_DXT5)
			{
				if(MipCount > 0)
				{
					Logger.LogError(string.Format("Error, Texture {0:X} at 0x{1:X} has MipLevels. Please investigate.", ObjectID, textureOffset));
					return false;
				}
			}
			
			//TODO: CCSTexture::Read(): Handle Mip Levels instead of skipping them.
			for(int i = 0; i < MipCount; i++)
			{
				bStream.ReadInt32();
				int mipSize = bStream.ReadInt32() << 2;
				bStream.BaseStream.Seek(mipSize, SeekOrigin.Current);
			}
			
			return true;
		}

/*		
		public bool Read_Gen3(System.IO.BinaryReader bStream, int sectionSize)
		{
			long textureOffset = bStream.BaseStream.Position;
			
			bStream.ReadInt32(); //unused
			bStream.ReadInt32(); //unused
			
			bStream.ReadByte();			
			TextureType = bStream.ReadByte();

			if(TextureType != CCS_TEXTURE_DXT1 && TextureType != CCS_TEXTURE_DXT5)
			{
				Logger.LogError(string.Format("Error, Texture {0:X} at 0x{1:X}, unknown texture type {2:X}.\n", ObjectID, textureOffset, TextureType));
				return false;
			}
			MipCount = bStream.ReadByte();
			if(MipCount > 0)
			{
				Logger.LogError(string.Format("Error, Texture {0:X} at 0x{1:X}, has mip levels. these need to be investigated.\n", ObjectID, textureOffset));
				return false;
			}
			bStream.ReadByte();
			
			int texWidth = 1 << bStream.ReadByte();
			int texHeight = 1 << bStream.ReadByte();
			bStream.ReadInt16();
			
			bStream.BaseStream.Seek(0x10, SeekOrigin.Current);
			Width = bStream.ReadInt16();
			Height = bStream.ReadInt16();
			if(texWidth != Width || texHeight != Height)
			{
				Logger.LogError(string.Format("Error, Texture {0:X} at 0x{1:X}, Texture Width/Height mismatch...I don't know who to belive.\n", ObjectID, textureOffset));
				return false;
			}
			
			bStream.BaseStream.Seek(0x14, SeekOrigin.Current);
			
			int textureDataSize = bStream.ReadInt32() - 0x40;
			bStream.BaseStream.Seek(0x1c, SeekOrigin.Current);
			TextureIndices = new byte[textureDataSize];
			bStream.Read(TextureIndices, 0, textureDataSize);
			
			
			return true;
		}
*/		
		public bool InitTexture(int _clutID)
		{
			if(NonP2)
			{
				Logger.LogError("CCSTexture::InitTexture(): Non-Power of Two textures are currently unsupported\n");
				return false;
			}
			/*
			if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen3)
			{
				return InitTexture_Gen3();
			}
			*/
			//else
			//{
			if(TextureType == CCS_TEXTURE_DXT1 || TextureType == CCS_TEXTURE_DXT5)
			{
				if(TextureID == 0) TextureID = GL.GenTexture();
				GL.BindTexture(TextureTarget.Texture2D, TextureID);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
				
				PixelInternalFormat tmpFormat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
				if(TextureType == CCS_TEXTURE_DXT5) tmpFormat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
				
				GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, tmpFormat, Width, Height, 0, TextureIndices.Length, TextureIndices);
				GL.BindTexture(TextureTarget.Texture2D, 0);
				return true;
			}
			else
			{
				Bitmap FinalTexture = ToBitmap(_clutID);
				if(TextureID == 0) TextureID = GL.GenTexture();
				
				CurrentCLUTID = _clutID;
				GL.BindTexture(TextureTarget.Texture2D, TextureID);
				
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
				
				System.Drawing.Imaging.BitmapData textureData = FinalTexture.LockBits(new Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, textureData.Scan0);
				
				FinalTexture.UnlockBits(textureData);
				
				GL.BindTexture(TextureTarget.Texture2D, 0);
			
				
			}
			return true;
			//}
		}
		
		public bool InitTexture_Gen3()
		{
			if(TextureID == 0) TextureID = GL.GenTexture();
			
			GL.BindTexture(TextureTarget.Texture2D, TextureID);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
			
			PixelInternalFormat tmpFormat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
			if(TextureType == CCS_TEXTURE_DXT1) tmpFormat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
			else if(TextureType == CCS_TEXTURE_DXT5) tmpFormat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
			else
			{
				GL.BindTexture(TextureTarget.Texture2D, 0);
				return true;
			}
			
			GL.CompressedTexImage2D(TextureTarget.Texture2D, 0, tmpFormat, Width, Height, 0, TextureIndices.Length, TextureIndices);
				//(TextureTarget.Texture2D, 0, tmpFormat, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, TextureIndices);
			
			GL.BindTexture(TextureTarget.Texture2D, 0);
			return true;
		}
		
		public Bitmap ToBitmap(int _clutID)
		{
			Bitmap OutBitmap = new Bitmap(Width, Height);
			if(TextureType == CCS_TEXTURE_I4 || TextureType == CCS_TEXTURE_I8)
			{
				CCSClut tmpClut = ParentFile.GetObject<CCSClut>(_clutID);
				if(tmpClut == null) return null;
				
				HasAlpha = tmpClut.HasAlpha;
	
				if(TextureType == CCS_TEXTURE_I4)
				{
					for(int y = 0; y < Height; y++)
					{
						for(int x = 0; x < (Width / 2); x++)
						{
							int indiceOffset = (y * (Width / 2)) + x;
							byte indices = TextureIndices[indiceOffset];
							OutBitmap.SetPixel(x * 2, y, tmpClut.Palette[indices & 0xf]);
							OutBitmap.SetPixel((x * 2) + 1, y, tmpClut.Palette[(indices >> 4) & 0xf]);
						}
					}
				}
				else if(TextureType == CCS_TEXTURE_I8)
				{
					for(int y = 0; y < Height; y++)
					{
						for(int x = 0; x < Width; x++)
						{
							int indiceOffset = (y * Width) + x;
							OutBitmap.SetPixel(x, y, tmpClut.Palette[TextureIndices[indiceOffset]]);
						}
					}
				}
			}
			else if(TextureType == CCS_TEXTURE_RGBA32)
			{
				//TODO: CCSTexture::ToBitmap(): RGBA32 Textures: There's got to be a better way to do this...
				for(int y = 0; y < Height; y++)
				{
					for(int x = 0; x < Width; x++)
					{
						int i = ((y * Width) + x) * 4;
						OutBitmap.SetPixel(x,y, Color.FromArgb(TextureIndices[i + 3], TextureIndices[i + 2], TextureIndices[i + 1], TextureIndices[i]));
					}
				}
			}
			
			return OutBitmap;
		}
		
		public string GetTextureTypeStr()
		{
			if(TextureType == CCS_TEXTURE_I4)
			{
				return "4bit Indexed";
			}
			else if(TextureType == CCS_TEXTURE_I8)
			{
				return "8bit Indexed";
			}
			else if(TextureType == CCS_TEXTURE_RGBA32)
			{
				return "32bit RGBA";
			}
			else if(TextureType == CCS_TEXTURE_DXT1)
			{
				return "DXT1 Compressed";
			}
			else if(TextureType == CCS_TEXTURE_DXT5)
			{
				return "DXT5 Compressed";
			}
			
			return "Unknown";
		}
		
		public override System.Windows.Forms.TreeNode ToNode()
		{
			var retNode = base.ToNode();
			string textureTypeStr = string.Format("  Type: {0}", GetTextureTypeStr());
			
			retNode.Text += textureTypeStr;
			
			//Collect other CLUT nodes in file.
			IndexFileEntry parentSubFile = ParentFile.GetParentSubFile(ObjectID);
			if(parentSubFile != null)
			{
				for (int i = 0; i < parentSubFile.ObjectIDs.Count; i++)
				{
					int tmpID = parentSubFile.ObjectIDs[i];
					int tmpType = ParentFile.GetSubObjectType(tmpID);
					if(tmpType == CCSFile.SECTION_CLUT)
					{
						CCSClut tmpClut = ParentFile.GetObject<CCSClut>(tmpID);
						TreeNode tmpSubNode = null;
						if(tmpClut == null)
						{
							tmpSubNode = Util.NonExistantNode(ParentFile, tmpID);
						}
						else
						{
							tmpSubNode = tmpClut.ToNode();
						}
						
						if(tmpID == CLUTID) tmpSubNode.Text += " (Default)";
						if(tmpID == CurrentCLUTID) tmpSubNode.Text += " (Current)";
						retNode.Nodes.Add(tmpSubNode);
					}
				}
			}
			
			return retNode;
		}
		
		public string GetReport(int level = 0)
		{
			string retVal = Util.Indent(level) + string.Format("Texture 0x{0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			retVal += Util.Indent(level + 1) + string.Format("Type: {0}, {1}x{2}, {3} MipMap Levels\n", GetTextureTypeStr(), Width, Height, MipCount);
			if(TextureType != CCS_TEXTURE_RGBA32)
			{
				retVal += Util.Indent(level + 1) + string.Format("CLUT: {0}: {1}\n", CLUTID, ParentFile.GetSubObjectName(CLUTID));
			}
			
			return retVal;
		}
		
		public void DumpToMtl(StreamWriter fStream, string outputPath)
		{
			
			string textureName = ParentFile.GetSubObjectName(ObjectID);
			//Write .mtl line
			fStream.WriteLine(string.Format("newmtl {0}", textureName));
			fStream.WriteLine("Ka 1.0 1.0 1.0");
			fStream.WriteLine("Kd 1.0 1.0 1.0");
			fStream.WriteLine("Ks 0.0 0.0 0.0");
			
			if(TextureType == CCS_TEXTURE_DXT1 || TextureType == CCS_TEXTURE_DXT5)
			{
				fStream.WriteLine(string.Format("map_Kd {0}.dds", textureName));
				string outputTextureName = string.Format("{0}.dds", System.IO.Path.Combine(outputPath, textureName));
				DumpToDDS(outputTextureName);
			}
			else
			{
				fStream.WriteLine(string.Format("map_Kd {0}.png", textureName));
			
				//Now, dump texture to file
				string outputTextureFileName = string.Format("{0}.png", System.IO.Path.Combine(outputPath, textureName));
				Logger.LogInfo(string.Format("\tDumping {0}...\n", outputTextureFileName));
				Bitmap texture = ToBitmap(CurrentCLUTID);
				
				//Swap color channels
				System.Drawing.Imaging.BitmapData sourceData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
	
				byte[] resultBuffer = new byte[sourceData.Stride * sourceData.Height];
				Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);
				
				
				byte tmpByte = 0;
				for(int i = 0; i < resultBuffer.Length; i += 4)
				{
					tmpByte = resultBuffer[i];
					resultBuffer[i] = resultBuffer[i + 2];
					resultBuffer[i + 2] = tmpByte;
				}
				
				Marshal.Copy(resultBuffer, 0, sourceData.Scan0, resultBuffer.Length);
				texture.UnlockBits(sourceData);
				
				texture.Save(outputTextureFileName, System.Drawing.Imaging.ImageFormat.Png);
			}
		}
		
		public void DumpToDDS(string outputFileName)
		{
			using(FileStream fs = new FileStream(outputFileName, FileMode.OpenOrCreate))
			{
				using(BinaryWriter bs = new BinaryWriter(fs))
				{
					//TODO: CCSTexture::DumpToDDS(): Legit DDS implementation?
					//Write Header
					//Write DDS Magic
					bs.Write((int)0x20534444);
					//Write DDS Header size
					bs.Write((int)0x7c);
					//Write DDS Flags
					bs.Write((int)0xa1007);
					//Write Height
					bs.Write((int)Height);
					//Write Width
					bs.Write((int)Width);
					int blockSize = 16;
					if(TextureType == CCS_TEXTURE_DXT1) blockSize = 8;
					int pitch = Math.Max(1,((Width + 3)/4)) * blockSize;
					//Write Pitch
					bs.Write(pitch);
					//Write Depth
					bs.Write((int)0);
					//Write MipCount
					bs.Write((int)0);
					//Write reserved
					for(int i = 0; i < 11; i++)
					{
						bs.Write((int)0);
					}
					
					//Write Pixel Format
					//write size
					bs.Write((int)0x20);
					//write flags
					bs.Write((int)4);
					//write fourCC
					int fourCC = 0x31545844; //44 58 54 35
					if(TextureType == CCS_TEXTURE_DXT5) fourCC = 0x35545844;
					bs.Write(fourCC);
					//write RGBBitCount
					bs.Write((int)0);
					//write RBitMask
					bs.Write((int)0);
					//write GBitMask
					bs.Write((int)0);
					//write BBitMask
					bs.Write((int)0);
					//write ABitMask
					bs.Write((int)0);
					//write CAPS
					bs.Write((int)0x401008);
					//write caps2
					bs.Write((int)0);
					//write caps3
					bs.Write((int)0);
					//write caps4
					bs.Write((int)0);
					//write reserved2
					bs.Write((int)0);
					//write texture data
					bs.Write(TextureIndices);
				}
			}
		}
		
	}
}
