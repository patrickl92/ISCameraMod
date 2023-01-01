namespace ISCameraMod.Wrapper
{
	/// <summary>
	/// Interface for wrapping the InfraSpace logging mechanism.
	/// </summary>
	/// <typeparam name="T">The type of the consumer of the interface, to also log this information automatically.</typeparam>
	public interface ILogger<T>
	{
		/// <summary>
		/// Writes a log message.
		/// </summary>
		/// <param name="message">The log message.</param>
		void Log(string message);
	}
}
