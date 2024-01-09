namespace CardManager.Domain.exceptions;

public class DuplicatedPhysicalCardException : Exception
{
    public DuplicatedPhysicalCardException()
    {
    }

    public DuplicatedPhysicalCardException(string message) : base(message)
    {
    }

    public DuplicatedPhysicalCardException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
