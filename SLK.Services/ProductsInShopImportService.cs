using OfficeOpenXml;
using SLK.DataLayer;
using SLK.Domain.Core;
using SLK.Services.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SLK.Services
{
    public static class ProductsInShopImportService
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

            var productsInShop = new List<ProductInShop>();
            productsInShop.Capacity = 1000;

            var products = context.Products;

            var properties = typeof(ProductInShop).GetProperties();

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
                        else if ("SKU" == worksheet.Cells[$"{col}1"].Value.ToString())
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
                        string SKU = "";
                        var productInShop = new ProductInShop();

                        foreach (var pair in propToCol)
                        {
                            var prop = pair.Value;
                            col = pair.Key;

                            var value = worksheet.Cells[$"{col}{row}"].Value?.ToString();

                            if (prop == null)
                            {
                                SKU = value;
                                continue;
                            }

                            if (prop.PropertyType == typeof(Boolean))
                            {
                                prop.SetValue(productInShop, value != "0");
                            }
                            else if (value != "")
                            {
                                prop.SetValue(productInShop, Convert.ChangeType(value, prop.PropertyType));
                            }
                           
                        }

                        var product = products.Where(p => p.SKU.EndsWith(SKU));

                        if (product != null && product.Count() == 1 && SKU.Length > 4)
                        {
                            productInShop.ProductID = product.First().ID;
                            productsInShop.Add(productInShop);
                        }                        

                        ++row;
                        if (row % 100 == 0)
                        {
                            context.Products.AddRange(products);
                            context.SaveChanges();                            

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
