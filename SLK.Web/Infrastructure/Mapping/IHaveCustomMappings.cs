using AutoMapper;

namespace SLK.Web.Infrastructure.Mapping
{
    interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}
