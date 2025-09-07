namespace Common.Application.Interfaces;

public interface IImageService
{
    Task<string> UploadImage(FileEntityDto fileEntityDto);
}