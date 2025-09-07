using AutoMapper;
using Common.Application.DTOs.Filter;
using Common.Application.Interfaces;
using Common.Application.Pagination;
using Common.Domain.Entities;
using Common.Domain.Exceptions;
using Common.Domain.Filter;
using Common.Domain.Interfaces;

namespace Common.Application.Services;

public abstract class AbstractService<TEntity, TEntityDto, TCreateEntityDto, TUpdateEntityDto> 
    : IService<TEntityDto, TCreateEntityDto, TUpdateEntityDto> where TEntity : BaseEntity
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;
    
    public AbstractService(
        IRepository<TEntity> repository,
        IMapper mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }
    
    public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync()
    {
        IEnumerable<TEntity> result = await Repository.GetAllAsync();
        
        return Mapper.Map<IEnumerable<TEntityDto>>(result);
    }

    public virtual async Task<Page<TEntityDto>> GetPagedAsync(FilterEntityDto filter, CancellationToken cancellationToken)
    {
        FilterData filterData = Mapper.Map<FilterData>(filter);
        
        IPaged<TEntity> result = await Repository.GetPagedAsync(filterData, cancellationToken);
        
        return Mapper.Map<Page<TEntityDto>>(result);
    }

    public virtual async Task<TEntityDto> GetByIdAsync(int id)
    {
        TEntity? result = await Repository.GetAsync(id);
        
        return result is null 
            ? throw new NotFoundEntityException(typeof(TEntity).Name, id)
            : Mapper.Map<TEntityDto>(result);
    }

    public virtual async Task<TEntityDto> GetByCodeAsync(Guid code)
    {
        TEntity? result = await Repository.GetByCodeAsync(code);
        
        return result is null 
            ? throw new NotFoundEntityException(typeof(TEntity).Name, code)
            : Mapper.Map<TEntityDto>(result);
    }

    public virtual async Task<Guid> CreateAsync(TCreateEntityDto createEntityDto)
    {
        CheckDataValidity(createEntityDto);
        
        TEntity entity = Mapper.Map<TEntity>(createEntityDto);
        
        if (entity.Code == Guid.Empty)
            entity.Code = Guid.NewGuid();

        await Repository.AddAsync(entity);

        return entity.Code;
    }

    public virtual async Task<Guid> UpdateAsync(Guid code, TUpdateEntityDto updateEntityDto)
    {
        TEntity entity = await GetEntity(code);

        Mapper.Map(updateEntityDto, entity);
        
        await Repository.UpdateAsync(entity);
        
        return entity.Code;
    }

    public virtual async Task DeleteAsync(Guid code)
    {
        TEntity entity = await GetEntity(code);

        await Repository.DeleteAsync(entity);
    }
    
    protected abstract void CheckDataValidity(TCreateEntityDto createEntityDto);
    
    protected async Task<TEntity> GetEntity(Guid code)
    {
        if (code == Guid.Empty)
            throw new ArgumentNullException(nameof(code));
        
        TEntity? entity = await Repository.GetByCodeAsync(code);
        
        if (entity is null)
            throw new NotFoundEntityException(typeof(TEntity).Name, code);
        
        return entity;
    }
    
}