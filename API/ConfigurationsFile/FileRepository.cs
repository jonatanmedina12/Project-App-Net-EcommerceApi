using Data.Repositories;
using Errors.Errors;
using Microsoft.AspNetCore.Hosting;
using Models.DTOs;
using Models.Entidades;
using Models.Interface;
using System.ComponentModel.DataAnnotations;

namespace API.ConfigurationsFile
{
    public class FileRepository : IFileRepository
    {
        private readonly string _webRootPath;
        private readonly ILogger<FileRepository> _logger;
        private readonly IUserRepository _userRepository;

        public FileRepository(
            IWebHostEnvironment env,
            ILogger<FileRepository> logger,
            IUserRepository userRepository)
        {
            _webRootPath = env.WebRootPath;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string directory)
        {
            try
            {
                var uploadPath = Path.Combine(_webRootPath, directory);
                Directory.CreateDirectory(uploadPath);

                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!IsValidFileExtension(fileExtension))
                {
                    throw new ValidationException("Tipo de archivo no permitido");
                }

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Path.Combine(directory, fileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar archivo: {FileName}", file.FileName);
                throw;
            }
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_webRootPath, filePath);
                if (!File.Exists(fullPath))
                {
                    _logger.LogWarning("Archivo no encontrado: {FilePath}", filePath);
                    throw new FileNotFoundException("Archivo no encontrado", filePath);
                }

                return await File.ReadAllBytesAsync(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener archivo: {FilePath}", filePath);
                throw;
            }
        }

        public void DeleteFile(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_webRootPath, filePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("Archivo eliminado: {FilePath}", filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar archivo: {FilePath}", filePath);
                throw;
            }
        }

        public bool FileExists(string filePath)
        {
            var fullPath = Path.Combine(_webRootPath, filePath);
            return File.Exists(fullPath);
        }

        public string GetFileUrl(string filePath, HttpRequest request)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            return $"{request.Scheme}://{request.Host}/api/files/{filePath}";
        }

        public async Task<UserDto> UpdateProfileAsync(UpdateUserProfileDto updateUserProfileDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(updateUserProfileDto.Id);
                if (user == null)
                {
                    throw new NotFoundException($"Usuario no encontrado: {updateUserProfileDto.Id}");
                }

                if (await _userRepository.IsUsernameOrEmailTakenAsync(
                    updateUserProfileDto.Id,
                    updateUserProfileDto.Username,
                    updateUserProfileDto.Email))
                {
                    throw new ValidationException("Username o email ya en uso");
                }

                user.Username = updateUserProfileDto.Username;
                user.Email = updateUserProfileDto.Email;

                if (updateUserProfileDto.ProfilePhoto != null)
                {
                    if (!ValidateFile(
                        updateUserProfileDto.ProfilePhoto,
                        FileConstants.FileTypes.AllowedImageExtensions,
                        FileConstants.FileTypes.MaxImageSize))
                    {
                        throw new ValidationException("Imagen inválida");
                    }

                    if (!string.IsNullOrEmpty(user.ProfilePhoto))
                    {
                        DeleteFile(user.ProfilePhoto);
                    }

                    var photoPath = await SaveFileAsync(
                        updateUserProfileDto.ProfilePhoto,
                        FileConstants.Directories.ProfileImages);

                    user.ProfilePhoto = photoPath;
                }

                await _userRepository.UpdateAsync(user);

                return new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando perfil: {UserId}", updateUserProfileDto.Id);
                throw;
            }
        }

        public bool ValidateFile(IFormFile file, string[] allowedExtensions, long maxSize)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) &&
                   allowedExtensions.Contains(extension) &&
                   file.Length <= maxSize;
        }

        private bool IsValidFileExtension(string extension)
        {
            return FileConstants.FileTypes.AllowedImageExtensions.Contains(extension.ToLower());
        }
    }
}
