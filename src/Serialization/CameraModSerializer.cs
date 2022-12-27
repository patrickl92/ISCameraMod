namespace ISCameraMod.Serialization
{
	using ISCameraMod.Serialization.V1;
	using Newtonsoft.Json;
	using UnityEngine;

	public static class CameraModSerializer
	{
		public static string Serialize(CameraPosition[] cameraPositions)
		{
			if (cameraPositions == null)
			{
				return null;
			}

			var serializableCameraPositions = new SerializableCameraPositionsV1();

			for (var i = 0; i < cameraPositions.Length; i++)
			{
				if (cameraPositions[i].IsValid)
				{
					serializableCameraPositions.CameraPositions.Add(new SerializableCameraPositionV1(i, cameraPositions[i]));
				}
			}

			Log("Serializing " + serializableCameraPositions.CameraPositions.Count + " camera position(s)");

			return JsonConvert.SerializeObject(serializableCameraPositions, Formatting.None);
		}

		public static void Deserialize(string data, CameraPosition[] targetArray)
		{
			if (data == null || targetArray == null)
			{
				return;
			}

			var versionInfo = JsonConvert.DeserializeObject<SerializableVersionInfo>(data);
			if (versionInfo == null)
			{
				Log("Deserialized version info is null");
				return;
			}

			Log($"Version of data to load: {versionInfo.Version}");

			switch (versionInfo.Version)
			{
				case 1:
					DeserializeV1(data, targetArray);
					break;
				default:
					Log("Unsupported data version");
					break;
			}
		}

		private static void DeserializeV1(string data, CameraPosition[] targetArray)
		{
			var serializableCameraPositions = JsonConvert.DeserializeObject<SerializableCameraPositionsV1>(data);
			if (serializableCameraPositions == null)
			{
				return;
			}

			foreach (var serializablePosition in serializableCameraPositions.CameraPositions)
			{
				if (serializablePosition.NumpadKey > targetArray.Length)
				{
					Log($"Invalid numpad key '{serializablePosition.NumpadKey}', skipping camera position");
					continue;
				}

				Log($"Loading camera position for numpad key '{serializablePosition.NumpadKey}'");

				targetArray[serializablePosition.NumpadKey].Position = serializablePosition.Position;
				targetArray[serializablePosition.NumpadKey].RotationX = serializablePosition.RotationX;
				targetArray[serializablePosition.NumpadKey].RotationY = serializablePosition.RotationY;
				targetArray[serializablePosition.NumpadKey].ZoomLevel = serializablePosition.ZoomLevel;

				// Loaded positions are always valid
				targetArray[serializablePosition.NumpadKey].IsValid = true;
			}
		}

		private static void Log(string message)
		{
			MonoBehaviour.print($"{typeof(CameraModSerializer).Name}: {message}");
		}
	}
}
