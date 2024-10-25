using Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UpdateStockDto
    {
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El tipo de operación es requerido")]
        public StockOperationType OperationType { get; set; }

        [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres")]
        public string Reason { get; set; }
    }
}
