using AutoMapper;
using ProductGrpc.Protos;
using ShoppingCartGrpc.Protos;

namespace ShoppingCartWorkerService.Mapper
{
    internal class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductModel, CartItemModel>()
                .ForMember(dest => dest.ProductName, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Quantity, o => o.MapFrom(src => 1));


            CreateMap<CartItemModel, ProductModel>()
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.ProductName));
        }
    }
}
