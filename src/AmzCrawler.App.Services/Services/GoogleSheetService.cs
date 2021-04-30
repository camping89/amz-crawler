using AmzCrawler.App.Services.Configurations;
using AmzCrawler.App.Services.Helpers;
using AmzCrawler.App.Services.Models;
using AmzCrawler.App.Services.Services.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AmzCrawler.App.Services.Services
{
    public class GoogleSheetService : IGoogleSheetService
    {
        //Google Drive scopes Documentation:   https://developers.google.com/drive/web/scopes
        private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly GoogleConfiguration _configuration;
        private readonly SheetsService _sheetsService;

        public GoogleSheetService(IOptions<GoogleConfiguration> options)
        {
            _configuration = options.Value;
            _sheetsService = BuildSheetService();
        }

        public List<ProductModel> GetDefaultProducts()
        {
            return GetProducts(_configuration.SpreadSheetId, _configuration.SheetName);
        }

        public List<ProductModel> GetProducts(string spreadsheetId, int? sheetId)
        {
            var sheetName = GetSheetName(spreadsheetId, sheetId);

            return GetProducts(spreadsheetId, sheetName);
        }

        public List<ProductModel> GetProducts(string spreadsheetId, string sheetName)
        {
            var results = new List<ProductModel>();
            var sheetRange = $"{sheetName}!A1:O";

            var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetRange);

            var response = request.Execute();
            var values = response.Values;
            var headers = new List<string>();
            if (values != null && values.Count > 0)
            {
                headers = values[0].Select(h => h.ToString()).ToList();
                values.RemoveAt(0);
                results = values.Select((row, index) =>
                {
                    return new ProductModel
                    {
                        GoogleSheetRowIndex = index + 2,
                        Code = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Code), headers, row),
                        Title = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Title), headers, row),
                        Model = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Model), headers, row),
                        Link = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Link), headers, row),
                        Asin = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Asin), headers, row),
                        CurrentPrice = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.CurrentPrice), headers, row).ToNullableDouble(),
                        AmazonPrice = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.AmazonPrice), headers, row).ToNullableDouble(),
                        InStock = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.InStock), headers, row).ToNullableBoolean(),
                        Arrive = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Arrive), headers, row),
                        FastestDelivery = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.FastestDelivery), headers, row),
                        Upc = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Upc), headers, row),
                        StoreLink = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.StoreLink), headers, row),
                        AdminLink = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.AdminLink), headers, row),
                        Note = GoogleSheetHelper.GetCellValue<ProductModel>(nameof(ProductModel.Note), headers, row),
                    };
                }).ToList();
            }

            return results;
        }

        public Task<int> UpdateDefaultProducts(IList<ProductModel> products, params string[] columns)
        {
            return UpdateProducts(products, _configuration.SpreadSheetId, _configuration.SheetId, columns);
        }

        public async Task<int> UpdateProducts
            (IList<ProductModel> products, string spreadsheetId, int? sheetId, params string[] columns)
        {
            var request = BuildBatchUpdateValuesRequest(spreadsheetId, sheetId, products, columns);

            var update = _sheetsService.Spreadsheets.Values.BatchUpdate(request, spreadsheetId);
            var updateResponse = await update.ExecuteAsync();
            return updateResponse.TotalUpdatedCells.GetValueOrDefault();
        }

        private SheetsService BuildSheetService()
        {
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GetServiceAccount(@"Configs\amz-crawler-295119-1e91ad34695f.json"),
                ApplicationName = _configuration.ApplicationName,
            });
            return service;
        }

        //https://stackoverflow.com/questions/41267813/authenticate-to-use-google-sheets-with-service-account-instead-of-personal-accou
        /// <summary>
        /// Get Service Account Credential
        /// </summary>
        /// <param name="serviceAccountKeyFilePath">The path to service account key file, retrieve from google console</param>
        /// <returns></returns>
        private ServiceAccountCredential GetServiceAccount(string serviceAccountKeyFilePath)
        {
            var path = Path.Combine(Environment.CurrentDirectory, serviceAccountKeyFilePath);
            if (!File.Exists(path))
            {
                Console.WriteLine("An Error occurred - Key file does not exist");
                return null;
            }

            ServiceAccountCredential credential;
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                credential = (ServiceAccountCredential)
                    GoogleCredential.FromStream(stream).UnderlyingCredential;

                var initializer = new ServiceAccountCredential.Initializer(credential.Id)
                {
                    Key = credential.Key,
                    Scopes = Scopes
                };
                credential = new ServiceAccountCredential(initializer);
            }

            return credential;
        }

        private string GetSheetName(string spreadsheetId, int? sheetId)
        {
            var getSpreadsheet = _sheetsService.Spreadsheets.Get(spreadsheetId);
            var spreadsheet = getSpreadsheet.Execute();

            var sheetName = spreadsheet.Sheets.FirstOrDefault(x => x.Properties.SheetId == sheetId)?.Properties.Title;

            if (sheetName.IsNullOrWhiteSpace())
            {
                throw new InvalidDataException("Invalid sheet id");
            }

            return sheetName;
        }

        private BatchUpdateValuesRequest BuildBatchUpdateValuesRequest(string spreadsheetId, int? sheetId, IList<ProductModel> products, params string[] columnNames)
        {
            var data = new List<ValueRange>();

            var sheetName = GetSheetName(spreadsheetId, sheetId);
            var headers = GetHeaders(spreadsheetId, sheetName);

            foreach (var columnName in columnNames)
            {
                var columnLetter = GoogleSheetHelper.GetColumnLetter<ProductModel>(headers, columnName);

                if (columnLetter.IsNotNullOrWhiteSpace())
                {
                    data.AddRange(products.Select(dto => new ValueRange
                    {
                        Range = $"{sheetName}!{columnLetter}{dto.GoogleSheetRowIndex}:{columnLetter}{dto.GoogleSheetRowIndex + 1}",
                        Values = new List<IList<object>> { new List<object> { GoogleSheetHelper.GetPropValue(dto, columnName) } }
                    }));
                }

            }

            return new BatchUpdateValuesRequest
            {
                Data = data,
                ValueInputOption = GoogleValueInputOption.USER_ENTERED.ToString(),
            };
        }

        private List<string> GetHeaders(string spreadsheetId, string sheetName)
        {
            var sheetRange = $"{sheetName}!A1:Z";
            var request = _sheetsService.Spreadsheets.Values.Get(spreadsheetId, sheetRange);
            var response = request.Execute();
            var values = response.Values;
            var headers = new List<string>();
            if (values != null && values.Count > 1)
                headers = values[0].Select(h => h.ToString()).ToList();

            return headers;
        }
    }
}

public enum GoogleValueInputOption
{
    INPUT_VALUE_OPTION_UNSPECIFIED = 1,
    RAW = 2,
    USER_ENTERED = 3
}