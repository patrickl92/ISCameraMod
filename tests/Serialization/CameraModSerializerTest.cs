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
			var cameraPositions = CreateArray(10);
			var target = new CameraModSerializer(null);
			target.Deserialize(null, cameraPositions);

			Assert.IsTrue(cameraPositions.All(position => !position.IsValid), "All positions must be invalid if no input data was provided");
		}

		[TestMethod]
		public void V1_NoSavedCameraPositions()
		{
			var cameraPositions = CreateArray(10);
			var data = @"{""CameraPositions"":[],""Version"":1}";

			var target = new CameraModSerializer(null);
			target.Deserialize(data, cameraPositions);

			Assert.IsTrue(cameraPositions.All(position => !position.IsValid), "All positions must be invalid if the input data does not contain any camera positions");
		}

		[TestMethod]
		public void V1_MultipleSavedCameraPositions()
		{
			var cameraPositions = CreateArray(10);
			var data = @"{""CameraPositions"":[{""NumpadKey"":0,""PositionX"":1797.94482,""PositionY"":60.0,""PositionZ"":1741.16931,""RotationX"":-5.6245923,""RotationY"":842.4311,""ZoomLevel"":2.57508087},{""NumpadKey"":1,""PositionX"":1535.61731,""PositionY"":60.0,""PositionZ"":1982.5896,""RotationX"":35.210907,""RotationY"":819.3053,""ZoomLevel"":2.241456},{""NumpadKey"":5,""PositionX"":1568.50171,""PositionY"":63.0,""PositionZ"":1968.88916,""RotationX"":-20.0030479,""RotationY"":959.7377,""ZoomLevel"":1.94266248}],""Version"":1}";

			var target = new CameraModSerializer(null);
			target.Deserialize(data, cameraPositions);

			Assert.AreEqual(new Vector3(1797.94482f, 60f, 1741.16931f), cameraPositions[0].Position, "Position at index 0 was not loaded correctly");
			Assert.AreEqual(-5.6245923f, cameraPositions[0].RotationX, "RotationX at index 0 was not loaded correctly");
			Assert.AreEqual(842.4311f, cameraPositions[0].RotationY, "RotationY at index 0 was not loaded correctly");
			Assert.AreEqual(2.57508087f, cameraPositions[0].ZoomLevel, "ZoomLevel at index 0 was not loaded correctly");
			Assert.IsTrue(cameraPositions[0].IsValid, "Entry at index 0 must be valid");

			Assert.AreEqual(new Vector3(1535.61731f, 60f, 1982.5896f), cameraPositions[1].Position, "Position at index 1 was not loaded correctly");
			Assert.AreEqual(35.210907f, cameraPositions[1].RotationX, "RotationX at index 1 was not loaded correctly");
			Assert.AreEqual(819.3053f, cameraPositions[1].RotationY, "RotationY at index 1 was not loaded correctly");
			Assert.AreEqual(2.241456f, cameraPositions[1].ZoomLevel, "ZoomLevel at index 1 was not loaded correctly");
			Assert.IsTrue(cameraPositions[1].IsValid, "Entry at index 1 must be valid");

			Assert.AreEqual(new Vector3(1568.50171f, 63f, 1968.88916f), cameraPositions[5].Position, "Position at index 5 was not loaded correctly");
			Assert.AreEqual(-20.0030479f, cameraPositions[5].RotationX, "RotationX at index 5 was not loaded correctly");
			Assert.AreEqual(959.7377f, cameraPositions[5].RotationY, "RotationY at index 5 was not loaded correctly");
			Assert.AreEqual(1.94266248f, cameraPositions[5].ZoomLevel, "ZoomLevel at index 5 was not loaded correctly");
			Assert.IsTrue(cameraPositions[5].IsValid, "Entry at index 5 must be valid");

			Assert.AreEqual(7, cameraPositions.Count(position => !position.IsValid), "Positions which were not loaded must be invalid");
		}

		private static CameraPosition[] CreateArray(int count)
		{
			return Enumerable.Repeat(new CameraPosition { IsValid = false }, count).ToArray();
		}
	}
}
