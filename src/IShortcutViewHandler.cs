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
		/// Gets the current shortcut views.
		/// The key of the dictionary is the associated numpad key of the camera position.
		/// </summary>
		Dictionary<int, CameraPosition> ShortcutViews { get; }

		/// <summary>
		/// Gets called every frame to perform the business logic.
		///  </summary>
		/// <returns>True if the shortcut views have been changed during that frame, otherwise false.</returns>
		bool FrameUpdate();
	}
}
