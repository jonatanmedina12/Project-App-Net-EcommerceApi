using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
           : base("El producto no fue encontrado.")
        {
        }

        public ProductNotFoundException(int productId)
            : base($"No se encontró el producto con ID '{productId}'.")
        {
            ProductId = productId;
        }

        public ProductNotFoundException(string message, int productId)
            : base(message)
        {
            ProductId = productId;
        }

        public int ProductId { get; }
    }
}
