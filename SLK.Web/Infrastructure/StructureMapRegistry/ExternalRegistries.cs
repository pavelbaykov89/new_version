using StructureMap;

namespace SLK.Web.Infrastructure.StructureMapRegistry
{
    public class ExternalRegistries : Registry
    {
        public ExternalRegistries()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory();
                scan.LookForRegistries();
            });
        }
    }
}