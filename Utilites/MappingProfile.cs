using AutoMapper;
using Models.DTOs;
using Models.DTOs.Product;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilites
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Product, ProductDto>();
            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.CreatedByUserName,
                    opt => opt.MapFrom(src => src.CreatedByUser.Username));
            CreateMap<Product, ProductStockDto>()
                .ForMember(dest => dest.CreatedByUserName,
                    opt => opt.MapFrom(src => src.CreatedByUser.Username));
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<StockHistory, StockResponseDto>()
              .ForMember(dest => dest.ProductName,
                  opt => opt.MapFrom(src => src.Product.Name))
              .ForMember(dest => dest.ModifiedByUser,
                  opt => opt.MapFrom(src => src.ModifiedByUser.UserName))
              .ForMember(dest => dest.ModifiedAt,
                  opt => opt.MapFrom(src => src.CreatedAt));
        }

    }
}
