using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace NFCEPS.UI.Shared.Security;

public class NoOpAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public NoOpAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // No authentication performed — always return "no result"
        // (user remains unauthenticated unless set elsewhere like Blazor state provider)

        return Task.FromResult(AuthenticateResult.NoResult());
    }
}



