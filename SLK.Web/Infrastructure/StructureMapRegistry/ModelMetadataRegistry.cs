using SLK.Web.Infrastructure.ModelMetadata;
using StructureMap;
using System.Web.Mvc;

namespace SLK.Web.Infrastructure.StructureMapRegistry
{
    public class ModelMetadataRegistry : Registry
    {
        public ModelMetadataRegistry()
        {
            For<ModelMetadataProvider>().Use<ExtensibleModelMetadataProvider>();

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AddAllTypesOf<IModelMetadataFilter>();
            });
        }
    }
}