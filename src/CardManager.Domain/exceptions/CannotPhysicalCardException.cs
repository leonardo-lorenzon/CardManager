namespace CardManager.Domain.exceptions;

public class CannotPhysicalCardException : Exception
{
    public CannotPhysicalCardException()
    {
    }

    public CannotPhysicalCardException(string message) : base(message)
    {
    }

    public CannotPhysicalCardException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
