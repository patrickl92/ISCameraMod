namespace ISCameraMod.Wrapper
{
	using ISCameraMod.Model;
	using UnityEngine;

	/// <summary>
	/// Wrapper for the InfraSpace camera.
	/// </summary>
	internal class ISCameraWrapper : ICameraWrapper
	{
		/// <summary>
		/// The duration for moving the camera to its target position (in seconds).
		/// </summary>
		private const float CameraMoveDuration = 0.5f;

		private AnimatedCameraPosition _animatedCameraPosition;

		/// <summary>
		/// Updates the movement of the camera.
		/// </summary>
		public void FrameUpdate()
		{
			if (_animatedCameraPosition != null)
			{
				if (!_animatedCameraPosition.Finished)
				{
					SetCameraPosition(_animatedCameraPosition.Evaluate(Time.time));
				}
				else
				{
					// Animated moving has finished, so set the target position and clear the animation instance
					SetCameraPosition(_animatedCameraPosition.TargetPosition);
					_animatedCameraPosition = null;
				}
			}
		}

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
		/// Starts to move the camera position of the player towards the target position in the world.
		/// </summary>
		/// <param name="cameraPosition">The target camera position.</param>
		public void MovePlayerCameraToPosition(CameraPosition cameraPosition)
		{
			_animatedCameraPosition = new AnimatedCameraPosition(GetPlayerCameraPosition(), cameraPosition, Time.time, Time.time + CameraMoveDuration);
		}

		/// <summary>
		/// Get a value indicating whether the player camera is currently active.
		/// An active camera means that the player is able to move the camera in the world, and is not e.g. in the loading screen or main menu.
		/// </summary>
		/// <returns>True if the player can move the camera, otherwise false.</returns>
		public bool IsPlayerCameraActive()
		{
			if (!WorldScripts.Inst.saveManager.HasFinishedLoading())
			{
				// Game is still loading
				return false;
			}

			if (GameScripts.Inst.modalManager.IsGameModal())
			{
				// A modal overlay (e.g. the main menu or research panel) is open
				return false;
			}

			// TODO: Check if the spaceship view is active
			return true;
		}

		private void SetCameraPosition(CameraPosition cameraPosition)
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
		/// Helper class to animate the movement of the camera to its target position.
		/// </summary>
		private class AnimatedCameraPosition
		{
			private readonly float _timeEnd;
			private readonly AnimationCurve _animationPositionX;
			private readonly AnimationCurve _animationPositionY;
			private readonly AnimationCurve _animationPositionZ;
			private readonly AnimationCurve _animationRotationX;
			private readonly AnimationCurve _animationRotationY;
			private readonly AnimationCurve _animationZoomLevel;

			public AnimatedCameraPosition(CameraPosition from, CameraPosition to, float timeStart, float timeEnd)
			{
				_timeEnd = timeEnd;
				_animationPositionX = AnimationCurve.EaseInOut(timeStart, from.Position.x, timeEnd, to.Position.x);
				_animationPositionY = AnimationCurve.EaseInOut(timeStart, from.Position.y, timeEnd, to.Position.y);
				_animationPositionZ = AnimationCurve.EaseInOut(timeStart, from.Position.z, timeEnd, to.Position.z);
				_animationRotationX = AnimationCurve.EaseInOut(timeStart, from.RotationX, timeEnd, to.RotationX);
				_animationRotationY = AnimationCurve.EaseInOut(timeStart, from.RotationY, timeEnd, to.RotationY);
				_animationZoomLevel = AnimationCurve.EaseInOut(timeStart, from.ZoomLevel, timeEnd, to.ZoomLevel);

				TargetPosition = to;
			}

			public bool Finished => Time.time >= _timeEnd;

			public CameraPosition TargetPosition { get; }

			public CameraPosition Evaluate(float time)
			{
				return new CameraPosition
				{
					Position = new Vector3(_animationPositionX.Evaluate(time), _animationPositionY.Evaluate(time), _animationPositionZ.Evaluate(time)),
					RotationX = _animationRotationX.Evaluate(time),
					RotationY = _animationRotationY.Evaluate(time),
					ZoomLevel = _animationZoomLevel.Evaluate(time)
				};
			}
		}
	}
}
