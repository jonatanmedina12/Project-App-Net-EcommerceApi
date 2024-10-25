using Microsoft.AspNetCore.Http;
using Models.DTOs.Account;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(CreateUserDto createUserDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto);
        Task<string> UploadProfilePhotoAsync(int userId, IFormFile photo);
    }
}
