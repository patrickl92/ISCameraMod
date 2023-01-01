namespace ISCameraMod
{
	using System.Collections.Generic;
	using ISCameraMod.Wrapper;

	public class ShortcutViewHandler
	{
		private readonly ILogger<ShortcutViewHandler> _logger;

		public ShortcutViewHandler(ILogger<ShortcutViewHandler> logger)
		{
			_logger = logger;

			ShortcutViews = new Dictionary<int, CameraPosition>();
		}

		public Dictionary<int, CameraPosition> ShortcutViews { get; }

		public bool FrameUpdate()
		{
			var shortcutViewsChanged = false;

			var numpadKey = UnityInputWrapper.GetPressedNumpadKey();
			if (numpadKey.HasValue)
			{
				if (UnityInputWrapper.IsSaveModifierKeyPressed())
				{
					shortcutViewsChanged = SavePosition(numpadKey.Value);
				}
				else
				{
					ApplyPosition(numpadKey.Value);
				}
			}

			return shortcutViewsChanged;
		}

		private bool SavePosition(int numpadKey)
		{
			Log($"Saving current camera position for numpad key '{numpadKey}'");

			if (ShortcutViews.ContainsKey(numpadKey))
			{
				ShortcutViews.Remove(numpadKey);
			}

			// Create a serializable instance of the InfraSpace CameraMovement and then save the position data from this instance
			var serializableCameraMovement = new CameraMovement.Serializable(WorldScripts.Inst.cameraMovement);
			var cameraPosition = new CameraPosition
			{
				Position = serializableCameraMovement.position,
				RotationX = serializableCameraMovement.rotX,
				RotationY = serializableCameraMovement.rotY,
				ZoomLevel = serializableCameraMovement.zoomLevel
			};

			ShortcutViews.Add(numpadKey, cameraPosition);

			return true;
		}

		private void ApplyPosition(int numpadKey)
		{
			if (!ShortcutViews.ContainsKey(numpadKey))
			{
				Log($"No camera position for numpad key '{numpadKey}'");
				return;
			}

			Log($"Applying saved camera position for numpad key '{numpadKey}'");

			// Create a serializable instance of the InfraSpace CameraMovement and then use this instance to apply the position
			var serializableCameraMovement = new CameraMovement.Serializable
			{
				position = ShortcutViews[numpadKey].Position,
				rotX = ShortcutViews[numpadKey].RotationX,
				rotY = ShortcutViews[numpadKey].RotationY,
				zoomLevel = ShortcutViews[numpadKey].ZoomLevel
			};

			WorldScripts.Inst.cameraMovement.InitFromSerializable(serializableCameraMovement);
		}

		private void Log(string message)
		{
			_logger?.Log(message);
		}
	}
}
