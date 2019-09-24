using AutoMapper;
using OnlineStore.Abstract;
using OnlineStore.Areas.Items.Models.Request.Items;
using OnlineStore.Areas.Items.Models.Response.Items;
using OnlineStore.Models.Database;

namespace OnlineStore.Areas.Items
{
    public class ItemsMapperInit : IMapperInit
    {
        public void MapperInit(IMapperConfigurationExpression config)
        {
            config.CreateMap<Item, ItemResponseModel>();
            config.CreateMap<ItemRequestModel, Item>();
        }
    }
}