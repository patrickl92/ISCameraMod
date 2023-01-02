namespace ISCameraMod.Wrapper
{
	using System;
	using System.Linq;
	using ISCameraMod.Model;
	using UnityEngine;

	/// <summary>
	/// Wrapper for the InfraSpace camera.
	/// </summary>
	internal class ISCameraWrapper : ICameraWrapper
	{
		private AnimatedCameraPosition _animatedCameraPosition;

		/// <summary>
		/// Creates a new instance of the <see cref="ISCameraWrapper"/> class.
		/// </summary>
		public ISCameraWrapper()
		{
			CameraMoveDuration = TimeSpan.FromSeconds(0.5);
		}

		/// <summary>
		/// Gets or sets the duration for moving the camera to its target position (in seconds).
		/// </summary>
		public TimeSpan CameraMoveDuration { get; set; }

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
			if (CameraMoveDuration == TimeSpan.Zero)
			{
				// No animation, set the target position directly
				SetCameraPosition(cameraPosition);
			}
			else
			{
				_animatedCameraPosition = new AnimatedCameraPosition(GetPlayerCameraPosition(), cameraPosition, Time.time, Time.time + (float)CameraMoveDuration.TotalSeconds);
			}
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

			if (WorldScripts.Inst.simulator.simulationBlockers.OfType<SpaceScene>().Any())
			{
				// Spaceship view is active
				return false;
			}

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
				var normalizedRotationY = NormalizeRotationY(from.RotationY, to.RotationY);

				_timeEnd = timeEnd;
				_animationPositionX = AnimationCurve.EaseInOut(timeStart, from.Position.x, timeEnd, to.Position.x);
				_animationPositionY = AnimationCurve.EaseInOut(timeStart, from.Position.y, timeEnd, to.Position.y);
				_animationPositionZ = AnimationCurve.EaseInOut(timeStart, from.Position.z, timeEnd, to.Position.z);
				_animationRotationX = AnimationCurve.EaseInOut(timeStart, from.RotationX, timeEnd, to.RotationX);
				_animationRotationY = AnimationCurve.EaseInOut(timeStart, normalizedRotationY.From, timeEnd, normalizedRotationY.To);
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

			/// <summary>
			/// This methods normalizes the start and end position of the rotation around the Y axis. 
			/// </summary>
			/// <remarks>
			/// The camera can be rotated between 0° and 360° (or 359°, since 0° and 360° are resulting in the same rotation).
			/// If the player rotates the camera a few times, the rotation can be more than 360°, or less than 0°, depending on the rotation direction.
			/// The animation may would rotate a few times before reaching the end position if this would not be taken into account.
			/// The start and end position of the rotation are adapted to animate the shortest distance between the positions.
			/// </remarks>
			/// <param name="from">The start position.</param>
			/// <param name="to">The end position.</param>
			/// <returns>The adapted positions.</returns>
			private (float From, float To) NormalizeRotationY(float from, float to)
			{
				// Step 1: Adapt the values to be between 0 and 360
				while (from > 360)
				{
					from -= 360;
				}

				while (from < 0)
				{
					from += 360;
				}

				while (to > 360)
				{
					to -= 360;
				}

				while (to < 0)
				{
					to += 360;
				}

				// Step 2: Check if it is shorter to animate from the start position to the end position, or vice versa
				var diff = Math.Abs(to - from);
				if (diff > 180)
				{
					// It is shorter to animate "the other way round"
					// Let's say, the From position is 10° and the To position is 350°, so it takes only 20° to go from 10 to 0, and then from 360 to 350
					// An easy way to accomplish that is to simply adapt the To position by either adding or subtracting 360°, depending on the shorter direction
					// In the mention example, we would then animate from 10° to -10°
					to += (to > from ? -360 : 360);
				}

				return (from, to);
			}
		}
	}
}
