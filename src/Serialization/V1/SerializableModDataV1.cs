namespace ISCameraMod.Serialization.V1
{
	using System;
	using System.Collections.Generic;
	using ISCameraMod.Model;

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
		/// Creates a new instance of the <see cref="SerializableModDataV1"/> class.
		/// </summary>
		/// <param name="modData">The mod data to serialize.</param>
		public SerializableModDataV1(ModData modData)
			: this()
		{
			modData = modData ?? throw new ArgumentNullException(nameof(modData));

			CameraMoveDuration = modData.CameraMoveDuration;

			foreach (var entry in modData.CameraPositions)
			{
				CameraPositions.Add(new SerializableCameraPositionV1(entry.Key, entry.Value));
			}
		}

		/// <summary>
		/// Gets or sets the duration for moving the camera to its target position (in seconds).
		/// </summary>
		public float CameraMoveDuration { get; set; }

		/// <summary>
		/// Gets the list of serialized camera positions.
		/// </summary>
		public List<SerializableCameraPositionV1> CameraPositions { get; }
	}
}
