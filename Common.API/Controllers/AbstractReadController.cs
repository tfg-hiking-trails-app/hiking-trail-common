using AutoMapper;
using Common.API.DTOs.Filter;
using Common.API.Utils;
using Common.Application.DTOs.Filter;
using Common.Application.Interfaces;
using Common.Application.Pagination;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.API.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class AbstractReadController<TDto, TEntityDto, TCreateEntityDto, TUpdateEntityDto> 
    : ControllerBase
{
    protected readonly IService<TEntityDto, TCreateEntityDto, TUpdateEntityDto> Service;
    protected readonly IMapper Mapper;
    
    protected AbstractReadController(
        IService<TEntityDto, TCreateEntityDto, TUpdateEntityDto> service, 
        IMapper mapper)
    {
        Service = service;
        Mapper = mapper;
    }
    
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        IEnumerable<TEntityDto> list = await Service.GetAllAsync();
        
        return Ok(Mapper.Map<IEnumerable<TDto>>(list));
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<Page<TDto>>> GetPaged(
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = Pagination.PageNumber,
        [FromQuery] int pageSize = Pagination.PageSize,
        [FromQuery] string sortField = Pagination.SortField,
        [FromQuery] string sortDirection = Pagination.SortDirection)
    {
        FilterDto filter = new FilterDto(pageNumber, pageSize, sortField, sortDirection);
        
        Page<TEntityDto> page = await Service
            .GetPagedAsync(Mapper.Map<FilterEntityDto>(filter), cancellationToken);
        
        return Ok(Mapper.Map<Page<TDto>>(page));
    }
    
    [HttpGet("{code:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<ActionResult<TDto>> GetByCode(Guid code)
    {
        try
        {
            TEntityDto entityDto = await Service.GetByCodeAsync(code);

            return Ok(Mapper.Map<TDto>(entityDto));
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}