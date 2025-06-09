using AIRMDataManager.Library.Modules.ECommerce.Brand.Models;
using AIRMDataManager.Library.Modules.ECommerce.Catalog.Models;
using AIRMDataManager.Library.Modules.ECommerce.Product.Models;
using AIRMDataManager.Library.Modules.ECommerce.ProductType.Models;

namespace AiclaRM.Server.Services.ECommerce
{
    public interface ICatalogService
    {
        Task<List<BrandModel>> GetCatalogBrandsAsync();
        Task<List<ProductTypeModel>> GetCatalogProductTypesAsync();
        Task<List<CatalogProductModel>> GetCatalogProductsAsync(int pageIndex, int pageSize);
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<List<ProductModel>> GetProductByIdsAsync(int[] ids);
    }
}
