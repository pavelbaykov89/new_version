using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System.Web.Mvc;

namespace SLK.Web.Models.ShopModels
{
    public class ShopTypeListViewModel : ListModel, IMapFrom<ShopType>
    {
        [HiddenInput]
        public int ID { get; protected set; }

        public string Name { get; protected set; }

        public int DisplayOrder { get; protected set; }
    }
}