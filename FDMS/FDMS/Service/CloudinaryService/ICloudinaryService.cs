namespace FDMS.Service.CloudinaryService
{
    public interface ICloudinaryService
    {
        Task<bool> DeleteCloudinary(string url,string folder);
        Task<string> UploadCloudinary(IFormFile file, string folder);
        Task<byte[]> DownloadDocument(string fileUrl);
        Task<Stream> DownloadFilesAsZip(List<int> fileIds);
    }
}
