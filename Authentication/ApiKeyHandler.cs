using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace GameBacklog.Authentication;

public class ApiKeyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public ApiKeyHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check whether the request includes an API key.
        if (!Request.Headers.TryGetValue("X-API-Key", out var providedKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // Reject an incorrect key.
        if (providedKey.ToString() != "hello123")
        {
            return Task.FromResult(
                AuthenticateResult.Fail("Invalid API key.")
            );
        }

        // Describe the authenticated caller.
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "GameStats"),
            new Claim("subscription", "gold")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var caller = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(caller, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}