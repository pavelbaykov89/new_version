using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models.ManufacturerModels
{
    public class AddEditManufacturerForm : AddEditForm, IMapFrom<Manufacturer>
    {
        [HiddenInput]
        public int ID { get; protected set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [DataType("Integer")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Published")]
        public bool Published { get; set; }
    }
}