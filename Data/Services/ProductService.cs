using Data.Repositories;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using Models.DTOs.Account;
using Models.DTOs.Product;
using Models.Entidades;
using Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class ProductService 
    {
        private readonly IProductRepository productRepository;
        private readonly IUserRepository userRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository , IUserRepository userRepository, ILogger<ProductService> logger)
        {
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            _logger=logger;
        }

        public async Task<Product> RegistarProducto(ProductDto productDto)
        {
            try
            {
                User user = await userRepository.GetByIdAsync(productDto.CreatedId);

                if (user == null)
                {
                    throw new InvalidOperationException($"Error no se puede crear el producto '{user.Id}' no existe");

                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Stock = productDto.Stock,
                    IsDeleted = false,
                    CreatedByUserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                };

                return await productRepository.AddAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto");
                throw;
            }

        }
        public async Task<Product> ActualizarProducto(int id, ProductDto productDto)
        {
            try
            {
                var existingProduct = await productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    throw new InvalidOperationException($"No se encontró el producto con ID: {id}");
                }

                existingProduct.Name = productDto.Name;
                existingProduct.Description = productDto.Description;
                existingProduct.Price = productDto.Price;

                return await productRepository.UpdateAsync(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el producto con ID: {id}");
                throw;
            }
        }

        public async Task<bool> EliminarProducto(int id)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    throw new InvalidOperationException($"No se encontró el producto con ID: {id}");
                }

                product.IsDeleted = true;
                await productRepository.UpdateAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el producto con ID: {id}");
                throw;
            }
        }

        public async Task<Product> ObtenerProductoPorId(int id)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);
                if (product == null || product.IsDeleted)
                {
                    throw new InvalidOperationException($"No se encontró el producto con ID: {id}");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener el producto con ID: {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> BuscarProductos(string searchTerm)
        {
            try
            {
                var products = await productRepository.SearchAsync(p =>
                    !p.IsDeleted &&
                    (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                );
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al buscar productos con término: {searchTerm}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> ObtenerTodosLosProductos()
        {
            try
            {
                var product = await productRepository.GetAllAsync();
                var mostrar = new List<Product>();
                foreach (var item in product)
                {
                    if (!item.IsDeleted)
                    {
                        mostrar.Add(item);
                    }
                }
                return mostrar;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos");
                throw;
            }
        }

    }
}
