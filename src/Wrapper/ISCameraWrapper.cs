namespace ISCameraMod.Wrapper
{
	using ISCameraMod.Model;

	internal class ISCameraWrapper : ICameraWrapper
	{
		public CameraPosition GetCurrentPlayerCameraPosition()
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

		public void SetCurrentPlayerCameraPosition(CameraPosition cameraPosition)
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
	}
}
