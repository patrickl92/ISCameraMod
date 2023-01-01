namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using ISCameraMod.Wrapper;

	public static class CameraModFactory
	{
		static CameraModFactory()
		{
			CreateSerializerFunc = () => new CameraModSerializer(new ISLogger<CameraModSerializer>());
			CreateShortcutViewHandlerFunc = () => new ShortcutViewHandler(new UnityInputWrapper(), new ISCameraWrapper(), new ISLogger<ShortcutViewHandler>());
		}

		public static Func<ISerializer> CreateSerializerFunc { get; set; }

		public static Func<IShortcutViewHandler> CreateShortcutViewHandlerFunc { get; set; }
	}
}
