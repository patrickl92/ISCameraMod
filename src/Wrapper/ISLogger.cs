namespace ISCameraMod.Wrapper
{
	using UnityEngine;

	public class ISLogger<T> : ILogger<T>
	{
		private readonly string _context;

		public ISLogger()
		{
			_context = typeof(T).Name;
		}

		public void Log(string message)
		{
			MonoBehaviour.print($"{_context}: {message}");
		}
	}
}
