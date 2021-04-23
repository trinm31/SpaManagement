using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using SpaManagement.DataAccess.Repository.IRepository;

namespace SpaManagement.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    public class ExportExcel : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExportExcel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> ExcelProductList()
        {
            var productList = await _unitOfWork.Product.GetAllAsync(includeProperties: "Supplier,TypeOfProduct,Manufacturer");
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "ImportPrice";
                worksheet.Cell(currentRow, 5).Value = "Price";
                worksheet.Cell(currentRow, 6).Value = "Type Of Product";
                worksheet.Cell(currentRow, 7).Value = "Supplier";
                worksheet.Cell(currentRow, 8).Value = "Manufacturer";
                foreach (var product in productList)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = product.Id;
                    worksheet.Cell(currentRow, 2).Value = product.Name;
                    worksheet.Cell(currentRow, 3).Value = product.Description;
                    worksheet.Cell(currentRow, 4).Value = product.ImportPrice;
                    worksheet.Cell(currentRow, 5).Value = product.Price;
                    worksheet.Cell(currentRow, 6).Value = product.TypeOfProduct.Name;
                    worksheet.Cell(currentRow, 7).Value = product.Supplier.Name;
                    worksheet.Cell(currentRow, 8).Value = product.Manufacturer.Name;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }
        }
    }
}