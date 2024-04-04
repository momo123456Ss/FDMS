using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FDMS.Model;
using Microsoft.EntityFrameworkCore;

namespace FDMS.Service.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(Cloudinary cloudinary)
        {
            this._cloudinary = cloudinary;
        }
        public async Task<bool> DeleteCloudinary(string url, string folder)
        {
            string currentFileUrl = url;
            string publicId = string.Empty;
            string path = string.Empty;
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(currentFileUrl))
            {
                Uri uri = new Uri(currentFileUrl);
                path = uri.AbsolutePath;
                fileName = Path.GetFileName(path);
                publicId = Path.GetFileNameWithoutExtension(path);
            }
            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DelResParams()
                {
                    PublicIds = new List<string> { folder + fileName },
                    Type = "upload",
                    ResourceType = ResourceType.Raw
                };
                var deletionResult = await _cloudinary.DeleteResourcesAsync(deletionParams);
                if (deletionResult != null && deletionResult.Deleted != null && deletionResult.Deleted.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<string> UploadCloudinary(IFormFile file, string folder)
        {
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString();
        }
    }
}
