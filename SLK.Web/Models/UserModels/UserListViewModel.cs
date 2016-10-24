using SLK.Domain.Core;
using SLK.Web.Infrastructure.Mapping;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SLK.Web.Models.UserModels
{
    public class UserListViewModel : ListModel, IMapFrom<User>
    {
        [HiddenInput]
        public int ID { get; set; }

        [HiddenInput]
        public string IdentityID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        [Display(Name="Linked To Shop")]
        public string LinkedToShopName { get; set; }

        [Display(Name = "Registration Date")]
        public string CreationDate { get; set; }
    }
}