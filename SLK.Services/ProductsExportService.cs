using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SLK.Services
{
    public static class ProductsExportService
    {
        private static Dictionary<string, string> propOrder = new Dictionary<string, string>
        {
            { "SKU", "A" },
            { "Name", "B" },
            { "CategoryName", "C" },
            { "ShortDescription", "D" },
            { "FullDescription", "E" },
            { "SoldByWeight", "F" },
            { "Capacity", "G" },
            { "MeasureUnit", "H" },
            { "ProductMeasureDisplayName", "I" },
            { "ContentUnitPriceMultiplicator", "J" },
            { "UnitsPerPackage", "K" },
            { "ManufacturerName", "L" },
            { "Brand", "M" },
            { "MadeCountry", "N" },
            { "ProductShopOptions", "O" },
            { "Components", "P" },
            { "DisplayOrder", "Q" },
            { "IsKosher", "R" },
            { "KosherType", "S" },
            { "NoTax", "T" },
            { "SeoDescription", "U" },
            { "SeoKeywords", "V" },
            // Flag on 'W' column
            { "Image", "X" }
        };

        private static Dictionary<int, int> propWidth = new Dictionary<int, int>
        {
            { 1, 15 },
            { 2, 15 },
            { 3, 13 },
            { 4, 16 },
            { 5, 18 },
            { 6, 12 },
            { 7, 10 },
            { 8, 11 },
            { 9, 24 },
            { 10, 24 },
            { 11, 14 },
            { 12, 17 },
            { 13, 12 },
            { 14, 12 },
            { 15, 18 },
            { 16, 12 },
            { 17, 12 },
            { 18, 10 },
            { 19, 11 },
            { 20, 10 },
            { 21, 15 },
            { 22, 15 },
            // Flag on 'W' column
            { 24, 50 }
        };

        public static byte[] ExportProductsToExcelFile(PropertyInfo[] properties, IQueryable<dynamic> products, 
                    string pictureFilter, string categoryFilter, bool anyShop, Dictionary<string,bool> shopFilter)
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");
            
            worksheet.Row(1).Style.Font.Bold = true;

            foreach (var prop in properties)
            {
                worksheet.Cells[$"{propOrder[prop.Name]}1"].Value = prop.Name;               
            }
            worksheet.Cells[$"W1"].Value = "Flag";

            int row = 2;
            
            var imageProp = properties.First(p => p.Name == "Image");
            var categoryProp = properties.First(p => p.Name == "CategoryName");

            foreach (var product in products)
            {
                if (pictureFilter == "With Picture" && string.IsNullOrEmpty(imageProp.GetValue(product)) ||
                    pictureFilter == "Without Picture" && !string.IsNullOrEmpty(imageProp.GetValue(product)) ||
                    categoryFilter == "With Category" && string.IsNullOrEmpty(categoryProp.GetValue(product)) ||
                    categoryFilter == "Without Category" && !string.IsNullOrEmpty(categoryProp.GetValue(product)))
                {
                    continue;
                }

                foreach (var prop in properties)
                {  
                    if (prop.PropertyType == typeof(bool))
                    {
                        worksheet.Cells[$"{propOrder[prop.Name]}{row}"].Value = prop.GetValue(product) ? "true" : "false";
                    }
                    else
                    {
                        worksheet.Cells[$"{propOrder[prop.Name]}{row}"].Value = prop.GetValue(product);
                    }
                }
                worksheet.Cells[$"W{row}"].Value = "1";

                ++row;
            }

            foreach (var width in propWidth)
            {
                worksheet.Column(width.Key).Width = width.Value;
            }

            worksheet.Column(23).Width = 7; // Flag column

            return package.GetAsByteArray();
        }
    }
}
