/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/25/2017
 * Time: 3:45 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using System.Diagnostics;

namespace StudioCCS.libCCS
{
	public partial class CCSAnime : CCSBaseObject
	{
		public interface IAnimationKey<T>
		{
			void Read(BinaryReader bStream, int _frameNum);
			T Value();
			int FrameNumber();
			void SetFrameCount(int fCount);
			int GetFrameCount();
		}
		
		public class Vec4Key_Color : IAnimationKey<Vector4>
		{
			int FrameNum;
			int FrameCount;
			Vector4 KeyValue;
			
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = Util.ReadVec4RGBA32(bStream);
			}
			
			public Vector4 Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			
			#endregion
		}
		
		public class Vec4Key_Rotation : IAnimationKey<Quaternion>
		{
			int FrameNum;
			int FrameCount;
			Quaternion KeyValue;
			
			#region IAnimationKey implemenetation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = new Quaternion(bStream.ReadSingle(), bStream.ReadSingle(), bStream.ReadSingle(), bStream.ReadSingle());
			}
			
			public Quaternion Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		public class Vec3Key_Rotation : IAnimationKey<Vector3>
		{
			Vector3 KeyValue;
			int FrameNum;
			int FrameCount;
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = Util.ReadVec3Rotation(bStream);
			}
			
			public Vector3 Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		public class Vec3Key_Position : IAnimationKey<Vector3>
		{
			Vector3 KeyValue;
			int FrameNum;
			int FrameCount;
			
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = Util.ReadVec3Position(bStream);
			}
			
			public Vector3 Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		public class Vec3Key_Scale : IAnimationKey<Vector3>
		{
			Vector3 KeyValue = Vector3.One;
			int FrameNum;
			int FrameCount;
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = Util.ReadVec3Scale(bStream);
			}
			
			public Vector3 Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		public class Vec2Key_UV : IAnimationKey<Vector2>
		{
			int FrameNum;
			int FrameCount;
			Vector2 KeyValue;
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = Util.ReadVec2UV(bStream);
			}
			
			public Vector2 Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		public class F32Key : IAnimationKey<float>
		{
			int FrameNum;
			int FrameCount;
			float KeyValue;
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = bStream.ReadSingle();
			}
			
			public float Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		public class Int32Key : IAnimationKey<int>
		{
			public int FrameNum;
			public int KeyValue;
			public int FrameCount;
			#region IAnimationKey implementation
			public void Read(BinaryReader bStream, int _frameNum)
			{
				FrameNum = _frameNum;
				KeyValue = bStream.ReadInt32();
			}
			
			public int Value()
			{
				return KeyValue;
			}
			
			public int FrameNumber()
			{
				return FrameNum;
			}
			
			public int GetFrameCount()
			{
				return FrameCount;
			}
			
			public void SetFrameCount(int fCount)
			{
				FrameCount = fCount;
			}
			#endregion
		}
		
		
		//TODO: CCAnime Controller Tracks: Can these be templated without "abusing" generics?

/*		
#region Auto-Generated with meta.py

	public class Vec4Rotation_Track
	{
		public List<Vec4Key_Rotation> Keys = new List<Vec4Key_Rotation>();
		public Vec4Key_Rotation FixedValue = new Vec4Key_Rotation();
		public int KeyCount = 0;
		public int CurrentKey = 0;
		
		public Vec4Key_Rotation GetValue(int keyID)
		{
			if(KeyCount == 0)
			{
				return FixedValue;
			}
			else if(keyID < KeyCount)
			{
				return Keys[keyID];
			}
			return Keys[Keys.Count - 1];
		}
		
		public void Read(BinaryReader bStream, int TrackType)
		{
			if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
			{
				FixedValue.Read(bStream, 0);
			}
			else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
			{
				KeyCount = bStream.ReadInt32();
				for(int i = 0; i < KeyCount; i++)
				{
					int frameNum = bStream.ReadInt32();
					var tmpKey = new Vec4Key_Rotation();
					tmpKey.Read(bStream, frameNum);
					if(Keys.Count > 0)
					{
						var lastKeyFrame = Keys[Keys.Count - 1];
						if(lastKeyFrame.FrameNumber() == frameNum)
						{
							//Kill duplicated keyframe
							//TODO: Vec4Unk_Track: Properly handle duplicate keyframes
							Keys.Remove(lastKeyFrame);
						}
					}
					Keys.Add(tmpKey);
					if(Keys.Count > 1)
					{
						int LastKeyID = Keys.Count - 1;
						Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
					}
				}
				int LastKeyID = Keys.Count - 1;
				Keys[LastKeyID].SetFrameCount(FrameC
				//TODO: Vec4Unk_Track::Read(): Warn about unkown track type.
			}
		}
		
        public Quaternion GetInterpolatedValue(int frameNumber, int frameCount)
        {
        	if(KeyCount == 0) return FixedValue.Value();
        	
        	var CurrentValue = GetValue(CurrentKey);
        	var NextValue = GetValue(CurrentKey + 1);
        	if(frameNumber >= NextValue.FrameNumber())
        	{        		
        		CurrentKey += 1;
        		if(CurrentKey > (KeyCount - 1)) CurrentKey = 0;
        		CurrentValue = GetValue(CurrentKey);
        		NextValue = GetValue(CurrentKey + 1);
        	}
        	
        	float range = 1.0f / Util.GetRangeOfFrames(CurrentValue.FrameNumber(), NextValue.FrameNumber(), frameCount);
        	int percent = frameNumber - CurrentValue.FrameNumber();
        	return Quaternion.Slerp(CurrentValue.Value(), NextValue.Value(), range *percent); //Util.Slerp(range * percent, CurrentValue.Value(), NextValue.Value());
        }
	}

    public class Vec4Color_Track
    {
        public List<Vec4Key_Color> Keys = new List<Vec4Key_Color>();
        public Vec4Key_Color FixedValue = new Vec4Key_Color();
        int KeyCount = 0;

        public Vec4Key_Color GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Color Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec4Key_Color();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec4Key_Color lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec4Color_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            //TODO: Vec4Color_Track::Read(): Warn about unkown track type.
        }
    }

    public class Vec3Rotation_Track
    {
        public List<Vec3Key_Rotation> Keys = new List<Vec3Key_Rotation>();
        public Vec3Key_Rotation FixedValue = new Vec3Key_Rotation();
        int KeyCount = 0;
        int CurrentKey = 0;
        
        public Vec3Key_Rotation GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Rotation Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec3Key_Rotation();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec3Key_Rotation lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec3Rotation_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            //TODO: Vec3Rotation_Track::Read(): Warn about unkown track type.
        }
    
        
        public Vector3 GetInterpolatedValue(int frameNumber, int frameCount)
        {
        	if(KeyCount == 0) return FixedValue.Value();
        
			        	
        	//var CurrentValue = GetValue(CurrentKey);
        	//var NextValue = GetValue(CurrentKey + 1);
        	//if(frameNumber >= NextValue.FrameNumber())
        	//{        		
        	//	CurrentKey += 1;
        	//	if(CurrentKey > (KeyCount - 1)) CurrentKey = 0;
        	//	CurrentValue = GetValue(CurrentKey);
        	//	NextValue = GetValue(CurrentKey + 1);
        	//}
        	
        	//float range = 1.0f / Util.GetRangeOfFrames(CurrentValue.FrameNumber(), NextValue.FrameNumber(), frameCount);
        	//int percent = frameNumber - CurrentValue.FrameNumber();
        	//return Util.Slerp(range * percent, CurrentValue.Value(), NextValue.Value());
        	
        	
        	//TODO: Just hold a reference to next key value?
        	
        	int NextKey = CurrentKey + 1;
        	if(NextKey > (KeyCount - 1)) NextKey = 0;
        	
        	var CurrentValue = GetValue(CurrentKey);
        	var NextValue = GetValue(NextKey);
        	
        	
        	if(frameNumber > NextValue.FrameNumber())
        	{
        		CurrentKey = NextKey;
        		NextKey = CurrentKey + 1;
        		
        	}
        	
        	
        }
    }
    

    public class Vec3Position_Track
    {
        public List<Vec3Key_Position> Keys = new List<Vec3Key_Position>();
        public Vec3Key_Position FixedValue = new Vec3Key_Position();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec3Key_Position GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Position Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec3Key_Position();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec3Key_Position lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec3Position_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            
            //TODO: Vec3Position_Track::Read(): Warn about unkown track type.
        }
        
        public Vector3 GetInterpolatedValue(int frameNumber, int frameCount)
        {
        	if(KeyCount == 0) return FixedValue.Value();
        	
        	var CurrentValue = GetValue(CurrentKey);
        	var NextValue = GetValue(CurrentKey + 1);
        	if(frameNumber > NextValue.FrameNumber())
        	{
        		if(CurrentKey > (KeyCount - 1)) CurrentKey = 0;
        		CurrentValue = GetValue(CurrentKey);
        		NextValue = GetValue(CurrentKey + 1);
        	}
        	
        	float range = 1.0f / Util.GetRangeOfFrames(CurrentValue.FrameNumber(), NextValue.FrameNumber(), frameCount);
        	int percent = frameNumber - CurrentValue.FrameNumber();
        	return Vector3.Lerp(CurrentValue.Value(), NextValue.Value(), range * percent);
        }
    }

    public class Vec3Scale_Track
    {
        public List<Vec3Key_Scale> Keys = new List<Vec3Key_Scale>();
        public Vec3Key_Scale FixedValue = new Vec3Key_Scale();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec3Key_Scale GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Scale Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec3Key_Scale();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec3Key_Scale lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec3Scale_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            //TODO: Vec3Scale_Track::Read(): Warn about unkown track type.
        }
        
        public Vector3 GetInterpolatedValue(int frameNumber, int frameCount)
        {
        	if(KeyCount == 0) return FixedValue.Value();
        	
        	var CurrentValue = GetValue(CurrentKey);
        	var NextValue = GetValue(CurrentKey + 1);
        	if(CurrentKey > NextValue.FrameNumber())
        	{
        		CurrentKey += 1;
        		if(CurrentKey > (KeyCount - 1)) CurrentKey = 0;
        		CurrentValue = GetValue(CurrentKey);
        		NextValue = GetValue(CurrentKey + 1);
        	}
        	
        	float range = 1.0f / Util.GetRangeOfFrames(CurrentValue.FrameNumber, NextValue.FrameNumber, frameCount);
        	int percent = frameNumber - CurrentValue.FrameNumber();
        	return Vector3.Lerp(CurrentValue.Value(), NextValue.Value(), range * percent);
        }
        
    }

    public class Vec2UV_Track
    {
        public List<Vec2Key_UV> Keys = new List<Vec2Key_UV>();
        public Vec2Key_UV FixedValue = new Vec2Key_UV();
        int KeyCount = 0;

        public Vec2Key_UV GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Texture Offset Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec2Key_UV();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec2Key_UV lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec2UV_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            //TODO: Vec2UV_Track::Read(): Warn about unkown track type.
        }
    }

    public class F32_Track
    {
        public List<F32Key> Keys = new List<F32Key>();
        public F32Key FixedValue = new F32Key();
        int KeyCount = 0;

        public F32Key GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Float Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new F32Key();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        F32Key lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: F32_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            //TODO: F32_Track::Read(): Warn about unkown track type.
        }
    }

    public class Int32_Track
    {
        public List<Int32Key> Keys = new List<Int32Key>();
        public Int32Key FixedValue = new Int32Key();
        int KeyCount = 0;

        public Int32Key GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    //Debug.WriteLine("\t\tReading Int Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Int32Key();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Int32Key lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Int32_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    Keys.Add(tmpKey);
                }
            }
            //TODO: Int32_Track::Read(): Warn about unkown track type.
        }
    }

#endregion
*/

#region Auto-Generated with meta.py
    public class Vec4Color_Track
    {
        public List<Vec4Key_Color> Keys = new List<Vec4Key_Color>();
        public Vec4Key_Color FixedValue = new Vec4Key_Color();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec4Key_Color GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Color Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec4Key_Color();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec4Key_Color lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec4Color_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Vec4Color_Track::Read(): Warn about unkown track type.
        }

        public Vector4 GetInterpolatedValue(int frameNumber)
        {
            //Placeholder
            //TODO: Vec4Color_Track::GetInterpolatedValue();
            if(KeyCount == 0) return FixedValue.Value();

            if(frameNumber == 0) CurrentKey = 0;
            
            return GetValue(CurrentKey).Value();
        }
    }

    public class Vec3Rotation_Track
    {
        public List<Vec3Key_Rotation> Keys = new List<Vec3Key_Rotation>();
        public Vec3Key_Rotation FixedValue = new Vec3Key_Rotation();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec3Key_Rotation GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Rotation Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec3Key_Rotation();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec3Key_Rotation lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec3Rotation_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Vec3Rotation_Track::Read(): Warn about unkown track type.
        }

        public Vector3 GetInterpolatedValue(int frameNumber)
        {
            if(KeyCount == 0) return FixedValue.Value();
            
            if(frameNumber == 0) CurrentKey = 0;
            
            var CurrentValue = GetValue(CurrentKey);
            if(frameNumber >= CurrentValue.GetFrameCount())
            {
            	CurrentKey += 1;
            	if(CurrentKey > (Keys.Count - 1)) CurrentKey = 0;
            }
            CurrentValue = GetValue(CurrentKey);
            
            int NextKey = CurrentKey + 1;
            if(NextKey > (Keys.Count - 1))
            {
            	
            	NextKey = 0;
            }
            var NextValue = GetValue(NextKey);
            
            float range = 1.0f / (CurrentValue.GetFrameCount() - CurrentValue.FrameNumber());
            int percent = frameNumber - CurrentValue.FrameNumber();
            //return Util.Slerp(range * percent, CurrentValue.Value(), NextValue.Value());
            //return Util.V3Slerp(range * percent, CurrentValue.Value(), NextValue.Value());
            
            //TODO: Fix vector3 slerping
            return Vector3.Lerp(CurrentValue.Value(), NextValue.Value(), range * percent);
            

        }
    }

    public class Vec4Rotation_Track
    {
        public List<Vec4Key_Rotation> Keys = new List<Vec4Key_Rotation>();
        public Vec4Key_Rotation FixedValue = new Vec4Key_Rotation();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec4Key_Rotation GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Rotation4 Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec4Key_Rotation();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec4Key_Rotation lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec4Key_Rotation: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Vec4Key_Rotation::Read(): Warn about unkown track type.
        }

        public Quaternion GetInterpolatedValue(int frameNumber)
        {
            //Placeholder
            //TODO: Vec4Key_Rotation::GetInterpolatedValue();
            if(KeyCount == 0) return FixedValue.Value();

            if(frameNumber == 0) CurrentKey = 0;
            
            return GetValue(CurrentKey).Value();
        }
    }

    public class Vec3Position_Track
    {
        public List<Vec3Key_Position> Keys = new List<Vec3Key_Position>();
        public Vec3Key_Position FixedValue = new Vec3Key_Position();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec3Key_Position GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Position Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec3Key_Position();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec3Key_Position lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec3Position_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Vec3Position_Track::Read(): Warn about unkown track type.
        }

        public Vector3 GetInterpolatedValue(int frameNumber)
        {
            //Placeholder
            //TODO: Vec3Position_Track::GetInterpolatedValue();
            if(KeyCount == 0) return FixedValue.Value();

            if(frameNumber == 0) CurrentKey = 0;
            
            return GetValue(CurrentKey).Value();
        }
    }

    public class Vec3Scale_Track
    {
        public List<Vec3Key_Scale> Keys = new List<Vec3Key_Scale>();
        public Vec3Key_Scale FixedValue = new Vec3Key_Scale();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec3Key_Scale GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Scale Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec3Key_Scale();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec3Key_Scale lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec3Scale_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Vec3Scale_Track::Read(): Warn about unkown track type.
        }

        public Vector3 GetInterpolatedValue(int frameNumber)
        {
            if(KeyCount == 0) return FixedValue.Value();

            if(frameNumber == 0) CurrentKey = 0;
            
            var CurrentValue = GetValue(CurrentKey);
            if(frameNumber >= CurrentValue.GetFrameCount())
            {
            	CurrentKey += 1;
            	if(CurrentKey > (Keys.Count - 1)) CurrentKey = 0;
            }
            CurrentValue = GetValue(CurrentKey);
            
            int NextKey = CurrentKey + 1;
            if(NextKey > (Keys.Count - 1))
            {
            	NextKey = 0;
            }
            var NextValue = GetValue(NextKey);
            
            float range = 1.0f / (CurrentValue.GetFrameCount() - CurrentValue.FrameNumber());
            int percent = frameNumber - CurrentValue.FrameNumber();
            
            return Vector3.Lerp(CurrentValue.Value(), NextValue.Value(), range * percent);
        }
    }

    public class Vec2UV_Track
    {
        public List<Vec2Key_UV> Keys = new List<Vec2Key_UV>();
        public Vec2Key_UV FixedValue = new Vec2Key_UV();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Vec2Key_UV GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Texture Offset Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Vec2Key_UV();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Vec2Key_UV lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Vec2UV_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Vec2UV_Track::Read(): Warn about unkown track type.
        }

        public Vector2 GetInterpolatedValue(int frameNumber)
        {
            //Placeholder
            //TODO: Vec2UV_Track::GetInterpolatedValue();
            if(KeyCount == 0) return FixedValue.Value();

            if(frameNumber == 0) CurrentKey = 0;
            
            return GetValue(CurrentKey).Value();
        }
    }

    public class F32_Track
    {
        public List<F32Key> Keys = new List<F32Key>();
        public F32Key FixedValue = new F32Key();
        int KeyCount = 0;
        int CurrentKey = 0;

        public F32Key GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Float Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new F32Key();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        F32Key lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: F32_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount-1);
            }
            //TODO: F32_Track::Read(): Warn about unkown track type.
        }

        public float GetInterpolatedValue(int frameNumber)
        {
            //Placeholder
            //TODO: F32_Track::GetInterpolatedValue();
            if(KeyCount == 0) return FixedValue.Value();

            if(frameNumber == 0) CurrentKey = 0;
            
            return GetValue(CurrentKey).Value();
        }
        
        public float GetNonInterpolatedValue(int frameNumber)
        {
        	if(KeyCount == 0) return FixedValue.Value();
        	
        	//Fix for broken UV animation that for some reason ends before it should...
        	if(frameNumber == 0) CurrentKey = 0;
        	
        	var CurrentValue = GetValue(CurrentKey);
        	if(frameNumber >= CurrentValue.GetFrameCount())
        	{
        		CurrentKey += 1;
        		if(CurrentKey > (Keys.Count - 1)) CurrentKey = 0;
        	}
        	CurrentValue = GetValue(CurrentKey);
        	
        	return CurrentValue.Value();
        }
    }

    public class Int32_Track
    {
        public List<Int32Key> Keys = new List<Int32Key>();
        public Int32Key FixedValue = new Int32Key();
        int KeyCount = 0;
        int CurrentKey = 0;

        public Int32Key GetValue(int keyID)
        {
            if(KeyCount == 0)
            {
                return FixedValue;
            }
            else if(keyID < KeyCount)
            {
                return Keys[keyID];
            }
            return Keys[Keys.Count - 1];
        }

        public void Read(BinaryReader bStream, int TrackType, int frameCount)
        {
            if(TrackType == CCS_ANIME_CONTROLLER_TYPE_FIXED)
            {
                FixedValue.Read(bStream, 0);
            }
            else if(TrackType == CCS_ANIME_CONTROLLER_TYPE_ANIMATED)
            {
                KeyCount = bStream.ReadInt32();
                for(int i = 0; i < KeyCount; i++)
                {
                    int frameNum = bStream.ReadInt32();
                    Debug.WriteLine("\t\tReading Int Track Key {0} of {1}, Frame: {2}", i, KeyCount, frameNum);
                    var tmpKey = new Int32Key();
                    tmpKey.Read(bStream, frameNum);
                    if(Keys.Count > 0)
                    {
                        Int32Key lastKeyframe = Keys[Keys.Count - 1];
                        if(lastKeyframe.FrameNumber() == frameNum)
                        {
                            //kill duplicate keyframe...
                            //TODO: Int32_Track: Properly handle duplicate keyframes
                            Keys.Remove(lastKeyframe);
                        }
                    }
                    if(Keys.Count > 0)
                    {
                        int LastKeyID = Keys.Count - 1;
                        Keys[LastKeyID].SetFrameCount(tmpKey.FrameNumber());
                    }
                    Keys.Add(tmpKey);
                }
                int FinalKeyID = Keys.Count - 1;
                Keys[FinalKeyID].SetFrameCount(frameCount);
            }
            //TODO: Int32_Track::Read(): Warn about unkown track type.
        }

        public int GetInterpolatedValue(int frameNumber)
        {
            //Placeholder
            //TODO: Int32_Track::GetInterpolatedValue();
            if(KeyCount == 0) return FixedValue.Value();
            
            if(frameNumber == 0) CurrentKey = 0;

            return GetValue(CurrentKey).Value();
        }
    }

#endregion

	}
}
