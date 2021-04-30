using System.Collections.Generic;

namespace AmzCrawler.App.Services.Models
{
    public class ApifyResultModel
    {
        public string Title { get; set; }
        public string Asin { get; set; }
        public IList<ApifySellerModel> Sellers { get; set; }
        public ApifyItemDetailModel ItemDetail { get; set; }
    }

    public class ApifySellerModel
    {
        public string Price { get; set; }
        public double? PriceParsed { get; set; }
        public string Condition { get; set; }
    }

    public class ApifyItemDetailModel
    {
        public bool InStock { get; set; }
        public string Delivery { get; set; }
    }

}
