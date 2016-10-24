using System.Web.Mvc;

namespace SLK.Web.Models
{
    public abstract class AddEditForm
    {
        [HiddenInput]
        public string AddOrEditUrl { get; set; }

        public AddEditForm(string addOrEditUrl)
        {
            AddOrEditUrl = addOrEditUrl;
        }
    }
}