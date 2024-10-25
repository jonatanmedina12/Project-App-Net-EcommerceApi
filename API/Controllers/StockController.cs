using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using System.Security.Claims;

namespace API.Controllers
{

    public class StockController : AutenticacionController
    {
        private readonly StockService _stockService;
        private readonly ILogger<StockController> _logger;

        public StockController(StockService stockService, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            // Buscar el ID en el claim "nameid" que es donde está en tu token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("nameid");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Usuario no autorizado");
            }
            return userId;
        }

        [HttpPut("{productId}/update")]

        public async Task<IActionResult> UpdateStock(int productId, [FromBody] UpdateStockDto request)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var result = await _stockService.UpdateStockAsync(productId, request, userId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Producto no encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operación inválida");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el stock");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene el historial de stock de un producto
        /// </summary>
        [HttpGet("{productId}/history")]

        public async Task<IActionResult> GetStockHistory(int productId)
        {
            try
            {
                var history = await _stockService.GetStockHistoryAsync(productId);
                if (history == null || !history.Any())
                {
                    return NotFound(new { message = $"No se encontró historial para el producto {productId}" });
                }

                return Ok(history);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Producto no encontrado");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de stock");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene el historial de operaciones de stock realizadas por el usuario actual
        /// </summary>
        [HttpGet("my-history")]

        public async Task<IActionResult> GetMyStockHistory()
        {
            try
            {
                var userId = GetCurrentUserId();


                var history = await _stockService.GetUserStockHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de stock del usuario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene el historial de operaciones de stock de un usuario específico (solo para administradores)
        /// </summary>
        [HttpGet("user/{userId}/history")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<StockHistoryDto>), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetUserStockHistory(int userId)
        {
            try
            {
                var history = await _stockService.GetUserStockHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de stock del usuario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }


}
