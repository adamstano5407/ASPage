namespace APIKros.Models;

public interface IModel<TK> where TK : IComparable<TK>, IEquatable<TK>
{
    public TK Id { get; set; } 
    
}