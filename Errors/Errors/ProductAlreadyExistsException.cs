using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Errors.Errors
{
    public class ProductAlreadyExistsException : Exception
    {
        public ProductAlreadyExistsException()
            : base("El producto ya existe en la base de datos.")
        {
        }

        public ProductAlreadyExistsException(string productName)
            : base($"Ya existe un producto con el nombre '{productName}'.")
        {
            ProductName = productName;
        }

        public ProductAlreadyExistsException(string productName, Exception inner)
            : base($"Ya existe un producto con el nombre '{productName}'.", inner)
        {
            ProductName = productName;
        }

        public string ProductName { get; }
    }
}
