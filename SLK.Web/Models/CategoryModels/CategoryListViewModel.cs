using SLK.Web.Infrastructure.Mapping;
using SLK.Domain.Core;
using System.Web.Mvc;

namespace SLK.Web.Models.CategoryModels
{
    public class CategoryListViewModel : ListModel, IMapFrom<Category>
    {
        [HiddenInput]
        public int ID { get; protected set; }

        public string Name { get; protected set; }

        public string ParentCategoryName { get; protected set; }
       
        public string ImagePath { get; protected set; }

        public int DisplayOrder { get; protected set; }

        public bool Published { get; protected set; }
    }
}