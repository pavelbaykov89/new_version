using SLK.Web.Models;
using System.Collections.Generic;

namespace SLK.Web.ProductModels
{
    public class PaginationProductList
    {
        public IEnumerable<ProductsListViewModel> Products { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}