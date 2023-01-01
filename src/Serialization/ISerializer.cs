namespace ISCameraMod.Serialization
{
	using System.Collections.Generic;
	using ISCameraMod.Model;

	/// <summary>
	/// Interface for serializing the camera positions.
	/// </summary>
	public interface ISerializer
	{
		/// <summary>
		/// Serializes the camera positions into a string value.
		/// </summary>
		/// <param name="cameraPositions">The camera positions to serialize.</param>
		/// <returns>A string containing the camera positions.</returns>
		string Serialize(IReadOnlyDictionary<int, CameraPosition> cameraPositions);

		/// <summary>
		/// Deserializes the camera positions from a string value.
		/// </summary>
		/// <param name="data">The string containing the camera positions.</param>
		/// <returns>The deserialized camera positions</returns>
		IReadOnlyDictionary<int, CameraPosition> Deserialize(string data);
	}
}
