namespace ISCameraMod
{
	using System;
	using ISCameraMod.Serialization;
	using ISCameraMod.Wrapper;

	/// <summary>
	/// Factory for creating the instances required by the <see cref="CameraMod"/> class.
	/// This factory is a workaround to be able to write unit tests for the <see cref="CameraMod"/> class.
	/// Because InfraSpace requires a constructor without any parameters for the mods, we need another way to inject mocked instances into the mod.
	/// Therefore the mod uses the functions of this factory to create its instances, which can be overwritten in an unit test.
	/// </summary>
	public static class CameraModFactory
	{
		static CameraModFactory()
		{
			ResetFactoryFunctions();
		}

		/// <summary>
		/// Gets or sets a function to create a <see cref="ISerializer"/>.
		/// </summary>
		public static Func<ISerializer> CreateSerializerFunc { get; set; }

		/// <summary>
		/// Gets or sets a function to create a <see cref="IInputWrapper"/>.
		/// </summary>
		public static Func<IInputWrapper> CreateInputWrapperFunc { get; set; }

		/// <summary>
		/// Gets or sets a function to create a <see cref="ICameraWrapper"/>.
		/// </summary>
		public static Func<ICameraWrapper> CreateCameraWrapperFunc { get; set; }

		/// <summary>
		/// Gets or sets a function to create a <see cref="IShortcutViewHandler"/>.
		/// </summary>
		public static Func<IInputWrapper, ICameraWrapper, IShortcutViewHandler> CreateShortcutViewHandlerFunc { get; set; }

		/// <summary>
		/// Resets the functions to the default (production) functions.
		/// </summary>
		public static void ResetFactoryFunctions()
		{
			CreateSerializerFunc = () => new CameraModSerializer(new ISLogger<CameraModSerializer>());
			CreateInputWrapperFunc = () => new UnityInputWrapper();
			CreateCameraWrapperFunc = () => new ISCameraWrapper();
			CreateShortcutViewHandlerFunc = (inputWrapper, cameraWrapper) => new ShortcutViewHandler(inputWrapper, cameraWrapper, new ISLogger<ShortcutViewHandler>());
		}
	}
}
