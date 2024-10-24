using API.ConfigurationsFile;
using API.Errors;
using Data.Services;
using Errors.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Interface;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace API.Controllers
{

    public class AccountController : AutenticacionController
    {

        private readonly ILogger<AccountController> _logger;

        private readonly FileService fileService;
        private readonly IUserRepository userRepository;

        public AccountController(ILogger<AccountController> logger, FileService fileService, IUserRepository userRepository)
        {
            _logger = logger;
            this.fileService = fileService;
            this.userRepository = userRepository;
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromForm] UpdateUserProfileDto updateDto)
        {
            try
            {
               
                var updatedUser = await fileService.UpdateProfileAsync(updateDto);

                return Ok(ApiResponse<UserDto>.Successful(
                    updatedUser,
                    "Perfil actualizado correctamente"
                ));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiResponse<object>.Failed(ex.Message, "VALIDATION_ERROR"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Failed(ex.Message, "NOT_FOUND"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                return StatusCode(500, ApiResponse<object>.Failed(
                    "Error interno del servidor",
                    "INTERNAL_ERROR"
                ));
            }
        }

        [HttpGet("photo/{userId}")]
        public async Task<IActionResult> GetProfilePhoto(int userId)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(userId);
                if (user == null || string.IsNullOrEmpty(user.ProfilePhoto))
                {
                    return NotFound(ApiResponse<object>.Failed("Foto de perfil no encontrada"));
                }

                // Obtener los bytes de la imagen
                var photoBytes = await fileService.GetFileAsync(user.ProfilePhoto);

                // Determinar el tipo de contenido basado en la extensión del archivo
                string contentType = fileService.GetContentType(user.ProfilePhoto);

                // Devolver el archivo con el tipo de contenido correcto
                return File(photoBytes, contentType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener foto de perfil para usuario {UserId}", userId);
                return StatusCode(500, ApiResponse<object>.Failed("Error al obtener la foto de perfil"));
            }
        }
        [HttpDelete("photo/{userId}")]
        public async Task<IActionResult> DeleteProfilePhoto(int userId)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(ApiResponse<object>.Failed("Usuario no encontrado"));
                }

                if (string.IsNullOrEmpty(user.ProfilePhoto))
                {
                    return NotFound(ApiResponse<object>.Failed("El usuario no tiene foto de perfil"));
                }

                // Eliminar el archivo físico
                fileService.DeleteFile(user.ProfilePhoto);

                // Actualizar el registro del usuario
                user.ProfilePhoto = null;
                await userRepository.UpdateAsync(user);

                return Ok(ApiResponse<object>.Successful("Foto de perfil eliminada exitosamente"));
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(ex, "Archivo de foto de perfil no encontrado para usuario {UserId}", userId);
                return NotFound(ApiResponse<object>.Failed("Archivo de foto no encontrado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar foto de perfil para usuario {UserId}", userId);
                return StatusCode(500, ApiResponse<object>.Failed("Error al eliminar la foto de perfil"));
            }
        }

    }
}
