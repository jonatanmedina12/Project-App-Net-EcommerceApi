using Models.DTOs;
using Models.DTOs.Stock;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IStockHistoryRepository : IBaseRepository<StockHistory>
    {
        Task<StockHistory> AddAsync(StockHistory stockHistory);
        Task<IEnumerable<StockHistory>> GetHistoryByProductIdAsync(int productId);
        Task<IEnumerable<StockHistory>> GetHistoryByUserIdAsync(int userId);

    }
}
