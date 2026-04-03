namespace DeviceManager.Core.Exceptions;

/// <summary>
/// Thrown when a requested resource does not exist in the database.
/// Maps to HTTP 404 Not Found.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string resourceName, string id)
        : base($"{resourceName} with ID '{id}' not found.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }
}
