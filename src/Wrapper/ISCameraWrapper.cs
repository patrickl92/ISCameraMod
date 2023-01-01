namespace ISCameraMod.Wrapper
{
	using ISCameraMod.Model;

	/// <summary>
	/// Wrapper for the InfraSpace camera.
	/// </summary>
	internal class ISCameraWrapper : ICameraWrapper
	{
		/// <summary>
		/// Gets the current camera position of the player in the world.
		/// </summary>
		/// <returns>The current camera position.</returns>
		public CameraPosition GetPlayerCameraPosition()
		{
			// Create a serializable instance of the InfraSpace CameraMovement and then save the position data from this instance
			var serializableCameraMovement = new CameraMovement.Serializable(WorldScripts.Inst.cameraMovement);
			
			return new CameraPosition
			{
				Position = serializableCameraMovement.position,
				RotationX = serializableCameraMovement.rotX,
				RotationY = serializableCameraMovement.rotY,
				ZoomLevel = serializableCameraMovement.zoomLevel
			};
		}

		/// <summary>
		/// Sets the current camera position of the player in the world.
		/// </summary>
		/// <param name="cameraPosition">The camera position to set.</param>
		public void SetPlayerCameraPosition(CameraPosition cameraPosition)
		{
			// Create a serializable instance of the InfraSpace CameraMovement and then use this instance to apply the position
			var serializableCameraMovement = new CameraMovement.Serializable
			{
				position = cameraPosition.Position,
				rotX = cameraPosition.RotationX,
				rotY = cameraPosition.RotationY,
				zoomLevel = cameraPosition.ZoomLevel
			};

			WorldScripts.Inst.cameraMovement.InitFromSerializable(serializableCameraMovement);
		}

		/// <summary>
		/// Get a value indicating whether the player camera is currently active.
		/// An active camera means that the player is able to move the camera in the world, and is not e.g. in the loading screen or main menu.
		/// </summary>
		/// <returns>True if the player can move the camera, otherwise false.</returns>
		public bool IsPlayerCameraActive()
		{
			// TODO: Implement
			return true;
		}
	}
}
