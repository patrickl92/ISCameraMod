namespace ISCameraMod
{
	using System.Collections.Generic;
	using ISCameraMod.Model;

	public interface IShortcutViewHandler
	{
		Dictionary<int, CameraPosition> ShortcutViews { get; }

		bool FrameUpdate();
	}
}
