using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FDMS.Entity;
using FDMS.Model;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.IO.Compression;

namespace FDMS.Service.CloudinaryService
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly FDMSContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly IHttpClientFactory _clientFactory;

        public CloudinaryService(Cloudinary cloudinary, IHttpClientFactory clientFactory, FDMSContext context)
        {
            this._cloudinary = cloudinary;
            this._clientFactory = clientFactory;
            this._context = context;
        }
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
        {
            { ".pdf", "application/pdf" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".mp4", "video/mp4" }
            // Thêm các phần mở rộng và kiểu mime tương ứng của chúng nếu cần thiết
        };
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

        public async Task<byte[]> DownloadDocument(string fileUrl)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(fileUrl);
            var fileExtension = System.IO.Path.GetExtension(fileUrl).ToLower();
            var contentType = MimeTypes.GetValueOrDefault(fileExtension, "application/octet-stream");
            return await response.Content.ReadAsByteArrayAsync();
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

        public async Task<Stream> DownloadFilesAsZip(List<int> fileIds)
        {
            MemoryStream outputStream = new MemoryStream();
            using (ZipArchive zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
            {
                foreach (int fileId in fileIds)
                {
                    var fileInfo = await _context.FlightDocuments
                        .Include(dt => dt.DocumentTypeNavigation)
                        .Include(c => c.AccountNavigation).ThenInclude(r => r.RoleNavigation)
                        .Include(f => f.FlightNavigation)
                        .FirstOrDefaultAsync(f => f.FlightDocumentId.Equals(fileId));
                    if (fileInfo == null)
                    {
                        continue;
                    }
                    var fileBytes = await DownloadDocument(fileInfo.FileUrl);
                    var entry = zipArchive.CreateEntry($"{fileInfo.FlightNavigation.FlightCode}{fileInfo.FlightNavigation.FlightId.ToString("D3")}-{fileInfo.FileName}-v{fileInfo.Version}.{fileInfo.VersionPatch}-{DateTime.Now:yyyy-MM-dd HH:mm:ss:ffff}-{fileInfo.FlightDocumentId}{fileInfo.FileType}");
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                }
            }
            outputStream.Seek(0, SeekOrigin.Begin);
            return outputStream;
        }
    }
}
