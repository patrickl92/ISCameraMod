namespace ISCameraMod.Serialization
{
	using System.Collections.Generic;
	using ISCameraMod.Model;
	using ISCameraMod.Serialization.V1;
	using ISCameraMod.Wrapper;
	using Newtonsoft.Json;
	using UnityEngine;

	public class CameraModSerializer : ISerializer
	{
		private readonly ILogger<CameraModSerializer> _logger;

		public CameraModSerializer(ILogger<CameraModSerializer> logger)
		{
			_logger = logger;
		}

		public string Serialize(IReadOnlyDictionary<int, CameraPosition> cameraPositions)
		{
			if (cameraPositions == null)
			{
				return null;
			}

			var serializableCameraPositions = new SerializableCameraPositionsV1();

			foreach (var entry in cameraPositions)
			{
				serializableCameraPositions.CameraPositions.Add(new SerializableCameraPositionV1(entry.Key, entry.Value));
			}

			Log("Serializing " + serializableCameraPositions.CameraPositions.Count + " camera position(s)");

			return JsonConvert.SerializeObject(serializableCameraPositions, Formatting.None);
		}

		public IReadOnlyDictionary<int, CameraPosition> Deserialize(string data)
		{
			if (data != null)
			{
				SerializableVersionInfo versionInfo;

				try
				{
					versionInfo = JsonConvert.DeserializeObject<SerializableVersionInfo>(data);
				}
				catch (JsonReaderException exception)
				{
					Log($"Could not deserialize data as JSON: {exception.Message}");
					versionInfo = null;
				}

				if (versionInfo != null)
				{
					Log($"Version of data to load: {versionInfo.Version}");

					switch (versionInfo.Version)
					{
						case 1:
							return DeserializeV1(data);
						default:
							Log("Unsupported data version");
							break;
					}
				}
				else
				{
					Log("Deserialized version info is null");
				}
			}

			return new Dictionary<int, CameraPosition>();
		}

		private IReadOnlyDictionary<int, CameraPosition> DeserializeV1(string data)
		{
			var result = new Dictionary<int, CameraPosition>();

			var serializableCameraPositions = JsonConvert.DeserializeObject<SerializableCameraPositionsV1>(data);
			if (serializableCameraPositions != null)
			{
				foreach (var serializablePosition in serializableCameraPositions.CameraPositions)
				{
					if (result.ContainsKey(serializablePosition.NumpadKey))
					{
						Log($"Multiple entries with numpad key '{serializablePosition.NumpadKey}', replacing already loaded camera position");
						result.Remove(serializablePosition.NumpadKey);
					}

					var cameraPosition = new CameraPosition
					{
						Position = new Vector3(serializablePosition.PositionX, serializablePosition.PositionY, serializablePosition.PositionZ),
						RotationX = serializablePosition.RotationX,
						RotationY = serializablePosition.RotationY,
						ZoomLevel = serializablePosition.ZoomLevel
					};

					result.Add(serializablePosition.NumpadKey, cameraPosition);
				}
			}

			return result;
		}

		private void Log(string message)
		{
			_logger?.Log(message);
		}
	}
}
