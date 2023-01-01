namespace ISCameraMod.Wrapper
{
	public interface ICameraWrapper
	{
		CameraPosition GetCurrentPlayerCameraPosition();

		void SetCurrentPlayerCameraPosition(CameraPosition cameraPosition);
	}
}
