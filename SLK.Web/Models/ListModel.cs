using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Models
{
    public abstract class ListModel
    {
        [HiddenInput]
        public AddEditForm AddNewForm { get; set; }

        [HiddenInput]
        public string EditUrl { get; set; }

        [HiddenInput]
        public string DeleteUrl { get; set; }

        //[HiddenInput]
        //public string ControllerName { get; set; }

        //[HiddenInput]
        //public bool Editable { get; set;  }
    }
}