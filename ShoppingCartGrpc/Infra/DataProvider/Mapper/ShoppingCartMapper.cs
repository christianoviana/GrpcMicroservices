using AutoMapper;
using ShoppingCartGrpc.Domain.Models;
using ShoppingCartGrpc.Protos;

namespace ShoppingCartGrpc.Infra.DataProvider.Mapper
{
    public class ShoppingCartMapper : Profile
    {
        public ShoppingCartMapper()
        {
            CreateMap<ShoppingCartModel, ShoppingCart>().ReverseMap();
            CreateMap<CartItemModel, CartItem>().ReverseMap();
        }
    }
}
