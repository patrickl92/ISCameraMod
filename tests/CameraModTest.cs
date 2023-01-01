namespace ISCameraModTest
{
	using System.Collections.Generic;
	using GraphAndChartSimpleJSON;
	using ISCameraMod;
	using ISCameraMod.Model;
	using ISCameraMod.Serialization;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using UnityEngine;

	[TestClass]
	public class CameraModTest
	{
		private Mock<ISerializer> _serializerMock;

		private Mock<IShortcutViewHandler> _shortcutViewHandlerMock;

		private Dictionary<int, CameraPosition> _shortcutViews;

		[TestInitialize]
		public void TestInitialize()
		{
			_serializerMock = new Mock<ISerializer>(MockBehavior.Strict);
			_shortcutViewHandlerMock = new Mock<IShortcutViewHandler>(MockBehavior.Strict);

			_shortcutViews = new Dictionary<int, CameraPosition>();
			_shortcutViewHandlerMock.SetupGet(h => h.ShortcutViews).Returns(_shortcutViews);

			CameraModFactory.CreateSerializerFunc = () => _serializerMock.Object;
			CameraModFactory.CreateShortcutViewHandlerFunc = () => _shortcutViewHandlerMock.Object;
		}

		[TestCleanup]
		public void TestCleanup()
		{
			CameraModFactory.ResetFactoryFunctions();

			_serializerMock = null;
			_shortcutViewHandlerMock = null;
			_shortcutViews = null;
		}

		[TestMethod]
		public void Load_ManuallyInstantiatedObject_InitializesCorrectly()
		{
			_serializerMock.Setup(s => s.Deserialize(null)).Returns(new Dictionary<int, CameraPosition>());

			var target = CreateTarget();
			target.Load();

			Assert.AreEqual(0, _shortcutViews.Count, "No camera positions must be loaded");
		}

		[TestMethod]
		public void Load_DeserializedInstanceWithoutData_InitializesCorrectly()
		{
			_serializerMock.Setup(s => s.Deserialize(null)).Returns(new Dictionary<int, CameraPosition>());

			var target = CreateTargetFromJsonWithoutSerializedData();
			target.Load();

			Assert.AreEqual(0, _shortcutViews.Count, "No camera positions must be loaded");
		}

		[TestMethod]
		public void Load_DeserializedInstanceWithData_InitializesCorrectly()
		{
			var cameraPositions = new Dictionary<int, CameraPosition>
			{
				{0, new CameraPosition {Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6}},
				{5, new CameraPosition {Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1}}
			};

			_serializerMock.Setup(s => s.Deserialize("TheData")).Returns(cameraPositions);

			var target = CreateTargetWithSerializedData("TheData");
			target.Load();

			Assert.IsTrue(_shortcutViews.ContainsKey(0), "Camera position with key 0 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }, _shortcutViews[0], "Camera position with key 0 was not loaded correctly");

			Assert.IsTrue(_shortcutViews.ContainsKey(5), "Camera position with key 5 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 }, _shortcutViews[5], "Camera position with key 5 was not loaded correctly");

			Assert.AreEqual(2, _shortcutViews.Count, "Wrong count of camera positions");
		}

		[TestMethod]
		public void Load_DeserializedInstanceWithData_ReplacesExistingCameraPositions()
		{
			_shortcutViews.Add(0, new CameraPosition { Position = new Vector3(3, 2, 1), RotationX = 6, RotationY = 5, ZoomLevel = 4 });
			_shortcutViews.Add(3, new CameraPosition { Position = new Vector3(2, 3, 1), RotationX = 5, RotationY = 6, ZoomLevel = 4 });
			_shortcutViews.Add(4, new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 });

			var cameraPositions = new Dictionary<int, CameraPosition>
			{
				{0, new CameraPosition {Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6}},
				{5, new CameraPosition {Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1}}
			};

			_serializerMock.Setup(s => s.Deserialize("TheData")).Returns(cameraPositions);

			var target = CreateTargetWithSerializedData("TheData");
			target.Load();

			Assert.IsTrue(_shortcutViews.ContainsKey(0), "Camera position with key 0 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(1, 2, 3), RotationX = 4, RotationY = 5, ZoomLevel = 6 }, _shortcutViews[0], "Camera position with key 0 was not loaded correctly");

			Assert.IsTrue(_shortcutViews.ContainsKey(5), "Camera position with key 5 was not loaded");
			Assert.AreEqual(new CameraPosition { Position = new Vector3(6, 5, 4), RotationX = 3, RotationY = 2, ZoomLevel = 1 }, _shortcutViews[5], "Camera position with key 5 was not loaded correctly");

			Assert.AreEqual(2, _shortcutViews.Count, "Wrong count of camera positions");
		}

		[TestMethod]
		public void FrameUpdate_ShortcutViewHandlerReturnsFalse_DataIsNotSerialized()
		{
			_shortcutViewHandlerMock.Setup(h => h.FrameUpdate()).Returns(false);

			var target = CreateTarget();

			target.FrameUpdate();

			var jsonString = JsonConvert.SerializeObject(target);
			var json = (JObject)JsonConvert.DeserializeObject(jsonString);

			Assert.AreEqual(JTokenType.Null, json["SerializedData"].Type);
		}

		[TestMethod]
		public void FrameUpdate_ShortcutViewHandlerReturnsTrue_DataIsSerialized()
		{
			_serializerMock.Setup(s => s.Serialize(_shortcutViews)).Returns("SerializationResult");
			_shortcutViewHandlerMock.Setup(h => h.FrameUpdate()).Returns(true);

			var target = CreateTarget();

			target.FrameUpdate();

			var jsonString = JsonConvert.SerializeObject(target);
			var json = (JObject)JsonConvert.DeserializeObject(jsonString);

			Assert.AreEqual("SerializationResult", json["SerializedData"]);
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
