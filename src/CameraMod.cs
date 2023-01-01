namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using Newtonsoft.Json;

	/// <summary>
	/// The main entry point of the mod.
	/// </summary>
	/// <remarks>
	/// This class only contains the logic to persist and load the data of the mod. All other logic is contained in separate classes, to split up the business logic from the persistence layer (which is this class).
	/// The way InfraSpace is persisting mod data is by just serializing this class directly into the game file.
	/// This approach makes it hard to change things in the mod if you want to always be backwards compatible to previous versions of the mod.
	/// For example, if you would rename a property or field, this data would be lost.
	/// Also, if you add/rename properties of your types, you can't be sure about the state of the data once your mod has been loaded and the data was applied from the game file.
	/// In order to avoid this, this mod comes with its own serialization logic. The camera positions are serialized using the JSON format, and the result of that process is stored within a string field of the mod class.
	/// Since it is just a string, we don't need to care about anything. InfraSpace will write this string into the game file if the game is saved, and restore the string from the game file when it is loaded.
	/// During loading of the mod, this string will be deserialized by the serialization logic of the mod to restore the internal mod data.
	/// This allows us to store data independently of the InfraSpace serialization process, and to be more flexible with the data we persist.
	/// </remarks>
	[Serializable]
	public class CameraMod : Mod
	{
		[JsonIgnore] // Do not serialize this field
		private readonly IShortcutViewHandler _shortcutViewHandler;

		[JsonIgnore] // Do not serialize this field
		private readonly ISerializer _serializer;

		/// <summary>
		/// This field is used for persisting the camera positions to the current game, so they will be available when the game is loaded again.
		/// </summary>
		[JsonProperty("SerializedData")]
		private string _serializedData;

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraMod"/> class.
		/// </summary>
		public CameraMod()
		{
			_serializer = CameraModFactory.CreateSerializerFunc();
			_shortcutViewHandler = CameraModFactory.CreateShortcutViewHandlerFunc();
		}

		/// <summary>
		/// Gets called when the mod is loaded.
		/// Deserialization of this class has already been done at this stage.
		/// The serialized data is deserialized into the internal mod data here.
		/// </summary>
		public override void Load()
		{
			var loadedViews = _serializer.Deserialize(_serializedData);

			_shortcutViewHandler.ShortcutViews.Clear();

			foreach (var entry in loadedViews)
			{
				_shortcutViewHandler.ShortcutViews.Add(entry.Key, entry.Value);
			}
		}

		/// <summary>
		/// Gets called when the games is started.
		/// </summary>
		public override void Start()
		{
		}

		/// <summary>
		/// Gets called once per frame.
		/// The call is forwarded to the <see cref="ShortcutViewHandler"/> and the current mod data is serialized if something has changed.
		/// </summary>
		public override void FrameUpdate()
		{
			if (_shortcutViewHandler.FrameUpdate())
			{
				// New/changed camera position, serialize the data so it will be persisted when the mod class is serialized by InfraSpace
				_serializedData = _serializer.Serialize(_shortcutViewHandler.ShortcutViews);
			}
		}

		/// <summary>
		/// Gets called when other simulations are run.
		/// </summary>
		public override void SimulationUdpate()
		{
		}
	}
}
