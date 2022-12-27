namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using Newtonsoft.Json;
	using UnityEngine;

	[Serializable]
	public class CameraMod : Mod
	{
		[JsonIgnore] // Do not directly serialize the saved positions
		private readonly CameraPosition[] _savedPositions;

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
			_savedPositions = new CameraPosition[10];

			for (int i = 0; i < _savedPositions.Length; i++)
			{
				// Mark all positions as not valid initially
				// If a position is not valid, it does not get applied
				_savedPositions[i].IsValid = false;
			}
		}

		public override void Load()
		{
			Log("Deserializing positions");
			CameraModSerializer.Deserialize(_serializedData, _savedPositions);
		}

		public override void Start()
		{
		}

		public override void FrameUpdate()
		{
			var numpadKey = UnityInputWrapper.GetPressedNumpadKey();
			if (numpadKey.HasValue)
			{
				if (UnityInputWrapper.IsSaveModifierKeyPressed())
				{
					SavePosition(numpadKey.Value);
				}
				else
				{
					ApplyPosition(numpadKey.Value);
				}
			}
		}

		public override void SimulationUdpate()
		{
		}

		private void SavePosition(int numpadKey)
		{
			Log($"Saving current camera position for numpad key '{numpadKey}'");

			// Create a serializable instance of the InfraSpace CameraMovement and then save the position data from this instance
			var serializableCameraMovement = new CameraMovement.Serializable(WorldScripts.Inst.cameraMovement);

			_savedPositions[numpadKey].Position = serializableCameraMovement.position;
			_savedPositions[numpadKey].RotationX = serializableCameraMovement.rotX;
			_savedPositions[numpadKey].RotationY = serializableCameraMovement.rotY;
			_savedPositions[numpadKey].ZoomLevel = serializableCameraMovement.zoomLevel;

			// Position data has been set, so allow it to be applied by marking it as valid
			_savedPositions[numpadKey].IsValid = true;

			_serializedData = CameraModSerializer.Serialize(_savedPositions);
		}

		private void ApplyPosition(int numpadKey)
		{
			if (!_savedPositions[numpadKey].IsValid)
			{
				Log($"Camera position for numpad key '{numpadKey}' is not valid");
				return;
			}

			Log($"Applying saved camera position for numpad key '{numpadKey}'");

			// Create a serializable instance of the InfraSpace CameraMovement and then use this instance to apply the position
			var serializableCameraMovement = new CameraMovement.Serializable
			{
				position = _savedPositions[numpadKey].Position,
				rotX = _savedPositions[numpadKey].RotationX,
				rotY = _savedPositions[numpadKey].RotationY,
				zoomLevel = _savedPositions[numpadKey].ZoomLevel
			};

			WorldScripts.Inst.cameraMovement.InitFromSerializable(serializableCameraMovement);
		}

		private void Log(string message)
		{
			MonoBehaviour.print($"{GetType().Name}: {message}");
		}
	}
}
