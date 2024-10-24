using AutoMapper;
using Errors.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.DTOs.Product;
using Models.Entidades;
using Models.Interface;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger, IMapper mapper)
        : base(context)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Products
            .AnyAsync(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted);
        }

        public  async Task<IEnumerable<Product>> GetAllWithUserAsync()
        {
            try
            {
                return await _context.Products
                    .Include(p => p.CreatedByUser)
                    .Include(p => p.StockHistories)
                        .ThenInclude(sh => sh.ModifiedByUser)
                    .Where(p => !p.IsDeleted)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos con usuarios");
                throw;
            }
        }

        public async Task<Product> GetByIdWithUserAsync(int id)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.CreatedByUser)
                    .Include(p => p.StockHistories)
                        .ThenInclude(sh => sh.ModifiedByUser)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el producto {id} con usuario");
                throw;
            }
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products
            .Include(p => p.CreatedByUser)
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower() && !p.IsDeleted);
        }

        public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.CreatedByUser)
                    .Include(p => p.StockHistories)
                        .ThenInclude(sh => sh.ModifiedByUser)
                    .Where(p => p.CreatedByUserId == userId && !p.IsDeleted)
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener los productos del usuario {userId}");
                throw;
            }
        }

        public async Task<IDictionary<string, int>> GetStockInfoAsync()
        {
            try
            {
                return await _context.Products
                    .Where(p => !p.IsDeleted)
                    .ToDictionaryAsync(
                        p => p.Name,
                        p => p.Stock
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la información de stock");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> SearchAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products
             .Where(predicate)
             .ToListAsync();
        }
    }


}
