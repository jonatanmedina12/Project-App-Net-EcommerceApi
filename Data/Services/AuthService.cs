using API.Errors;
using Data.Repositories;
using Errors.Errors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs.Account;
using Models.Entidades;
using Models.Interface;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Data.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<User> RegisterAsync(CreateUserDto createUserDto)
        {
            return await _authRepository.RegisterAsync(createUserDto);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            return await _authRepository.LoginAsync(loginDto);

        }




    }
}
