using ExcelUpload.Models;
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

            var dataList = new List<ExcelData>();

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
                        dataList.Add(new ExcelData
                        {
                            Column1 = worksheet.Cells[row, 1].Text,
                            Column2 = worksheet.Cells[row, 2].Text,
                            Column3 = worksheet.Cells[row, 3].Text
                        });
                    }
                }
            }

            return View("List", dataList);
        }

    }
}
