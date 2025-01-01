using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Dtos;
using System.Linq.Expressions;

namespace RandevuPlus.API.Shared.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<PaginatedResult<TEntity>> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<TEntity?> GetByIdAsync(Guid id, string? include = null, List<string>? includes = null);
        Task DeleteAsync(Guid id);
        Task<bool> CheckAsync(Guid id);
    }
}
