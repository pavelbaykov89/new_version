using System.Web.Mvc;

namespace SLK.Web.Models.UserModel
{
    public class NewUserModel
    {
        [HiddenInput]
        public int ID { get; protected set; }

        [HiddenInput]
        public string IdentityID { get; protected set; }

        public string Email { get; protected set; }

        public string Login { get; protected set; }

        public string Password { get; protected set; }        

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool FollowSLKNews { get; set; }
    }
}