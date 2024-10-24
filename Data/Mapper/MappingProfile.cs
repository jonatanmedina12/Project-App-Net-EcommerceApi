using Models.DTOs.Stock;
using Models.DTOs;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Models.DTOs.Account;
using Models.DTOs.Product;

namespace Data.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null));

            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.CreatedByUserName,
                          opt => opt.MapFrom(src => src.CreatedByUser.Username));

            // User mapping
            CreateMap<User, UserDto>();

            CreateMap<StockHistory, StockHistoryDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ModifiedByUserName,
                    opt => opt.MapFrom(src => src.ModifiedByUser.Username));

            CreateMap<StockHistory, StockResponseDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ModifiedByUser,
                    opt => opt.MapFrom(src => src.ModifiedByUser.Username))
                .ForMember(dest => dest.ModifiedAt,
                    opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<StockHistory, StockHistoryDto>();

        }
    }
}

