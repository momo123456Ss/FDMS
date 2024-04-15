namespace FDMS.Model
{
    public class FlightDocumentDownloadModel
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int Version { get; set; }
        public int VersionPatch { get; set; }
    }
}
