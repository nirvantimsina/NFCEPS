using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using NFCEPS.UI.Features.Auth;
using NFCEPS.UI.Shared.Security;


namespace NFCEPS.UI.Shared.Layouts
{
    public partial class MainLayout(PermissionService PermissionService, NavigationManager Navigation,
    AuthenticationStateProvider AuthProvider, AuthSessionManager AuthSessionManager) : IDisposable
    {
        private bool _drawerOpen = true;
        private string _userName = string.Empty;
        private string _userInitials = "?";
        private string _roleName = string.Empty;
        private string _clock = string.Empty;
        private System.Threading.Timer? _timer;

        private readonly MudTheme _theme = new()
        {
            PaletteLight = new PaletteLight(),
            PaletteDark = new PaletteDark()
            {
                Black = "#0a0a0f",
                Background = "#0f1117",
                BackgroundGray = "#161820",
                Surface = "#1a1d27",
                DrawerBackground = "#13151e",
                DrawerText = "#cbd5e1",
                DrawerIcon = "#94a3b8",
                AppbarBackground = "#13151e",
                AppbarText = "#f1f5f9",
                TextPrimary = "#f1f5f9",
                TextSecondary = "#94a3b8",
                TextDisabled = "#475569",
                Primary = "#3b82f6",
                PrimaryContrastText = "#ffffff",
                Secondary = "#6366f1",
                Success = "#22c55e",
                Warning = "#f59e0b",
                Error = "#ef4444",
                Info = "#06b6d4",
                LinesDefault = "#1e2436",
                LinesInputs = "#2d3348",
                Divider = "#1e2436",
            },
            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "240px",
                DefaultBorderRadius = "6px",
            },
            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = ["'DM Sans'", "sans-serif"],
                    FontSize = ".875rem",
                },
                H6 = new H6Typography()
                {
                    FontFamily = ["'DM Mono'", "monospace"],
                    FontWeight = "600",
                }
            }
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;

            var token = await AuthSessionManager.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                Navigation.NavigateTo("/login", forceLoad: true);
                return;
            }

            await LoadUserAsync();
            await PermissionService.LoadFromTokenAsync(AuthSessionManager);
            StartClock();
            StateHasChanged();
        }

        // Remove OnInitializedAsync entirely

        private async Task LoadUserAsync()
        {
            var state = await AuthProvider.GetAuthenticationStateAsync();
            var user = state.User;

            _userName = user.FindFirst("userName")?.Value ?? "User";
            _roleName = user.FindFirst("roleName")?.Value ?? string.Empty;
            _userInitials = BuildInitials(_userName);
        }

        private void StartClock()
        {
            _clock = DateTime.Now.ToString("HH:mm:ss");
            _timer = new System.Threading.Timer(_ =>
            {
                _clock = DateTime.Now.ToString("HH:mm:ss");
                InvokeAsync(StateHasChanged);
            }, null, 1000, 1000);
        }

        private void ToggleDrawer() => _drawerOpen = !_drawerOpen;

        private async Task LogoutAsync()
        {
            await AuthSessionManager.LogoutAsync(); // or whatever your method is called
            Navigation.NavigateTo("/login", forceLoad: true);
        }

        private static string BuildInitials(string name)
        {
            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 2
                ? $"{parts[0][0]}{parts[^1][0]}".ToUpper()
                : name.Length > 0 ? name[0].ToString().ToUpper() : "?";
        }

        public void Dispose() => _timer?.Dispose();
    }
}


