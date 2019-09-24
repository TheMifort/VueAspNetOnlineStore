using AutoMapper;

namespace OnlineStore.Abstract
{
    public interface IMapperInit
    {
        void MapperInit(IMapperConfigurationExpression config);
    }
}