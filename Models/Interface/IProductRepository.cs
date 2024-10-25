using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllProductsWithDetailsAsync();
        Task<ProductDto> GetProductWithDetailsAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto productDto, int userId);
        Task<int> UpdateStockAsync(int id, UpdateStockDto updateStockDto);
    }
}
