using System.Net.Http.Headers;
using NFCEPS_UI.Auth;

namespace NFCEPS_UI.Auth;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly AuthSessionManager _session;

    public AuthorizationMessageHandler(AuthSessionManager session)
    {
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

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await _session.LogoutAsync();
        }

        return response;
    }
}