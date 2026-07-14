using APIKros.Models;

namespace APIKros.Repositories;

public interface IHierarchyNodeRepository<T, TK> : IRepository<T, TK>
    where T : HierarchyNode, IModel<TK>
    where TK : IEquatable<TK>, IComparable<TK>
{
    Task<bool> CodeExistsAsync(string code, TK? excludeId = default, CancellationToken cancellationToken = default);
    Task UnassignManagerAsync(TK id);
    Task<Employee?> GetManagerOfNodeAsync(TK id);
    
    Task<bool> CodeExistsWithinParentAsync(
        TK parentId,
        string code,
        TK? excludeId = default,
        CancellationToken cancellationToken = default);
}

public interface IHasParentNode<TK, TParent> where TParent : HierarchyNode
{
    Task<TParent?> GetParentOfNodeAsync(TK id);
    Task DeleteAllByParentIdAsync(TK parentId); 
}

public interface IHasChildNodes<TK, TChild> where TChild : HierarchyNode
{
    Task<ICollection<TChild>> GetAllChildNodesAsync(TK id);
}


public interface ICompanyRepository
    : IHierarchyNodeRepository<Company, int>, IHasChildNodes<int, Division>
{ }

public interface IDivisionRepository
    : IHierarchyNodeRepository<Division, int>, IHasParentNode<int, Company>, IHasChildNodes<int, Project> { }

public interface IDepartmentRepository
    : IHierarchyNodeRepository<Department, int>, IHasParentNode<int, Project> { }
    
public interface IProjectRepository 
    : IHierarchyNodeRepository<Project, int>, IHasParentNode<int, Division>, IHasChildNodes<int, Department> {}