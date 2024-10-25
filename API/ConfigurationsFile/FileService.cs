using Data.Repositories;
using Models.Entidades;
using Models.Interface;
using System.ComponentModel.DataAnnotations;
using API.Errors;
using Errors.Errors;
using Models.DTOs.Account;
namespace API.ConfigurationsFile
{
    public class FileService : IFileRepository
    {
        private readonly IFileRepository _fileRepository;
        private readonly ILogger<FileService> _logger;
        private readonly IUserRepository _userRepository;

        public FileService(IFileRepository fileRepository, ILogger<FileService> logger, IUserRepository userRepository)
        {
            _fileRepository = fileRepository;
            _logger = logger;
            _userRepository = userRepository;
        }

        public void DeleteFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentException("La ruta del archivo es nula o vacía");
                }

                _fileRepository.DeleteFile(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FileService.DeleteFile");
                throw;
            }
        }

        public bool FileExists(string filePath)
        {
            return _fileRepository.FileExists(filePath);
        }

        public  async Task<byte[]> GetFileAsync(string filePath)
        {
            try
            {
                return await _fileRepository.GetFileAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FileService.GetFileAsync");
                throw;
            }
        }

        public string GetContentType(string filePath)
        {
            // Obtener la extensión del archivo
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            // Mapear las extensiones comunes a tipos MIME
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream" // Tipo por defecto si no se reconoce la extensión
            };
        }
        public string GetFileUrl(string filePath, HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public async  Task<string> SaveFileAsync(IFormFile file, string directory)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("No se ha proporcionado ningún archivo");
                }

                // Validar tamaño del archivo (ejemplo: 10MB máximo)
                if (file.Length > 10 * 1024 * 1024)
                {
                    throw new ArgumentException("El archivo excede el tamaño máximo permitido");
                }

                return await _fileRepository.SaveFileAsync(file, directory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FileService.SaveFileAsync");
                throw;
            }
        }

        public async Task<UserDto> UpdateProfileAsync(UpdateUserProfileDto updateUserProfileDto)
        {
            try
            {
                return await _fileRepository.UpdateProfileAsync(updateUserProfileDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FileService.GetFileAsync");
                throw;
            }
        }

        public bool ValidateFile(IFormFile file, string[] allowedExtensions, long maxSize)
        {
            throw new NotImplementedException();
        }
    }
}
