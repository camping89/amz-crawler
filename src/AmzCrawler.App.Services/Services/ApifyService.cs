using AmzCrawler.App.Services.Configurations;
using AmzCrawler.App.Services.Helpers;
using AmzCrawler.App.Services.Models;
using AmzCrawler.App.Services.Services.Interfaces;
using Flurl.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmzCrawler.App.Services.Services
{
    public class ApifyService : IApifyService
    {
        private readonly ApifyConfiguration _configurations;

        public ApifyService(IOptions<ApifyConfiguration> options)
        {
            _configurations = options.Value;
        }

        public async Task<IList<ApifyResultModel>> GetDefaultCrawlingResults()
        {
            return await GetCrawlingResults(_configurations.TaskId, _configurations.Token);
        }

        public async Task StartDefaultCrawling()
        {
            await StartCrawling(_configurations.TaskId, _configurations.Token);
        }

        public async Task UpdateDefaultTaskInput(IEnumerable<string> asins)
        {
            await UpdateTaskInput(asins, _configurations.TaskId, _configurations.Token);
        }

        public async Task<IList<ApifyResultModel>> GetCrawlingResults(string taskId, string token)
        {
            var url = ApifyUrlHelper.CreateLastRunDataUrl(taskId, token);
            try
            {
                return await url.GetJsonAsync<IList<ApifyResultModel>>();
            }
            catch (System.Exception)
            {
                return new List<ApifyResultModel>();
            }
        }

        public async Task StartCrawling(string taskId, string token)
        {
            var taskUrl = ApifyUrlHelper.CreateRunTaskUrl(taskId, token);
            await taskUrl.WithHeader("Content-Type", "application/json").PostAsync(null).ReceiveJson();
        }

        public async Task UpdateTaskInput(IEnumerable<string> urls, string taskId, string token)
        {
            var input = new ApifyTaskInputModel
            {
                Search = string.Join(',', urls.Where(x => x.IsNotNullOrEmpty()))
            };
            var url = ApifyUrlHelper.CreateTaskInputUrl(taskId, token);
            await url.WithHeader("Content-Type", "application/json").PutJsonAsync(input).ReceiveJson();
        }
    }
}
