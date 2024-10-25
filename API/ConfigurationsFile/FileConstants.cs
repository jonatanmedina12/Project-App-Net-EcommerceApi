namespace API.ConfigurationsFile
{
    public class FileConstants
    {
        public static class Directories
        {
            public const string ProfileImages = "uploads/profiles";
            public const string ProductImages = "uploads/products";
            public const string Documents = "documents";
        }

        public static class FileTypes
        {
            public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            public const int MaxImageSize = 5 * 1024 * 1024; // 5MB
        }
    }
}
