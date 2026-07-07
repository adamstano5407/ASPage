namespace APIKros.Exceptions;

public abstract class RuntimeException : Exception
{
    protected RuntimeException()
    {
    }

    protected RuntimeException(string message)
        : base(message)
    {
    }

    protected RuntimeException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}