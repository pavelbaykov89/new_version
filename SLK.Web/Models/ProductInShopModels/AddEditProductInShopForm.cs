using SLK.Domain.Core;
using SLK.Web.Filters;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SLK.Web.Models.ProductInShopModels
{
    public class AddEditProductInShopForm : AddEditForm, IMapFrom<ProductInShop>
    {
        [HiddenInput]
        public int ID { get; protected set; }

        [Required, Display(Name = "Product")]
        [PopulateProductsList]
        public int ProductID { get; set; }

        [Required, Display(Name = "Shop")]
        [PopulateShopsList]
        public int ShopID { get; set; }
        
        public decimal Price { get; set; }

        public decimal Quantity { get; set; }

        [HiddenInput]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Creation Date")]
        public string StringCreationDate { get; set; }

        public decimal PricebyUnit { get; set; }

        public decimal MaxCartQuantity { get; set; }

        public int QuantityType { get; set; }
    }
}