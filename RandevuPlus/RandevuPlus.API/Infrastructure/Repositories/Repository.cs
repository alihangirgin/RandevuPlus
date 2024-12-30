using Microsoft.EntityFrameworkCore;
using RandevuPlus.API.Infrastructure.Data;
using RandevuPlus.API.Shared.Domain;
using RandevuPlus.API.Shared.Interfaces.Repository;

namespace RandevuPlus.API.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedAt = DateTime.UtcNow; //TODO: override saveChanges
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, string? include = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(include))
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existingEntity == null)
                return null;

            entity.UpdatedAt = DateTime.UtcNow;  //TODO: override saveChanges
            _dbSet.Update(entity);
            return existingEntity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public async Task<bool> CheckAsync(Guid id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }
    }
}
