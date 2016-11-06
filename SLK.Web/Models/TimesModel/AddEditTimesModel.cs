using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLK.Web.Models.TimesModel
{
    public class AddEditTimesModel
    {
        public bool Enabled { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}