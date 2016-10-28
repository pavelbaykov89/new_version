using SLK.Services.FileStorage;
using StructureMap;

namespace SLK.Services
{
    public class ServicesRegistry : Registry
    {
        public ServicesRegistry()
        {
            For<IFilesRepository>().Use<FilesRepository>();           
        }
    }
}
