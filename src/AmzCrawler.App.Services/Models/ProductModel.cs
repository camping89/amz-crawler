using AmzCrawler.App.Services.Attributes;

namespace AmzCrawler.App.Services.Models
{
    public class ProductModel
    {
        [GoogleSheetHeader("Code")]
        public string Code { get; set; }

        [GoogleSheetHeader("Product Name")]
        public string Title { get; set; }

        [GoogleSheetHeader("Model")]
        public string Model { get; set; }

        [GoogleSheetHeader("WAREHOUSE #1")]
        public string Link { get; set; }

        [GoogleSheetHeader("Asin")]
        public string Asin { get; set; }

        [GoogleSheetHeader("Giá Cost Store Hiện Tại")]
        public double? CurrentPrice { get; set; }

        [GoogleSheetHeader("Giá Amazon Hiện Tại (Auto)")]
        public double? AmazonPrice { get; set; }

        [GoogleSheetHeader("In Stock (Auto)")]
        public bool? InStock { get; set; }

        [GoogleSheetHeader("Arrives (Auto)")]
        public string Arrive { get; set; }

        [GoogleSheetHeader("Check (Auto)")]
        public string FastestDelivery { get; set; }

        [GoogleSheetHeader("Upc")]
        public string Upc { get; set; }

        [GoogleSheetHeader("Store Link")]
        public string StoreLink { get; set; }

        [GoogleSheetHeader("Admin Link")]
        public string AdminLink { get; set; }

        [GoogleSheetHeader("Note")]
        public string Note { get; set; }

        public int GoogleSheetRowIndex { get; set; }
    }
}
