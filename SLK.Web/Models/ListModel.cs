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
        public object AddNewForm { get; set; }

        //[HiddenInput]
        //public string EditUrl { get; set; }

        //[HiddenInput]
        //public string DeleteUrl { get; set; }

        //[HiddenInput]
        //public string ControllerName { get; set; }

        [HiddenInput]
        public bool Editable { get; set; } = true;

        [HiddenInput]
        public bool Popup { get; set; }
    }
}