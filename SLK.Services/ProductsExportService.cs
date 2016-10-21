using OfficeOpenXml;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services.Task;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SLK.Services
{
    public static class ProductsExportService
    {
        public static byte[] ExportProductsToExcelFile(PropertyInfo[] properties, IQueryable<dynamic> products)
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");
            
            worksheet.Row(1).Style.Font.Bold = true;
                        
            int row = 1, num = 1;

            string col = "A";
            var sb = new StringBuilder(col);

            foreach (var prop in properties)
            {                
                worksheet.Cells[$"{col}{row}"].Value = prop.Name;               

                col = sb.Append(sb[sb.Length - 1]++, 1).Remove(sb.Length - 1, 1).ToString();
                
                if (sb.ToString().EndsWith("["))
                {
                    sb = new StringBuilder("AA");
                    col = sb.ToString();
                }
            }
            worksheet.Cells[$"{col}{row}"].Value = "Flag";
            
            row = 2;

            foreach (var product in products)
            {
                col = "A";
                sb = new StringBuilder(col);

                foreach (var prop in properties)
                {
                    if (prop.PropertyType == typeof(bool))
                    {
                        worksheet.Cells[$"{col}{row}"].Value = prop.GetValue(product) ? "true" : "false";
                    }
                    else
                    {
                        worksheet.Cells[$"{col}{row}"].Value = prop.GetValue(product);
                    }

                    col = sb.Append(sb[sb.Length - 1]++, 1).Remove(sb.Length - 1, 1).ToString();

                    if (sb.ToString().EndsWith("["))
                    {
                        sb = new StringBuilder("AA");
                        col = sb.ToString();
                    }
                }
                worksheet.Cells[$"{col}{row}"].Value = "1";

                ++row;
            }

            return package.GetAsByteArray();
        }
    }
}
