using AutoMapper;
using OnlineStore.Abstract;
using OnlineStore.Areas.Admin.Models.Request.Customer;
using OnlineStore.Areas.Admin.Models.Response.Customer;
using OnlineStore.Models;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Admin
{
    public class AdminMapperInit : IMapperInit
    {
        public void MapperInit(IMapperConfigurationExpression config)
        {
            config.CreateMap<Customer, CustomerResponseModel>()
                .ForMember(e => e.Code, e => e.MapFrom(l => l.Code.ToString()));

            config.CreateMap<CustomerResponseModel, Customer>()
                .ForMember(e => e.Code, e => e.MapFrom(l => CustomerCode.FromString(l.Code)));

            config.CreateMap<Customer, CustomerRequestModel>()
                .ForMember(e => e.Code, e => e.MapFrom(l => l.Code.ToString()));

            config.CreateMap<CustomerRequestModel, Customer>()
                .ForMember(e => e.Code, e => e.MapFrom(l => CustomerCode.FromString(l.Code)));
        }
    }
}