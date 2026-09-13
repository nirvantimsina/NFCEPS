using NFCEPS.Shared.Wrappers;
using NFCEPS.UI.Features.Auth.Managers.Interface;
using NFCEPS.UI.Features.Auth.Managers.Route;
using NFCEPS.UI.Shared.Infrastructure;

namespace NFCEPS.UI.Features.Auth.Managers.Implementation
{
    public class MenuSetupManager(IHttpClientFactory factory, AuthSessionManager sessionManager) 
        : BaseManager(sessionManager), IMenuSetupManager
    {
        public async Task<ApiResponse<List<MenuListModel>>> GetAllMenuListAsync()
        {
            var http = factory.CreateClient("API");
            var response = await http.GetAsync(MenuSetupRoute.MenuList);
            return await HandleResponse<List<MenuListModel>>(response);
        }
    }
}