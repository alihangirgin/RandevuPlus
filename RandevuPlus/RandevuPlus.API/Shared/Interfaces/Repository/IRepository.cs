using RandevuPlus.API.Shared.Domain;

namespace RandevuPlus.API.Shared.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id, string? include = null);
        Task DeleteAsync(Guid id);
        Task<bool> CheckAsync(Guid id);
    }
}
