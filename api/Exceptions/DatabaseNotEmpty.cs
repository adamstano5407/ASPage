namespace APIKros.Exceptions;

public class DatabaseNotEmpty() : RuntimeException("Database is not empty. Seeder failed.")
{
    
}