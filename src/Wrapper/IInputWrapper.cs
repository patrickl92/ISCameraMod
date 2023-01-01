namespace ISCameraMod.Wrapper
{
	/// <summary>
	/// Interface for wrapping the InfraSpace player input.
	/// </summary>
	public interface IInputWrapper
	{
		/// <summary>
		/// Gets the number of the pressed numpad key. If this method returns true, subsequent calls return false until the key is released and pressed again.
		/// </summary>
		/// <returns>The number of the pressed numpad key, or null if no numpad key is pressed.</returns>
		int? GetPressedNumpadKey();

		/// <summary>
		/// Gets a value indicating whether the save modifier key is pressed. This method returns true as long as the correct keys are pressed.
		/// </summary>
		/// <returns>True if the save modifier key is currently pressed, otherwise false.</returns>
		bool IsSaveModifierKeyPressed();
	}
}
