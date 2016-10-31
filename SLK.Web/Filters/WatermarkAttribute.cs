using System;
using System.Web.Mvc;

namespace SLK.Web.Filters
{
    public class WatermarkAttribute : Attribute, IMetadataAware
    {
        private readonly string _placeholder;
        public WatermarkAttribute(string placeholder)
        {
            _placeholder = placeholder;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["watermark"] = _placeholder;
        }
    }
}