namespace ISCameraMod
{
	using System;
	using System.Collections.Generic;
	using ISCameraMod.Model;
	using ISCameraMod.Wrapper;

	public class ShortcutViewHandler
	{
		private readonly IInputWrapper _inputWrapper;

		private readonly ICameraWrapper _cameraWrapper;

		private readonly ILogger<ShortcutViewHandler> _logger;

		public ShortcutViewHandler(IInputWrapper inputWrapper, ICameraWrapper cameraWrapper, ILogger<ShortcutViewHandler> logger)
		{
			_inputWrapper = inputWrapper ?? throw new ArgumentNullException(nameof(inputWrapper));
			_cameraWrapper = cameraWrapper ?? throw new ArgumentNullException(nameof(cameraWrapper));
			_logger = logger;

			ShortcutViews = new Dictionary<int, CameraPosition>();
		}

		public Dictionary<int, CameraPosition> ShortcutViews { get; }

		public bool FrameUpdate()
		{
			var shortcutViewsChanged = false;

			var numpadKey = _inputWrapper.GetPressedNumpadKey();
			if (numpadKey.HasValue && _cameraWrapper.IsPlayerCameraActive())
			{
				if (_inputWrapper.IsSaveModifierKeyPressed())
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

			ShortcutViews.Add(numpadKey, _cameraWrapper.GetPlayerCameraPosition());

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

			_cameraWrapper.SetPlayerCameraPosition(ShortcutViews[numpadKey]);
		}

		private void Log(string message)
		{
			_logger?.Log(message);
		}
	}
}
