namespace ISCameraMod
{
	using System.Collections.Generic;
	using ISCameraMod.Model;

	/// <summary>
	/// Interface for the <see cref="ShortcutViewHandler"/> class.
	/// This interface allows the implementation to be mocked in a unit test.
	/// </summary>
	public interface IShortcutViewHandler
	{
		/// <summary>
		/// Gets a dictionary which contains the shortcut views.
		/// The key of the dictionary is the associated numpad key of the camera position.
		/// </summary>
		Dictionary<int, CameraPosition> ShortcutViews { get; }

		/// <summary>
		/// Gets called every frame to perform the business logic.
		///  </summary>
		void FrameUpdate();
	}
}
