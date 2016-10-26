using SLK.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SLK.Services
{
    public class JsonResultModel<T>
    {
        public string draw;

        public int recordsTotal;

        public int recordsFiltered;

        public T[] data;
    }

    public static class PopulateService
    {
        public static JsonResultModel<T> PopulateByFilters<T>(IQueryable<T> entities, NameValueCollection filters, PropertyInfo[] properties)
        {
            int total = entities.Count();

            foreach (var prop in properties)
            {
                var filter = Convert.ToString(filters[prop.Name]);

                if (prop.PropertyType == typeof(string) && prop.Name.Contains("Date"))
                {
                    var filterFrom = Convert.ToString(filters[prop.Name + "From"]);
                    var filterTo = Convert.ToString(filters[prop.Name + "To"]);

                    if (!string.IsNullOrEmpty(filterFrom))
                    {
                        var nums = filterFrom.Split('/').Select(d => Convert.ToInt32(d)).ToArray();

                        var propName = prop.Name.Substring(6);

                        entities = entities.Where($"{propName} >= DateTime({nums[2]}, {nums[1]}, {nums[0]}, 0, 0, 0)");
                    }

                    if (!string.IsNullOrEmpty(filterTo))
                    {
                        var nums = filterTo.Split('/').Select(d => Convert.ToInt32(d)).ToArray();

                        var propName = prop.Name.Substring(6);

                        entities = entities.Where($"{propName} <= DateTime({nums[2]}, {nums[1]}, {nums[0]}, 23, 59, 59)");
                    }
                }
                else if (!string.IsNullOrEmpty(filter))
                {
                    
                    if (prop.PropertyType == typeof(string))
                    {
                        entities = entities.Where($"{prop.Name}.Contains(\"{filter}\") ");
                    }
                    else if (prop.PropertyType == typeof(bool) && filter != "any")
                    {
                        entities = entities.Where($"{prop.Name} = {filter.ToUpper()} ");
                    }
                    else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(decimal))
                    {
                        entities = entities.Where($"{prop.Name} = {filter} ");
                    }                    
                }
            }

            string ordering = "";
            int ind = 0;

            while (filters[$"order[{ind}][column]"] != null)
            {
                int sortColumnIndex = Convert.ToInt32(filters[$"order[{ind}][column]"]);
                var sortDirection = filters[$"order[{ind}][dir]"];

                if (properties[sortColumnIndex].Name.Contains("Date"))
                {
                    ordering += properties[sortColumnIndex].Name.Substring(6);
                }
                else
                {
                    ordering += properties[sortColumnIndex].Name;
                }

                // asc or desc
                ordering += " " + sortDirection.ToUpper() + ", ";

                ++ind;
            }

            if (!string.IsNullOrEmpty(ordering))
            {
                ordering = ordering.Substring(0, ordering.Length - 2);

                entities = entities.OrderBy(ordering).AsQueryable();
            }

            var filtered = entities.Count();

            entities = entities
                .Skip(Convert.ToInt32(filters["start"]))
                .Take(Convert.ToInt32(filters["length"]));
            
            return new JsonResultModel<T>
            {
                draw = filters["draw"],
                recordsTotal = total,
                recordsFiltered = filtered,
                data = entities.ToArray()
            };
        }
    }
}
