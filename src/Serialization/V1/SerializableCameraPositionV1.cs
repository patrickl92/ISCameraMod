namespace ISCameraMod.Serialization.V1
{
	using System;
	using ISCameraMod.Model;
	using UnityEngine;

	/// <summary>
	/// Represents a serializable <see cref="CameraPosition"/> with its associated numpad key.
	/// </summary>
	[Serializable]
	public class SerializableCameraPositionV1
	{
		/// <summary>
		/// Creates a new instance of the <see cref="SerializableCameraPositionV1"/> class. This constructor is required for deserialization.
		/// </summary>
		public SerializableCameraPositionV1()
		{
			// Used for deserialization
		}

		/// <summary>
		/// Creates a new instance of the <see cref="SerializableCameraPositionV1"/> class.
		/// </summary>
		/// <param name="numpadKey">The associated numpad key.</param>
		/// <param name="cameraPosition">The camera position to serialize.</param>
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

		/// <summary>
		/// Gets or sets the associated numpad key.
		/// </summary>
		public int NumpadKey { get; set; }

		/// <summary>
		/// Gets or sets the position of the camera on the X axis.
		/// </summary>
		public float PositionX { get; set; }

		/// <summary>
		/// Gets or sets the position of the camera on the Y axis.
		/// </summary>
		public float PositionY { get; set; }

		/// <summary>
		/// Gets or sets the position of the camera on the Z axis.
		/// </summary>
		public float PositionZ { get; set; }

		/// <summary>
		/// Gets or sets the rotation of the camera on the X axis.
		/// </summary>
		public float RotationX { get; set; }

		/// <summary>
		/// Gets or sets the rotation of the camera on the Y axis.
		/// </summary>
		public float RotationY { get; set; }

		/// <summary>
		/// Gets or sets the zoom level of the camera.
		/// </summary>
		public float ZoomLevel { get; set; }

		/// <summary>
		/// Converts the serialized value to a <see cref="CameraPosition"/>.
		/// </summary>
		/// <returns>The created camera position.</returns>
		public CameraPosition ToCameraPosition()
		{
			return new CameraPosition
			{
				Position = new Vector3(PositionX, PositionY, PositionZ),
				RotationX = RotationX,
				RotationY = RotationY,
				ZoomLevel = ZoomLevel
			};
		}
	}
}
