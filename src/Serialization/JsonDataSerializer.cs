namespace ISCameraMod.Serialization
{
	using ISCameraMod.Model;
	using ISCameraMod.Serialization.V1;
	using ISCameraMod.Wrapper;
	using Newtonsoft.Json;
	using UnityEngine;

	/// <summary>
	/// A serializer which uses JSON for serializing and deserializing the mod data.
	/// </summary>
	public class JsonDataSerializer : ISerializer
	{
		private readonly ILogger<JsonDataSerializer> _logger;

		/// <summary>
		/// Creates a new instance of the <see cref="JsonDataSerializer"/> class.
		/// </summary>
		/// <param name="logger">The logger to use. Can be null.</param>
		public JsonDataSerializer(ILogger<JsonDataSerializer> logger)
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

			var serializableCameraPositions = new SerializableModDataV1(modData);
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
			var serializableModData = JsonConvert.DeserializeObject<SerializableModDataV1>(data);
			if (serializableModData == null)
			{
				Log("Deserialized mod data is null");
				return null;
			}

			var result = new ModData
			{
				CameraMoveDuration = serializableModData.CameraMoveDuration
			};

			foreach (var serializablePosition in serializableModData.CameraPositions)
			{
				if (result.CameraPositions.ContainsKey(serializablePosition.NumpadKey))
				{
					Log($"Multiple entries with numpad key '{serializablePosition.NumpadKey}', replacing already loaded camera position");
					result.CameraPositions.Remove(serializablePosition.NumpadKey);
				}

				result.CameraPositions.Add(serializablePosition.NumpadKey, serializablePosition.ToCameraPosition());
			}

			return result;
		}

		private void Log(string message)
		{
			_logger?.Log(message);
		}
	}
}
