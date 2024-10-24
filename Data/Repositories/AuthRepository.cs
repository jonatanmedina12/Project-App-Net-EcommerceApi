using API.Errors;
using Data.ConfigurationJwt;
using Data.Services;
using Errors.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.Entidades;
using Models.Interface;
using System.ComponentModel.DataAnnotations;


namespace Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _iuserRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository userRepository;
        private readonly IConfiguration _configuration;

        private ConfigJwt  _configurationJwt;

    
        public AuthRepository(IUserRepository iuserRepository, 
            IRoleRepository roleRepository, ILogger<AuthService> logger, IUserRepository userRepository, IConfiguration configuration, ConfigJwt configurationJwt)
        {
            _iuserRepository = iuserRepository;
            _roleRepository = roleRepository;
            _logger = logger;
            this.userRepository = userRepository;
            _configuration = configuration;
            _configurationJwt = configurationJwt;
        }

        public Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Validar parámetros de entrada
                if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    throw new ValidationException("El usuario y la contraseña son requeridos");
                }

                // Obtener usuario
                var user = await _iuserRepository.GetByEmailAsync(loginDto.Email);

                // Validar existencia del usuario y contraseña
                if (user == null || !_configurationJwt.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _logger.LogWarning("Intento de inicio de sesión fallido para el usuario: {password}", loginDto.Password);
                    throw new InvalidCredentialsException();
                }

                // Validar si el usuario está activo
                if (!user.IsActive)
                {
                    _logger.LogWarning("Intento de inicio de sesión con cuenta inactiva: {Username}", loginDto.Email);
                    throw new UserNotActiveException();
                }

                // Obtener la configuración de expiración del token
                var tokenExpirationMinutes = 60; // valor por defecto
                if (int.TryParse(_configuration["Jwt:TokenExpirationMinutes"], out int configuredMinutes))
                {
                    tokenExpirationMinutes = configuredMinutes;
                }

                // Generar token
                var (token, expiration) = _configurationJwt.GenerateJwtToken(user, tokenExpirationMinutes);

                _logger.LogInformation("Usuario {Email} ha iniciado sesión exitosamente", loginDto.Email);

                // Crear respuesta
                return new LoginResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>(),
                    TokenExpiration = expiration
                };
            }
            catch (Exception ex) when (ex is not CustomException)
            {
                _logger.LogError(ex, "Error durante el proceso de login para el usuario {email}", loginDto.Email);
                throw new AuthenticationException("Error en el proceso de autenticación");
            }

        }

        public virtual  async Task<User> RegisterAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _iuserRepository.GetByUsernameOrEmail(createUserDto.Username, createUserDto.Email);
            if (existingUser != null)
            {
                if (existingUser.Username.Equals(createUserDto.Username, StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserAlreadyExistsException($"El nombre de usuario '{createUserDto.Username}' ya está registrado");
                }
                throw new UserAlreadyExistsException($"El correo electrónico '{createUserDto.Email}' ya está registrado");
            }


            _configurationJwt.CreatePasswordHash(createUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Obtener los roles de la base de datos
            var existingRoles = await _roleRepository.GetRolesByNames(createUserDto.Roles);

            // Verificar si todos los roles existen
            var missingRoles = createUserDto.Roles.Where(roleName =>
                !existingRoles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (missingRoles.Any())
            {
                throw new RoleNotFoundException($"Los siguientes roles no existen: {string.Join(", ", missingRoles)}");
            }


            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = true,
                UserRoles = existingRoles.Select(role => new UserRole
                {
                    Role = role
                }).ToList()
            };

            return await _iuserRepository.CreateUserAsync(user);
        }

        public Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadProfilePhotoAsync(int userId, IFormFile photo)
        {
            throw new NotImplementedException();
        }
    }
}
