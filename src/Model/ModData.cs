namespace ISCameraMod.Model
{
	using System.Collections.Generic;

	/// <summary>
	/// Holds the data of the mod.
	/// </summary>
	public class ModData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ModData"/> class.
		/// </summary>
		public ModData()
		{
			CameraPositions = new Dictionary<int, CameraPosition>();
		}

		/// <summary>
		/// Gets a dictionary which contains the shortcut views.
		/// The key of the dictionary is the associated numpad key of the camera position.
		/// </summary>
		public Dictionary<int, CameraPosition> CameraPositions { get; }
	}
}
