using NFCEPS.Shared.Wrappers;
using NFCEPS.UI.Shared.Infrastructure;

namespace NFCEPS.UI.Features.Auth.Managers.Interface
{
    public interface IMenuSetupManager
    {
        Task<ApiResponse<MenuListModel>> GetAllMenuListAsync();
    }
}