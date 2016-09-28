using System;
using System.Collections.Generic;

namespace SLK.Web.Infrastructure.ModelMetadata.Filters
{
    public class CategoryDropDownByNameFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                string.IsNullOrEmpty(metadata.DataTypeName) &&
                metadata.PropertyName.ToLower().Contains("category"))
            {
                metadata.DataTypeName = "CategoryID";
            }
        }
    }
}