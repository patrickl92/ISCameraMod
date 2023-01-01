namespace ISCameraMod.Wrapper
{
	using UnityEngine;

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
