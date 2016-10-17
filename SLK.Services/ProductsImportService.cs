using OfficeOpenXml;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services.Task;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLK.Services
{
    public static class ProductsImportService
    {
        public static long GetRowsCountToImport(string filename)
        {
            long count = 0;

            using (var package = new ExcelPackage(new FileInfo(filename)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    int step = worksheet.Cells.Rows / 2, cur = worksheet.Cells.Rows / 2;

                    do
                    {
                        step /= 2;

                        if (worksheet.Cells[$"A{cur}"].Value == null || worksheet.Cells[$"A{cur}"].Value.ToString().Length == 0)
                        {
                            cur -= step;
                        }
                        else
                        {
                            cur += step;
                        }
                    }
                    while (step > 10);

                    count += cur;
                }
            }

            return count;
        }

        public static void ImportProductsFromExcelFile(string filename, ApplicationDbContext context, TaskDescription task)
        {
            long total = GetRowsCountToImport(filename);
                       
            int count = 0;

            var products = new List<Product>();
            products.Capacity = 1000;

            var categories = context.Categories.ToList();

            var manufacturers = context.Manufacturers.ToList();

            using (var package = new ExcelPackage(new FileInfo(filename)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {                        
                    int row = 3;
                    while (worksheet.Cells[$"A{row}"].Value != null && worksheet.Cells[$"A{row}"].Value.ToString().Length != 0)
                    {
                        string categoryName = worksheet.Cells[$"C{row}"].Value?.ToString();
                        string manufacturerName = worksheet.Cells[$"H{row}"].Value?.ToString();

                        var category = categories.FirstOrDefault(c => c.Name == categoryName);

                        var manufacturer = manufacturers.FirstOrDefault(m => m.Name == manufacturerName);

                        if (category == null)
                        {
                            category = new Category(categoryName);
                            context.Categories.Add(category);
                            categories.Add(category);
                        }

                        if (manufacturer == null)
                        {
                            manufacturer = new Manufacturer(manufacturerName);
                            context.Manufacturers.Add(manufacturer);
                            manufacturers.Add(manufacturer);
                        }

                        var product = new Product(
                            worksheet.Cells[$"B{row}"].Value?.ToString(),
                            category,
                            manufacturer,
                            worksheet.Cells[$"N{row}"].Value?.ToString(),
                            worksheet.Cells[$"O{row}"].Value?.ToString(),
                            worksheet.Cells[$"A{row}"].Value?.ToString(),
                            worksheet.Cells[$"S{row}"].Value?.ToString());

                        products.Add(product);

                        ++row;
                        if (row%1000 == 0)
                        {
                            context.Products.AddRange(products);
                            context.SaveChanges();
                            products.Clear();

                            task.Progress = row * 100 / total;
                        }
                    }

                    count += row - 3;
                }
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            task.Progress = 100;

            return;
        }
    }
}
