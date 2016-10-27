using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SLK.Web.Models.ShopTypeModels
{
    public class AddEditShopTypeForm : AddEditForm, IMapFrom<ShopType>
    {
        [HiddenInput]
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType("Integer")]
        public int DisplayOrder { get; set; }

    }
}