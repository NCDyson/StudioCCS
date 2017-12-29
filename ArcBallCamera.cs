/*
 * Created by SharpDevelop.
 * User: NCDyson
 * Date: 7/30/2017
 * Time: 4:47 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using OpenTK;

namespace StudioCCS
{
	/// <summary>
	/// Description of ArcBallCamera.
	/// </summary>
	public class ArcBallCamera
	{
		public Vector3 Position = new Vector3(0.0f, 0.0f, 0.0f);
		public Vector3 Rotation = new Vector3(180.0f, -45.0f, .0f);
		public Vector3 Target = new Vector3(0.0f, 0.0f, 0.0f);
		public float Distance = 10.0f;
		
		public ArcBallCamera()
		{

		}
		
		public ArcBallCamera(Vector3 _position, float _distance)
		{
			Position = _position;
			Rotation = new Vector3(0.0f, 0.0f, 0.0f);
			Distance = _distance;
		}

		/*		
		public Vector3 Rotation {
			get {
				return this.rotation;
			}
			set {
				this.rotation = value;
				if(this.rotation.Y > 0.0f) this.rotation.Y = 0.0f;
				if(this.rotation.Y < -90.0f) this.rotation.Y = -90.0f;
				
				if(this.rotation.X > 360.0f) this.rotation.X = 0;
				if(this.rotation.X < 0) this.rotation.X = 360.0f;
			}
		}

		public float Distance {
			get {
				return this.distance;
			}
			set {
				this.distance = value;
				if(this.distance < 1.0f) this.distance = 1.0f;
			}
		}
		*/
		
		private void Clamp()
		{
			if(Rotation.Y > 90.0f) Rotation.Y = 90.0f;
			if(Rotation.Y < -90.0f) Rotation.Y = -90.0f;
			//Waiting for this to bug up
			if(Rotation.X > 360.0f) Rotation.X = 0.0f;
			if(Rotation.X < 0) Rotation.X = 360.0f;
			
			if(Distance < 0.1f) Distance = 0.1f;
		}
		
		public Matrix4 GetMatrix()
		{
			Calculate();
			Matrix4 cameraMatrix = Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY);
			
			return Matrix4.CreateTranslation(Target) * cameraMatrix;
		}
		
		public Matrix4 GetMatrixDistanced(float dist)
		{
			//Clamp();
			const float rads = (float)(Math.PI / 180.0f);
			var CamPos = new Vector3();
			CamPos.X = (float)(dist * -Math.Sin(Rotation.X * rads) * Math.Cos(Rotation.Y * rads));
			CamPos.Y = (float)(dist * -Math.Sin(Rotation.Y * rads));
			CamPos.Z = -(float)(-dist * Math.Cos(Rotation.X * rads) * Math.Cos(Rotation.Y * rads));
		
			Matrix4 cameraMatrix = Matrix4.LookAt(CamPos, Vector3.Zero, Vector3.UnitY);
			return Matrix4.CreateTranslation(0.0f,0.0f, 0.0f) * cameraMatrix;
			
		}
		
		private void Calculate()
		{
			Clamp();
			const float rads = (float)(Math.PI / 180.0f);
			Position.X = (float)(Distance * -Math.Sin(Rotation.X * rads) * Math.Cos(Rotation.Y * rads));
			Position.Y = (float)(Distance * -Math.Sin(Rotation.Y * rads));
			Position.Z = -(float)(-Distance * Math.Cos(Rotation.X * rads) * Math.Cos(Rotation.Y * rads));
		}
	}
}
