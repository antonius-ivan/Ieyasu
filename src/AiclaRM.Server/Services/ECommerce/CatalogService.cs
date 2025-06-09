using AIRMDataManager.Library.DataAccess.Repositories;
using AIRMDataManager.Library.Modules.ECommerce.Brand.Models;
using AIRMDataManager.Library.Modules.ECommerce.Catalog.DataAccess;
using AIRMDataManager.Library.Modules.ECommerce.Catalog.Models;
using AIRMDataManager.Library.Modules.ECommerce.Product.Models;
using AIRMDataManager.Library.Modules.ECommerce.ProductType.Models;

namespace AiclaRM.Server.Services.ECommerce
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public Task<List<BrandModel>> GetCatalogBrandsAsync() => _catalogRepository.GetCatalogBrands();
            
        public Task<List<ProductTypeModel>> GetCatalogProductTypesAsync() => _catalogRepository.GetCatalogProductTypes();

        public Task<List<CatalogProductModel>> GetCatalogProductsAsync(int pageIndex, int pageSize)
            => _catalogRepository.GetCatalogProducts(pageIndex, pageSize);

        public Task<ProductModel> GetProductByIdAsync(int id) => _catalogRepository.GetProductById(id);

        public Task<List<ProductModel>> GetProductByIdsAsync(int[] ids) => _catalogRepository.GetProductsByIds(ids);
    }
}
