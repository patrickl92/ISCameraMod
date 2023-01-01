namespace ISCameraModTest.Serialization
{
	using System.Linq;
	using ISCameraMod;
	using ISCameraMod.Serialization;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using UnityEngine;

	[TestClass]
	public class CameraModSerializerTest
	{
		[TestMethod]
		public void NullInputData()
		{
			var target = new CameraModSerializer(null);
			var result = target.Deserialize(null);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void V1_NoSavedCameraPositions()
		{
			var data = @"{""CameraPositions"":[],""Version"":1}";

			var target = new CameraModSerializer(null);
			var result = target.Deserialize(data);

			Assert.IsNotNull(result);
			Assert.AreEqual(0, result.Count);
		}

		[TestMethod]
		public void V1_MultipleSavedCameraPositions()
		{
			var data = @"{""CameraPositions"":[{""NumpadKey"":0,""PositionX"":1797.94482,""PositionY"":60.0,""PositionZ"":1741.16931,""RotationX"":-5.6245923,""RotationY"":842.4311,""ZoomLevel"":2.57508087},{""NumpadKey"":1,""PositionX"":1535.61731,""PositionY"":60.0,""PositionZ"":1982.5896,""RotationX"":35.210907,""RotationY"":819.3053,""ZoomLevel"":2.241456},{""NumpadKey"":5,""PositionX"":1568.50171,""PositionY"":63.0,""PositionZ"":1968.88916,""RotationX"":-20.0030479,""RotationY"":959.7377,""ZoomLevel"":1.94266248}],""Version"":1}";

			var target = new CameraModSerializer(null);
			var result = target.Deserialize(data);

			Assert.AreEqual(3, result.Count, "Wrong count of loaded camera positions");
			Assert.IsTrue(result.ContainsKey(0), "Loaded views do not contain key 0");
			Assert.IsTrue(result.ContainsKey(1), "Loaded views do not contain key 1");
			Assert.IsTrue(result.ContainsKey(5), "Loaded views do not contain key 5");

			Assert.AreEqual(new Vector3(1797.94482f, 60f, 1741.16931f), result[0].Position, "Position at key 0 was not loaded correctly");
			Assert.AreEqual(-5.6245923f, result[0].RotationX, "RotationX at key 0 was not loaded correctly");
			Assert.AreEqual(842.4311f, result[0].RotationY, "RotationY at key 0 was not loaded correctly");
			Assert.AreEqual(2.57508087f, result[0].ZoomLevel, "ZoomLevel at key 0 was not loaded correctly");

			Assert.AreEqual(new Vector3(1535.61731f, 60f, 1982.5896f), result[1].Position, "Position at key 1 was not loaded correctly");
			Assert.AreEqual(35.210907f, result[1].RotationX, "RotationX at key 1 was not loaded correctly");
			Assert.AreEqual(819.3053f, result[1].RotationY, "RotationY at key 1 was not loaded correctly");
			Assert.AreEqual(2.241456f, result[1].ZoomLevel, "ZoomLevel at key 1 was not loaded correctly");

			Assert.AreEqual(new Vector3(1568.50171f, 63f, 1968.88916f), result[5].Position, "Position at key 5 was not loaded correctly");
			Assert.AreEqual(-20.0030479f, result[5].RotationX, "RotationX at key 5 was not loaded correctly");
			Assert.AreEqual(959.7377f, result[5].RotationY, "RotationY at key 5 was not loaded correctly");
			Assert.AreEqual(1.94266248f, result[5].ZoomLevel, "ZoomLevel at key 5 was not loaded correctly");
		}
	}
}
