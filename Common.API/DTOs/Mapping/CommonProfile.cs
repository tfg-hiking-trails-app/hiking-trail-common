using AutoMapper;
using Common.API.DTOs.Filter;
using Common.Application;
using Common.Application.DTOs.Filter;
using Common.Application.Pagination;
using Common.Infrastructure.Converters;
using Microsoft.AspNetCore.Http;

namespace Common.API.DTOs.Mapping;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<FilterDto, FilterEntityDto>().ReverseMap();
        CreateMap(typeof(Page<>), typeof(Page<>)).ConvertUsing(typeof(PageConverter<,>));
        CreateMap<IFormFile, FileEntityDto>().ConvertUsing(src => MapFile(src));
    }
    
    private FileEntityDto MapFile(IFormFile file)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file), "ActivityFile is null");
                
        using Stream stream = file.OpenReadStream();
        using MemoryStream memoryStream = new MemoryStream();
                
        stream.CopyTo(memoryStream);
                
        return new FileEntityDto
        {
            ContentType = file.ContentType,
            ContentDisposition = file.ContentDisposition,
            Length = file.Length,
            Name = file.Name,
            FileName = file.FileName,
            Content = memoryStream.ToArray()
        };
    }
}