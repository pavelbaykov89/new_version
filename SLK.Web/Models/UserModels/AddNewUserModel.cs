using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System.Web.Mvc;

namespace SLK.Web.Models.UserModels
{
    public class AddNewUserModel : AddEditForm, IMapFrom<User>
    {
        [HiddenInput]
        public int ID { get; set; }

        [HiddenInput]
        public string IdentityID { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool FollowSLKNews { get; set; }
    }
}