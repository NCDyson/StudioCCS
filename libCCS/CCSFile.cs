/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/21/2017
 * Time: 11:14 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace StudioCCS.libCCS
{
	/// <summary>
	/// Description of CCSFile.
	/// </summary>
	public class CCSFile
	{
        //Useful defines
        //Higher level sections
        public const int SECTION_HEADER = 0x0001;
        public const int SECTION_INDEX = 0x0002;
        public const int SECTION_SETUP = 0x0003;
        public const int SECTION_STREAM = 0x0005;

        //Lower level sections
        public const int SECTION_OBJECT = 0x0100;
        public const int SECTION_MATERIAL = 0x0200;
        public const int SECTION_TEXTURE = 0x0300;
        public const int SECTION_CLUT = 0x0400;
        public const int SECTION_CAMERA = 0x0500;
        public const int SECTION_LIGHT = 0x0600;
        public const int SECTION_ANIME = 0x0700;
        public const int SECTION_MODEL = 0x0800;
        public const int SECTION_CLUMP = 0x0900;
        public const int SECTION_EXTERNAL = 0x0a00;
        public const int SECTION_HITMESH = 0x0b00;
        public const int SECTION_BBOX = 0x0c00;
        public const int SECTION_PARTICLE = 0x0d00;
        public const int SECTION_EFFECT = 0x0e00;
        public const int SECTION_BLTGROUP = 0x1000;
        public const int SECTION_FBPAGE = 0x1100;
        public const int SECTION_FBRECT = 0x1200;
        public const int SECTION_DUMMYPOS = 0x1300;
        public const int SECTION_DUMMYPOSROT = 0x1400;
        public const int SECTION_LAYER = 0x1700;
        public const int SECTION_SHADOW = 0x1800;
        public const int SECTION_MORPHER = 0x1900;
        public const int SECTION_OBJECT2 = 0x2000;
        public const int SECTION_PCM = 0x2200;

        //Added in GU/LINK
        public const int SECTION_BINARYBLOB = 0x2400;
        
        public static Dictionary<int, string> ObjectTypeNames = new Dictionary<int, string>()
        {
        	{SECTION_HEADER, "Header"},
        	{SECTION_INDEX, "Index"},
        	{SECTION_SETUP, "Setup"},
        	{SECTION_STREAM, "Stream"},
        	{SECTION_OBJECT, "Object"},
        	{SECTION_MATERIAL, "Material"},
        	{SECTION_TEXTURE, "Texture"},
        	{SECTION_CLUT, "CLUT"},
        	{SECTION_CAMERA, "Camera"},
        	{SECTION_LIGHT, "Light"},
        	{SECTION_ANIME, "Animation"},
        	{SECTION_MODEL, "Model"},
        	{SECTION_CLUMP, "Clump"},
        	{SECTION_EXTERNAL, "External"},
        	{SECTION_HITMESH, "Hit Mesh"},
        	{SECTION_BBOX, "Bounding Box"},
        	{SECTION_PARTICLE, "Particle"},
        	{SECTION_EFFECT, "Effect"},
        	{SECTION_BLTGROUP, "Blit Group"},
        	{SECTION_FBPAGE, "FrameBuffer Page"},
        	{SECTION_FBRECT, "FrameBuffer Rect"},
        	{SECTION_DUMMYPOS, "Dummy(Position)"},
        	{SECTION_DUMMYPOSROT, "Dummy(Position & Rotation)"},
        	{SECTION_LAYER, "Layer"},
        	{SECTION_SHADOW, "Shadow"},
        	{SECTION_MORPHER, "Mopher"},
        	{SECTION_OBJECT2, "Object 2"},
        	{SECTION_PCM, "PCM Audio"},
        	{SECTION_BINARYBLOB, "Binary Blob"}
        };
        
        public CCSFileHeader Header = new CCSFileHeader();
        public IndexFileEntry[] FileIndex = null;
        public IndexObjectEntry[] ObjectIndex = null;
        public int FileIndexCount = 0;
        public int ObjectIndexCount = 0;
        public CCSFileHeader.CCSVersion Version;
        public int StreamFrameCount = 0;
        public int StreamOffset = 0;
        public string FileName = "";
        public bool NeedsInit = true;
        
        //Useful lists
        public List<CCSClump> ClumpList = new List<CCSClump>();
        public List<CCSMaterial> MaterialList = new List<CCSMaterial>();
        public List<CCSTexture> TextureList = new List<CCSTexture>();
        public List<CCSHitMesh> HitList = new List<CCSHitMesh>(); //Lol
        public List<CCSBoundingBox> BBoxList = new List<CCSBoundingBox>();
        public List<CCSDummy> DummyList = new List<CCSDummy>();
        public List<CCSAnime> AnimeList = new List<CCSAnime>();
        
        //OMGWTFHAX for static models
        public CCSClump LastClump = null;
        public CCSObject LastObject = null;
        
		public CCSFile()
		{
			
		}
		
		~CCSFile()
		{
			//I just really want to know, ok?
			Debug.WriteLine(string.Format("CCS File {0} was killed...", FileName));
		}
		
		public bool Init()
		{
			foreach(var tmpHit in HitList)
			{
				if(!tmpHit.Init()) return false;
			}
			
			
			foreach(var tmpClump in ClumpList)
			{
				if(!tmpClump.Init()) return false;
			}
			
			foreach(var tmpTexture in TextureList)
			{
				if(!tmpTexture.Init()) return false;
			}
			
			NeedsInit = false;
			return true;
		}
		
		public bool DeInit()
		{
			foreach(var tmpHit in HitList)
			{
				tmpHit.DeInit();
			}
			
			return true;
		}
		
		public bool Read(string _fileName)
		{
			FileName = _fileName;
			var fStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
			var bStream = new BinaryReader(fStream);
			Logger.LogInfo(string.Format("Loading {0}...\n", _fileName));
			bool result = Read(bStream);
			Logger.LogInfo("Done.\n");
			return result;
		}
		
		public bool Read(BinaryReader bStream)
		{
			var ErrorString = string.Format("Error reading CCS File {0}, ", FileName);
			if(!Header.Read(bStream))
			{
				Logger.LogError(ErrorString + "Header section could not be read\n");
				return false;
			}
			
			if(!ReadIndexSection(bStream))
			{
				Logger.LogError(ErrorString + "Index section could not be read\n");
				return false;
			}
			
			if(!ReadSetupSection(bStream))
			{
				Logger.LogError(ErrorString + "Setup section could not be read\n");
				return false;
			}
			
			if(!ReadStreamSection(bStream))
			{
				Logger.LogError(ErrorString + "Stream section could not be read\n");
				return false;
			}
			
			return true;
		}
		
		public TreeNode ToNode()
		{
			//In my preferred order
			var retNode = new TreeNode(Header.CCSFName)
			{
				Tag = new TreeNodeTag(this, 0, 0, TreeNodeTag.NodeType.File, 0)
			};
			
			//In this order, because that's how I feel like ordering them.
			var ClumpNode = new TreeNode("Clumps");
			foreach(var tmpClump in ClumpList)
			{
				ClumpNode.Nodes.Add(tmpClump.ToNode());
			}
			retNode.Nodes.Add(ClumpNode);
			
			var MaterialNode = new TreeNode("Materials");
			foreach(var tmpMat in MaterialList)
			{
				MaterialNode.Nodes.Add(tmpMat.ToNode());
			}
			retNode.Nodes.Add(MaterialNode);
			
			var TextureNode = new TreeNode("Textures");
			foreach(var tmpTex in TextureList)
			{
				TextureNode.Nodes.Add(tmpTex.ToNode());
			}
			retNode.Nodes.Add(TextureNode);
			
			var HitNode = new TreeNode("HitMeshes");
			foreach(var tmpHit in HitList)
			{
				HitNode.Nodes.Add(tmpHit.ToNode());
			}
			retNode.Nodes.Add(HitNode);
			
			var BBoxNode = new TreeNode("Bounding Boxes");
			foreach(var tmpBBox in BBoxList)
			{
				BBoxNode.Nodes.Add(tmpBBox.ToNode());
			}
			retNode.Nodes.Add(BBoxNode);
			
			var DummyNode = new TreeNode("Dummies");
			foreach(var tmpDummy in DummyList)
			{
				DummyNode.Nodes.Add(tmpDummy.ToNode());
			}
			retNode.Nodes.Add(DummyNode);
			
			var AnimeNode = new TreeNode("Animations");
			foreach(var tmpAnime in AnimeList)
			{
				AnimeNode.Nodes.Add(tmpAnime.ToNode());
			}
			retNode.Nodes.Add(AnimeNode);
			
			return retNode;
		}
		
		public CCSFileHeader.CCSVersion GetVersion()
		{
			return Header.GetVersionType();
		}
		
		public string GetVersionString()
		{
			return Header.GetVersionString();
		}
		
		private bool ReadIndexSection(BinaryReader bStream)
		{
			var isIndexSection = bStream.ReadInt32() & 0xFFFF;
			if(isIndexSection != SECTION_INDEX)
			{
				Logger.LogError("Index section mismatch\n");
				return false;
			}
			
			int indexSectionLength = bStream.ReadInt32() - 1;
			FileIndexCount = bStream.ReadInt32();
			ObjectIndexCount = bStream.ReadInt32();
			
			FileIndex = new IndexFileEntry[FileIndexCount];
			for(int i = 0; i < FileIndexCount; i++)
			{
				var tmpSubFile = new IndexFileEntry();
				tmpSubFile.Read(bStream);
				FileIndex[i] = tmpSubFile;
			}
			
			ObjectIndex = new IndexObjectEntry[ObjectIndexCount];
			for(int i = 0; i < ObjectIndexCount; i++)
			{
				var tmpSubObj = new IndexObjectEntry();
				tmpSubObj.Read(bStream);
				ObjectIndex[i] = tmpSubObj;
			}
			
			return true;
		}
		
		private bool ReadSetupSection(BinaryReader bStream)
		{
			var isSetupSection = bStream.ReadInt32() & 0xffff;
			if(isSetupSection != SECTION_SETUP)
			{
				Logger.LogError("Setup section mismatch\n");
				return false;
			}
			
			//Setup section should have a zero size. IMOQ just ignores the setup section block, 
			//but we will use it as a check to make sure we're in the right place because it *should* 
			//always be between the Index Section and the Contents.
			var setupSectionSize = bStream.ReadInt32();
			
			while(true)
			{
				var tmpSectionOffset = (int)bStream.BaseStream.Position;
				var tmpSectionType = bStream.ReadInt32() & 0xFFFF;
				if(tmpSectionType == SECTION_STREAM)
				{
					//Back it up
					bStream.BaseStream.Seek(-4, SeekOrigin.Current);
					break;
				}
				
				var tmpSectionSize = bStream.ReadInt32() - 1;
				var tmpSectionID = bStream.ReadInt32();
				if(tmpSectionID > ObjectIndexCount)
				{
					Logger.LogError(string.Format("Error reading sub object, ID out of bounds ({0} > {1}) at 0x{2:X}\n", tmpSectionID, ObjectIndexCount, tmpSectionOffset));
					return false;
				}
				bool sectionRead = true;
				switch(tmpSectionType)
				{
					case SECTION_OBJECT:
						{
							var tmpObject = new CCSObject(tmpSectionID, this);
							sectionRead = tmpObject.Read(bStream, tmpSectionSize);
							//For now, just check and override.
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpObject);
							LastObject = tmpObject;
							break;
						}
					case SECTION_MATERIAL:
						{
							var tmpMaterial = new CCSMaterial(tmpSectionID, this);
							sectionRead = tmpMaterial.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpMaterial);
							MaterialList.Add(tmpMaterial);
							break;
						}
					case SECTION_TEXTURE:
						{
							var tmpTexture = new CCSTexture(tmpSectionID, this);
							sectionRead = tmpTexture.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpTexture);
							TextureList.Add(tmpTexture);
							break;
						}
					case SECTION_CLUT:
						{
							var tmpClut = new CCSClut(tmpSectionID, this);
							sectionRead = tmpClut.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpClut);
							break;
						}
					case SECTION_CAMERA:
						{
							var tmpCamera = new CCSCamera(tmpSectionID, this);
							sectionRead = tmpCamera.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpCamera);
							break;
						}
					case SECTION_LIGHT:
						{
							var tmpLight = new CCSLight(tmpSectionID, this);
							sectionRead = tmpLight.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpLight);
							break;
						}
					case SECTION_ANIME:
						{
							var tmpAnime = new CCSAnime(tmpSectionID, this);
							sectionRead = tmpAnime.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpAnime);
							AnimeList.Add(tmpAnime);
							break;
						}
					case SECTION_MODEL:
						{
							var tmpModel = new CCSModel(tmpSectionID, this);
							sectionRead = tmpModel.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpModel);
							break;
						}
					case SECTION_CLUMP:
						{
							var tmpClump = new CCSClump(tmpSectionID, this);
							sectionRead = tmpClump.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpClump);
							ClumpList.Add(tmpClump);
							LastClump = tmpClump;
							break;
						}
					case SECTION_EXTERNAL:
						{
							var tmpExt = new CCSExt(tmpSectionID, this);
							sectionRead = tmpExt.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpExt);
							break;
						}
					case SECTION_HITMESH:
						{
							var tmpHit = new CCSHitMesh(tmpSectionID, this);
							sectionRead = tmpHit.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpHit);
							HitList.Add(tmpHit);
							break;
						}
					case SECTION_BBOX:
						{
							var tmpBBox = new CCSBoundingBox(tmpSectionID, this);
							sectionRead = tmpBBox.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpBBox);
							BBoxList.Add(tmpBBox);
							break;
						}
					case SECTION_PARTICLE:
						{
							var tmpParticle = new CCSParticle(tmpSectionID, this);
							sectionRead = tmpParticle.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpParticle);
							break;
						}
					case SECTION_EFFECT:
						{
							var tmpEffect = new CCSEffect(tmpSectionID, this);
							sectionRead = tmpEffect.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpEffect);
							break;
						}
					case SECTION_BLTGROUP:
						{
							var tmpBlitGrp = new CCSBlitGroup(tmpSectionID, this);
							sectionRead = tmpBlitGrp.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpBlitGrp);
							break;
						}
					case SECTION_FBRECT:
						{
							var tmpFBRect = new CCSFBRect(tmpSectionID, this);
							sectionRead = tmpFBRect.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpFBRect);
							break;
						}
					case SECTION_FBPAGE:
						{
							var tmpFBPage = new CCSFBPage(tmpSectionID, this);
							sectionRead = tmpFBPage.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpFBPage);
							break;
						}
					case SECTION_DUMMYPOS:
					case SECTION_DUMMYPOSROT:
						{
							var tmpDummy = new CCSDummy(tmpSectionID, this, tmpSectionType);
							sectionRead = tmpDummy.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpDummy);
							DummyList.Add(tmpDummy);
							break;
						}
					case SECTION_LAYER:
						{
							var tmpLayer = new CCSLayer(tmpSectionID, this);
							sectionRead = tmpLayer.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpLayer);
							break;
						}
					case SECTION_SHADOW:
						{
							var tmpShadow = new CCSShadow(tmpSectionID, this);
							sectionRead = tmpShadow.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpShadow);
							break;
						}
					case SECTION_MORPHER:
						{
							var tmpMorpher = new CCSMorpher(tmpSectionID, this);
							sectionRead = tmpMorpher.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpMorpher);
							break;
						}
					case SECTION_OBJECT2:
						{
							//Skip OBJ2 since they're kind of useless for our needs, and they use objectID's that belong to other objects.
							Util.SkipSection(bStream, tmpSectionSize);
							break;
						}
					case SECTION_PCM:
						{
							var tmpPCM = new CCSPCM(tmpSectionID, this);
							sectionRead = tmpPCM.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpPCM);
							break;
						}
						//GENERATION 2 Sections
					case SECTION_BINARYBLOB:
						{
							//Warn about section type not introduced until next generation...
							if(GetVersion() == CCSFileHeader.CCSVersion.Gen1) Logger.LogWarning(string.Format("Binary Blob (Type 0x2400) with ID of {0} found in Generation 1 CCS File at 0x{1:X}, File may fail in game!\n", tmpSectionID, tmpSectionOffset));
							//But read it anyways.
							var tmpBinary = new CCSBinaryBlob(tmpSectionID, this);
							sectionRead = tmpBinary.Read(bStream, tmpSectionSize);
							SetObjectRef(tmpSectionID, tmpSectionType, tmpSectionOffset, tmpBinary);
							break;
						}
					default:
						{
							Logger.LogError(string.Format("Unknown section of type {0:X} found at 0x{2:X}, skipping...\n", tmpSectionType, tmpSectionID, tmpSectionOffset));
							sectionRead = true;
							Util.SkipSection(bStream, tmpSectionSize);
							break;
						}
				}
				
				if(!sectionRead)
				{
					Logger.LogError(string.Format("Error reading section {0} of type {1}({2}) at 0x{3:X}, size: 0x{4:X}\n", tmpSectionID, tmpSectionType, GetObjectTypeString(tmpSectionType), tmpSectionOffset, tmpSectionSize));
					return false;
				}
			}
			
			LastClump = null;
			LastObject = null;
			
			return true;
		}
		
		public int SearchClump(int _objectID)
		{
			//OMG WTF Hax
			foreach(var tmpClump in ClumpList)
			{
				int tmpID = tmpClump.SearchNodeID(_objectID);
				if(tmpID != -1) return tmpID;
			}
			
			return -1;
		}
		
		private void SetObjectRef(int _objectID, int _objectType, int _objectOffset, CCSBaseObject _objectRef)
		{
			var tmpObj = ObjectIndex[_objectID];
			if(tmpObj.ObjectRef != null)
			{
				
				Logger.LogWarning(string.Format("Duplicate sub object definition for object {0}:\n", _objectID));
				Logger.LogWarning(string.Format("\tType: {0}({1}) at 0x{2:X}\n", _objectType, GetObjectTypeString(_objectType), _objectOffset));
				Logger.LogWarning(string.Format("\tOriginally defined as Type: {0}({1}) at 0x{2:X}\n", tmpObj.ObjectType, GetObjectTypeString(tmpObj.ObjectType), tmpObj.ObjectOffset));
			}
			tmpObj.ObjectOffset = _objectOffset;
			tmpObj.ObjectType = _objectType;
			tmpObj.ObjectRef = _objectRef;
		}
		
		public string GetObjectTypeString(int _objectType)
		{
			if(ObjectTypeNames.ContainsKey(_objectType))
			{
				return ObjectTypeNames[_objectType];
			}
			
			return string.Format("Unknown Object Type: 0x{0:X}", _objectType);
		}
		
		private bool ReadStreamSection(BinaryReader bStream)
		{
			//TODO: CCSFile::ReadStreamSection(): Support stream animation.
			StreamOffset = (int)bStream.BaseStream.Position;
			var isStreamSection = bStream.ReadInt32() & 0xFFFF;
			if(isStreamSection != SECTION_STREAM)
			{
				Logger.LogError("Stream section mismatch!\n");
				return false;
			}
			var streamHeaderSize = bStream.ReadInt32();
			StreamFrameCount = bStream.ReadInt32();
			
			//End reading here for now.
			//TODO: Reading CCS Stream Animation.
			
			return true;
		}
		
		public string GetSubFileName(int _fileID)
		{
			if(_fileID < FileIndexCount)
			{
				var tmpIndexFile = FileIndex[_fileID];
				return tmpIndexFile.FileName;
			}
			
			return string.Format("<Invalid File ID: {0}>", _fileID);
		}
		
		public string GetSubObjectName(int _objectID)
		{
			if(_objectID < ObjectIndexCount)
			{
				var tmpIndexObject = ObjectIndex[_objectID];
				return tmpIndexObject.ObjectName;
			}
			
			return string.Format("<Invalid Object ID: {0}>", _objectID);
		}
		
		public int GetSubObjectType(int _objectID)
		{
			if(_objectID < ObjectIndexCount)
			{
				var tmpIndexObject = ObjectIndex[_objectID];
				return tmpIndexObject.ObjectType;
			}
			
			return 0;
		}
		
		public int GetIDByNameAndType(string _name, int _objectType)
		{
			for(int i = 0; i < ObjectIndexCount; i++)
			{
				var tmpObj = ObjectIndex[i];
				if(tmpObj.ObjectType == _objectType)
				{
					return i;
				}
			}
			
			return 0;
		}
		
		public T GetObject<T>(int _objectID) where T: CCSBaseObject
		{
			if(_objectID < ObjectIndexCount)
			{
				var tmpObj = ObjectIndex[_objectID];
				return tmpObj.ObjectRef as T;
			}
			return null;
		}
		
		public T GetObjectByNameAndType<T>(int _objectType, string _objectName) where T: CCSBaseObject
		{
			for(int i = 0; i < ObjectIndexCount; i++)
			{
				var tmpObj = ObjectIndex[i];
				if(tmpObj.ObjectType == _objectType)
				{
					if(tmpObj.ObjectName == _objectName) return tmpObj.ObjectRef as T;
				}
			}
			
			return null;
		}
		
		public IndexFileEntry GetParentSubFile(int _objectID)
		{
			if(_objectID < ObjectIndexCount)
			{
				IndexObjectEntry tmpObjectIndex = ObjectIndex[_objectID];
				return FileIndex[tmpObjectIndex.FileID];
			}
			return null;
		}
		
		public string GetReport()
		{
			string retVal = string.Format("CCS File {0}:\n", Header.CCSFName);
			retVal += string.Format("\tVersion: {0}, {1} Files, {2} Objects\n", Header.GetVersionString(), FileIndexCount, ObjectIndexCount);
			retVal += string.Format("\t{0} Stream Animation frames\n", StreamFrameCount - 1);
			
			retVal += "Sub Files:\n";
			for(int i = 0; i < FileIndexCount; i++)
			{
				var f = FileIndex[i];
				string fileName = f.FileName;
				if(fileName == string.Empty) fileName = "<NONE>";
				retVal += string.Format("\t0x{0:X}: {1}\n", i, fileName);
			}
			
			retVal += "Sub Objects:\n";
			for(int i = 0; i < ObjectIndexCount; i++)
			{
				var o = ObjectIndex[i];
				string isInFile = "";
				string objName = o.ObjectName;
				if(objName == string.Empty) objName = "<NONE>";
				if(o.ObjectRef == null) isInFile = "(Not present)";
				retVal += string.Format("\t0x{0:X4}: {1,-30}{2:X4}{3}\n", i, objName, o.FileID,isInFile);
			}
			
			retVal += "Materials:\n";
			foreach(var tmpMat in MaterialList)
			{
				retVal += tmpMat.GetReport(1);
			}
			
			retVal += "Textures:\n";
			foreach(var tmpTex in TextureList)
			{
				retVal += tmpTex.GetReport(1);
			}
			
			retVal += "Clumps:\n";
			foreach(var tmpClump in ClumpList)
			{
				retVal += tmpClump.GetReport(1);
			}
			
			retVal += "Animations:\n";
			foreach(var tmpAnime in AnimeList)
			{
				retVal += tmpAnime.GetReport(1);
			}
			
			
			
			return retVal;
		}
		
		public void DumpToObj(string outputPath, bool collision, bool splitSubModels, bool splitCollision, bool withNormals, bool dummies)
		{
			//First, check and make output dir
			string fullPath = Path.Combine(outputPath, Header.CCSFName);
			if(!Directory.Exists(fullPath))
			{
				if(File.Exists(fullPath))
				{
					Logger.LogError(string.Format("Error, Cannot dump CCS File, {0} exists as file", Header.CCSFName));
					return;
				}
				else
				{
					Directory.CreateDirectory(fullPath);
				}
			}
			
			//Output OBJ
			
			string outputFileName = Path.Combine(fullPath, Header.CCSFName);
			Logger.LogInfo(string.Format("Dumping {0} to {0}.obj...\n", Header.CCSFName, outputFileName));
			using(System.IO.StreamWriter fStream = new StreamWriter(outputFileName + ".obj", false))
			{
				fStream.WriteLine(string.Format("mtllib {0}.mtl", Header.CCSFName));
				//start at vert 1 because for some reason it's 1 based indices
				int totalVertCount = 1;
				foreach(var tmpClump in ClumpList)
				{
					totalVertCount = tmpClump.DumpToObj(fStream, totalVertCount, splitSubModels, withNormals);
				}
			}
			
			//Output Mtl
			Logger.LogInfo("Creating MTL file and dumping textures...\n");
			using(System.IO.StreamWriter fStream = new StreamWriter(outputFileName + ".mtl", false))
			{
				foreach(var tmpTexture in TextureList)
				{
					tmpTexture.DumpToMtl(fStream, Path.GetDirectoryName(outputFileName + ".mtl"));
				}
			}
			
			if(collision)
			{
				Logger.LogInfo(string.Format("Dumping Collision Meshes to {0}_collision.obj...\n", outputFileName));
				using(System.IO.StreamWriter fStream = new StreamWriter(outputFileName + "_collison.obj", false))
				{
					int totalVertCount = 1;
					foreach(var tmpHit in HitList)
					{
						totalVertCount = tmpHit.DumpToObj(fStream, totalVertCount, splitCollision);
					}
				}
			}
			
			if(dummies)
			{
				Logger.LogInfo(string.Format("Dumping Dummies to {0}_dummies.txt...\n", outputFileName));
				using(System.IO.StreamWriter fStream = new StreamWriter(outputFileName + "_dummies.txt", false))
				{
					fStream.WriteLine(string.Format("{0}", DummyList.Count));
					foreach(var tmpDummy in DummyList)
					{
						tmpDummy.DumpToTxt(fStream);
					}
				}
			}
			
			
			Logger.LogInfo("Done.");
		}
		
		public void DumpToSMD(string outputPath, bool withNormals)
		{
			string fullPath = Path.Combine(outputPath, Header.CCSFName);
			if(!Directory.Exists(fullPath))
			{
				if(File.Exists(fullPath))
				{
					Logger.LogError(string.Format("Error, Cannot dump CCS File, {0} exists as file", Header.CCSFName));
					return;
				}
				else
				{
					Directory.CreateDirectory(fullPath);
				}
			}
			
				foreach(var tmpClump in ClumpList)
				{
					tmpClump.DumpToSMD(fullPath, withNormals);
				}
				
				Logger.LogInfo("Done.");
			
		}
		
		public void DumpAnimationsToText(string outPath)
		{
			//string fullPath = Path.Combine(outPath, string.Format("{0}_animations.txt"));
			string fullPath = Path.Combine(outPath, Header.CCSFName);
			if(!Directory.Exists(fullPath))
			{
				if(File.Exists(fullPath))
			   	{
					Logger.LogError(string.Format("Error, Cannot dump Animations to file, '{0}' exists as file", outPath));
					return;
			   	}
				else
				{
					Directory.CreateDirectory(fullPath);
				}
			}
			
			string outputFileName = Path.Combine(fullPath, "animations.txt");
			FileMode mode = FileMode.OpenOrCreate;
			if(File.Exists(outputFileName))
			{
				mode = FileMode.Truncate;
			}
			using(var fs = new FileStream(outputFileName, mode))
			{
				using(var outf = new StreamWriter(fs))
				{
					outf.WriteLine(AnimeList.Count.ToString());
					foreach(var tmpAnime in AnimeList)
					{
						tmpAnime.DumpToText(outf);
					}
				}
			}
			
		}
		
	}
}
