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
public abstract class AbstractController<TDto, TCreateDto, TUpdateDto, TEntityDto, TCreateEntityDto, TUpdateEntityDto> 
    : ControllerBase
{
    protected readonly IService<TEntityDto, TCreateEntityDto, TUpdateEntityDto> Service;
    protected readonly IMapper Mapper;

    protected AbstractController(
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
        IEnumerable<TEntityDto> page = await Service.GetAllAsync();
        
        return Ok(Mapper.Map<IEnumerable<TDto>>(page));
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
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto createDto)
    {
        try
        {
            TCreateEntityDto createEntityDto = Mapper.Map<TCreateEntityDto>(createDto);
        
            Guid code = await Service.CreateAsync(createEntityDto);

            string actionName = nameof(GetByCode);
            
            return CreatedAtAction(actionName, new { code }, new { Code = code });
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{code:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<ActionResult<TDto>> Update(Guid code, [FromBody] TUpdateDto updateDto)
    {
        try
        {
            TUpdateEntityDto updateEntityDto = Mapper.Map<TUpdateEntityDto>(updateDto);

            return Ok(await Service.UpdateAsync(code, updateEntityDto));
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
    
    [HttpDelete("{code:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<ActionResult> Delete(Guid code)
    {
        try
        {
            await Service.DeleteAsync(code);
            
            return NoContent();
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