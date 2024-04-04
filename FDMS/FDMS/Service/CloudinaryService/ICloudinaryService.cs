namespace FDMS.Service.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<bool> DeleteCloudinary(string url,string folder);
        Task<string> UploadCloudinary(IFormFile file, string folder);
    }
}
