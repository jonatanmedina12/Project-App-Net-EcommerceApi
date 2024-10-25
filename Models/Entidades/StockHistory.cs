using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class StockHistory : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public StockOperationType OperationType { get; set; }
        public string Reason { get; set; }
        public int ModifiedByUserId { get; set; }
        public virtual User ModifiedByUser { get; set; }
        public int PreviousStock { get; set; }
        public int NewStock { get; set; }
    }
}
