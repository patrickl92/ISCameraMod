namespace ISCameraMod.Serialization.V1
{
	using System;
	using ISCameraMod.Model;

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
			PositionX = cameraPosition.Position.x;
			PositionY = cameraPosition.Position.y;
			PositionZ = cameraPosition.Position.z;
			RotationX = cameraPosition.RotationX;
			RotationY = cameraPosition.RotationY;
			ZoomLevel = cameraPosition.ZoomLevel;
		}

		public int NumpadKey { get; set; }

		public float PositionX { get; set; }

		public float PositionY { get; set; }

		public float PositionZ { get; set; }

		public float RotationX { get; set; }

		public float RotationY { get; set; }

		public float ZoomLevel { get; set; }
	}
}
