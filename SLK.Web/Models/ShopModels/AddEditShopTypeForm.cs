using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SLK.Web.Models.ShopModels
{
    public class AddEditShopTypeForm : AddEditForm
    {
        public AddEditShopTypeForm() : base(null) { }

        public AddEditShopTypeForm(string addOrEditUrl) : base(addOrEditUrl) { }

        public AddEditShopTypeForm(string addOrEditUrl, int id) : base(addOrEditUrl)
        {
            ID = id;
        }

        [HiddenInput]
        public int? ID { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType("Integer")]
        public int DisplayOrder { get; set; }

    }
}