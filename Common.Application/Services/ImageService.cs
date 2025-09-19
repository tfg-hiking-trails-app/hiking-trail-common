using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common.Application.Interfaces;
using Common.Application.Utils;
using Newtonsoft.Json.Linq;

namespace Common.Application.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService()
    {
        string cloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL")?.Trim() 
                            ?? throw new InvalidOperationException("CLOUDINARY_URL environment variable not set");
        
        _cloudinary = new Cloudinary(cloudinaryUrl);
    }

    public async Task<string> UploadImage(FileEntityDto fileEntityDto)
    {
        CheckImageFile(fileEntityDto);        

        fileEntityDto.FileName = Guid.NewGuid() + GetExtensionFile(fileEntityDto);
        
        await using Stream stream = new MemoryStream(fileEntityDto.Content);

        ImageUploadParams uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileEntityDto.FileName, stream),
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };
        
        JToken jsonResult = (await _cloudinary.UploadAsync(uploadParams)).JsonObj;

        return jsonResult["secure_url"]?.ToString() ?? throw new InvalidOperationException("secure_url not exists");
    }

    public async Task RemoveImage(string imageId)
    {
        DeletionParams deletionParams = new DeletionParams(imageId)
        {
            ResourceType = ResourceType.Image
        };
        
        await _cloudinary.DestroyAsync(deletionParams);
    }

    public string GetPublicIdFromUrl(string url)
    {
        Uri uri = new Uri(url);
        string[] segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        string fileName = segments.Last();
        string folderPath = string.Join("/", segments.Skip(segments.ToList().IndexOf("upload") + 1).SkipLast(1));

        return Path.Combine(folderPath, Path.GetFileNameWithoutExtension(fileName))
            .Replace("\\", "/");
    }

    private void CheckImageFile(FileEntityDto fileEntityDto)
    {
        if (fileEntityDto.Length == 0)
            throw new ArgumentException("FileEntityDto.Length is zero");
        
        if (!Validator.ValidExtensionsImage.Contains(GetExtensionFile(fileEntityDto)))
            throw new ArgumentException("Invalid file extension");
        
        if (fileEntityDto.ContentType is null || !Validator.ValidContentTypeImage.Contains(fileEntityDto.ContentType))
            throw new ArgumentException("Invalid file type");
    }

    private string GetExtensionFile(FileEntityDto fileEntityDto)
    {
        return Path.GetExtension(fileEntityDto.FileName) 
               ?? throw new ArgumentException("FileName is not a file");
    }
    
}