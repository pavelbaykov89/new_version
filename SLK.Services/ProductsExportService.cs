using OfficeOpenXml;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services.Task;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SLK.Services
{
    public static class ProductsExportService
    {
        public static void ExportProductsToExcelFile(PropertyInfo[] properties, IQueryable<dynamic> products, TaskDescription task, int total, string path)
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");
            
            worksheet.Row(1).Style.Font.Bold = true;

            string col = "A";
            int row = 1;
            
            foreach (var prop in properties)
            {                
                worksheet.Cells[$"{col}{row}"].Value = prop.Name;

                col = (++col.ToArray()[col.Length - 1]).ToString();

                if (col.EndsWith("["))
                {
                    col = "AA";
                }
            }
           
            task.Start();

            col = "A";
            row = 2;

            foreach (var product in products)
            {
                col = "A";

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

                    col = (++col.ToArray()[col.Length - 1]).ToString();

                    if (col.EndsWith("["))
                    {
                        col = "AA";
                    }
                }

                ++row;

                if (row % 100 == 0)
                {
                    task.Progress = row * 100 / total;
                }
            }

            task.Progress = 100;

            package.SaveAs(new FileInfo(path));
        }
    }
}
