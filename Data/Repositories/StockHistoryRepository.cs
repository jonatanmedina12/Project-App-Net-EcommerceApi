using Errors.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.DTOs.Stock;
using Models.Entidades;
using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class StockHistoryRepository : BaseRepository<StockHistory>, IStockHistoryRepository
    {
        private readonly ILogger<StockHistoryRepository> _logger;
        private readonly IProductRepository productRepository;
        public StockHistoryRepository(ApplicationDbContext context)
        : base(context)
        {
        }

        public  async Task<IEnumerable<StockHistory>> GetHistoryByProductIdAsync(int productId)
        {
            try
            {
                return await _context.StockHistory
                    .Include(sh => sh.Product)
                    .Include(sh => sh.ModifiedByUser)
                    .Where(sh => sh.ProductId == productId)
                    .OrderByDescending(sh => sh.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el historial del producto {productId}");
                throw;
            }
        }

        public async Task<IEnumerable<StockHistory>> GetHistoryByUserIdAsync(int userId)
        {
            try
            {
                return await _context.StockHistory
                    .Include(sh => sh.Product)
                    .Include(sh => sh.ModifiedByUser)
                    .Where(sh => sh.ModifiedByUserId == userId)
                    .OrderByDescending(sh => sh.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el historial del usuario {userId}");
                throw;
            }
        }
    }
 }        
    

