namespace ISCameraMod.Serialization
{
	using System.Collections.Generic;
	using ISCameraMod.Model;
	using ISCameraMod.Serialization.V1;
	using ISCameraMod.Wrapper;
	using Newtonsoft.Json;
	using UnityEngine;

	/// <summary>
	/// The default serializer, which uses JSON for serializing and deserializing the mod data.
	/// </summary>
	public class CameraModSerializer : ISerializer
	{
		private readonly ILogger<CameraModSerializer> _logger;

		/// <summary>
		/// Creates a new instance of the <see cref="CameraModSerializer"/> class.
		/// </summary>
		/// <param name="logger">The logger to use. Can be null.</param>
		public CameraModSerializer(ILogger<CameraModSerializer> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Serializes the mod data into a JSON string.
		/// </summary>
		/// <param name="modData">The mod data to serialize.</param>
		/// <returns>A JSON string containing the serialized mod data, or null, if the mod data is null.</returns>
		public string Serialize(ModData modData)
		{
			if (modData == null)
			{
				return null;
			}

			var serializableCameraPositions = new SerializableModDataV1();

			foreach (var entry in modData.CameraPositions)
			{
				serializableCameraPositions.CameraPositions.Add(new SerializableCameraPositionV1(entry.Key, entry.Value));
			}

			return JsonConvert.SerializeObject(serializableCameraPositions, Formatting.None);
		}

		/// <summary>
		/// Deserializes the mod data from a JSON string.
		/// </summary>
		/// <param name="dataString">The JSON string containing the serialized mod data.</param>
		/// <returns>The deserialized mod data, or null, if the deserialization of the data has failed.</returns>
		public ModData Deserialize(string dataString)
		{
			if (dataString == null)
			{
				return null;
			}

			SerializableVersionInfo versionInfo;

			try
			{
				versionInfo = JsonConvert.DeserializeObject<SerializableVersionInfo>(dataString);
			}
			catch (JsonReaderException exception)
			{
				Log($"Could not deserialize data as JSON: {exception.Message}");
				return null;
			}

			if (versionInfo == null)
			{
				Log("Deserialized version info is null");
				return null;
			}

			Log($"Version of data to load: {versionInfo.Version}");

			switch (versionInfo.Version)
			{
				case 1:
					return DeserializeV1(dataString);
				default:
					Log("Unsupported data version");
					return null;
			}
		}

		private ModData DeserializeV1(string data)
		{
			var result = new ModData();

			var serializableCameraPositions = JsonConvert.DeserializeObject<SerializableModDataV1>(data);
			if (serializableCameraPositions != null)
			{
				foreach (var serializablePosition in serializableCameraPositions.CameraPositions)
				{
					if (result.CameraPositions.ContainsKey(serializablePosition.NumpadKey))
					{
						Log($"Multiple entries with numpad key '{serializablePosition.NumpadKey}', replacing already loaded camera position");
						result.CameraPositions.Remove(serializablePosition.NumpadKey);
					}

					var cameraPosition = new CameraPosition
					{
						Position = new Vector3(serializablePosition.PositionX, serializablePosition.PositionY, serializablePosition.PositionZ),
						RotationX = serializablePosition.RotationX,
						RotationY = serializablePosition.RotationY,
						ZoomLevel = serializablePosition.ZoomLevel
					};

					result.CameraPositions.Add(serializablePosition.NumpadKey, cameraPosition);
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
