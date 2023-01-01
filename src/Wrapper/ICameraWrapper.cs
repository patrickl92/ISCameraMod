namespace ISCameraMod.Wrapper
{
	using ISCameraMod.Model;

	public interface ICameraWrapper
	{
		CameraPosition GetPlayerCameraPosition();

		void SetPlayerCameraPosition(CameraPosition cameraPosition);

		bool IsPlayerCameraActive();
	}
}
