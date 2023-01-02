namespace ISCameraMod
{
	using System;
	using System.Collections.Generic;
	using ISCameraMod.Model;
	using ISCameraMod.Wrapper;

	/// <summary>
	/// This class contains the business logic to save and apply camera positions.
	/// </summary>
	public class ShortcutViewHandler : IShortcutViewHandler
	{
		private readonly IInputWrapper _inputWrapper;

		private readonly ICameraWrapper _cameraWrapper;

		private readonly ILogger<ShortcutViewHandler> _logger;

		/// <summary>
		/// Creates a new instance of the <see cref="ShortcutViewHandler"/> class.
		/// </summary>
		/// <param name="inputWrapper">The input wrapper to use.</param>
		/// <param name="cameraWrapper">The camera wrapper to use.</param>
		/// <param name="logger">The logger to use. Can be null.</param>
		public ShortcutViewHandler(IInputWrapper inputWrapper, ICameraWrapper cameraWrapper, ILogger<ShortcutViewHandler> logger)
		{
			_inputWrapper = inputWrapper ?? throw new ArgumentNullException(nameof(inputWrapper));
			_cameraWrapper = cameraWrapper ?? throw new ArgumentNullException(nameof(cameraWrapper));
			_logger = logger;

			ShortcutViews = new Dictionary<int, CameraPosition>();
		}

		/// <summary>
		/// Gets a dictionary which contains the shortcut views.
		/// The key of the dictionary is the associated numpad key of the camera position.
		/// </summary>
		public Dictionary<int, CameraPosition> ShortcutViews { get; }

		/// <summary>
		/// Gets called every frame to perform the business logic.
		///  </summary>
		public void FrameUpdate()
		{
			var numpadKey = _inputWrapper.GetPressedNumpadKey();
			if (numpadKey.HasValue && _cameraWrapper.IsPlayerCameraActive())
			{
				if (_inputWrapper.IsSaveModifierKeyPressed())
				{
					SavePosition(numpadKey.Value);
				}
				else
				{
					ApplyPosition(numpadKey.Value);
				}
			}
		}

		private void SavePosition(int numpadKey)
		{
			Log($"Saving current camera position for numpad key '{numpadKey}'");

			if (ShortcutViews.ContainsKey(numpadKey))
			{
				ShortcutViews.Remove(numpadKey);
			}

			ShortcutViews.Add(numpadKey, _cameraWrapper.GetPlayerCameraPosition());
		}

		private void ApplyPosition(int numpadKey)
		{
			if (!ShortcutViews.ContainsKey(numpadKey))
			{
				Log($"No camera position for numpad key '{numpadKey}'");
				return;
			}

			Log($"Applying saved camera position for numpad key '{numpadKey}'");

			_cameraWrapper.MovePlayerCameraToPosition(ShortcutViews[numpadKey]);
		}

		private void Log(string message)
		{
			_logger?.Log(message);
		}
	}
}
