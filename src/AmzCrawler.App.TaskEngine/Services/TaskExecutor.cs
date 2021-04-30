using AmzCrawler.App.Services.Helpers;
using AmzCrawler.App.Services.Models;
using AmzCrawler.App.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmzCrawler.App.TaskEngine.Services
{
    public interface ITaskExecutor
    {
        Task Excute(TaskInputModel taskInput);
        Task Excute(int taskId);
    }

    public class TaskExecutor : ITaskExecutor
    {
        private readonly IApifyService _apifyService;
        private readonly IGoogleSheetService _googleSheetService;
        private readonly List<TaskInputModel> _tasks;
        public TaskExecutor(IApifyService apifyService, IGoogleSheetService googleSheetService, List<TaskInputModel> tasks)
        {
            _apifyService = apifyService;
            _googleSheetService = googleSheetService;
            _tasks = tasks;
        }

        public async Task Excute(int taskId)
        {
            var task = _tasks.FirstOrDefault(x => x.TaskId == taskId);
            if (task == null)
            {
                throw new Exception("Invalid task id");
            }

            await Excute(task);
        }

        public async Task Excute(TaskInputModel taskInput)
        {
            ValidateTaskInputModel(taskInput);
            switch (taskInput.Command)
            {
                case "crawl":
                    await CrawlingProducts(taskInput);
                    break;
                case "update":
                    await UpdateProducts(taskInput);
                    break;
                default:
                    break;
            }
        }

        private async Task CrawlingProducts(TaskInputModel taskInput)
        {
            var products = _googleSheetService.GetProducts(taskInput.SpreadsheetId, taskInput.SheetId);
            var productsToCrawl = new List<ProductModel>();

            if (products.Any())
            {
                taskInput.EndIndex ??= products.Count + 10;

                if (taskInput.FailedToSyncOnly == true)
                {
                    productsToCrawl = products.Where(x => taskInput.StartIndex <= x.GoogleSheetRowIndex &&
                                                                   x.GoogleSheetRowIndex <= taskInput.EndIndex &&
                                                                   x.FastestDelivery.ToNullableBoolean() != true).ToList();
                }
                else
                {
                    productsToCrawl = products.Where(x => taskInput.StartIndex <= x.GoogleSheetRowIndex &&
                                                                   x.GoogleSheetRowIndex <= taskInput.EndIndex).ToList();
                }

                var urls = productsToCrawl.Select(x => x.Link.Trim());
                await _apifyService.UpdateTaskInput(urls, taskInput.ApifyTaskId, taskInput.ApifyToken);
                await _apifyService.StartCrawling(taskInput.ApifyTaskId, taskInput.ApifyToken);
            }
        }

        private async Task UpdateProducts(TaskInputModel taskInput)
        {
            var products = _googleSheetService.GetProducts(taskInput.SpreadsheetId, taskInput.SheetId);

            var productsToUpdate = new List<ProductModel>();

            if (products.Any())
            {
                taskInput.EndIndex ??= products.Count + 10;



                if (taskInput.FailedToSyncOnly == true)
                {
                    productsToUpdate = products.Where(x => taskInput.StartIndex <= x.GoogleSheetRowIndex &&
                                                                   x.GoogleSheetRowIndex <= taskInput.EndIndex &&
                                                                   x.FastestDelivery.ToNullableBoolean() != true).ToList();
                }
                else
                {
                    productsToUpdate = products.Where(x => taskInput.StartIndex <= x.GoogleSheetRowIndex &&
                                                                   x.GoogleSheetRowIndex <= taskInput.EndIndex).ToList();
                }

                var crawlingResults = await _apifyService.GetCrawlingResults(taskInput.ApifyTaskId, taskInput.ApifyToken);

                foreach (var product in productsToUpdate)
                {
                    var crawledProduct = crawlingResults.FirstOrDefault(x => x.Asin == product.Asin);
                    if (crawledProduct != null)
                    {
                        product.AmazonPrice = GetAmazonPrice(crawledProduct);
                        product.InStock = crawledProduct.ItemDetail.InStock;
                        product.Arrive = GetDeliveryDate(crawledProduct.ItemDetail.Delivery);
                        product.FastestDelivery = "Yes";
                    }
                    else
                    {
                        product.FastestDelivery = "No";
                    }
                }


                var succeeded = false;
                var failedTime = 0;
                while (!succeeded)
                {
                    try
                    {
                        await _googleSheetService.UpdateProducts(productsToUpdate,
                                                              taskInput.SpreadsheetId,
                                                              taskInput.SheetId,
                                                              nameof(ProductModel.AmazonPrice),
                                                              nameof(ProductModel.InStock),
                                                              nameof(ProductModel.FastestDelivery),
                                                              nameof(ProductModel.Arrive));
                        succeeded = true;
                    }
                    catch (Exception)
                    {
                        failedTime++;
                        Console.WriteLine($"Failed to update {failedTime} time(s). Will try again after 5 seconds.");
                        Thread.Sleep(5000);
                    }
                }
            }
        }

        private void ValidateTaskInputModel(TaskInputModel taskInput)
        {
            if (taskInput.Command.IsNullOrWhiteSpace()
                || taskInput.SpreadsheetId.IsNullOrWhiteSpace()
                || !taskInput.SheetId.HasValue
                || taskInput.ApifyTaskId.IsNullOrWhiteSpace()
                || taskInput.ApifyToken.IsNullOrWhiteSpace()
                || taskInput.EndIndex.HasValue && taskInput.EndIndex < taskInput.StartIndex)
            {
                throw new ValidationException("Invalid task input");
            }
        }

        private string GetDeliveryDate(string input)
        {
            if (input.IsNotNullOrWhiteSpace())
            {
                var strArray = input.Replace("\n", ";").Split(";");
                foreach (var str in strArray)
                {
                    if (DateTime.TryParse(str, out _))
                    {
                        return str;
                    }
                }
            }
            return null;
        }

        private double? GetAmazonPrice(ApifyResultModel result)
        {
            var price = result.Sellers.OrderBy(x => x.PriceParsed).FirstOrDefault(x => x.Condition.ToLower() == "new")?.PriceParsed ??
                        result.Sellers.OrderBy(x => x.PriceParsed).FirstOrDefault()?.PriceParsed;

            return price;
        }


    }
}
