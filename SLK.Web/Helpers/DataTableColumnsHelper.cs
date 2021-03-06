﻿using Knoema.Localization;
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
        public static IHtmlString CreateColumnsInfo(this HtmlHelper helper, IEnumerable<ModelMetadata> properties, bool manage_section, string controllerName, bool popup)
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
                result.Append("{data: 'EditDelete', bSortable: false, render: function(data, type, row) {");
                result.Append($"return '<a class=\"btn btn-sm btn-outline grey-salsa edit-delete");
                if(popup)
                    result.Append($" edit-popup\" data-toggle=\"modal-extended\" data-url=/{controllerName}/Edit/' + row['ID'] + '>");
                else
                    result.Append($"\" href=/{controllerName}/Edit/' + row['ID'] + '>");
                result.Append("<i class=\"glyphicon glyphicon-edit\"></i> " + "Edit".Resource(typeof(DataTableColumnsHelper)) + "</a><a class=\"btn btn-sm btn-outline red-mint delete-entity\" ");
                result.Append($"href =/{controllerName}/Delete/'");
                result.Append(" + row['ID'] + '><i class=\"fa fa-close\"></i> " + "Delete".Resource(typeof(DataTableColumnsHelper)) + "</a>';}}");
            }

            result.Append("]");

            return MvcHtmlString.Create(result.ToString());
        }
    }
}