using System.Web.Mvc;

namespace SLK.Web.Models
{
    public class AddEditForm
    {
        [HiddenInput]
        public string AddOrEditUrl { get; set; }

        //public AddEditForm(string addOrEditUrl)
        //{
        //    AddOrEditUrl = addOrEditUrl;
        //}
    }
}