using SLK.Domain.Core;
using SLK.Web.Filters;
using SLK.Web.Infrastructure.Mapping;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models.ShopModels
{
    public class AddEditShopForm //: AddEditForm, IMapFrom<Shop>
    {
        [HiddenInput]
        public int? ID { get; set; }

        public ShopMainTab MainTab { get; set; } = new ShopMainTab();

        public ShopDeliveryTab DeliveryTab { get; set; } = new ShopDeliveryTab();
    }

    public class ShopMainTab : IMapFrom<Shop>
    {
        [Required]
        [UIHint("UserID")]
        [DisplayName("Owner")]
        [PopulateUsersList]
        public int? OwnerID { get; set; }

        [Required]
        [DisplayName("Store name")]
        public string Name { get; set; }

        [DisplayName("Domain address extension")]
        public string SeoUrl { get; set; }

        [PopulateShopThemes]
        [UIHint("SimpleDropdown")]
        public string Theme { get; set; }

        [Required]
        [DisplayName("Orders delivery emails")]
        [Watermark("You can enter multiple values separated by a comma")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [UIHint("Phone")]
        public string Phone { get; set; }

        public string Address { get; set; }

        [DisplayName("Kosher")]
        public bool IsKosher { get; set; } = true;

        public HttpPostedFileBase Image { get; set; }

        public HttpPostedFileBase Logo { get; set; }

        public HttpPostedFileBase Favicon { get; set; }

        [DataType("MultilineText")]
        public string ShortDescription { get; set; }

        [AllowHtml]
        public string FullDescription { get; set; }
    }

    public class ShopDeliveryTab : IMapFrom<Shop>
    {
        public bool PickUp { get; set; }
    }
}