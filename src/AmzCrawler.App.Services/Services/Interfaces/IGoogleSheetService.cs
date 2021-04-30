using AmzCrawler.App.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmzCrawler.App.Services.Services.Interfaces
{
    public interface IGoogleSheetService
    {
        List<ProductModel> GetDefaultProducts();
        List<ProductModel> GetProducts(string spreadsheetId, string sheetName);
        List<ProductModel> GetProducts(string spreadsheetId, int? sheetId);
        Task<int> UpdateProducts(IList<ProductModel> products, string spreadsheetId, int? sheetId, params string[] columns);
        Task<int> UpdateDefaultProducts(IList<ProductModel> products, params string[] columns);
    }
}
