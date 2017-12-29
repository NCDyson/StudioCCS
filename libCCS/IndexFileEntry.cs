/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 12:04 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of IndexFileEntry.
	/// </summary>
	public class IndexFileEntry
	{
		public string FileName = "";
		public List<int> ObjectIDs = new List<int>();
		
		public void Read(BinaryReader bStream)
		{
			FileName = Util.ReadString(bStream);
		}
		
		public void AddObjectID(int _objectID)
		{
			ObjectIDs.Add(_objectID);
		}
		
	}
}
