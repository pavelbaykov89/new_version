using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models.ProductInShopModels
{
    public class ProductInShopListViewModel : ListModel, IMapFrom<ProductInShop>
    {
        [HiddenInput]
        public int ID { get; protected set; }

        public string ProductName { get; set; }
        
        public string ShopName { get; set; }

        public decimal Price { get; set; }

        public decimal Quantity { get; set; }
               
        [HiddenInput]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Creation Date")]
        public string StringCreationDate { get; set; }        
    }
}