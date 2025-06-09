using AIRMDataManager.Library.Modules.Menu.Models;
using AIRMDataManager.Library.SystemCoreDataAccess;

namespace AIRMDataManager.Library.DataAccess.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ISqlDataAccess _sql;

        public MenuRepository(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public async Task<List<app_mnu>> GetTopMenu()
        {
            var output = await _sql.LoadDataStoredProcedureAsync<app_mnu, dynamic>("dbo.sp_TopMenu_GetAll", new { }, "DefaultConnection");

            return output;
        }

        public async Task<List<app_mnu>> GetSideMenuSubMenuByTopMenu(int topMenuId)
        {
            var output = await _sql.LoadDataStoredProcedureAsync<app_mnu, dynamic>("dbo.sp_SideMenuSubMenu_GetByTopMenu", new { mnu_id = topMenuId }, "DefaultConnection");

            return output;
        }

        public async Task<List<app_mnu>> GetBareMenu()
        {
            var output = await _sql.LoadDataStoredProcedureAsync<app_mnu, dynamic>("dbo.sp_DetailMenu_GetAll", new { }, "DefaultConnection");

            return output;
        }
    }
}
