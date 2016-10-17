using System;

namespace SLK.Web.Models
{
    public class PageInfo
    {
        public int PageNumber { get; set; } // current page number
        public int PageSize { get; set; } // items on page count
        public int TotalItems { get; set; } // total items count
        public int TotalPages  // total pages count
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}