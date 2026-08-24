using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using NFCEPS.UI.Auth;
using NFCEPS.UI.Components;
using NFCEPS.UI.Pages.Auth.Managers.Implementation;
using NFCEPS.UI.Pages.Auth.Managers.Interface;
using NFCEPS.UI.Pages.Auth.Models.ResponseModel;
using NFCEPS.UI.Pages.Card.Managers.Implementation;
using NFCEPS.UI.Pages.Card.Managers.Interface;
using NFCEPS.UI.Pages.Dashboard.Managers.Implementation;
using NFCEPS.UI.Pages.Dashboard.Managers.Interface;
using NFCEPS.UI.Pages.Reports.Managers.Implementation;
using NFCEPS.UI.Pages.Reports.Managers.Interface;
using NFCEPS.UI.Pages.Reports.Pages;
using NFCEPS.UI.Services;

var builder = WebApplication.CreateBuilder(args);

//
// ===================== AUTH CORE =====================
//
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "NoOp";
    options.DefaultChallengeScheme = "NoOp";
})
.AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, NoOpAuthHandler>(
    "NoOp", _ => { });
// builder.Services.AddAuthorization();

builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthSessionManager>();

builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AuthStateProvider>());

builder.Services.AddCascadingAuthenticationState();

//
// ===================== HTTP + JWT HANDLER =====================
//

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5043/");
});

builder.Services.AddScoped<TokenStore>();

//
// ===================== APP SERVICES =====================
//

builder.Services.AddScoped<CurrentUser>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<IDashboardManager, DashboardManager>();
builder.Services.AddScoped<ICardManager, CardManager>();
builder.Services.AddScoped<IUserReportManager, UserReportManager>();

//
// ===================== UI SERVICES =====================
//

builder.Services.AddMudServices(config =>
{
    // Sets how long the toast stays visible on screen (3 seconds)
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.ShowTransitionDuration = 150;
    config.SnackbarConfiguration.HideTransitionDuration = 150;

    // 1. MANDATORY: Enables the timer progress bar tracker line
    config.SnackbarConfiguration.ShowCloseIcon = true;

    // 2. OPTIONAL: Automatically pauses the countdown timer bar if the user hovers their mouse cursor over the toast
    config.SnackbarConfiguration.RequireInteraction = false;

    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = true;
});

//
// ===================== RAZOR / BLAZOR =====================
//

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });

var app = builder.Build();

//
// ===================== PIPELINE =====================
//

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// app.UseHttpsRedirection(); // later uncomment this

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

