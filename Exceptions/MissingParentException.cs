namespace APIKros.Exceptions;

public class MissingParentException : DataIntegrityException
{
    public MissingParentException() : base("Entity has no parent assigned ")
    {
    }
}