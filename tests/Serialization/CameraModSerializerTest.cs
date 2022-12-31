namespace ISCameraModTest.Serialization
{
	using System;
	using System.Linq;
	using System.Reflection;
	using ISCameraMod;
	using ISCameraMod.Serialization;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class CameraModSerializerTest
	{
		private CameraPosition[] _cameraPositions;

		[TestInitialize]
		public void Initialize()
		{
			_cameraPositions = CreateArray(10);
		}

		[TestMethod]
		public void NullInputData()
		{
			CameraModSerializer.Deserialize(null, _cameraPositions);

			Assert.IsTrue(_cameraPositions.All(position => !position.IsValid), "All positions must be invalid if no input data was provided");
		}

		[Ignore]
		[TestMethod]
		public void V1_NoSavedCameraPositions()
		{
			var data = @"{""CameraPositions"":[],""Version"":1}";

			CameraModSerializer.Deserialize(data, _cameraPositions);

			Assert.IsTrue(_cameraPositions.All(position => !position.IsValid), "All positions must be invalid if the input data does not contain any camera positions");
		}

		[Ignore]
		[TestMethod]
		public void V1_MultipleSavedCameraPositions()
		{
			var data = @"{""CameraPositions"":[{""NumpadKey"":0,""Position"":{""x"":1797.94482,""y"":60.0,""z"":1741.16931},""RotationX"":-5.6245923,""RotationY"":842.4311,""ZoomLevel"":2.57508087},{""NumpadKey"":1,""Position"":{""x"":1535.61731,""y"":60.0,""z"":1982.5896},""RotationX"":35.210907,""RotationY"":819.3053,""ZoomLevel"":2.241456},{""NumpadKey"":5,""Position"":{""x"":1568.50171,""y"":60.0,""z"":1968.88916},""RotationX"":-20.0030479,""RotationY"":959.7377,""ZoomLevel"":1.94266248}],""Version"":1}";

			CameraModSerializer.Deserialize(data, _cameraPositions);

			Assert.IsTrue(_cameraPositions.All(position => !position.IsValid), "All positions must be invalid if the input data does not contain any camera positions");
		}

		private static CameraPosition[] CreateArray(int count)
		{
			return Enumerable.Repeat(new CameraPosition { IsValid = false }, count).ToArray();
		}
	}
}
