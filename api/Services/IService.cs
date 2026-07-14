namespace APIKros.Services;

public interface IService<TResponse, TCreateRequest, TUpdateRequest, TK>
    where TK : IEquatable<TK>, IComparable<TK>
{
    Task<TResponse> GetAsync(TK id);
    Task<IEnumerable<TResponse>> GetAllAsync();
    Task<TResponse> CreateAsync(TCreateRequest request);
    Task UpdateAsync(TK id, TUpdateRequest request);
    Task DeleteAsync(TK id);
}