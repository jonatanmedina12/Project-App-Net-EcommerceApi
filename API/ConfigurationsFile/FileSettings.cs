namespace API.ConfigurationsFile
{
    public class FileSettings
    {
        public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB por defecto
        public string[] AllowedExtensions { get; set; } = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
        public string UploadPath { get; set; } = "uploads";
    }
}
