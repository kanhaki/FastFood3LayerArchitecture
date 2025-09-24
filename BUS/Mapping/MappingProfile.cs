using AutoMapper;
using DAT.Entity;
using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product -> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ComboOptionGroups, opt => opt.MapFrom(src => src.ComboOptionGroups));

            // ComboOptionGroup -> DTO
            CreateMap<ComboOptionGroup, ComboOptionGroupDTO>()
                .ForMember(dest => dest.OptionItems, opt => opt.MapFrom(src => src.ComboOptionItems));

            // ComboOptionItem -> DTO
            CreateMap<ComboOptionItem, ComboOptionItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            // Category -> CategoryDto
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        }
    }
}
