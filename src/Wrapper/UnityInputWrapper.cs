namespace ISCameraMod.Wrapper
{
	using UnityEngine;

	/// <summary>
	/// Wrapper for the Unity <see cref="Input"/> class.
	/// </summary>
	internal class UnityInputWrapper : IInputWrapper
	{
		private static readonly KeyCode[] NumpadKeyCodes =
		{
			KeyCode.Keypad0,
			KeyCode.Keypad1,
			KeyCode.Keypad2,
			KeyCode.Keypad3,
			KeyCode.Keypad4,
			KeyCode.Keypad5,
			KeyCode.Keypad6,
			KeyCode.Keypad7,
			KeyCode.Keypad8,
			KeyCode.Keypad9
		};

		/// <summary>
		/// Gets the number of the pressed numpad key. If this method returns true, subsequent calls return false until the key is released and pressed again.
		/// </summary>
		/// <returns>The number of the pressed numpad key, or null if no numpad key is pressed.</returns>
		public int? GetPressedNumpadKey()
		{
			for (int i = 0; i < NumpadKeyCodes.Length; i++)
			{
				if (Input.GetKeyDown(NumpadKeyCodes[i]))
				{
					return i;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a value indicating whether the save modifier key is pressed. This method returns true as long as the correct keys are pressed.
		/// </summary>
		/// <returns>True if the save modifier key is currently pressed, otherwise false.</returns>
		public bool IsSaveModifierKeyPressed()
		{
			if (Input.GetKey(KeyCode.LeftShift)
			    || Input.GetKey(KeyCode.RightShift)
			    || Input.GetKey(KeyCode.LeftAlt)
			    || Input.GetKey(KeyCode.RightAlt)
			    || Input.GetKey(KeyCode.AltGr))
			{
				// Ignore if any additional modifier is pressed
				return false;
			}

			return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		}
	}
}
