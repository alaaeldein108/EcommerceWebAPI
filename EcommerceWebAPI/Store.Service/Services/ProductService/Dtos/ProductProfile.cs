using AutoMapper;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductService.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Store.Data.Entities.Product,ProductDetailsDto>()
                .ForMember(des=>des.BrandName,options=>options.MapFrom(src=>src.Brand.Name))
                .ForMember(des => des.TypeName, options => options.MapFrom(src => src.Type.Name))
                .ForMember(des => des.PictureUrl, options => options.MapFrom<ProductUrlResolver>());
            

            CreateMap<ProductBrand, BrandTypeDetailsDto>();
            CreateMap<ProductType, BrandTypeDetailsDto>();

        }
    }
}
