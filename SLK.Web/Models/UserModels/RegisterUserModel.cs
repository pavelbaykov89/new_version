﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SLK.Web.Models.UserModels
{
    public class RegisterUserModel
    {
        [Required]       
        public string UserName { get; set; }
                
        [Required]
        [EmailAddress]        
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]       
        public string Password { get; set; }

        [DataType(DataType.Password)]        
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool FollowSLKNews { get; set; }
    }
}