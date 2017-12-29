/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/25/2017
 * Time: 3:45 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Diagnostics;
using OpenTK;

namespace StudioCCS.libCCS
{
	public partial class CCSAnime : CCSBaseObject
	{
		public abstract class Controller
		{
			public int ControllerType;
			public int ControllerParams;
			public int ObjectID;
			public CCSFile ParentFile = null;
			public CCSAnime ParentAnime = null;
			
			public abstract bool Read(BinaryReader bStream, int dataSize);
			
			public int GetControllerType()
			{
				return ControllerType;
			}
			
			public int GetControllerParams()
			{
				return ControllerParams;
			}
			
			public int GetObjectID()
			{
				return ObjectID;
			}
			
			public int GetTrackParams(int trackID)
			{
				if(trackID > 10) return 0;
				return ((ControllerParams >> (3 * trackID)) & 0x7);
			}
			
			//TODO: CCSAnime.Controller: SHOULD set frame actually set the values?
			public abstract void SetFrame(int frameNum);
			
			public abstract string GetReport(int level = 0);
			
			public abstract void DumpToText(StreamWriter outf);
		}
		
		
		public class ObjectController : Controller
		{
			public Vec3Position_Track PositionTrack = new Vec3Position_Track();
			public Vec3Rotation_Track RotationTrack = new Vec3Rotation_Track();
			public Vec3Scale_Track ScaleTrack = new Vec3Scale_Track();
			public F32_Track AlphaTrack = new F32_Track();
			
			public ObjectController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				ControllerType = CCS_ANIME_OBJECT_CONTROLLER;
				ParentFile = _parentFile;
				ParentAnime = _parentAnime;
			}
			
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				//Debug.WriteLine("Reading Object Controller at {0:X}", bStream.BaseStream.Position);
				ObjectID = bStream.ReadInt32();
				ControllerParams = bStream.ReadInt32();
				
				//Debug.WriteLine("\tReading Position Track at 0x{1:X} type {0}",GetTrackParams(0), bStream.BaseStream.Position);
				PositionTrack.Read(bStream, GetTrackParams(0), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading Rotation Track at 0x{1:X} type {0}",GetTrackParams(1), bStream.BaseStream.Position);
				RotationTrack.Read(bStream, GetTrackParams(1), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading Scale Track at 0x{1:X} type {0}",GetTrackParams(2), bStream.BaseStream.Position);
				ScaleTrack.Read(bStream, GetTrackParams(2), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading Alpha Track at 0x{1:X} type {0}",GetTrackParams(3), bStream.BaseStream.Position);
				AlphaTrack.Read(bStream, GetTrackParams(3), ParentAnime.FrameCount);
				
				return true;
			}
			
			public override void SetFrame(int frameNum)
			{
				//TODO: this is kinda lame.
				//TODO: Interpolation
				//TODO: Game just delegates these out to individual frames. Should we do that too?
				
				Vec3Key_Scale tmpScale = null;
				F32Key tmpAlpha = null;
				
				foreach(var tmpKey in ScaleTrack.Keys)
				{
					if(tmpKey.FrameNumber() > frameNum) break;
					tmpScale = tmpKey;
				}
				
				foreach(var tmpKey in AlphaTrack.Keys)
				{
					if(tmpKey.FrameNumber() > frameNum) break;
					tmpAlpha = tmpKey;
				}
				
				//TODO: Move this to an init?
				var tmpExt = ParentFile.GetObject<CCSExt>(ObjectID);
				if(tmpExt != null)
				{
					var tmpObj = ParentFile.GetObject<CCSObject>(tmpExt.ReferencedObjectID);
					if(tmpObj != null)
					{
						Vector3 pK = PositionTrack.GetInterpolatedValue(frameNum);
						Vector3 rK = RotationTrack.GetInterpolatedValue(frameNum);
						Vector3 sK = ScaleTrack.GetInterpolatedValue(frameNum);
						
						//if(tmpScale != null) sK = tmpScale.Value();
						tmpObj.SetPose(pK, rK, sK);
						//TODO: CCSAnime.ObjectController::SetFrame(): Set frame rebinds the matrix buffer each frame. that's gonna be slow?
						//tmpObj.Alpha = tmpAlpha.Value();
					}
				}
				
			}
			
			public override void DumpToText(StreamWriter outf)
			{
				var tmpExt = ParentFile.GetObject<CCSExt>(ObjectID);
				if(tmpExt != null)
				{
					var tmpObj = ParentFile.GetObject<CCSObject>(tmpExt.ReferencedObjectID);
					if(tmpObj != null)
					{
						if(tmpObj.ModelID != 0)
						{
							outf.WriteLine(ParentFile.GetSubObjectName(tmpObj.ModelID));
							Vector3 tmpP = PositionTrack.GetValue(0).Value();
							Vector3 tmpR = RotationTrack.GetValue(0).Value();
							Vector3 tmpS = ScaleTrack.GetValue(0).Value();
							
							var fmtStr = "{0} {1} {2}";
							var pStr = string.Format(fmtStr, tmpP.X, tmpP.Y, tmpP.Z);
							var rStr = string.Format(fmtStr, tmpR.X, tmpR.Y, tmpR.Z);
							var sStr = string.Format(fmtStr, tmpS.X, tmpS.Y, tmpS.Z);
							
							outf.WriteLine(string.Format(fmtStr, pStr, rStr, sStr));
						}
					}
				}
			}
			
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level) + string.Format("Object Controller for object 0x{0:X}, {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			}
			
		}

		public class ObjectController_Gen2 : Controller
		{
			public Vec3Position_Track PositionTrack = new Vec3Position_Track();
			public Vec3Rotation_Track RotationTrack = new Vec3Rotation_Track();
			public Vec3Scale_Track ScaleTrack = new Vec3Scale_Track();
			public F32_Track AlphaTrack = new F32_Track();
			public Vec4Rotation_Track Rotation4Track = new Vec4Rotation_Track();
			
			public ObjectController_Gen2(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				ControllerType = CCS_ANIME_OBJECT_CONTROLLER;
				ParentFile = _parentFile;
				ParentAnime = _parentAnime;
			}
			
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				//Debug.WriteLine("Reading Object Controller at {0:X}", bStream.BaseStream.Position);
				ObjectID = bStream.ReadInt32();
				ControllerParams = bStream.ReadInt32();
				
				//Debug.WriteLine("\tReading Position Track at 0x{1:X} type {0}",GetTrackParams(0), bStream.BaseStream.Position);
				PositionTrack.Read(bStream, GetTrackParams(0), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading Rotation Track at 0x{1:X} type {0}",GetTrackParams(1), bStream.BaseStream.Position);
				int rotTackPos = (int)bStream.BaseStream.Position;
				RotationTrack.Read(bStream, GetTrackParams(1), ParentAnime.FrameCount);
				/*
				if(RotationTrack.Keys.Count > 0)
				{
					string trackTypeStr = "Unknown";
					int trackType = GetTrackParams(1);
					if(trackType == CCSAnime.CCS_ANIME_CONTROLLER_TYPE_ANIMATED) trackTypeStr = "Animated";
					else if(trackType == CCSAnime.CCS_ANIME_CONTROLLER_TYPE_FIXED) trackTypeStr = "Fixed";
					else if(trackType == CCSAnime.CCS_ANIME_CONTROLLER_TYPE_NONE) trackTypeStr = "None";
					Debug.WriteLine(string.Format("Found Euler Angle Rotation in a Gen2/3 Anime, Type: {0} at 0x{1:X} for obj {2}", trackTypeStr, rotTackPos, ParentFile.GetSubObjectName(ObjectID)));
				}
				*/
				//Debug.WriteLine("\tReading Scale Track at 0x{1:X} type {0}",GetTrackParams(2), bStream.BaseStream.Position);
				//TODO: CCSAnime.ObjectController_Gen2::Read(): Gen2 extra data.
				Rotation4Track.Read(bStream, GetTrackParams(8), ParentAnime.FrameCount);
				ScaleTrack.Read(bStream, GetTrackParams(2), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading Alpha Track at 0x{1:X} type {0}",GetTrackParams(3), bStream.BaseStream.Position);
				AlphaTrack.Read(bStream, GetTrackParams(3), ParentAnime.FrameCount);
				//Debug.WriteLine("Reading ObjectController_Gen2::Vec4Unkown_Track at {0:X}\n", bStream.BaseStream.Position);

				
				return true;
			}
			
			public override void SetFrame(int frameNum)
			{
				//TODO: this is kinda lame.
				//TODO: Interpolation
				//TODO: Game just delegates these out to individual frames. Should we do that too?
				Vec3Key_Scale tmpScale = null;
				F32Key tmpAlpha = null;
				
				
				foreach(var tmpKey in ScaleTrack.Keys)
				{
					if(tmpKey.FrameNumber() > frameNum) break;
					tmpScale = tmpKey;
				}
				
				foreach(var tmpKey in AlphaTrack.Keys)
				{
					if(tmpKey.FrameNumber() > frameNum) break;
					tmpAlpha = tmpKey;
				}
				
				//TODO: Move this to an init?
				var tmpExt = ParentFile.GetObject<CCSExt>(ObjectID);
				if(tmpExt != null)
				{
					var tmpObj = ParentFile.GetObject<CCSObject>(tmpExt.ReferencedObjectID);
					if(tmpObj != null)
					{
						Vector3 pK = PositionTrack.GetInterpolatedValue(frameNum);
						Quaternion rK = Quaternion.Identity;
						//rK.W = -rK.W;
						Vector3 sK = Vector3.One;
						float aK = 1.0f;
						
						//rK = Rotation4Track.GetInterpolatedValue(frameNum, ParentAnime.FrameCount); //tmpRot.Value();
						rK = Quaternion.FromEulerAngles(RotationTrack.GetInterpolatedValue(frameNum));
						
						
						if(tmpScale != null) sK = tmpScale.Value();
						if(tmpAlpha != null) aK = tmpAlpha.Value();
						
						tmpObj.SetPose(pK, rK, sK);
						tmpObj.Alpha = aK;
					}
				}
			
			}
			
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level) + string.Format("Object Controller for object 0x{0:X}, {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			}
			
			public override void DumpToText(StreamWriter outf)
			{
				var tmpExt = ParentFile.GetObject<CCSExt>(ObjectID);
				if(tmpExt != null)
				{
					var tmpObj = ParentFile.GetObject<CCSObject>(tmpExt.ReferencedObjectID);
					if(tmpObj != null)
					{
						if(tmpObj.ModelID != 0)
						{
							outf.WriteLine(ParentFile.GetSubObjectName(tmpObj.ModelID));
							Vector3 tmpP = PositionTrack.GetValue(0).Value();
							Vector3 tmpR = RotationTrack.GetValue(0).Value();
							Vector3 tmpS = ScaleTrack.GetValue(0).Value();
							
							var fmtStr = "{0} {1} {2}";
							var pStr = string.Format(fmtStr, tmpP.X, tmpP.Y, tmpP.Z);
							var rStr = string.Format(fmtStr, tmpR.X, tmpR.Y, tmpR.Z);
							var sStr = string.Format(fmtStr, tmpS.X, tmpS.Y, tmpS.Z);
							
							outf.WriteLine(string.Format(fmtStr, pStr, rStr, sStr));
						}
					}
				}
			}
		}
		
		public class MaterialController : Controller
		{
			//public Vec2UV_Track TextureOffsetTrack = new Vec2UV_Track();
			public F32_Track UOffsetTrack = new F32_Track();
			public F32_Track VOffsetTrack = new F32_Track();
			//Only seen in Gen2/3?
			public F32_Track UnkFTrack = new F32_Track();
			public F32_Track UnkFTrack2 = new F32_Track();
			
			public MaterialController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				ControllerType = CCS_ANIME_MATERIAL_CONTROLLER;
				ParentFile = _parentFile;
				ParentAnime = _parentAnime;
			}
			
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				ObjectID = bStream.ReadInt32();
				ControllerParams = bStream.ReadInt32();
				//TextureOffsetTrack.Read(bStream, GetTrackParams(0), ParentAnime.FrameCount);
				UOffsetTrack.Read(bStream, GetTrackParams(0), ParentAnime.FrameCount);
				VOffsetTrack.Read(bStream, GetTrackParams(1), ParentAnime.FrameCount);
				
				//Only seen in Gen2/3?
				UnkFTrack.Read(bStream, GetTrackParams(2), ParentAnime.FrameCount);
				UnkFTrack2.Read(bStream, GetTrackParams(3), ParentAnime.FrameCount);
				return true;
			}
			
			public override void SetFrame(int frameNum)
			{
				var material = ParentFile.GetObject<CCSMaterial>(ObjectID);
				if(material != null)
				{
					float uValue = UOffsetTrack.GetNonInterpolatedValue(frameNum);
					float vValue = VOffsetTrack.GetNonInterpolatedValue(frameNum);
					material.TextureOffset = new Vector2(uValue, vValue);
				}
			}
			
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level) + string.Format("Material Controller for material 0x{0:X}, {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			}
			
			public override void DumpToText(StreamWriter outf)
			{
				
			}
		}
		
		public class DirectionalLightController : Controller
		{
			public Vec3Rotation_Track DirectionTrack = new Vec3Rotation_Track();
			public Vec4Color_Track ColorTrack = new Vec4Color_Track();
			public F32_Track UnkTrack = new F32_Track();
			
			public DirectionalLightController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				ControllerType = CCS_ANIME_LIGHT_DIRECTIONAL_CONTROLLER;
				ParentFile = _parentFile;
				ParentAnime = _parentAnime;
			}
			
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				ObjectID = bStream.ReadInt32();
				ControllerParams = bStream.ReadInt32();
				DirectionTrack.Read(bStream, GetTrackParams(1), ParentAnime.FrameCount);
				ColorTrack.Read(bStream, GetTrackParams(2), ParentAnime.FrameCount);
				UnkTrack.Read(bStream, GetTrackParams(0), ParentAnime.FrameCount);
				
				return true;
			}
			
			public override void SetFrame(int frameNum)
			{
				//throw new NotImplementedException();
			}
			
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level) + string.Format("Directional Light Controller for light 0x{0:X}, {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			}
			
			public override void DumpToText(StreamWriter outf)
			{
				
			}
		}
		
		public class OmniLightController : Controller
		{
			public Vec3Position_Track PositionTrack = new Vec3Position_Track();
			public Vec4Color_Track ColorTrack = new Vec4Color_Track();
			public F32_Track UnkF32Track1 = new F32_Track();
			public F32_Track UnkF32Track2 = new F32_Track();
			public F32_Track UnkF32Track3 = new F32_Track();
			
			public OmniLightController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				ControllerType = CCS_ANIME_LIGHT_OMNI_CONTROLLER;
				ParentFile = _parentFile;
				ParentAnime = _parentAnime;
			}
			
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				//Debug.WriteLine("Reading OmniLight Controller");
				ObjectID = bStream.ReadInt32();
				ControllerParams = bStream.ReadInt32();
				//Debug.WriteLine("\tReading Position Track at 0x{1:X} type {0}",GetTrackParams(0), bStream.BaseStream.Position);
				PositionTrack.Read(bStream, GetTrackParams(0), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading Color Track at 0x{1:X} type {0}",GetTrackParams(2), bStream.BaseStream.Position);
				ColorTrack.Read(bStream, GetTrackParams(2), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading UnkF32-0 Track at 0x{1:X} type {0}",GetTrackParams(3), bStream.BaseStream.Position);
				UnkF32Track1.Read(bStream, GetTrackParams(3), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading UnkF32-1 Track at 0x{1:X} type {0}",GetTrackParams(4), bStream.BaseStream.Position);
				UnkF32Track2.Read(bStream, GetTrackParams(4), ParentAnime.FrameCount);
				//Debug.WriteLine("\tReading UnkF32-2 Track at 0x{1:X} type {0}",GetTrackParams(5), bStream.BaseStream.Position);
				UnkF32Track3.Read(bStream, GetTrackParams(5), ParentAnime.FrameCount);
				
				return true;
			}
			
			public override void SetFrame(int frameNum)
			{
				//throw new NotImplementedException();
			}
			
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level) + string.Format("Omni Light Controller for light 0x{0:X}, {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			}
			
			public override void DumpToText(StreamWriter outf)
			{
				
			}
		}
		
	}
}
