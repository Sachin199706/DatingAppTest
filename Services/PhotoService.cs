using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Helpers;
using DatingApp.Interface;
using Microsoft.Extensions.Options;

namespace DatingApp.Services
{
    public class PhotoService : IPhotoService
    {
        public readonly Cloudinary _Cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config) {
            Account acc = new Account(config.Value.CloudName, config.Value.APIKey, config.Value.APISecret);
            _Cloudinary = new Cloudinary(acc);

        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            ImageUploadResult imageUploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream=file.OpenReadStream();
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"),
                    Folder = "da-net8"
                };
                imageUploadResult=await _Cloudinary.UploadAsync(uploadParams);
            }
            return imageUploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string PublicId)
        {
            DeletionParams delete = new DeletionParams(PublicId);
            return await _Cloudinary.DestroyAsync(delete);

        }
    }
}
