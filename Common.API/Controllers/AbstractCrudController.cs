using AutoMapper;
using Common.Application.Interfaces;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.API.Controllers;

public abstract class AbstractCrudController<TDto, TCreateDto, TUpdateDto, TEntityDto, TCreateEntityDto, TUpdateEntityDto> 
    : AbstractReadController<TDto, TEntityDto, TCreateEntityDto, TUpdateEntityDto>
{
    protected AbstractCrudController(
        IService<TEntityDto, TCreateEntityDto, TUpdateEntityDto> service, 
        IMapper mapper) : base(service, mapper)
    {
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

            TEntityDto entityDto = await Service.GetByCodeAsync(code);
            
            TDto dto = Mapper.Map<TDto>(entityDto);
            
            string actionName = nameof(GetByCode);
            
            return CreatedAtAction(actionName, new { code }, dto);
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