namespace ISCameraMod.Serialization.V1
{
	using System;
	using UnityEngine;

	[Serializable]
	public class SerializableCameraPositionV1
	{
		public SerializableCameraPositionV1()
		{
			// Used for deserialization
		}

		public SerializableCameraPositionV1(int numpadKey, CameraPosition cameraPosition)
		{
			NumpadKey = numpadKey;
			Position = cameraPosition.Position;
			RotationX = cameraPosition.RotationX;
			RotationY = cameraPosition.RotationY;
			ZoomLevel = cameraPosition.ZoomLevel;
		}

		public int NumpadKey { get; set; }

		public Vector3 Position { get; set; }

		public float RotationX { get; set; }

		public float RotationY { get; set; }

		public float ZoomLevel { get; set; }
	}
}
