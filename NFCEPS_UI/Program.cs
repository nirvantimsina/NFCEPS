using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Components;
using NFCEPS_UI.Auth.Managers;
using NFCEPS_UI.Services;
using NFCEPS_UI.Models.Auth.ResponseModel;
using NFCEPS_UI.Managers.Dashboard.Interface;
using NFCEPS_UI.Managers.Dashboard.Implementation;

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

//
// ===================== UI SERVICES =====================
//

builder.Services.AddMudServices();

//
// ===================== RAZOR / BLAZOR =====================
//

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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