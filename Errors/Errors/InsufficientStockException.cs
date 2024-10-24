using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class InsufficientStockException : Exception
    {
        public InsufficientStockException()
           : base("No hay suficiente stock disponible.")
        {
        }

        public InsufficientStockException(string productName, int requestedQuantity, int availableStock)
            : base($"Stock insuficiente para '{productName}'. Solicitado: {requestedQuantity}, Disponible: {availableStock}")
        {
            ProductName = productName;
            RequestedQuantity = requestedQuantity;
            AvailableStock = availableStock;
        }

        public string ProductName { get; }
        public int RequestedQuantity { get; }
        public int AvailableStock { get; }
    }
}
