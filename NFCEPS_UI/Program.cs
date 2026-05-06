using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using NFCEPS_UI.Auth;
using NFCEPS_UI.Components;
using NFCEPS_UI.Managers;
using NFCEPS_UI.Services;

var builder = WebApplication.CreateBuilder(args);

//
// ===================== AUTH CORE =====================
//

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthSessionManager>();

builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AuthStateProvider>());

builder.Services.AddCascadingAuthenticationState();

//
// ===================== HTTP + JWT HANDLER =====================
//

builder.Services.AddScoped<AuthorizationMessageHandler>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5043/");
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

//
// ===================== APP SERVICES =====================
//

builder.Services.AddScoped<CurrentUser>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<PermissionService>();

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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();