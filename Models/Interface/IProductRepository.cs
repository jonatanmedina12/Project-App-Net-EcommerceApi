using Models.DTOs;
using Models.DTOs.Product;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllWithUserAsync();
        Task<Product> GetByIdWithUserAsync(int id);
        Task<IEnumerable<Product>> GetProductsByUserIdAsync(int userId);
        Task<IDictionary<string, int>> GetStockInfoAsync();
        Task<Product> GetByNameAsync(string name);
        Task<IEnumerable<Product>> SearchAsync(Expression<Func<Product, bool>> predicate);



    }
}
