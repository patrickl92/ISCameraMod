﻿namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using ISCameraMod.Wrapper;
	using Newtonsoft.Json;

	[Serializable]
	public class CameraMod : Mod
	{
		[JsonIgnore] // Do not serialize this field
		private readonly ShortcutViewHandler _shortcutViewHandler;

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
			_shortcutViewHandler = new ShortcutViewHandler(new ISLogger<ShortcutViewHandler>(), new CameraModSerializer(new ISLogger<CameraModSerializer>()));
		}

		public override void Load()
		{
			_shortcutViewHandler.Load(_serializedData);
		}

		public override void Start()
		{
		}

		public override void FrameUpdate()
		{
			if (_shortcutViewHandler.FrameUpdate())
			{
				_serializedData = _shortcutViewHandler.Save();
			}
		}

		public override void SimulationUdpate()
		{
		}
	}
}
