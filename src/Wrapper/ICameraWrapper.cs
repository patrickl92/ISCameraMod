namespace ISCameraMod.Wrapper
{
	using ISCameraMod.Model;

	public interface ICameraWrapper
	{
		CameraPosition GetCurrentPlayerCameraPosition();

		void SetCurrentPlayerCameraPosition(CameraPosition cameraPosition);
	}
}
