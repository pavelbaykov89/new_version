using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLK.Web.Infrastructure.ModelMetadata.Filters
{
    public class ManufacturerDropDownByNameFilter : IModelMetadataFilter
    {
        public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            if (!string.IsNullOrEmpty(metadata.PropertyName) &&
                string.IsNullOrEmpty(metadata.DataTypeName) &&
                metadata.PropertyName.ToLower().Contains("manufacturer"))
            {
                metadata.DataTypeName = "ManufacturerID";
            }
        }
    }
}