using System;
using AutoMapper;
using OnlineStore.Abstract;
using OnlineStore.Areas.Items.Models.Request.Items;
using OnlineStore.Areas.Items.Models.Request.Order;
using OnlineStore.Areas.Items.Models.Response.Items;
using OnlineStore.Areas.Items.Models.Response.Order;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Items
{
    public class ItemsMapperInit : IMapperInit
    {
        public void MapperInit(IMapperConfigurationExpression config)
        {
            var random = new Random();

            config.CreateMap<Item, ItemResponseModel>();
            config.CreateMap<ItemRequestModel, Item>();

            config.CreateMap<OrderItem, OrderItemResponseModel>();
            config.CreateMap<Order, OrderResponseModel>();

            config.CreateMap<OrderItemRequestModel, OrderItem>();
            config.CreateMap<OrderRequestModel, Order>()
                .ForMember(e => e.OrderNumber, e => e.MapFrom(l => random.Next()))
                .ForMember(e => e.OrderDate, e => e.MapFrom(l => DateTime.Now));
        }
    }
}