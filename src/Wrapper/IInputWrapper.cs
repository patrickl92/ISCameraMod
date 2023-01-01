namespace ISCameraMod.Wrapper
{
	public interface IInputWrapper
	{
		int? GetPressedNumpadKey();

		bool IsSaveModifierKeyPressed();
	}
}
