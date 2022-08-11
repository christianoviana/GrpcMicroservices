using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using ProductGrpc.Domain.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>()
                .ForMember(d => d.ProductId, o => o.MapFrom(src => src.Id))
                .ForMember(d => d.CreatedTime,
                o => o.MapFrom(s => Timestamp.FromDateTime(DateTime.SpecifyKind(s.CreatedAt, DateTimeKind.Utc))));

            CreateMap<ProductModel, Product>()
              .ForMember(d => d.Id, o => o.MapFrom(src => src.ProductId))
              .ForMember(d => d.CreatedAt, o =>
              o.MapFrom(s => s.CreatedTime.ToDateTime()));
        }
    }
}
