using System.Collections.Generic;

namespace SLK.Web.Models
{
    public class PaginationProductList
    {
        public IEnumerable<ProductsListViewModel> Products { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}