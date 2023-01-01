namespace ISCameraMod.Serialization
{
	using System.Collections.Generic;
	using ISCameraMod.Model;

	public interface ISerializer
	{
		string Serialize(IReadOnlyDictionary<int, CameraPosition> cameraPositions);

		IReadOnlyDictionary<int, CameraPosition> Deserialize(string data);
	}
}
