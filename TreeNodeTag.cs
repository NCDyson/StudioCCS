/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 12:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using StudioCCS.libCCS;

namespace StudioCCS
{
	/// <summary>
	/// Description of TreeNodeTag.
	/// </summary>
	public class TreeNodeTag
	{
		public enum NodeType {File, Main, SubNode};
		public CCSFile File = null;
		public int ObjectID = 0;
		public int ObjectType = 0;
		public int SubID = 0;
		public NodeType Type = NodeType.SubNode;
		
		public TreeNodeTag(CCSFile _File, int _objectID, int _objectType)
		{
			File = _File;
			ObjectID = _objectID;
			ObjectType = _objectType;
			Type = NodeType.Main;
			SubID = 0;
		}
		
		public TreeNodeTag(CCSFile _file, int _objectID, int _objectType, NodeType _nodeType, int _subID)
		{
			File = _file;
			ObjectID = _objectID;
			ObjectType = _objectType;
			Type = _nodeType;
			SubID = _subID;
		}
		
	}
}
