namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using ISCameraMod.Wrapper;

	public class ShortcutViewHandler
	{
		private readonly CameraPosition[] _savedPositions;

		private readonly CameraModSerializer _serializer;

		private readonly ILogger<ShortcutViewHandler> _logger;

		public ShortcutViewHandler(ILogger<ShortcutViewHandler> logger, CameraModSerializer serializer)
		{
			_logger = logger;
			_serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

			_savedPositions = new CameraPosition[10];

			for (int i = 0; i < _savedPositions.Length; i++)
			{
				// Mark all positions as not valid initially
				// If a position is not valid, it does not get applied
				_savedPositions[i].IsValid = false;
			}
		}

		public void Load(string data)
		{
			Log("Deserializing positions");
			_serializer.Deserialize(data, _savedPositions);
		}

		public string Save()
		{
			Log("Serializing positions");
			return _serializer.Serialize(_savedPositions);
		}

		public bool FrameUpdate()
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

			return false;
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
			_logger?.Log(message);
		}
	}
}
