namespace ISCameraModTest
{
	using ISCameraMod;
	using ISCameraMod.Model;
	using ISCameraMod.Wrapper;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using UnityEngine;

	[TestClass]
	public class ShortcutViewHandlerTest
	{
		private Mock<IInputWrapper> _inputWrapperMock;
		private Mock<ICameraWrapper> _cameraWrapperMock;

		[TestInitialize]
		public void TestInitialize()
		{
			_inputWrapperMock = new Mock<IInputWrapper>(MockBehavior.Strict);
			_cameraWrapperMock = new Mock<ICameraWrapper>(MockBehavior.Strict);
		}

		[TestCleanup]
		public void TestCleanup()
		{
			_inputWrapperMock = null;
			_cameraWrapperMock = null;
		}

		[TestMethod]
		public void Constructor_InitializesProperties()
		{
			var target = CreateTarget();

			Assert.IsNotNull(target.ShortcutViews);
			Assert.AreEqual(0, target.ShortcutViews.Count);
		}

		[TestMethod]
		public void FrameUpdate_NoNumpadKeyPressed_NothingHappens()
		{
			_inputWrapperMock.Setup(w => w.GetPressedNumpadKey()).Returns(() => null);

			var target = CreateTarget();
			target.FrameUpdate();

			// MockBehavior.Strict would throw an exception if an unexpected method of the mocks is called
		}

		[TestMethod]
		public void FrameUpdate_NumpadKeyPressedAndPlayerCameraNotActive_NothingHappens()
		{
			_inputWrapperMock.Setup(w => w.GetPressedNumpadKey()).Returns(3);

			_cameraWrapperMock.Setup(w => w.IsPlayerCameraActive()).Returns(false);

			var target = CreateTarget();
			target.FrameUpdate();

			// MockBehavior.Strict would throw an exception if an unexpected method of the mocks is called
		}

		[TestMethod]
		public void FrameUpdate_NumpadKeyPressedAndPlayerCameraActive_NoSavedCameraPosition_NothingHappens()
		{
			_inputWrapperMock.Setup(w => w.GetPressedNumpadKey()).Returns(3);
			_inputWrapperMock.Setup(w => w.IsSaveModifierKeyPressed()).Returns(false);

			_cameraWrapperMock.Setup(w => w.IsPlayerCameraActive()).Returns(true);

			var target = CreateTarget();
			target.FrameUpdate();

			// MockBehavior.Strict would throw an exception if an unexpected method of the mocks is called
		}

		[TestMethod]
		public void FrameUpdate_NumpadKeyPressedAndPlayerCameraActive_HasSavedCameraPosition_AppliesCameraPosition()
		{
			_inputWrapperMock.Setup(w => w.GetPressedNumpadKey()).Returns(3);
			_inputWrapperMock.Setup(w => w.IsSaveModifierKeyPressed()).Returns(false);

			_cameraWrapperMock.Setup(w => w.IsPlayerCameraActive()).Returns(true);
			_cameraWrapperMock.Setup(w => w.MovePlayerCameraToPosition(It.IsAny<CameraPosition>()));

			var target = CreateTarget();

			target.ShortcutViews.Add(3, new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });

			target.FrameUpdate();

			_cameraWrapperMock.Verify(w => w.MovePlayerCameraToPosition(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }), Times.Once);
			_cameraWrapperMock.Verify(w => w.MovePlayerCameraToPosition(It.IsAny<CameraPosition>()), Times.Once, "Only one camera position must be set");
		}

		[TestMethod]
		public void FrameUpdate_NumpadKeyWithSaveModifierPressedAndPlayerCameraActive_NoSavedCameraPosition_SavesCameraPosition()
		{
			_inputWrapperMock.Setup(w => w.GetPressedNumpadKey()).Returns(3);
			_inputWrapperMock.Setup(w => w.IsSaveModifierKeyPressed()).Returns(true);

			_cameraWrapperMock.Setup(w => w.IsPlayerCameraActive()).Returns(true);
			_cameraWrapperMock.Setup(w => w.GetPlayerCameraPosition()).Returns(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });

			var target = CreateTarget();
			target.FrameUpdate();

			Assert.AreEqual(1, target.ShortcutViews.Count, "No camera position was saved");
			Assert.IsTrue(target.ShortcutViews.ContainsKey(3), "Camera position was saved with a wrong key");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }, target.ShortcutViews[3]);
		}

		[TestMethod]
		public void FrameUpdate_NumpadKeyWithSaveModifierPressedAndPlayerCameraActive_HasSavedCameraPosition_OverridesCameraPosition()
		{
			_inputWrapperMock.Setup(w => w.GetPressedNumpadKey()).Returns(3);
			_inputWrapperMock.Setup(w => w.IsSaveModifierKeyPressed()).Returns(true);

			_cameraWrapperMock.Setup(w => w.IsPlayerCameraActive()).Returns(true);
			_cameraWrapperMock.Setup(w => w.GetPlayerCameraPosition()).Returns(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });

			var target = CreateTarget();

			target.ShortcutViews.Add(3, new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 });
			target.ShortcutViews.Add(4, new CameraPosition { Position = new Vector3(5, 6, 4), RotationX = 2, RotationY = 1, ZoomLevel = 3 });

			target.FrameUpdate();

			Assert.AreEqual(2, target.ShortcutViews.Count, "Wrong count of camera positions");
			Assert.IsTrue(target.ShortcutViews.ContainsKey(3), "Camera position was saved with a wrong key");
			Assert.IsTrue(target.ShortcutViews.ContainsKey(4), "Camera position with key 4 was removed");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }, target.ShortcutViews[3], "Camera position with key 3 was not updated");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(5, 6, 4), RotationX = 2, RotationY = 1, ZoomLevel = 3}, target.ShortcutViews[4], "Camera position with key 4 must not have changed");
		}

		private ShortcutViewHandler CreateTarget()
		{
			return new ShortcutViewHandler(_inputWrapperMock.Object, _cameraWrapperMock.Object, null);
		}
	}
}
