using SLK.Web.Models;

namespace SLK.Web.Infrastructure
{
    public interface ICurrentUser
    {
        ApplicationUser User { get; }
    }
}
