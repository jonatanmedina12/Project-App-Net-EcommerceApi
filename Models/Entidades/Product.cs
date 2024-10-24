using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedByUserId { get; set; }
        [JsonIgnore]

        public virtual User CreatedByUser { get; set; }
        [JsonIgnore]

        public virtual ICollection<StockHistory> StockHistories { get; set; }

    }
}
