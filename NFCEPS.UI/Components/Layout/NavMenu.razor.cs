using Microsoft.AspNetCore.Components;
using NFCEPS.UI.Auth;
using NFCEPS.UI.Models;
using NFCEPS.UI.Pages.Auth.Managers.Interface;

namespace NFCEPS.UI.Components.Layout
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        private IAuthManager authManager { get; set; } = default!;
        [Inject]
        private AuthSessionManager authSessionManager { get; set; } = default!;

        [Parameter]
        public bool IsPinned { get; set; }

        [Parameter]
        public EventCallback OnTogglePin { get; set; }

        private string activeMenu = "Dashboard";
        private int expandedMenuId = 0;
        private List<MenuListModel> menuListModel = new();

        protected override async Task OnInitializedAsync()
        {
            var userMenu = await authSessionManager.GetMenuListAsync();
            if (userMenu != null)
            {
                menuListModel = userMenu;
            }
        }

        private async Task TogglePin()
        {
            if (OnTogglePin.HasDelegate)
            {
                await OnTogglePin.InvokeAsync();
            }
        }

        private void ToggleChildMenu(MenuListModel parentMenu)
        {
            if (expandedMenuId == parentMenu.MenuId)
            {
                expandedMenuId = 0; // collapse
            }
            else
            {
                expandedMenuId = parentMenu.MenuId; // expand
            }
        }

        private void Redirect(MenuListModel menu)
        {
            activeMenu = menu.MenuName ?? "";
            if (!string.IsNullOrWhiteSpace(menu.Path))
            {
                NavigationManager.NavigateTo(menu.Path);
            }
        }

        private string _searchTerm = "";
        public string SearchTerm
        {
            get => _searchTerm;
            set { _searchTerm = value; }
        }

        public List<MenuListModel> FilteredMenuList
        {
            get
            {
                if (menuListModel == null) return null;
                if (string.IsNullOrWhiteSpace(SearchTerm)) return menuListModel;

                var lowerSearch = SearchTerm.ToLower();
                return menuListModel.Where(m =>
                    (m.MenuName != null && m.MenuName.ToLower().Contains(lowerSearch)) ||
                    menuListModel.Any(child => child.ParentId == m.MenuId && child.MenuName != null && child.MenuName.ToLower().Contains(lowerSearch)) ||
                    menuListModel.Any(parent => parent.MenuId == m.ParentId && parent.MenuName != null && parent.MenuName.ToLower().Contains(lowerSearch))
                ).ToList();
            }
        }

        private async Task HandleLogout()
        {
            await authSessionManager.LogoutAsync();
            NavigationManager.NavigateTo("/", true);
        }
    }
}


