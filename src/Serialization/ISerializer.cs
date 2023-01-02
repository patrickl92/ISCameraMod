namespace ISCameraMod.Serialization
{
	using ISCameraMod.Model;

	/// <summary>
	/// Interface for serializing the mod data.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Serializes the mod data into a string value.
		/// </summary>
		/// <param name="modData">The mod data to serialize.</param>
		/// <returns>A string containing the serialized mod data.</returns>
		string Serialize(ModData modData);

		/// <summary>
		/// Deserializes the mod data from a string value.
		/// </summary>
		/// <param name="dataString">The string containing the serialized mod data.</param>
		/// <returns>The deserialized mod data.</returns>
		ModData Deserialize(string dataString);
	}
}
