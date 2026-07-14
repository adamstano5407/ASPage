namespace APIKros.Exceptions;

public class DataIntegrityException : RuntimeException
{
    public DataIntegrityException(string message)
        : base(message)
    {
    }
}