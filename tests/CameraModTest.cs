namespace ISCameraModTest
{
	using System.Collections.Generic;
	using GraphAndChartSimpleJSON;
	using ISCameraMod;
	using ISCameraMod.Model;
	using ISCameraMod.Serialization;
	using ISCameraMod.Wrapper;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UnityEngine;

	[TestClass]
	public class CameraModTest
	{
		private Mock<ISerializer> _serializerMock;
		private Mock<IInputWrapper> _inputWrapperMock;
		private Mock<ICameraWrapper> _cameraWrapperMock;
		private Mock<IShortcutViewHandler> _shortcutViewHandlerMock;

		private Dictionary<int, CameraPosition> _shortcutViews;

		[TestInitialize]
		public void TestInitialize()
		{
			_serializerMock = new Mock<ISerializer>(MockBehavior.Strict);
			_inputWrapperMock = new Mock<IInputWrapper>(MockBehavior.Strict);
			_cameraWrapperMock = new Mock<ICameraWrapper>(MockBehavior.Strict);
			_shortcutViewHandlerMock = new Mock<IShortcutViewHandler>(MockBehavior.Strict);

			_shortcutViews = new Dictionary<int, CameraPosition>();
			_shortcutViewHandlerMock.SetupGet(h => h.ShortcutViews).Returns(_shortcutViews);

			CameraModFactory.CreateSerializerFunc = () => _serializerMock.Object;
			CameraModFactory.CreateInputWrapperFunc = () => _inputWrapperMock.Object;
			CameraModFactory.CreateCameraWrapperFunc = () => _cameraWrapperMock.Object;
			CameraModFactory.CreateShortcutViewHandlerFunc = (inputWrapper, cameraWrapper) =>
			{
				Assert.AreSame(_inputWrapperMock.Object, inputWrapper, "Unexpected inputWrapper instance was provided");
				Assert.AreSame(_cameraWrapperMock.Object, cameraWrapper, "Unexpected cameraWrapper instance was provided");
				return _shortcutViewHandlerMock.Object;
			};
		}

		[TestCleanup]
		public void TestCleanup()
		{
			CameraModFactory.ResetFactoryFunctions();

			_serializerMock = null;
			_inputWrapperMock = null;
			_cameraWrapperMock = null;
			_shortcutViewHandlerMock = null;
			_shortcutViews = null;
		}

		[TestMethod]
		public void LoadAndStart_ManuallyInstantiatedObject_InitializesCorrectly()
		{
			var target = CreateTarget();
			target.Load();
			target.Start();

			Assert.AreEqual(0, _shortcutViews.Count, "No camera positions must be loaded");
		}

		[TestMethod]
		public void LoadAndStart_DeserializedInstanceWithoutData_InitializesCorrectly()
		{
			_serializerMock.Setup(s => s.Deserialize(null)).Returns(() => null);

			var target = CreateTargetFromJsonWithoutSerializedData();
			target.Load();
			target.Start();

			Assert.AreEqual(0, _shortcutViews.Count, "No camera positions must be loaded");
		}

		[TestMethod]
		public void LoadAndStart_DeserializedInstanceWithData_InitializesCorrectly()
		{
			var modData = new ModData();
			modData.CameraPositions.Add(0, new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });
			modData.CameraPositions.Add(5, new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 });

			_serializerMock.Setup(s => s.Deserialize("TheData")).Returns(modData);

			var target = CreateTargetWithSerializedData("TheData");
			target.Load();
			target.Start();

			Assert.IsTrue(_shortcutViews.ContainsKey(0), "Camera position with key 0 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }, _shortcutViews[0], "Camera position with key 0 was not loaded correctly");

			Assert.IsTrue(_shortcutViews.ContainsKey(5), "Camera position with key 5 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 }, _shortcutViews[5], "Camera position with key 5 was not loaded correctly");

			Assert.AreEqual(2, _shortcutViews.Count, "Wrong count of camera positions");
		}

		[TestMethod]
		public void LoadAndStart_DeserializedInstanceWithData_ReplacesExistingCameraPositions()
		{
			_shortcutViews.Add(0, new CameraPosition { Position = new Vector3(3, 2, 1), RotationX = 6, RotationY = 5, ZoomLevel = 4 });
			_shortcutViews.Add(3, new CameraPosition { Position = new Vector3(2, 3, 1), RotationX = 5, RotationY = 6, ZoomLevel = 4 });
			_shortcutViews.Add(4, new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });

			var modData = new ModData();
			modData.CameraPositions.Add(0, new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });
			modData.CameraPositions.Add(5, new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 });

			_serializerMock.Setup(s => s.Deserialize("TheData")).Returns(modData);

			var target = CreateTargetWithSerializedData("TheData");
			target.Load();
			target.Start();

			Assert.IsTrue(_shortcutViews.ContainsKey(0), "Camera position with key 0 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }, _shortcutViews[0], "Camera position with key 0 was not loaded correctly");

			Assert.IsTrue(_shortcutViews.ContainsKey(5), "Camera position with key 5 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 }, _shortcutViews[5], "Camera position with key 5 was not loaded correctly");

			Assert.AreEqual(2, _shortcutViews.Count, "Wrong count of camera positions");
		}

		[TestMethod]
		public void FrameUpdate_CallsFrameUpdateOfCameraWrapper()
		{
			_cameraWrapperMock.Setup(w => w.FrameUpdate());
			_shortcutViewHandlerMock.Setup(h => h.FrameUpdate());

			var target = CreateTarget();

			target.FrameUpdate();

			_cameraWrapperMock.Verify(h => h.FrameUpdate(), Times.Once, "Method was not called");
		}

		[TestMethod]
		public void FrameUpdate_CallsFrameUpdateOfShortcutViewHandler()
		{
			_cameraWrapperMock.Setup(w => w.FrameUpdate());
			_shortcutViewHandlerMock.Setup(h => h.FrameUpdate());

			var target = CreateTarget();

			target.FrameUpdate();

			_shortcutViewHandlerMock.Verify(h => h.FrameUpdate(), Times.Once, "Method was not called");
		}

		[TestMethod]
		public void Serializing_WithoutCameraPositions_ModDataIsSerialized()
		{
			_serializerMock.Setup(s => s.Serialize(It.Is<ModData>(d => d.CameraPositions.Count == 0))).Returns("SerializationResult");

			var target = CreateTarget();

			var jsonString = JsonConvert.SerializeObject(target);
			var json = (JObject)JsonConvert.DeserializeObject(jsonString);

			Assert.AreEqual("SerializationResult", json["SerializedData"]);
			Assert.AreEqual(1, json.Count, "JSON must only have one property");
		}

		[TestMethod]
		public void Serializing_WithCameraPositions_ModDataIsSerialized()
		{
			_shortcutViews.Add(0, new CameraPosition { Position = new Vector3(3, 2, 1), RotationX = 6, RotationY = 5, ZoomLevel = 4 });
			_shortcutViews.Add(3, new CameraPosition { Position = new Vector3(2, 3, 1), RotationX = 5, RotationY = 6, ZoomLevel = 4 });

			_serializerMock
				.Setup(s => s.Serialize(It.IsAny<ModData>()))
				.Returns<ModData>(modData =>
				{
					Assert.AreEqual(2, modData.CameraPositions.Count, "Wrong count of camera positions");
					Assert.IsTrue(modData.CameraPositions.ContainsKey(0), "Camera position with key 0 was not saved");
					Assert.IsTrue(modData.CameraPositions.ContainsKey(3), "Camera position with key 3 was not saved");
					Assert.AreEqual(new CameraPosition { Position = new Vector3(3, 2, 1), RotationX = 6, RotationY = 5, ZoomLevel = 4 }, modData.CameraPositions[0], "Camera position with key 0 was not saved correctly");
					Assert.AreEqual(new CameraPosition { Position = new Vector3(2, 3, 1), RotationX = 5, RotationY = 6, ZoomLevel = 4 }, modData.CameraPositions[3], "Camera position with key 35 was not saved correctly");

					return "SerializationResult";
				});

			var target = CreateTarget();

			var jsonString = JsonConvert.SerializeObject(target);
			var json = (JObject)JsonConvert.DeserializeObject(jsonString);

			Assert.AreEqual("SerializationResult", json["SerializedData"]);
			Assert.AreEqual(1, json.Count, "JSON must only have one property");
		}

		private CameraMod CreateTarget()
		{
			return new CameraMod();
		}

		private CameraMod CreateTargetFromJsonWithoutSerializedData()
		{
			var json = new JSONObject();
			return JsonConvert.DeserializeObject<CameraMod>(json.ToString());
		}

		private CameraMod CreateTargetWithSerializedData(string data)
		{
			var json = new JSONObject { ["SerializedData"] = data };
			return JsonConvert.DeserializeObject<CameraMod>(json.ToString());
		}
	}
}
