using Common.Domain.Entities;
using Common.Domain.Filter;

namespace Common.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    IEnumerable<TEntity> GetAll();
    
    Task<IPaged<TEntity>> GetPagedAsync(FilterData filter, CancellationToken cancellationToken);
    
    Task<IEnumerable<TEntity>> GetAllAsync();
    
    TEntity? Get(int id);
    
    Task<TEntity?> GetAsync(int id);
    
    TEntity? GetByCode(Guid code);

    Task<TEntity?> GetByCodeAsync(Guid code);
    
    Task AddAsync(TEntity entity);
    
    Task UpdateAsync(TEntity entity);
    
    Task DeleteAsync(TEntity entity);
    
    void SaveChanges();
    
    Task SaveChangesAsync();
}