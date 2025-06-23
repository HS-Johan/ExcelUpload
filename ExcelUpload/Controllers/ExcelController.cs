using ExcelUpload.Models;
using ExcelUpload.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace ExcelUpload.Controllers
{
    public class ExcelController : Controller
    {
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

            foreach( var data in dataList )
            {
                
            }

            return View("List", dataList);
        }

    }
}
