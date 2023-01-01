namespace ISCameraMod.Serialization
{
	using System;

	/// <summary>
	/// Base class for the serializable classes. This allows to store the version information within the serialized data, to be later able to apply different deserialization strategies based on that information.
	/// </summary>
	[Serializable]
	public class SerializableVersionInfo
	{
		/// <summary>
		/// Gets or sets the version of the serialized data.
		/// </summary>
		public int Version { get; set; }
	}
}
