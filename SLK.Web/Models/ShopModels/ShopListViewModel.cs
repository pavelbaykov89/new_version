using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System.ComponentModel;

namespace SLK.Web.Models.ShopModels
{
    public class ShopListViewModel : ListModel, IMapFrom<Shop>
    {
        public int ID { get; protected set; }

        [DisplayName("Store name")]
        public string Name { get; protected set; }

        [DisplayName("Store importance")]
        public int DisplayOrder { get; protected set; }

        public string Phone { get; protected set; }

        [DisplayName("Cellular")]
        public string Phone2 { get; protected set; }

        [DisplayName("Orders delivery email")]
        public string Email { get; protected set; }

        [DisplayName("Kosher")]
        public bool IsKosher { get; protected set; }
        
        public bool Active { get; protected set; }

        [DisplayName("Domain address extension")]
        public string SeoUrl { get; set; }
    }
}