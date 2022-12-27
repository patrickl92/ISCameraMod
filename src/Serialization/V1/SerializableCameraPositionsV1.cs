﻿namespace ISCameraMod.Serialization.V1
{
	using System.Collections.Generic;

	public class SerializableCameraPositionsV1 : SerializableVersionInfo
	{
		public SerializableCameraPositionsV1()
		{
			Version = 1;
			CameraPositions = new List<SerializableCameraPositionV1>();
		}

		public List<SerializableCameraPositionV1> CameraPositions { get; }
	}
}