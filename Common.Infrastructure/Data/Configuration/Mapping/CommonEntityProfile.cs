using AutoMapper;
using Common.Application.DTOs.Filter;
using Common.Domain.Filter;

namespace Common.Infrastructure.Data.Configuration.Mapping;

public class CommonEntityProfile : Profile
{
    public CommonEntityProfile()
    {
        CreateMap<FilterEntityDto, FilterData>().ReverseMap();
    }
}