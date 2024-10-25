using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class StockHistoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public StockOperationType OperationType { get; set; }
        public string Reason { get; set; }
        public string ModifiedByUserName { get; set; }
        public int PreviousStock { get; set; }
        public int NewStock { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
