using Newtonsoft.Json;
using System.Collections.Generic;

namespace AmzCrawler.App.Services.Models
{
    public class ApifyTaskInputModel
    {
        [JsonProperty("country")]
        public string Country { get; set; } = "US";

        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("proxy")]
        public ApifyTaskProxySetting Proxy { get; set; } = new ApifyTaskProxySetting();
    }

    public class ApifyTaskProxySetting
    {

        [JsonProperty("useApifyProxy")]
        public bool UseApifyProxy { get; set; } = true;

        [JsonProperty("apifyProxyGroups")]
        public IList<string> ApifyProxyGroups { get; set; }

        [JsonProperty("proxyUrls")]
        public IList<string> ProxyUrls { get; set; }
    }
}
