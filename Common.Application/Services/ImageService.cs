using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common.Application.Interfaces;
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
        if (fileEntityDto.Length == 0)
            throw new ArgumentException("FileEntityDto.Length is zero");

        fileEntityDto.FileName = Guid.NewGuid() + Path.GetExtension(fileEntityDto.FileName);
        
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
    
}