using ExcelUpload.Data;
using ExcelUpload.Models;
using ExcelUpload.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace ExcelUpload.Controllers
{
    public class ExcelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExcelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile excelFile)
        {

            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.Message = "No file uploaded!";
                return View();
            }

            var dataList = new List<ProductVM>();

            using (var stream = new MemoryStream())
            {
                excelFile.CopyTo(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var worksheet = package.Workbook.Worksheets[0];
                    if (worksheet?.Dimension == null)
                    {
                        ViewBag.Message = "The worksheet is empty or invalid.";
                        return View();
                    }

                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        dataList.Add(new ProductVM
                        {
                            ProductName = worksheet.Cells[row, 1].Text,
                            ProductDescription = worksheet.Cells[row, 2].Text,
                            Sku = worksheet.Cells[row, 3].Text,
                            ProductCategory = worksheet.Cells[row, 4].Text,
                            SupplierId = worksheet.Cells[row, 5].Text,
                            Stock = int.TryParse(worksheet.Cells[row, 6].Text, out var stock) ? stock : 1,
                            IsPublished = worksheet.Cells[row, 7].Text == "1", // Convert "1" to true, "0" to false
                            ActiveStartDate = DateTime.TryParse(worksheet.Cells[row, 8].Text, out var activeStartDate) ? activeStartDate : DateTime.Now,
                            SellPrice = double.TryParse(worksheet.Cells[row, 9].Text, out var sellPrice) ? sellPrice : 0,
                            BuyPrice = double.TryParse(worksheet.Cells[row, 10].Text, out var buyPrice) ? buyPrice : 0,
                            EmployeeCost = double.TryParse(worksheet.Cells[row, 11].Text, out var employeeCost) ? employeeCost : 0,
                            OtherCost = double.TryParse(worksheet.Cells[row, 12].Text, out var otherCost) ? otherCost : 0,
                        });
                    }
                }
            }

            var productCount = 0;

            foreach( var data in dataList )
            {
                if( data.ProductName != string.Empty && data.Sku != string.Empty && data.SellPrice >= 0 )
                {
                    var productExist = _context.Product.Where(x => x.Sku == data.Sku).FirstOrDefault();

                    if( data.SupplierId == string.Empty )
                    {
                        data.SupplierId = "All";
                    }

                    if( productExist !=null )
                    {
                        
                        productExist.ProductName = data.ProductName;
                        productExist.SellPrice = data.SellPrice;

                        _context.Product.Update(productExist);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Product product = new Product
                        {
                            ProductName = data.ProductName,
                            //ProductDescription = data.ProductDescription,
                            Sku = data.Sku,
                            ProductCategory = data.ProductCategory,
                            SupplierGuidId = _context.Supplier
                                          .Where(x => x.SupplierName.Replace(" ", "").ToUpper() == data.SupplierId.Replace(" ", "").ToUpper())
                                          .Select(x => x.SupplierGuidId)
                                          .FirstOrDefault(),
                            Stock = data.Stock,
                            IsPublished = data.IsPublished,
                            ActiveStartDate = data.ActiveStartDate,
                            SellPrice = data.SellPrice,
                            BuyPrice = data.BuyPrice,
                            EmployeeCost = data.EmployeeCost,
                            OtherCost = data.OtherCost,

                            CreatedBy = Guid.Parse("B3DFF00D-1215-4AB1-935A-D73D81BF7E17"),
                            UpdatedBy = Guid.Parse("B3DFF00D-1215-4AB1-935A-D73D81BF7E17"),

                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        product.ProductGuidId = Guid.NewGuid();

                        _context.Product.Add(product);
                        _context.SaveChanges();
                    }

                    productCount++;
                }
            }

            var c = productCount;

            return View("List", dataList);
        }

    }
}
