namespace ISCameraMod.Serialization.V1
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a list of serialized camera positions with data version 1.
	/// </summary>
	[Serializable]
	public class SerializableCameraPositionsV1 : SerializableVersionInfo
	{
		/// <summary>
		/// Creates a new instance of the <see cref="SerializableCameraPositionsV1"/> class.
		/// </summary>
		public SerializableCameraPositionsV1()
		{
			Version = 1;
			CameraPositions = new List<SerializableCameraPositionV1>();
		}

		/// <summary>
		/// Gets the list of serialized camera positions.
		/// </summary>
		public List<SerializableCameraPositionV1> CameraPositions { get; }
	}
}
