namespace ISCameraMod.Model
{
	using UnityEngine;

	/// <summary>
	/// Describes the position of the camera in the world.
	/// </summary>
	public struct CameraPosition
	{
		/// <summary>
		/// The position of the camera on the X, Y and Z axis.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// The rotation of the camera on the X axis.
		/// </summary>
		public float RotationX;

		/// <summary>
		/// The rotation of the camera on the Y axis.
		/// </summary>
		public float RotationY;

		/// <summary>
		/// The zoom level of the camera.
		/// </summary>
		public float ZoomLevel;
	}
}
