using Common.Application.Extensions;
using Common.Domain.Entities;
using Common.Domain.Exceptions;
using Common.Domain.Filter;
using Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Data.Repositories;

public abstract class AbstractRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity 
{
    protected DbContext DbContext { get; }
    protected DbSet<TEntity> Entity { get; }

    protected AbstractRepository(DbContext dbContext)
    {
        DbContext = dbContext;
        Entity = dbContext.Set<TEntity>();
    }
    
    public virtual IEnumerable<TEntity> GetAll() => 
        Entity.ToList();

    public virtual async Task<IPaged<TEntity>> GetPagedAsync(FilterData filter, CancellationToken cancellationToken) =>
        await Entity.ToPageAsync(filter, cancellationToken);
    
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => 
        await Entity.ToListAsync();
    
    public virtual TEntity? Get(int id) => 
        Entity.Find(id);
    
    public virtual async Task<TEntity?> GetAsync(int id) => 
        await Entity.FindAsync(id);
    
    public virtual TEntity? GetByCode(Guid code) => 
        Entity.FirstOrDefault(e => e.Code == code);
    
    public virtual async Task<TEntity?> GetByCodeAsync(Guid code) => 
        await Entity.FirstOrDefaultAsync(e => e.Code == code);

    public virtual async Task AddAsync(TEntity entity)
    {
        entity.Code = Guid.NewGuid();
        
        Entity.Add(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(Guid code, TEntity entity)
    {
        if (!Entity.Any(e => e.Code.Equals(code))) 
            throw new NotFoundEntityException(nameof(TEntity), code);
        
        Entity.Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid code)
    {
        var entity = Entity.FirstOrDefault(e => e.Code.Equals(code));
        
        if (entity is null) 
            throw new NotFoundEntityException(nameof(TEntity), code);
        
        Entity.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public void SaveChanges()
    {
        DbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await DbContext.SaveChangesAsync();
    }
}