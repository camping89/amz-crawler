using AmzCrawler.App.Services;
using AmzCrawler.App.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AmzCrawler.App.Services.Services.Interfaces;

namespace AmzCrawler.App
{
    public partial class MainForm : Form
    {
        private readonly IGoogleSheetService _sheetService;
        private readonly IApifyService _apifyService;
        public MainForm(IApifyService apifyService, IGoogleSheetService sheetService)
        {
            _apifyService = apifyService;
            _sheetService = sheetService;
            InitializeComponent();
        }

        private async void StartCrawling(object sender, EventArgs eventArgs)
        {
            DisplayUpdateProductProgress();

            UpdateCurrentProcess(20, "Preparing data to crawl");
            var products = _sheetService.GetDefaultProducts();

            UpdateCurrentProcess(40, $"Updating task configurations");
            await _apifyService.UpdateDefaultTaskInput(products.Select(x => x.Asin));

            UpdateCurrentProcess(60, $"Sending request to crawl {products.Count} products");
            await _apifyService.StartDefaultCrawling();

            UpdateCurrentProcess(100, "Crawling request has been sent successfully");

            HideUpdateProductProgress();
            DisplayResultMessage($"Crawling request for {products.Count} products has been sent successfully");
        }

        private async void UpdateProductPrices(object sender, EventArgs eventArgs)
        {
            var products = GetCurrentProducts();
            var crawlingResults = await GetCrawlingResults();

            await UpdateProducts(products, crawlingResults);
        }

        private IList<ProductModel> GetCurrentProducts()
        {
            var products = _sheetService.GetDefaultProducts();

            PgrLabel.Text = $"Current product: {products.Count}";
            UpdateCurrentProcess(10, $"Current product: {products.Count}");

            return products;
        }

        private async Task<IList<ApifyResultModel>> GetCrawlingResults()
        {
            UpdateCurrentProcess(20, "Start getting crawled results");

            var results = await _apifyService.GetDefaultCrawlingResults();

            UpdateCurrentProcess(30, $"Crawled {results.Count} results from last run");

            return results;
        }

        private async Task UpdateProducts(IList<ProductModel> products, IList<ApifyResultModel> crawlingResults)
        {
            DisplayUpdateProductProgress();

            UpdateCurrentProcess(40, "Start updating products");

            foreach (var product in products)
            {
                var crawledProduct = crawlingResults.FirstOrDefault(x => x.Asin == product.Asin);
                if (crawledProduct != null)
                {
                    product.AmazonPrice = crawledProduct.Sellers.OrderBy(x => x.PriceParsed).FirstOrDefault(x => x.PriceParsed > 0 && x.Condition.ToLower() == "new").PriceParsed;
                    product.InStock = crawledProduct.ItemDetail.InStock;
                    product.Arrive = crawledProduct.ItemDetail.Delivery;
                }
            }

            UpdateCurrentProcess(50, "Updating products");

            var updatedRows = await _sheetService.UpdateDefaultProducts(products);

            UpdateCurrentProcess(100, $"Updated {updatedRows} products");

            HideUpdateProductProgress();

            DisplayResultMessage($"{updatedRows} products have been updated");
        }

        private void UpdateCurrentProcess(int progressValue, string currentStep)
        {
            PgrLabel.Text = currentStep;
            PgrUpdatePrices.Value = progressValue;
        }

        private void HideUpdateProductProgress()
        {
            PgrLabel.Visible = false;
            PgrUpdatePrices.Visible = false;
        }

        private void DisplayUpdateProductProgress()
        {
            PgrLabel.Visible = true;
            LbResult.Visible = false;
            PgrUpdatePrices.Visible = true;
        }

        private void DisplayResultMessage(string message)
        {
            LbResult.Visible = true;
            LbResult.Text = message;
        }
    }
}
