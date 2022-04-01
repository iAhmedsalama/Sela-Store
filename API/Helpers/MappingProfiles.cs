using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()

                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))

                //Mapping full PictureUrl
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            //Mapping Address to AddressDto
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            //Mapping from customerBasketDto to customerBasket
            CreateMap<CustomerBasketDto, CustomerBasket>();

            //Mapping from basketItemDto to basketItem
            CreateMap<BasketItemDto, BasketItem>();

            //Mapping Address to AddressDto for Order Aggregate
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            //Mapping Order to OrderDto
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

            //Mapping OrderDto to Order
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
}
