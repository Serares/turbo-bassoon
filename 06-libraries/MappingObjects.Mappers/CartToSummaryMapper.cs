using AutoMapper;
using AutoMapper.Internal;
using Northwind.EntityModels;
using Northwind.ViewModels;

namespace MappingObjects.Mappers;

public static class CartToSummaryMapper
{
    public static MapperConfiguration GetMapperConfiguration()
    {
        MapperConfiguration config = new(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
            // configure mapper using projections
            cfg.CreateMap<Cart, Summary>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                string.Format("{0} {1}", src.Customer.FirstName, src.Customer.LastName)
            ))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Items.Sum(item => item.UnitPrice * item.Quantity)));
        });

        return config;
    }
}

