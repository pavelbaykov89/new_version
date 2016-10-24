using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SLK.Web.Helpers
{
    public static class DataTableColumnsHelper
    {
        public static IHtmlString CreateColumnsInfo(this HtmlHelper helper, IEnumerable<ModelMetadata> properties, bool manage_section)
        {
            StringBuilder result = new StringBuilder();

            result.Append("[");

            foreach (var prop in properties.Where(p => p.ShowForEdit))
            {
                if (prop.TemplateHint != "HiddenInput")
                {
                    result.Append("{ data: '");
                    result.Append(prop.PropertyName);
                    result.Append("', bSortable: ");                  
                    if (prop.ModelType == typeof(bool))
                    {                        
                        result.Append("false,render: function(data, type, row) { return '<label class=\"mt-checkbox mt-checkbox-single mt-checkbox-outline\"><input type = \"checkbox\" class=\"checkboxes\" '+(data?'checked':'')+' value='+data+'><span></span></label>';}");
                    }
                    else
                    {
                        result.Append("true");
                    }
                    result.Append("},");
                }
            }

            if (manage_section)
            {
                result.Append( "{data: 'EditDelete', bSortable: false, render: function(data, type, row) {");
                result.Append("return '<a class=\"btn btn-sm btn-outline grey-salsa\" href=/Product/Edit/' + row['ID'] + '>");
                result.Append("<i class=\"glyphicon glyphicon-edit\"></i> Edit</a><a class=\"btn btn-sm btn-outline red-mint\" ");
                result.Append("href =/Product/Delete/' + row['ID'] + '><i class=\"fa fa-close\"></i> Delete</a>';}}");
            }

            result.Append("]");

            return MvcHtmlString.Create(result.ToString());
        }

        public static IHtmlString CreateColumnsInfo(this HtmlHelper helper, IEnumerable<ModelMetadata> properties, string editUrl, string deleteUrl)
        {
            StringBuilder result = new StringBuilder();

            result.Append("[");

            foreach (var prop in properties.Where(p => p.ShowForEdit))
            {
                if (prop.TemplateHint != "HiddenInput")
                {
                    result.Append("{ data: '");
                    result.Append(prop.PropertyName);
                    result.Append("', bSortable: ");
                    if (prop.ModelType == typeof(bool))
                    {
                        result.Append("false,render: function(data, type, row) { return '<label class=\"mt-checkbox mt-checkbox-single mt-checkbox-outline\"><input type = \"checkbox\" class=\"checkboxes\" '+(data?'checked':'')+' value='+data+'><span></span></label>';}");
                    }
                    else
                    {
                        result.Append("true");
                    }
                    result.Append("},");
                }
            }

            if (editUrl != null && deleteUrl != null)
            {
                result.Append("{data: 'EditDelete', bSortable: false, render: function(data, type, row) {");
                if (editUrl != null)
                    result.Append("return '<a class=\"btn btn-sm btn-outline grey-salsa edit-popup\" data-toggle=\"modal-extended\" data-url=" + editUrl + "/' + row['ID'] + '><i class=\"glyphicon glyphicon-edit\"></i> Edit</a>");
                if (deleteUrl != null)
                    result.Append("<a class=\"btn btn-sm btn-outline red-mint delete-entity\" href =" + deleteUrl + "/' + row['ID'] + '><i class=\"fa fa-close\"></i> Delete</a>';}}");
            }

            result.Append("]");

            return MvcHtmlString.Create(result.ToString());
        }
    }
}