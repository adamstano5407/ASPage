namespace APIKros.Exceptions;

public class NotFoundException : RuntimeException
{
    public NotFoundException()
        : base("The record was not found.")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}