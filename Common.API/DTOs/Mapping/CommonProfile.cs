using AutoMapper;
using Common.API.DTOs.Filter;
using Common.Application.DTOs.Filter;
using Common.Application.Pagination;
using Common.Infrastructure.Converters;

namespace Common.API.DTOs.Mapping;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<FilterDto, FilterEntityDto>().ReverseMap();
        CreateMap(typeof(Page<>), typeof(Page<>)).ConvertUsing(typeof(PageConverter<,>));
    }
}