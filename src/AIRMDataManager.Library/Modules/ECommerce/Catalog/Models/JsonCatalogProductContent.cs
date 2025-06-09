using AIRMDataManager.Library.Models;

namespace AIRMDataManager.Library.Modules.ECommerce.Catalog.Models
{
    public class JsonCatalogProductContent : JirmModelV01
    {
        public new List<CatalogProductModel> Data { get; set; }
    }
}
