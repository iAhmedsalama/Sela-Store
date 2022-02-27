using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()

                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))

                //mapping full PictureUrl
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            //mapping Address to AddressDto
            CreateMap<Address, AddressDto>().ReverseMap();

            //mapping from customerBasketDto to customerBasket
            CreateMap<CustomerBasketDto, CustomerBasket>();

            //mapping from basketItemDto to basketItem
            CreateMap<BasketItemDto, BasketItem>();
        }
    }
}
