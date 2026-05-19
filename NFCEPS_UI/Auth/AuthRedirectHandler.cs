using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using NFCEPS_UI.Auth;

public class AuthRedirectHandler : DelegatingHandler
{
    private readonly NavigationManager _navigation;
    private readonly AuthSessionManager _session;

    public AuthRedirectHandler(
        NavigationManager navigation,
        AuthSessionManager session)
    {
        _navigation = navigation;
        _session = session;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _session.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _session.LogoutAsync();
        }

        return response;
    }
}