using Microsoft.Extensions.Logging;
using Models.DTOs.Stock;
using Models.DTOs;
using Models.Entidades;
using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class StockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly ILogger<StockService> _logger;

        public StockService(IProductRepository productRepository, IStockHistoryRepository stockHistoryRepository, ILogger<StockService> logger)
        {
            _productRepository = productRepository;
            _stockHistoryRepository = stockHistoryRepository;
            _logger = logger;
        }

        public async Task<StockResponseDto> UpdateStockAsync(int productId, UpdateStockDto request, int userId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"No se encontró el producto con ID {productId}");
                }

                int previousStock = product.Stock;
                int newStock;

                switch (request.OperationType)
                {
                    case StockOperationType.Addition:
                        newStock = previousStock + request.Quantity;
                        break;

                    case StockOperationType.Subtraction:
                        if (previousStock < request.Quantity)
                        {
                            throw new InvalidOperationException(
                                $"Stock insuficiente. Stock actual: {previousStock}, Cantidad solicitada: {request.Quantity}");
                        }
                        newStock = previousStock - request.Quantity;
                        break;

                    default:
                        throw new ArgumentException("Tipo de operación no válida");
                }

                // Actualizar el stock del producto
                product.Stock = newStock;
                product.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);

                // Registrar el movimiento en el historial
                var stockHistory = new StockHistory
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    OperationType = request.OperationType,
                    Reason = request.Reason,
                    ModifiedByUserId = userId,
                    PreviousStock = previousStock,
                    NewStock = newStock
                };

                await _stockHistoryRepository.AddAsync(stockHistory);

                return new StockResponseDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PreviousStock = previousStock,
                    NewStock = newStock,
                    ModifiedByUser = product.CreatedByUser?.Username ?? "Sistema",
                    ModifiedAt = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el stock del producto {productId}");
                throw;
            }
        }
        public async Task<IEnumerable<StockHistoryDto>> GetStockHistoryAsync(int productId)
        {
            try
            {
                var history = await _stockHistoryRepository.GetHistoryByProductIdAsync(productId);
                return MapToStockHistoryDto(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el historial de stock del producto {productId}");
                throw;
            }
        }

        public async Task<IEnumerable<StockHistoryDto>> GetUserStockHistoryAsync(int userId)
        {
            try
            {
                var history = await _stockHistoryRepository.GetHistoryByUserIdAsync(userId);
                return MapToStockHistoryDto(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el historial de stock del usuario {userId}");
                throw;
            }
        }
        private IEnumerable<StockHistoryDto> MapToStockHistoryDto(IEnumerable<StockHistory> history)
        {
            return history.Select(item => new StockHistoryDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "Producto Eliminado",
                Quantity = item.Quantity,
                OperationType = item.OperationType,
                Reason = item.Reason,
                ModifiedByUserName = item.ModifiedByUser?.Username ?? "Usuario Eliminado",
                PreviousStock = item.PreviousStock,
                NewStock = item.NewStock,
                CreatedAt = item.CreatedAt
            });
        }
    }
}
