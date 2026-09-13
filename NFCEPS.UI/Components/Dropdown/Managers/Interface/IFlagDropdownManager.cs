using NFCEPS.Shared.Models;
using NFCEPS.Shared.Wrappers;
using NFCEPS.UI.Components.Dropdown.Models.RequestModel;

namespace NFCEPS.UI.Components.Dropdown.Managers.Interface
{
    public interface IFlagDropdownManager
    {
        Task<ApiResponse<List<DropdownListModel>>> DropdownListAsync(DropdownRequestModel request);
    }
}