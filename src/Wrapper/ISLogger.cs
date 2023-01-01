namespace ISCameraMod.Wrapper
{
	using UnityEngine;

	/// <summary>
	/// Wrapper of the InfraSpace logging mechanism.
	/// </summary>
	/// <typeparam name="T">The type of the consumer of the interface, to also log this information automatically.</typeparam>
	internal class ISLogger<T> : ILogger<T>
	{
		private readonly string _context;

		/// <summary>
		/// Creates a new instance of the <see cref="ISLogger{T}"/> class.
		/// </summary>
		public ISLogger()
		{
			_context = typeof(T).Name;
		}

		/// <summary>
		/// Writes a log message using the <see cref="MonoBehaviour.print"/> method.
		/// </summary>
		/// <param name="message">The log message.</param>
		public void Log(string message)
		{
			MonoBehaviour.print($"{_context}: {message}");
		}
	}
}
