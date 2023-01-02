namespace ISCameraMod.Serialization.V1
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a serializable mod data with data version 1.
	/// </summary>
	[Serializable]
	public class SerializableModDataV1 : SerializableVersionInfo
	{
		/// <summary>
		/// Creates a new instance of the <see cref="SerializableModDataV1"/> class.
		/// </summary>
		public SerializableModDataV1()
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
