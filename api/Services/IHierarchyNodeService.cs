using APIKros.Requests;

namespace APIKros.Services;

public interface IHierarchyNodeService<TResponse, TCreateRequest, TUpdateRequest, TK>
    : IService<TResponse, TCreateRequest, TUpdateRequest, TK> where TK : IEquatable<TK>, IComparable<TK>
{
    Task UnassignManagerAsync(TK id);
    Task AssignManagerAsync(TK id, AssignManagerRequest request);
    
}

public interface IHasChildrenService<TChildResponse, TK>
{
    Task<IEnumerable<TChildResponse>> GetChildrenAsync(TK id);
}

public interface IHasParentService<TParentResponse, TK>
{
    Task<TParentResponse> GetParentAsync(TK id);
}