using SLK.Web.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace SLK.Web.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            result.Append(
                createSingleLink(
                    "First",
                    pageUrl(1),
                    ""
                ));

            result.Append(
                createSingleLink(
                    "Previous",                
                    pageInfo.PageNumber != 1 ? pageUrl(pageInfo.PageNumber - 1) : "",
                    pageInfo.PageNumber == 1 ? "disabled" : ""
                ));

            int first, last;
            calculateLinkRange(pageInfo, out first, out last);

            if (first > 1)
            {
                result.Append(
                createSingleLink(
                    "...",
                    "",
                    "disabled"
                ));
            }

            for (int i = first; i <= last; i++)
            {
                result.Append(
                createSingleLink(
                    i.ToString(),
                    pageUrl(i),
                    pageInfo.PageNumber == i ? "active" : ""
                ));                
            }

            if (last < pageInfo.TotalPages)
            {
                result.Append(
                createSingleLink(
                    "...",
                    "",
                    "disabled"
                ));
            }

            result.Append(
                createSingleLink(
                    "Next",
                    pageInfo.PageNumber != pageInfo.TotalPages ? pageUrl(pageInfo.PageNumber + 1) : "",
                    pageInfo.PageNumber == pageInfo.TotalPages ? "disabled" : ""
                ));

            result.Append(
                createSingleLink(
                    "Last",
                    pageUrl(pageInfo.TotalPages),
                    ""
                ));

            return MvcHtmlString.Create(result.ToString());           
        }

        private static string createSingleLink(string inner, string link, string tagclass)
        {
            StringBuilder result = new StringBuilder();

            TagBuilder li_tag = new TagBuilder("li");
            TagBuilder a_tag = new TagBuilder("a");

            a_tag.InnerHtml = inner;     
            if (link != "") a_tag.MergeAttribute("href", link);

            if (tagclass != "") li_tag.AddCssClass(tagclass);
            li_tag.AddCssClass("paginate_button previous");
            li_tag.Attributes.Add("aria-controls", "dataTables-example");
            li_tag.Attributes.Add("tabindex", "0");
            li_tag.InnerHtml = a_tag.ToString();

            return li_tag.ToString();
        }

        private static void calculateLinkRange(PageInfo pageInfo, out int first, out int last)
        {
            first = pageInfo.PageNumber - 3;
            last = pageInfo.PageNumber + 3;

            if (first < 1)
            {
                last += 1 - first;
                first = 1;
            }

            if (last > pageInfo.TotalPages)
            {
                first -= last - pageInfo.TotalPages;
                last = pageInfo.TotalPages;
            }

            if (first < 1)
            {
                first = 1;
            }
        }
    }
}