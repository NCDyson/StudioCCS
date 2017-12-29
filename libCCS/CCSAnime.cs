/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/24/2017
 * Time: 5:24 PM
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
	/// Description of CCSAnime.
	/// </summary>
	public partial class CCSAnime : CCSBaseObject
	{
		//Useful defines
		//KNOWN ANIME/STREAM SUB-SECTIONS
		/// <summary>
		/// <c>CCS_ANIME_FRAME = 0xff01</c>
		/// </summary>
		public const int CCS_ANIME_FRAME = 0xff01;
		//CCSObject
		/// <summary>
		/// <c>CCS_ANIME_OBJECT_KEYFRAME = 0x0101</c>
		/// </summary>
		public const int CCS_ANIME_OBJECT_KEYFRAME = 0x0101;
		/// <summary>
		/// <c>CCS_ANIME_OBJECT_CONTROLLER = 0x0102</c>
		/// </summary>
		public const int CCS_ANIME_OBJECT_CONTROLLER = 0x0102;
		//CCSMaterial
		/// <summary>
		/// <c>CCS_ANIME_MATERIAL_CONTROLLER = 0x0202</c>
		/// </summary>
		public const int CCS_ANIME_MATERIAL_CONTROLLER = 0x0202;
		//CCSLight
		/// <summary>
		/// <c>CCS_ANIME_LIGHT_AMBIENT_KEYFRAME = 0x0601</c>
		/// </summary>
		public const int CCS_ANIME_LIGHT_AMBIENT_KEYFRAME = 0x0601;
		/// <summary>
		/// <c>CCS_ANIME_LIGHT_DIRECTIONAL_CONTROLLER = 0x0603</c>
		/// </summary>
		public const int CCS_ANIME_LIGHT_DIRECTIONAL_CONTROLLER = 0x0603;
		/// <summary>
		/// <c>CCS_ANIME_LIGHT_OMNI_CONTROLLER = 0x0609</c>
		/// </summary>
		public const int CCS_ANIME_LIGHT_OMNI_CONTROLLER = 0x0609;
		
		
		//CCSMoprher
		/// <summary>
		/// <c>CCS_ANIME_MOPRH_KEYFRAME = 0x1901</c>
		/// </summary>
		public const int CCS_ANIME_MORPH_KEYFRAME = 0x1901;
		
		//Misc Anime Defines
		public const int CCS_ANIME_PLAY_ONCE = -1;
		public const int CCS_ANIME_PLAY_REPEAT = -2;
		public const int CCS_ANIME_CONTROLLER_TYPE_NONE = 0;
		public const int CCS_ANIME_CONTROLLER_TYPE_FIXED = 1;
		public const int CCS_ANIME_CONTROLLER_TYPE_ANIMATED = 2;

		//Fields
		public List<Controller> Controllers = new List<Controller>();
		public List<AnimationFrame> Frames = new List<AnimationFrame>();
		public int FrameCount = 0;
		public int PlaybackType = 0;
		public int CurrentFrame = 0;
		public bool HasEnded = false;
		
		public CCSAnime(int _objectID, CCSFile _parentFile)
		{
			ObjectID = _objectID;
			ParentFile = _parentFile;
			ObjectType = CCSFile.SECTION_ANIME;
		}

		public override bool Init()
		{
			throw new NotImplementedException();
		}

		public override bool DeInit()
		{
			throw new NotImplementedException();
		}

		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			Debug.WriteLine("Reading Anime {0} at 0x{1:X}...", ParentFile.GetSubObjectName(ObjectID), bStream.BaseStream.Position);
			FrameCount = bStream.ReadInt32();
			var currentFrame = new AnimationFrame(0);
			Frames.Add(currentFrame);
			int restBlockSize = bStream.ReadInt32();
			
			while(true)
			{
				int blockOffset = (int)bStream.BaseStream.Position;
				int blockType = bStream.ReadInt32() & 0xFFFF;
				int blockSize = bStream.ReadInt32();
				//Debug.WriteLine("Reading Anime Block Type {0:X} at {1:X}", blockType, blockOffset);
				
				if(blockType == CCS_ANIME_FRAME)
				{
					int newFrameNumber = bStream.ReadInt32();
					if(newFrameNumber == CCS_ANIME_PLAY_ONCE || newFrameNumber == CCS_ANIME_PLAY_REPEAT)
					{
						PlaybackType = newFrameNumber;
						break;
					}
					else
					{
						currentFrame = new AnimationFrame(newFrameNumber);
						Frames.Add(currentFrame);
					}
					continue;
				}
				//Debug.WriteLine("Reading Block type {0:X} at 0x{1:X}", blockType, blockOffset);
				switch (blockType)
				{
					//---------Object
					case CCS_ANIME_OBJECT_CONTROLLER:
					{
						if(ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1)
						{
							var tmpController = new ObjectController(ParentFile, this);
							tmpController.Read(bStream, blockSize);
							Controllers.Add(tmpController);
							break;
						}
						else if(ParentFile.GetVersion() != CCSFileHeader.CCSVersion.Gen1)
						{
							var tmpController = new ObjectController_Gen2(ParentFile, this);
							tmpController.Read(bStream, blockSize);
							Controllers.Add(tmpController);
							break;
						}
						break;
					}
					//---------Material
					//TODO: Material Controller is two f32 fields, not 1 vec2 field	
					
					case CCS_ANIME_MATERIAL_CONTROLLER:
					{
						var tmpController = new MaterialController(ParentFile, this);
						tmpController.Read(bStream, blockSize);
						Controllers.Add(tmpController);
						break;
					}
					
					//---------Camera
					//---------Light
					case CCS_ANIME_LIGHT_AMBIENT_KEYFRAME:
					{
						var tmpKeyFrame = new AmbientLightKeyFrame();
						tmpKeyFrame.Read(bStream);
						currentFrame.KeyFrames.Add(tmpKeyFrame);
						break;
					}
					case CCS_ANIME_LIGHT_DIRECTIONAL_CONTROLLER:
					{
						var tmpController = new DirectionalLightController(ParentFile, this);
						tmpController.Read(bStream, blockSize);
						Controllers.Add(tmpController);
						break;
					}
					case CCS_ANIME_LIGHT_OMNI_CONTROLLER:
					{
						var tmpController = new OmniLightController(ParentFile, this);
						tmpController.Read(bStream, blockSize);
						Controllers.Add(tmpController);
						break;
					}
					//---------Model
					//---------Morpher
					case CCS_ANIME_MORPH_KEYFRAME:
					{
						var tmpKeyFrame = new MorphKeyFrame();
						tmpKeyFrame.Read(bStream);
						currentFrame.KeyFrames.Add(tmpKeyFrame);
						break;
					}
					default:
					{
						//Warn about unkown block type
						Logger.LogWarning(string.Format("CCSAnime::Read(): Skipped unknown animation controller or keyframe type {0:X} at 0x{1:X}\n", blockType, blockOffset));
						Util.SkipSection(bStream, blockSize);
						break;
					}
					
				}
			}
			
			
			return true;
		}
		
		public override TreeNode ToNode()
		{
			//TODO: CCSAnime::ToNode()
			return base.ToNode();
		
			
		
		}
		
		public void FrameForward()
		{
			//Don't bother calculating frames for animations that aren't even running
			if(HasEnded) return;
			CurrentFrame += 1;
			if(CurrentFrame >= (FrameCount - 1))
			//if(CurrentFrame >= FrameCount)
			{
				if(PlaybackType == CCS_ANIME_PLAY_ONCE)
				{
					CurrentFrame = 0;
					HasEnded = true;
					return;
				}
				else
				{
					CurrentFrame = 0;
				}
			}
			
			//SetFrame(CurrentFrame);
		}
		
		public void SetFrame(int frameNumber)
		{
			foreach(var tmpController in Controllers)
			{
				//TODO: CCSAnime::SetFrame(): Make controller key search more efficient
				tmpController.SetFrame(frameNumber);
			}
		}
		
		public string GetPlayTypeString()
		{
			if(PlaybackType == CCS_ANIME_PLAY_ONCE) return "Play Once";
			else if(PlaybackType == CCS_ANIME_PLAY_REPEAT) return "Repeat";
			
			return "Unknown";
		}
		
		public string GetReport(int level = 0)
		{
			string retVal = Util.Indent(level) + string.Format("Animation 0x{0:X}: {1}\n", ObjectID, ParentFile.GetSubObjectName(ObjectID));
			retVal += Util.Indent(level) + string.Format("{0} Frames, Type: {1}\n", FrameCount, GetPlayTypeString());
			
			
			foreach(var tmpController in Controllers)
			{
				retVal += tmpController.GetReport(level + 1);
			}
			
			
			return retVal;
		}
		
		public void Render(Matrix4 ProjView, int extraOptions = 0)
		{
			//SetFrame(1);
			/*
			CCSClump tmpClump = null;
			foreach(var tmpController in Controllers)
			{
				if(tmpController.ControllerType == CCS_ANIME_OBJECT_CONTROLLER)
				{
					var tmpExt = ParentFile.GetObject<CCSExt>(tmpController.ObjectID);
					if(tmpExt != null)
					{
						var tmpObj = ParentFile.GetObject<CCSObject>(tmpExt.ReferencedObjectID);
						if(tmpObj != null)
						{
							if(tmpObj.ParentClump != tmpClump)
							{
								if(tmpClump != null)
								{
									tmpClump.FrameForward();
									tmpClump.BindMatrixList();
									tmpClump.Render(ProjView, extraOptions);
								}
								tmpClump = tmpObj.ParentClump;
							}
							
							
							//tmpObj.Render(ProjView, extraOptions);
						}
						else
						{
							Logger.LogError(string.Format("Error finding Object 0x{0:X} for animation...", tmpExt.ReferencedObjectID), Logger.LogType.LogOnceValue);
						}
					}
				}
			}
			*/
			
			FrameForward();
			
			CCSClump boundClump = null;
			foreach(var tmpController in Controllers)
			{
				tmpController.SetFrame(CurrentFrame);
				var tmpExt = ParentFile.GetObject<CCSExt>(tmpController.ObjectID);
				if(tmpExt != null)
				{
					var tmpObj = ParentFile.GetObject<CCSObject>(tmpExt.ReferencedObjectID);
					if(tmpObj != null)
					{
						if(tmpObj.ParentClump != boundClump)
						{
							tmpObj.ParentClump.BindMatrixList();
							boundClump = tmpObj.ParentClump;
						}
						tmpObj.ParentClump.FrameForward();
						tmpObj.ParentClump.UpdateMatrixList();
						tmpObj.Render(ProjView, extraOptions);
					}
				}
			}
			
			
		}
		
		public void DumpToText(StreamWriter outf)
		{
			outf.WriteLine(ParentFile.GetSubObjectName(ObjectID));
			outf.WriteLine(Controllers.Count.ToString());
			foreach(var tmpController in Controllers)
			{
				tmpController.DumpToText(outf);
			}
		}

	}
}
