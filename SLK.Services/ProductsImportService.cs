using OfficeOpenXml;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services.Task;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

            var productMeasures = context.Measuries.ToList();

            var properties = typeof(Product).GetProperties();

            var propToCol = new Dictionary<string, PropertyInfo>();

            task.Start();

            using (var package = new ExcelPackage(new FileInfo(filename)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    string col = "A";
                    var sb = new StringBuilder(col);

                    while (worksheet.Cells[$"{col}1"].Value != null && worksheet.Cells[$"{col}1"].Value.ToString() != "")                        
                    {
                        var prop = properties.FirstOrDefault(p => p.Name == worksheet.Cells[$"{col}1"].Value.ToString());

                        if (prop != null)
                        {
                            propToCol[col] = prop;
                        }
                        else if ("Flag" == worksheet.Cells[$"{col}1"].Value.ToString())
                        {   
                            propToCol[col] = null;
                        }

                        col = sb.Append(sb[sb.Length - 1]++, 1).Remove(sb.Length - 1, 1).ToString();

                        if (sb.ToString().EndsWith("["))
                        {
                            sb = new StringBuilder("AA");
                            col = sb.ToString();
                        }
                    }

                    int row = 2;
                    while (worksheet.Cells[$"A{row}"].Value != null && worksheet.Cells[$"A{row}"].Value.ToString().Length != 0)
                    {
                        int ImportFlag = 3;

                        var product = new Product();

                        foreach(var pair in propToCol)
                        {
                            var prop = pair.Value;
                            col = pair.Key;

                            var value = worksheet.Cells[$"{col}{row}"].Value?.ToString();

                            if (prop == null)
                            {
                                ImportFlag = Convert.ToInt32(value);
                                continue;
                            }

                            if (prop.Name == "Category")
                            {
                                var category = categories.FirstOrDefault(c => c.Name == value);

                                if (category == null)
                                {
                                    category = new Category(value);
                                    context.Categories.Add(category);
                                    categories.Add(category);
                                }

                                product.Category = category;
                            }
                            else if (pair.Value.Name == "Manufacturer")
                            {
                                var manufacturer = manufacturers.FirstOrDefault(m => m.Name == value);

                                if (manufacturer == null)
                                {
                                    manufacturer = new Manufacturer(value);
                                    context.Manufacturers.Add(manufacturer);
                                    manufacturers.Add(manufacturer);
                                }

                                product.Manufacturer = manufacturer;
                            }
                            else if (pair.Value.Name == "ProductMeasure")
                            {
                                var productMeasure = productMeasures.FirstOrDefault(m => m.Name == value);

                                if (productMeasure == null)
                                {
                                    productMeasure = new ContentUnitMeasure(value);
                                    context.Measuries.Add(productMeasure);
                                    productMeasures.Add(productMeasure);
                                }

                                product.ProductMeasure = productMeasure;
                            }
                            else
                            {
                                if (prop.PropertyType == typeof(Boolean))
                                {
                                    prop.SetValue(product, value != "0");
                                }
                                else if (value != "")
                                {                                    
                                    prop.SetValue(product, Convert.ChangeType(value, prop.PropertyType));
                                }
                            }
                        }

                        var existing = context.Products.Where(p => p.SKU == product.SKU).ToArray();

                        if (existing.Count() == 0 && ImportFlag != 0)
                        {   
                            products.Add(product);
                        }
                        else
                        {
                            switch(ImportFlag)
                            {
                                case 0:
                                    foreach(var p in existing)
                                    {
                                        context.Products.Remove(p);
                                    }
                                    break;
                                case 1:
                                    foreach (var p in existing)
                                    {
                                        foreach (var pair in propToCol)
                                        {
                                            var prop = pair.Value;

                                            if (prop != null && prop.PropertyType == typeof(string) && !string.IsNullOrEmpty(prop.GetValue(p)?.ToString()))
                                            {
                                                prop.SetValue(p, prop.GetValue(product));
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    foreach (var p in existing)
                                    {
                                        foreach (var pair in propToCol)
                                        {
                                            var prop = pair.Value;

                                            if (prop != null && !string.IsNullOrEmpty(worksheet.Cells[$"{pair.Key}{row}"].Value?.ToString()))
                                            {
                                                prop.SetValue(p, prop.GetValue(product));
                                            }
                                        }
                                    }
                                    break;
                                case 3:
                                    foreach (var p in existing)
                                    {
                                        foreach (var pair in propToCol)
                                        {
                                            var prop = pair.Value;

                                            if (prop!=null)
                                            {
                                                prop.SetValue(p, prop.GetValue(product));
                                            }
                                        }   
                                    }
                                    break;
                            }
                        }

                        ++row;
                        if (row % 100 == 0)
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
