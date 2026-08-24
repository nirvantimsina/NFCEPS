using Microsoft.JSInterop;
using NFCEPS.UI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace NFCEPS.UI.Auth;

public class AuthSessionManager(IJSRuntime js, TokenStore tokenStore)
{
    private const string Key = "authToken";
    private const string MenuKey = "menuList";

    public async Task<string?> GetTokenAsync()
    {
        // Return from memory first — but validate it hasn't expired
        if (!string.IsNullOrWhiteSpace(tokenStore.Token))
        {
            if (!IsTokenExpired(tokenStore.Token))
                return tokenStore.Token;

            // Token in memory is expired — clear it and fall through
            tokenStore.Token = null;
        }

        // Fall back to localStorage (page reload / tab reopen scenario)
        try
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", Key);
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (IsTokenExpired(token))
                {
                    // Expired token in storage — clean it up
                    await LogoutAsync();
                    return null;
                }

                tokenStore.Token = token;
                return token;
            }
        }
        catch (Exception)
        {
            return null;
        }

        return null;
    }

    public async Task<List<MenuListModel>> GetMenuListAsync()
    {
        if (tokenStore.MenuList != null && tokenStore.MenuList.Any())
        {
            return tokenStore.MenuList;
        }

        try
        {
            var json = await js.InvokeAsync<string?>("localStorage.getItem", MenuKey);
            if (!string.IsNullOrWhiteSpace(json))
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var list = JsonSerializer.Deserialize<List<MenuListModel>>(json, options);

                if (list != null)
                {
                    tokenStore.MenuList = list;
                    return list;
                }
            }
        }
        catch { }

        return new List<MenuListModel>();
    }

    private bool IsTokenExpired(string jwt)
    {
        try
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            // Add 30s clock skew tolerance
            return token.ValidTo < DateTime.UtcNow.AddSeconds(-30);
        }
        catch
        {
            return true; // Treat unreadable tokens as expired
        }
    }

    public async Task LoginAsync(string token, List<MenuListModel> menuList)
    {
        // set in memory immediately
        tokenStore.Token = token;
        tokenStore.MenuList = menuList;

        await js.InvokeVoidAsync("localStorage.setItem", Key, token);

        var json = JsonSerializer.Serialize(tokenStore.MenuList);
        await js.InvokeVoidAsync("localStorage.setItem", MenuKey, json);
    }

    public async Task LogoutAsync()
    {
        tokenStore.Token = null;
        tokenStore.MenuList = null;
        try
        {
            await js.InvokeVoidAsync("localStorage.removeItem", Key);
            await js.InvokeVoidAsync("localStorage.removeItem", MenuKey);
        }
        catch (InvalidOperationException) { }
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return Enumerable.Empty<Claim>();
        return Parse(token);
    }

    private IEnumerable<Claim> Parse(string jwt)
    {
        try
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt).Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }
}

