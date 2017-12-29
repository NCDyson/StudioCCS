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

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSFileHeader.
	/// </summary>
	public class CCSFileHeader
	{
		//Useful defines
		public const int CCS_MAGIC = 0x46534343;
		public const int CCS_VERSION_ONE = 0x0110;
		public const int CCS_VERSION_TWO = 0x120;
		public const int CCS_VERSION_THREE = 0x125;
		public enum CCSVersion {Gen1, Gen2, Gen3};
		
		//Fields
		public string CCSFName = "";
		public int CCSFVersion = 0;
		public int Unk1 = 0;
		public int Unk2 = 0;
		public int Unk3 = 0;
		
		
		public CCSFileHeader()
		{
			
		}
		
		public bool Read(BinaryReader bStream)
		{
			var isHeader = bStream.ReadInt32() & 0xFFFF;
			if(isHeader != CCSFile.SECTION_HEADER)
			{
				Logger.LogError("Header Section Mismatch!\n");
				return false;
			}
			int sectionSize = bStream.ReadInt32();
			int magic = bStream.ReadInt32();
			if(magic != CCS_MAGIC)
			{
				Logger.LogError("Invalid CCS Magic.\n");
				return false;
			}
			
			CCSFName = Util.ReadString(bStream);
			CCSFVersion = bStream.ReadInt32();
			Unk1 = bStream.ReadInt32();
			Unk2 = bStream.ReadInt32();
			Unk3 = bStream.ReadInt32();
			
			
			if(CCSFVersion >= CCS_VERSION_THREE)
			{
				Logger.LogError("Support for Generation 3 (Last Recode) CCS Files is currently experimental...\n");
			}
			else if(CCSFVersion >= CCS_VERSION_TWO)
			{
				Logger.LogError("Support for Generation 2(GU/Link) CCS Files is currently experimental...\n");
				//return false;
			}
			
			
			return true;
		}
		
		public CCSVersion GetVersionType()
		{
			/*
  			if(CCSFVersion > CCS_VERSION_ONE) return CCSVersion.Gen2;
			return CCSVersion.Gen1;
			*/
			if(CCSFVersion >= CCS_VERSION_THREE) return CCSVersion.Gen3;
			else if(CCSFVersion >= CCS_VERSION_TWO) return CCSVersion.Gen2;
			return CCSVersion.Gen1;
		}
		
		public string GetVersionString(bool generation = false)
		{
			byte vMaj = (byte)((CCSFVersion >> 16) & 0xf);
			byte vMin = (byte)((CCSFVersion >> 8) & 0xf);
			byte vRev = (byte)(CCSFVersion & 0xf);
			
			string genText = "";
			if(generation)
			{
				if(CCSFVersion > CCS_VERSION_TWO) genText = "(Gen. 3)";
				else if(CCSFVersion > CCS_VERSION_ONE) genText = "(Gen. 2)";
				else genText = "(Gen. 1)";
			}
			
			return string.Format("{0}.{1}.{2}{3}", vMaj, vMin, vRev, genText);
		}
	}
}
