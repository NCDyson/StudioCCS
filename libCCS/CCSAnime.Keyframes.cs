/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/25/2017
 * Time: 4:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;

namespace StudioCCS.libCCS
{
	public partial class CCSAnime : CCSBaseObject
	{
		public abstract class AnimationKeyFrame
		{
			public int KeyFrameType;
			public abstract void Read(BinaryReader bStream);
		}
		
		public struct MorphKeyValue
		{
			public int ModelID;
			public float Value;
		}
		
		public class MorphKeyFrame : AnimationKeyFrame
		{
			public int MorphID = 0;
			public MorphKeyValue[] MorpherKeys = null;
			public int MorphKeyCount = 0;
			
			public MorphKeyFrame()
			{
				KeyFrameType = CCS_ANIME_MORPH_KEYFRAME;
			}
			
			public override void Read(BinaryReader bStream)
			{
				MorphID = bStream.ReadInt32();
				MorphKeyCount = bStream.ReadInt32();
				MorpherKeys = new MorphKeyValue[MorphKeyCount];
				for(int i = 0; i < MorphKeyCount; i++)
				{
					MorphKeyValue tmpKey = new MorphKeyValue()
					{
						ModelID = bStream.ReadInt32(),
						Value = bStream.ReadSingle()
					};
					
				}
			}
		}
		
		public class AmbientLightKeyFrame : AnimationKeyFrame
		{
			public Vector4 Value = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
			
			public AmbientLightKeyFrame()
			{
				KeyFrameType = CCS_ANIME_LIGHT_AMBIENT_KEYFRAME;
			}
			
			public override void Read(BinaryReader bStream)
			{
				Value = Util.ReadVec4RGBA32(bStream);
			}
			
		}
		
		public class AnimationFrame
		{
			public List<AnimationKeyFrame> KeyFrames = new List<AnimationKeyFrame>();
			public int FrameNumber = 0;
			
			public AnimationFrame(int _frameNumber)
			{
				FrameNumber = _frameNumber;
			}
		}
		
		
	}
}
