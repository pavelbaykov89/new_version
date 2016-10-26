using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models.ShopModels
{
    public class AddEditShopForm : IMapFrom<Shop>
    {
        [HiddenInput]
        public int? ID { get; set; }

        [Required]
        [DisplayName("Store name")]
        public string Name { get; set; }
    
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        [DisplayName("Store importance")]
        public int DisplayOrder { get; set; }

        public bool HasImage { get; set; }

        public string ImagePath { get; set; }

        public string LogoPath { get; set; }

        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [DisplayName("Cellular")]
        public string Phone2 { get; set; }

        [Required]
        [DisplayName("Orders delivery email")]
        public string Email { get; set; }

        [DisplayName("Kosher")]
        public bool IsKosher { get; set; }

        [DisplayName("ShipEnabled")]
        public bool IsShipEnabled { get; set; }

        public bool Active { get; set; }

        [DisplayName("Domain address extension")]
        public string SeoUrl { get; set; }
    }
}