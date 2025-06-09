namespace AIRMDataManager.Library.Modules.ECommerce.Product.Models
{
    public class ProductModel
    {
        public int id { get; set; }
        public string product_nm { get; set; }
        public string description { get; set; }
        public decimal retail_price { get; set; }
        public int qty_instock { get; set; }
        public bool istaxable { get; set; }
        public string photo_url { get; set; }
    }
}
