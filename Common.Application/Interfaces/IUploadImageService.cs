namespace Common.Application.Interfaces;

public interface IUploadImageService
{
    Task<string> UploadImage(FileEntityDto fileEntityDto);
    Task RemoveImage(string imageId);
    string GetPublicIdFromUrl(string url);
}