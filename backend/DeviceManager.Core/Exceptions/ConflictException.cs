namespace DeviceManager.Core.Exceptions;

/// <summary>
/// Thrown when an operation conflicts with existing data (e.g. duplicate device).
/// Maps to HTTP 409 Conflict.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}
