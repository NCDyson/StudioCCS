/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 12:29 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Windows.Forms;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSBaseObject.
	/// </summary>
	public abstract class CCSBaseObject
	{
		public int ObjectID = 0;
		public int ObjectType = 0;
		public CCSFile ParentFile;
		
		public virtual TreeNode ToNode()
		{
			TreeNode retNode = new TreeNode(string.Format("{0}: {1}", ObjectID, ParentFile.GetSubObjectName(ObjectID)))
			{
				Tag = new TreeNodeTag(ParentFile, ObjectID, ObjectType)
			};
			return retNode;
		}
		
		public abstract bool Init();
		public abstract bool DeInit();
		public abstract bool Read(BinaryReader bStream, int sectionSize);
	}
}
