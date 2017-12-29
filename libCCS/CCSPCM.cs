﻿/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 3:11 AM
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
	/// Description of CCSPCM.
	/// </summary>
	public class CCSPCM : CCSBaseObject
	{
		public byte[] Data;
		public int DataSize;
		
		public CCSPCM(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_PCM;
		}

		public override bool Init()
		{
			//Currently nothing to be done for CCSPCM::Init()
			return true;
		}

		public override bool DeInit()
		{
			//Currently nothing to be done for CCSPCM::DeInit()
			return true;
		}

		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			DataSize = sectionSize * 4;
			Data = new byte[DataSize];
			bStream.Read(Data, 0, DataSize);
			
			return true;
		}

	}
}
