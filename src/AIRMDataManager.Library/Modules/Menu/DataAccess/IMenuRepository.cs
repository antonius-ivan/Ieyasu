using AIRMDataManager.Library.Modules.Menu.Models;

namespace AIRMDataManager.Library.DataAccess.Repositories
{
    public interface IMenuRepository
    {
        Task<List<app_mnu>> GetBareMenu();
        Task<List<app_mnu>> GetSideMenuSubMenuByTopMenu(int topMenuId);
        Task<List<app_mnu>> GetTopMenu();
    }
}
