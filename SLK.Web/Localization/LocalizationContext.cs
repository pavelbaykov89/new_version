using System.Data.Entity;

namespace SLK.Web.Localization
{
    public class LocalizationContext : DbContext
    {
        public LocalizationContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<LocalizedObject> Objects { get; set; }
    }
}