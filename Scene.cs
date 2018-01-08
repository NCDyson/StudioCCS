/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/22/2017
 * Time: 2:15 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using OpenTK.Graphics;
using System.Windows.Forms;
using StudioCCS;
using StudioCCS.libCCS;
using System.Runtime.InteropServices;

namespace StudioCCS
{
	/// <summary>
	/// Description of Scene.
	/// </summary>
	public static class Scene
	{
		
		/*
		public class SceneClumpInstance
		{
			public CCSClump Clump = null;
			public CCSFile File = null;
			public Vector3 Position = Vector3.Zero;
			public Vector3 Rotation = Vector3.Zero;
			public Vector3 Scale = Vector3.One;
			public bool AttachedDummy = false;
			public CCSDummy Dummy = null;
			
			public SceneClumpInstance FromTag(TreeNodeTag tag)
			{
				return new SceneClumpInstance()
				{
					File = tag.File,
					Clump = File.GetObject<CCSClump>(tag.ObjectID),
					Position = new Vector3(0.0f, 0.0f, 0.0f),
					Rotation = new Vector3(0.0f, 0.0f, 0.0f),
					Scale = new Vector3(1.0f, 1.0f, 1.0f)
				};
			}
			
			public void AttachToDummy(CCSDummy _dummy)
			{
				Dummy = _dummy;
				AttachedDummy = true;
			}
			
			public void DetatchFromDummy()
			{
				Dummy = null;
				AttachedDummy = false;
			}
		}
		*/
		
		public class SceneInstanceObject
		{
			public CCSFile File = null;
			public CCSBaseObject Object = null;
			public int ObjectType = 0;
			public Vector3 PositionOffset;
			public Vector3 RotationOffset;
			
			public SceneInstanceObject Parent;
			
			public SceneInstanceObject(TreeNodeTag tag)
			{
				File = tag.File;
				ObjectType = tag.ObjectType;
				
			}
			
			public void AttachTo(SceneInstanceObject NewParent)
			{
				Parent = NewParent;
			}
			
			public void Detatch()
			{
				Parent = null;
			}
			
			public void Render()
			{
				
			}
				
			
		}
		
		//public enum RenderMode {Wireframe, Flat, Smooth, Textured};
		public enum SceneMode {Preview, Scene, All};
		public enum KeyStatus {Up, Pressed, Repeated};
		private const int AxisViewSize = 80;
		
		//Draw Mode Flags for rendering
		public static int SCENE_DRAW_LINES = 1;
		public static int SCENE_DRAW_VERTEX_COLORS = 2;
		public static int SCENE_DRAW_SMOOTH = 4;
		public static int SCENE_DRAW_TEXTURE = 8;
		public static int SCENE_DRAW_SELECTION = 16;
		public static int SCENE_DRAW_FLIP_TEXCOORDS = 32;
		


		//public static RenderMode DisplayMode = RenderMode.Wireframe;
		public static SceneMode SceneDisplay = SceneMode.Preview;

		public static bool BackfaceCull = false;
		
		//public static IGraphicsContext Context = null;
		//public static IWindowInfo WiFo = null;
		public static GLControl control = null;
		public static bool WasInit = false;
		public static int ViewWidth;
		public static int ViewHeight;
		public static Stopwatch Timer = new Stopwatch();
		
		public static Matrix4 ProjectionMtx = Matrix4.Identity;
		public static Matrix4 AxisProjectionMtx = Matrix4.Identity;
		
		//Draw options
		public static bool DrawViewAxis = true;
		public static bool DrawViewGrid = true;
		public static bool DrawCollisionMeshes = true;
		public static bool DrawWorldCenter = true;
		public static bool DrawDummyHelpers = true;
		public static bool DrawLightHelpers = true;
		
		public static bool DrawWireframe = false;
		public static bool DrawVertexColors = false;
		public static bool DrawVertexNormals = false;
		public static bool DrawTextures = false;
		
		//Camera
		public static ArcBallCamera PreviewCamera = new ArcBallCamera();
		public static ArcBallCamera SceneCamera = new ArcBallCamera();
		public static ArcBallCamera AllCamera = new ArcBallCamera();
		public static bool DefaultToAxisMovement = false;
		
		//Input
		public static float LastMouseX = 0.0f;
		public static float LastMouseY = 0.0f;
		public static float MouseSensitivity = 0.2f;
		public static float MouseWheelSensitivity = 0.000050f;
		public static float MovementSpeed = 0.0025f;
		public static float DeltaTime = 1.0f;
		
		//Keys we handle
		//TODO: Make these remappable?
		private static KeyStatus MoveForward = KeyStatus.Up;
		private static KeyStatus MoveBackward = KeyStatus.Up;
		private static KeyStatus MoveLeft = KeyStatus.Up;
		private static KeyStatus MoveRight = KeyStatus.Up;
		private static KeyStatus MoveUp = KeyStatus.Up;
		private static KeyStatus MoveDown = KeyStatus.Up;
		private static KeyStatus ZoomIn = KeyStatus.Up;
		private static KeyStatus ZoomOut = KeyStatus.Up;
		private static KeyStatus ShiftModifier = KeyStatus.Up;
		private static KeyStatus ControlModifier = KeyStatus.Up;
		
		private static Keys MoveForward_Key = Keys.W;
		private static Keys MoveBackward_Key = Keys.S;
		private static Keys MoveLeft_Key = Keys.A;
		private static Keys MoveRight_Key = Keys.D;
		private static Keys MoveUp_Key = Keys.X;
		private static Keys MoveDown_Key = Keys.Z;
		
		private static Keys ZoomIn_Key = Keys.Oemplus;
		private static Keys ZoomOut_Key = Keys.OemMinus;
		
		//Items We'll Render
		public static List<CCSFile> CCSFileList = new List<CCSFile>();
		public static List<CCSAnime> ActiveAnimes = new List<CCSAnime>();
		public static TreeNodeTag SelectedPreviewItemTag = null;
		
		public static int LoadProgram(string programName, bool hasGeometryShader = false)
		{
			bool result = true;
			
			int vShaderID = GL.CreateShader(ShaderType.VertexShader);
			int fShaderID = GL.CreateShader(ShaderType.FragmentShader);
			int gShaderID = 0;
			if(hasGeometryShader)
			{
				gShaderID = GL.CreateShader(ShaderType.GeometryShader);
			}
			
			if(!LoadShader("data/shaders/" + programName + ".vsh", vShaderID))
			{
				result = false;
			}
			
			if(!LoadShader("data/shaders/" + programName + ".fsh", fShaderID))
			{
				result = false;
			}
			
			if(hasGeometryShader)
			{
				if(!LoadShader("data/shaders/" + programName + ".gsh", gShaderID))
				{
					result = false;
				}
			}
			
			int programID = GL.CreateProgram();
			if(result)
			{
				GL.AttachShader(programID, vShaderID);
				GL.AttachShader(programID, fShaderID);
				if(hasGeometryShader) GL.AttachShader(programID, gShaderID);
				
				GL.LinkProgram(programID);
				int programLinkResult = 0;
				GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out programLinkResult);
				if(programLinkResult == 0)
				{
					Logger.LogError(string.Format("Error linking program {0}:\n{1}\n", programName, GL.GetProgramInfoLog(programID)));
					result = false;
				}
				
			}
			
			GL.DeleteShader(vShaderID);
			GL.DeleteShader(fShaderID);
			if(hasGeometryShader) GL.DeleteShader(gShaderID);
			if(result)
			{
				return programID;
			}
			
			GL.DeleteProgram(programID);
			return -1;
		}
		
		private static bool LoadShader(string fileName, int shaderID)
		{
			using(var sr = new StreamReader(fileName))
			{
				string shaderCode = sr.ReadToEnd();
				GL.ShaderSource(shaderID, shaderCode);
				GL.CompileShader(shaderID);
				int compileResult = 0;
				GL.GetShader(shaderID, ShaderParameter.CompileStatus, out compileResult);
				if(compileResult == 0)
				{
					Logger.LogError(string.Format("Error compiling shader {0}:\n{1}\n", fileName, GL.GetShaderInfoLog(shaderID)));
					return false;
				}
				return true;
			}
		}
		
		public static TreeNode LoadCCSFile(string fileName)
		{
			var tmpCCS = new CCSFile();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			if(tmpCCS.Read(fileName))
			{
				if(tmpCCS.Init())
				{
					sw.Stop();
					Debug.WriteLine("Read and Initialized {0} in {1}ms...", fileName, sw.ElapsedMilliseconds);
					CCSFileList.Add(tmpCCS);
					return tmpCCS.ToNode();
				}
			}
			sw.Stop();
			Debug.WriteLine("Failed to read {0} in {1}ms...", fileName, sw.ElapsedMilliseconds);
			return null;
		}
		
		public static bool UnloadCCSFile(CCSFile file)
		{
			file.DeInit();
			CCSFileList.Remove(file);
			return true;
		}
		
		public static TreeNode ToNode()
		{
			var retNode = new TreeNode();
			foreach(var tmpCCS in CCSFileList)
			{
				retNode.Nodes.Add(tmpCCS.ToNode());
			}
			
			return retNode;
		}
		
		public static TreeNode ToSceneNode()
		{
			
			var tmpMainAnmNode = new TreeNode("Animations");
			foreach(var tmpAnmNode in ActiveAnimes)
			{
				tmpMainAnmNode.Nodes.Add(tmpAnmNode.ToNode());
			}
			
			return tmpMainAnmNode;
		}
		
		public static void Init(GLControl glCtrl)
		{
			//WiFo = Utilities.CreateWindowsWindowInfo(ctrl.Handle);
			//Context = new GraphicsContext(GraphicsMode.Default, WiFo);
			//Context.MakeCurrent(WiFo);
			//Context.LoadAll();
			control = glCtrl;
			control.MakeCurrent();
			WasInit = true;
			
			GL.ClearColor(64 / 255.0f, 64 / 255.0f, 64 / 255.0f, 1.0f);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.Enable(EnableCap.AlphaTest);
			
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcColor);
			
			GL.Enable(EnableCap.Texture2D);
			
			GL.Disable(EnableCap.CullFace);
			
			//Init other stuff here
			AxisMarker.Init();
			WireHelper.Init();
			Grid.Init();
			TexturePreview.Init();
			
			//PreviewCamera.Target = new Vector3(-46.28392f, -0.38321028f, -1.108414f);
		}
		
		public static void DeInit()
		{
			
			foreach(var tmpCCS in CCSFileList)
			{
				tmpCCS.DeInit();
			}
			AxisMarker.DeInit();
			WireHelper.DeInit();
			Grid.DeInit();
			TexturePreview.DeInit();
		}
		
		
		private static void HandleInput()
		{
			ArcBallCamera curCamera = CurrentCamera();
			Vector3 CamTarget = curCamera.Target;
			
			//Check for Movement
			bool shiftMod = (ShiftModifier != KeyStatus.Up);
			if(!DefaultToAxisMovement) shiftMod = !shiftMod;
			
			if(shiftMod)
			{	
				Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);
				
				Matrix4 viewMtx = curCamera.GetMatrix();
				Vector3 forward = new Vector3(viewMtx[0, 2], viewMtx[1, 2], viewMtx[2, 2]).Normalized();
				Vector3 up = new Vector3(viewMtx[0,1], viewMtx[1,1], viewMtx[2,1]).Normalized();
				Vector3 right = Vector3.Cross(forward, up).Normalized();
				
				if(MoveForward != KeyStatus.Up)
				{
					
					movement += forward;
				}
				
				if(MoveBackward != KeyStatus.Up)
				{
					movement -= forward;
				}
				
				if(MoveLeft != KeyStatus.Up)
				{
					movement -= right;
						
				}
				
				if(MoveRight != KeyStatus.Up)
				{
					movement += right;
				}

				if(ControlModifier == KeyStatus.Up)
				{
					movement *= new Vector3(1.0f, 0.0f, 1.0f);
				}
				
				if(MoveUp != KeyStatus.Up)
				{
					movement += Vector3.UnitY;
				}
				
				if(MoveDown != KeyStatus.Up)
				{
					movement -= Vector3.UnitY;
				}
				
				CamTarget += (movement * MovementSpeed * DeltaTime);
			}
			else
			{
				if(MoveForward != KeyStatus.Up)
				{
					CamTarget.Z -= DeltaTime * MovementSpeed;
				}
				if(MoveBackward != KeyStatus.Up)
				{
					CamTarget.Z += DeltaTime * MovementSpeed;
				}
				if(MoveLeft != KeyStatus.Up)
				{
					CamTarget.X -= DeltaTime * MovementSpeed;
				}
				if(MoveRight != KeyStatus.Up)
				{
					CamTarget.X += DeltaTime * MovementSpeed;
				}
				if(MoveUp != KeyStatus.Up)
				{
					CamTarget.Y -= DeltaTime * MovementSpeed;
				}
				if(MoveDown != KeyStatus.Up)
				{
					CamTarget.Y += DeltaTime * MovementSpeed;
				}
			}
			curCamera.Target = CamTarget;
			
			float keyZoom = 0.0075f;
			float distToZoom = DeltaTime * keyZoom;
			if(ShiftModifier != KeyStatus.Up) distToZoom *= 0.25f;
			if(ZoomIn != KeyStatus.Up)
			{
				curCamera.Distance -= distToZoom;
			}
			
			if(ZoomOut != KeyStatus.Up)
			{
				curCamera.Distance += distToZoom;
			}
		}
		
		public static void Render()
		{
			MakeCurrent();
			
			/*
			if(DisplayMode == RenderMode.Wireframe)
			{
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			}
			else
			{
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
			}
			*/
			
			if(BackfaceCull)
			{
				GL.Enable(EnableCap.CullFace);
			}
			else
			{
				GL.Disable(EnableCap.CullFace);
			}
			
			Timer.Stop();
			DeltaTime = (float)Timer.Elapsed.TotalMilliseconds;
			Timer.Reset();
			Timer.Start();
			
			//Handle Keyboard input
			HandleInput();
			ArcBallCamera curCamera = CurrentCamera();
			
			//Clear
			GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
			
			SetMainViewport();
			Matrix4 CamMtx = curCamera.GetMatrix();
			Matrix4 ProjViewMtx = CamMtx * ProjectionMtx;
			
			//ProjViewMtx *= Matrix4.CreateRotationX(90.0f);
			Matrix4 CCSMatrix = Matrix4.CreateRotationX(-90.0f * (float)Math.PI / 180.0f) * ProjViewMtx;
			//Matrix4 CCSMatrix = ProjViewMtx;
			//GL.Enable(EnableCap.Blend);
			//GL.Enable(EnableCap.DepthTest);
			
			Matrix4 Helper1 = Matrix4.CreateTranslation(-4.0f, 0.0f, 0.0f) * ProjViewMtx;
			Matrix4 Helper2 = Matrix4.CreateTranslation(4.0f, 0.0f, 0.0f) * ProjViewMtx;
			//LightHelper.RenderOmniHelper(Helper2, 2.0f);
			//cmesh.Render(Helper1);
			//LightHelper.RenderDirectionalHelper(Helper1);
			
			/*
			if(IsPreviewTexture())
			{
				CCSTexture tex = SelectedPreviewItemTag.File.GetObject<CCSTexture>(SelectedPreviewItemTag.ObjectID);
				if(tex != null)
				{
					GL.Viewport(0, 0, ViewWidth, ViewHeight);
					Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI * (45.0f / 180.0f)), ViewWidth / (float)ViewHeight, 0.1f, 100000.0f);
					
					float texW = (float)tex.Width;
					float texH = (float)tex.Height;
					TexturePreview.Render(proj, tex.TextureID, texW, texH);
				}
			}
			else
			{
			*/
				if(DrawWorldCenter)
				{				
					//AxisMarker.Render(Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f) * ProjViewMtx, 1.0f);
					RenderViewAxisGizmo(10.0f, ProjectionMtx);
				}
				if(DrawViewAxis)
				{
					SetAxisViewport();
					RenderViewAxisGizmo(1.75f, AxisProjectionMtx, true);
					SetMainViewport();
				}
				if(DrawViewGrid) Grid.Render(Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f) * ProjViewMtx, 1.0f);
	
				//LightHelper.RenderOmniHelper(Helper2, 1.0f);
				
				//fuck it, frame forward animes here...
				/*
				foreach(var tmpAnime in ActiveAnimes)
				{
					//tmpAnime.FrameForward();
				}
				*/
				
				if(SceneDisplay == SceneMode.Preview) PreviewRender(CCSMatrix);
				else if(SceneDisplay == SceneMode.Scene) SceneRender(CCSMatrix);
				else AllRender(CCSMatrix);
			//}
			

			
			control.SwapBuffers();
		}
		
		private static bool IsPreviewTexture()
		{
			if(SelectedPreviewItemTag == null) return false;
			if(SelectedPreviewItemTag.ObjectType != CCSFile.SECTION_TEXTURE) return false;
			
			return true;
		}
		
		public static int GetRenderMode()
		{
			int retVal = 0;
			/*
			if(DrawWireframeOverlay || (DisplayMode == RenderMode.Wireframe )) retVal |= SCENE_DRAW_LINES;
			if(DisplayMode == RenderMode.Smooth) retVal |= SCENE_DRAW_FILL;
			if(DisplayMode == RenderMode.Textured) retVal |= (SCENE_DRAW_TEXTURE | SCENE_DRAW_FILL);
			*/
			if(DrawWireframe) retVal |= SCENE_DRAW_LINES;
			if(DrawVertexColors) retVal |= SCENE_DRAW_VERTEX_COLORS;
			if(DrawVertexNormals) retVal |= SCENE_DRAW_SMOOTH;
			if(DrawTextures) retVal |= SCENE_DRAW_TEXTURE;
			
			return retVal;
		}
		
		private static void AllRender(Matrix4 ProjViewMtx)
		{
			RenderAllCCS(ProjViewMtx);
		}
		
		private static void SceneRender(Matrix4 ProjViewMtx)
		{		
			
			foreach(var tmpAnime in ActiveAnimes)
			{
				tmpAnime.Render(ProjViewMtx, GetRenderMode());
			}
			
			//RenderAllCCS(ProjViewMtx);
		}
		
		private static void PreviewRender(Matrix4 ProjViewMtx)
		{
			int extraOptions = GetRenderMode();
			
			if(SelectedPreviewItemTag != null)
			{
				if(SelectedPreviewItemTag.ObjectType == CCSFile.SECTION_CLUMP)
				{
					var tmpClump = SelectedPreviewItemTag.File.GetObject<CCSClump>(SelectedPreviewItemTag.ObjectID);
					tmpClump.Render(ProjViewMtx, extraOptions);
				}
				else if(SelectedPreviewItemTag.ObjectType == CCSFile.SECTION_OBJECT)
				{
					var tmpObj = SelectedPreviewItemTag.File.GetObject<CCSObject>(SelectedPreviewItemTag.ObjectID);
					tmpObj.ParentClump.FrameForward();
					tmpObj.Render(ProjViewMtx, extraOptions);
				}
				else if(SelectedPreviewItemTag.ObjectType == CCSFile.SECTION_MODEL)
				{
					int subNode = -1;
					if(SelectedPreviewItemTag.Type == TreeNodeTag.NodeType.SubNode) subNode = SelectedPreviewItemTag.SubID;
					
					var tmpModel = SelectedPreviewItemTag.File.GetObject<CCSModel>(SelectedPreviewItemTag.ObjectID);
					tmpModel.ClumpRef.FrameForward();
					tmpModel.Render(ProjViewMtx, extraOptions, subNode);
				}
				else if(SelectedPreviewItemTag.ObjectType == CCSFile.SECTION_TEXTURE)
				{
					CCSTexture tex = SelectedPreviewItemTag.File.GetObject<CCSTexture>(SelectedPreviewItemTag.ObjectID);
					if(tex != null)
					{
						float texW = (float)tex.Width;
						float texH = (float)tex.Height;
						TexturePreview.Render(ProjViewMtx, tex.TextureID, texW, texH);
					}
				}
				else if(SelectedPreviewItemTag.ObjectType == CCSFile.SECTION_ANIME)
				{
					CCSAnime tmpAnime = SelectedPreviewItemTag.File.GetObject<CCSAnime>(SelectedPreviewItemTag.ObjectID);
					if(tmpAnime != null)
					{
						tmpAnime.Render(ProjViewMtx, extraOptions);
					}
				}
			}
			
		}
		
		private static  void RenderAllCCS(Matrix4 ProjViewMtx)
		{
			int extraOptions = GetRenderMode();
			foreach(var tmpCCS in CCSFileList)
			{	
				List<CCSClump> clumpList = tmpCCS.ClumpList;
				foreach(var tmpClump in clumpList)
				{
					tmpClump.Render(ProjViewMtx, extraOptions);
				}
				
				if(DrawCollisionMeshes)
				{
					List<CCSHitMesh> hitList = tmpCCS.HitList;
					foreach(var tmpHit in hitList)
					{
						tmpHit.RenderAll(ProjViewMtx);
						//tmpHit.RenderOne(ProjViewMtx, 0);
					}
				}
				
				if(DrawDummyHelpers)
				{
					List<CCSDummy> dummyList = tmpCCS.DummyList;
					foreach(var tmpDummy in dummyList)
					{
						WireHelper.RenderDummyHelper(ProjViewMtx, tmpDummy);
					}
				}
			}
		}
		
		public static void RenderViewAxisGizmo(float size, Matrix4 ProjMtx, bool disableDepth = false)
		{
			//SetAxisViewport();
			if(disableDepth) GL.Disable(EnableCap.DepthTest);
			ArcBallCamera curCam = CurrentCamera();
			Matrix4 ProjViewMtx = curCam.GetMatrixDistanced(size) * ProjMtx * Matrix4.Identity;
			AxisMarker.Render(Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f) * ProjViewMtx, 1.0f);
			GL.Enable(EnableCap.DepthTest);
			//SetMainViewport();
		}
		
		private static void SetMainViewport()
		{
			GL.Viewport(0, 0, control.Width, control.Height);
			ProjectionMtx = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI * (45.0f / 180.0f)), control.Width / (float)control.Height, 0.1f, 100000.0f);
		}
		
		private static void SetAxisViewport()
		{
			GL.Viewport(control.Width - AxisViewSize, control.Height - AxisViewSize, AxisViewSize, AxisViewSize);
			AxisProjectionMtx = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI * (45.0f / 180.0f)), 1.0f, 0.01f, 100000.0f);
		}
		
		public static void MouseMove(MouseEventArgs e)
		{
			float mX = (float)e.X;
			float mY = (float)e.Y;
			float dX = mX - LastMouseX;
			float dY = mY - LastMouseY;
			
			//ArcBallCamera CurrentCam = (SceneDisplay == SceneMode.Preview) ? PreviewCamera : SceneCamera;
			var curCam = CurrentCamera();
			Vector3 camRot = curCam.Rotation;
			Vector3 camTarget = curCam.Target;
			if((e.Button & MouseButtons.Right) != 0)
			{
				float dXm = MouseSensitivity * dX;
				float dYm = MouseSensitivity * dY;
				//if(ShiftModifier == KeyStatus.Up)
				//{
					curCam.Rotation = new Vector3(camRot.X + dXm, camRot.Y + dYm, 0.0f);
				//}
				
				//else
				//{
					/*
					
					float cmx = camRot.X * dego;
					float cmy = camRot.Y * dego;
					float cmz = camRot.Z * dego;
					CurrentCam.Target = new Vector3(camTarget.X - ((float)Math.Sin(cmy) * dXm), camTarget.Y + ((float)Math.Sin(cmx) * dYm), camTarget.Z + ((float)Math.Sin(cmy) * dXm));
					*/
					//float dego = 180.0f / 3.14592654f;
					//Vector3 forward = new Vector3((float)Math.Sin(camRot.X * dego), 0.0f, (float)Math.Cos(camRot.X * dego));
				//	Vector3 forward = CurrentCam.Position - CurrentCam.Target;
				//	forward.Normalize();
				//	Vector3 right = Vector3.Cross(forward, Vector3.UnitY);
					//CurrentCam.Target -= forward * (dYm * 0.01f);
				//	CurrentCam.Target += right * (dXm * 0.1f);
					//CurrentCam.Target.Z += dYm;
				//}
			}
			
			LastMouseX = mX;
			LastMouseY = mY;	
		}
		
		public static void MouseWheel(MouseEventArgs e)
		{
			
			//ArcBallCamera CurrentCam = (SceneDisplay == SceneMode.Preview) ? PreviewCamera : SceneCamera;
			var curCam = CurrentCamera();
			float distToZoom = ((e.Delta * MouseWheelSensitivity) * DeltaTime);
			if(ShiftModifier != KeyStatus.Up) distToZoom *= 0.25f;
			curCam.Distance += distToZoom;
			
		}
		
		public static void KeyPress(KeyEventArgs e)
		{
			if(e.KeyCode == MoveForward_Key) MoveForward = (MoveForward == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			else if(e.KeyCode == MoveBackward_Key) MoveBackward = (MoveBackward == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			else if(e.KeyCode == MoveLeft_Key) MoveLeft = (MoveLeft == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			else if(e.KeyCode == MoveRight_Key) MoveRight = (MoveRight == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			else if(e.KeyCode == MoveUp_Key) MoveUp = (MoveUp == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			else if(e.KeyCode == MoveDown_Key) MoveDown = (MoveDown == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;			
			else if(e.KeyCode == ZoomIn_Key) ZoomIn = (ZoomIn == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			else if(e.KeyCode == ZoomOut_Key) ZoomOut = (ZoomOut == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			
			if(e.Shift) ShiftModifier = (ShiftModifier == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
			if(e.Control) ControlModifier = (ControlModifier == KeyStatus.Pressed) ? KeyStatus.Repeated : KeyStatus.Pressed;
		}
		
		public static void KeyRelease(KeyEventArgs e)
		{
			if(e.KeyCode == MoveForward_Key) MoveForward = KeyStatus.Up;
			else if(e.KeyCode == MoveBackward_Key) MoveBackward = KeyStatus.Up;
			else if(e.KeyCode == MoveLeft_Key) MoveLeft = KeyStatus.Up;
			else if(e.KeyCode == MoveRight_Key) MoveRight = KeyStatus.Up;
			else if(e.KeyCode == MoveUp_Key) MoveUp = KeyStatus.Up;
			else if(e.KeyCode == MoveDown_Key) MoveDown = KeyStatus.Up;
			else if(e.KeyCode == ZoomIn_Key) ZoomIn = KeyStatus.Up;
			else if(e.KeyCode == ZoomOut_Key) ZoomOut = KeyStatus.Up;
			
			if(!e.Shift) ShiftModifier = KeyStatus.Up;
			if(!e.Control) ControlModifier = KeyStatus.Up;
		}
		
		
		public static ArcBallCamera CurrentCamera()
		{
			//ArcBallCamera CurrentCam = (SceneDisplay == SceneMode.Preview) ? PreviewCamera : SceneCamera;
			if(SceneDisplay == SceneMode.Preview) return PreviewCamera;
			if(SceneDisplay == SceneMode.Scene) return SceneCamera;
			return AllCamera;
		}
		
		public static void MakeCurrent()
		{
			//Debug.WriteLine("Making Context Current...");
			control.MakeCurrent();
		}
		
		public static void DumpToObj(string outputPath, bool collision, bool splitSubModels, bool splitCollision, bool withNormals, bool dummies, bool animes)
		{
			foreach(var tmpCCS in CCSFileList)
			{
				tmpCCS.DumpToObj(outputPath, collision, splitSubModels, splitCollision, withNormals, dummies);
				if(animes)
				{
					tmpCCS.DumpAnimationsToText(outputPath);
				}
			}
		}
		
		public static void DumpToSMD(string outputPath, bool withNormals)
		{
			foreach(var tmpCCS in CCSFileList)
			{
				tmpCCS.DumpToSMD(outputPath, withNormals);
			}
		}
		
		public static void AddAnime(CCSAnime anime)
		{
			for(int i = 0; i < ActiveAnimes.Count; i++)
			{
				var tmpAnime = ActiveAnimes[i];
				if(tmpAnime == anime) return;
			}
			ActiveAnimes.Add(anime);
		}
		
		public static void RemoveAnime(CCSAnime anime)
		{
			ActiveAnimes.RemoveAll(item => item == anime);
		}
		
	}
}
