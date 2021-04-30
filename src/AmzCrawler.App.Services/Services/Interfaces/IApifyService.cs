using System.Collections.Generic;
using System.Threading.Tasks;
using AmzCrawler.App.Services.Models;

namespace AmzCrawler.App.Services.Services.Interfaces
{
    public interface IApifyService
    {
        Task StartDefaultCrawling();
        Task UpdateDefaultTaskInput(IEnumerable<string> asins);
        Task<IList<ApifyResultModel>> GetDefaultCrawlingResults();
        Task StartCrawling(string taskId, string token);
        Task UpdateTaskInput(IEnumerable<string> urls, string taskId, string token);
        Task<IList<ApifyResultModel>> GetCrawlingResults(string taskId, string token);
    }
}
