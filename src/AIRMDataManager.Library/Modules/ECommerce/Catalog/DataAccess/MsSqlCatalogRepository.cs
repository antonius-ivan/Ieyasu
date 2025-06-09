using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.Modules.ECommerce.Brand.Models;
using AIRMDataManager.Library.Modules.ECommerce.Catalog.Models;
using AIRMDataManager.Library.Modules.ECommerce.Product.Models;
using AIRMDataManager.Library.Modules.ECommerce.ProductType.Models;
using AIRMDataManager.Library.SystemCoreDataAccess;

namespace AIRMDataManager.Library.Modules.ECommerce.Catalog.DataAccess
{
    public class MsSqlCatalogRepository : ICatalogRepository
    {
        // Example: Inject your database connection factory here
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly ISqlDataAccess _sqlDataAccess;

        public MsSqlCatalogRepository(IDatabaseConnectionFactory connectionFactory, ISqlDataAccess sqlDataAccess)
        {
            _connectionFactory = connectionFactory;
            _sqlDataAccess = sqlDataAccess;
        }
        public async Task<List<BrandModel>> GetCatalogBrands()
        {
            // Assuming you have a stored procedure named 'spGetCatalogBrands'
            var storedProcedure = "sp_Brand_GetAll";
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var brands = await _sqlDataAccess.LoadDataStoredProcedureAsync<BrandModel, dynamic>(storedProcedure, null, databaseCode);
            return brands;
        }

        public async Task<List<ProductTypeModel>> GetCatalogProductTypes()
        {
            // Assuming you have a stored procedure named 'spGetCatalogBrands'
            var storedProcedure = "sp_ProductType_GetAll";
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var productTypes = await _sqlDataAccess.LoadDataStoredProcedureAsync<ProductTypeModel, dynamic>(storedProcedure, null, databaseCode);
            return productTypes;
        }
        public async Task<List<ProductModel>> GetCatalogProducts()
        {
            // Assuming you have a stored procedure named 'spGetCatalogItems'
            var storedProcedure = "sp_CatalogProducts_GetAll"; // Update this to match your actual stored procedure name
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var parameters = new { };

            var catalogItems = await _sqlDataAccess.LoadDataStoredProcedureAsync<ProductModel, dynamic>(storedProcedure, parameters, databaseCode);
            return catalogItems;
        }
        public async Task<List<CatalogProductModel>> GetCatalogProducts(int pageIndex, int pageSize, int? brandId = null, int? typeId = null)
        {
            // Assuming you have a stored procedure named 'spGetCatalogItems'
            var storedProcedure = "sp_CatalogProductsPaginated_GetAll"; // Update this to match your actual stored procedure name
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var parameters = new { PageIndex = pageIndex, PageSize = pageSize, BrandId = brandId, TypeId = typeId };

            var catalogItems = await _sqlDataAccess.LoadDataStoredProcedureAsync<CatalogProductModel, dynamic>(storedProcedure, parameters, databaseCode);
            return catalogItems;
        }


        public async Task<ProductModel> GetProductById(int id)
        {
            // Assuming you have a stored procedure named 'spGetCatalogItems'
            var storedProcedure = "sp_Product_GetById";
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var parameters = new { Id = id };

            var catalogProduct = await _sqlDataAccess.LoadDataStoredProcedureAsync<ProductModel, dynamic>(storedProcedure, parameters, databaseCode);

            // Assuming LoadDataStoredProcedureAsync returns a list, and you want to return a single item
            return catalogProduct?.FirstOrDefault();
        }

        public async Task<List<ProductModel>> GetProductsByIds(int[] ids)
        {
            // Assuming you have a stored procedure named 'spGetCatalogItems'
            var storedProcedure = "sp_Product_GetByIds";
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            //var parameters = new { Id = Id };

            //var catalogProduct = await _sqlDataAccess.LoadDataStoredProcedureAsync<ProductModel, dynamic>(storedProcedure, parameters, databaseCode);

            // Assuming LoadDataStoredProcedureAsync returns a list, and you want to return a single item
            //return catalogProduct?.FirstOrDefault();
            var allProducts = await GetCatalogProducts();

            // Filter the products based on specific IDs
            var filteredProducts = allProducts.Where(p => ids.Contains(p.id)).ToList();

            return filteredProducts;


            //List<ProductModel> asdf = new List<ProductModel>();

            //return asdf;
        }
    }
}
