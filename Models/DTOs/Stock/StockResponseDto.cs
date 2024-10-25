using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Stock
{
    public class StockResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int PreviousStock { get; set; }
        public int NewStock { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
