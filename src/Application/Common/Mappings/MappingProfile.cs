using Application.Common.DTOs;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.UpdateProduct;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.OrderAggregate;

namespace Application.Common.Mappings
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();

            CreateMap<Basket, BasketDto>().ReverseMap();

            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.Name, y => { y.MapFrom(z => z.Product!.Name); })
                .ForMember(x => x.Price, y => { y.MapFrom(z => z.Product!.Price); })
                .ForMember(x => x.PictureUrl, y => { y.MapFrom(z => z.Product!.PictureUrl); })
                .ForMember(x => x.Brand, y => { y.MapFrom(z => z.Product!.Brand); })
                .ForMember(x => x.Type, y => { y.MapFrom(z => z.Product!.Type); });

            CreateMap<Order, OrderDto>()
                .ForMember(x => x.OrderStatus, y => { y.MapFrom(z => z.OrderStatus.ToString()); })
                .ForMember(x => x.Total, y => { y.MapFrom(z => z.GetTotal()); })
                .ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.Name, y => { y.MapFrom(z => z.ItemOrdered!.Name); })
                .ForMember(x => x.PictureUrl, y => { y.MapFrom(z => z.ItemOrdered!.PictureUrl); })
                .ReverseMap();
        }
    }
}
