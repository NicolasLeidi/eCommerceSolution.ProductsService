using AutoMapper;
using Products.BusinessLogic.DTO;
using Products.DataAccess.Entities;

namespace Products.BusinessLogic.Mapper;

public class AddRequestMappingProfile : Profile
{
    public AddRequestMappingProfile()
    {
        CreateMap<ProductAddRequest, Product>()
            .ForMember(dest => dest.ProductID, opt => opt.Ignore())
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
            .ReverseMap();
    }
}
