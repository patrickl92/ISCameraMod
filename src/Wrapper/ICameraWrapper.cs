namespace ISCameraMod.Wrapper
{
	using System;
	using ISCameraMod.Model;

	/// <summary>
	/// Interface for wrapping the InfraSpace camera.
	/// </summary>
	public interface ICameraWrapper
	{
		/// <summary>
		/// Gets or sets the duration for moving the camera to its target position.
		/// </summary>
		TimeSpan CameraMoveDuration { get; set; }

		/// <summary>
		/// Updates the movement of the camera.
		/// </summary>
		void FrameUpdate();

		/// <summary>
		/// Gets the current camera position of the player in the world.
		/// </summary>
		/// <returns>The current camera position.</returns>
		CameraPosition GetPlayerCameraPosition();

		/// <summary>
		/// Sets the current camera position of the player in the world.
		/// </summary>
		/// <param name="cameraPosition">The camera position to set.</param>
		void MovePlayerCameraToPosition(CameraPosition cameraPosition);

		/// <summary>
		/// Get a value indicating whether the player camera is currently active.
		/// An active camera means that the player is able to move the camera in the world, and is not e.g. in the loading screen or main menu.
		/// </summary>
		/// <returns>True if the player can move the camera, otherwise false.</returns>
		bool IsPlayerCameraActive();
	}
}
