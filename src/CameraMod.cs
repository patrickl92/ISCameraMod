namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using Newtonsoft.Json;

	[Serializable]
	public class CameraMod : Mod
	{
		[JsonIgnore] // Do not serialize this field
		private readonly IShortcutViewHandler _shortcutViewHandler;

		[JsonIgnore] // Do not serialize this field
		private readonly ISerializer _serializer;

		/**
		 * This field is used for persisting the camera positions to the current game, so they will be available when the game is loaded again.
		 * The way InfraSpace is persisting mod data is by just serializing this class directly into the game file.
		 * This approach makes it hard to change things in the mod if you want to always be backwards compatible to previous versions of the mod.
		 * For example, if you would rename a property or field, this data would be lost.
		 * Also, if you add/rename properties of your types, you can't be sure about the state of the data once your mod has been loaded and the data was applied from the game file.
		 * In order to avoid this, this mod comes with its own serialization logic. The camera positions are serialized using the JSON format, and the result of that process is stored in this field.
		 * Since it is just a string, we don't need to care about anything. InfraSpace will write this string into the game file if the game is saved, and restore the string from the game file when it is loaded.
		 * During loading of the mod, this string will be deserialized by the serialization logic of the mod to restore the internal mod data.
		 * This allows us to store data independently of the InfraSpace serialization process, and to be more flexible with the data we persist.
		 */
		[JsonProperty("SerializedData")]
		private string _serializedData;

		public CameraMod()
		{
			_serializer = CameraModFactory.CreateSerializerFunc();
			_shortcutViewHandler = CameraModFactory.CreateShortcutViewHandlerFunc();
		}

		public override void Load()
		{
			var loadedViews = _serializer.Deserialize(_serializedData);

			_shortcutViewHandler.ShortcutViews.Clear();

			foreach (var entry in loadedViews)
			{
				_shortcutViewHandler.ShortcutViews.Add(entry.Key, entry.Value);
			}
		}

		public override void Start()
		{
		}

		public override void FrameUpdate()
		{
			if (_shortcutViewHandler.FrameUpdate())
			{
				_serializedData = _serializer.Serialize(_shortcutViewHandler.ShortcutViews);
			}
		}

		public override void SimulationUdpate()
		{
		}
	}
}
