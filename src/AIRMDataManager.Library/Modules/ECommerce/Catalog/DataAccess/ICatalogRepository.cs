using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRMDataManager.Library.Modules.ECommerce.Brand.Models;
using AIRMDataManager.Library.Modules.ECommerce.Catalog.Models;
using AIRMDataManager.Library.Modules.ECommerce.Product.Models;
using AIRMDataManager.Library.Modules.ECommerce.ProductType.Models;

namespace AIRMDataManager.Library.Modules.ECommerce.Catalog.DataAccess
{
    public interface ICatalogRepository
    {
        Task<List<BrandModel>> GetCatalogBrands();
        Task<List<ProductTypeModel>> GetCatalogProductTypes();
        Task<List<CatalogProductModel>> GetCatalogProducts(int pageIndex, int pageSize, int? brandId = null, int? typeId = null);
        Task<ProductModel> GetProductById(int id);
        Task<List<ProductModel>> GetProductsByIds(int[] ids);
    }
}
