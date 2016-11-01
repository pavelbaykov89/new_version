using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models.CategoryModels
{
    public class AddEditCategoryForm : AddEditForm, IMapFrom<Category>
    {
        [HiddenInput]
        public int ID { get; protected set; }

        public string Name { get; set; }

        [Required, Display(Name = "Parent Category")]
        public string CategoryID { get; set; }

        public string ImagePath { get; set; }

        public int DisplayOrder { get; set; }

        public bool Published { get; set; }
    }
}