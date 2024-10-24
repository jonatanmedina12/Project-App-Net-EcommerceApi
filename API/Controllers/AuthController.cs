using API.Errors;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models.DTOs.Account;

namespace API.Controllers
{

    public class AuthController : BaseController
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Datos de registro no válidos.", errors = ModelState });
                }

                var user = await _authService.RegisterAsync(createUserDto);

                if (user == null)
                {
                    return BadRequest(new { message = "El registro falló. Es posible que el nombre de usuario o el correo electrónico ya estén en uso." });
                }

                return Ok(new { message = "Usuario registrado correctamente", user = new { user.Username, user.Email } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Se produjo un error durante el registro", error = ex.Message });
            }
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(ApiResponse<LoginResponseDto>.Successful(response, "Inicio de sesión exitoso"));
            }
            catch (CustomException ex)
            {
                return Unauthorized(ApiResponse<object>.Failed(ex.Message, ex.ErrorCode));
            }
        }
    }
}
