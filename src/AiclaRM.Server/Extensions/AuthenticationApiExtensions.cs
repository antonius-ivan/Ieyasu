using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
//app\AiclaRM.Server\Extensions\AuthenticationApiExtensions.cs

namespace AiclaRM.Server.Extensions
{
    public static class AuthenticationApiExtensions
    {
        private const string Scheme = OpenIddictClientAspNetCoreDefaults.AuthenticationScheme;

        public static void MapAuthenticationAPI(this IEndpointRouteBuilder endpoints)
        {

            // (you can add metadata here if you need, e.g. .WithName("Login"))
            // 3) Log out locally and then SLO at the provider
            // 3) Local logout + SLO at the provider
            endpoints.MapGet("/logout", async (HttpContext http) =>
            {

                var returnUrl = http.Request.Query["returnUrl"].ToString();
                var redirectUri = Uri.IsWellFormedUriString(returnUrl, UriKind.Relative)
                                  ? returnUrl
                                  : "/";
                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                };
                // Retrieve the identity stored in the local authentication cookie. If it's not available,
                // this indicate that the user is already logged out locally (or has not logged in yet).
                //
                // For scenarios where the default authentication handler configured in the ASP.NET Core
                // authentication options shouldn't be used, a specific scheme can be specified here.
                var result = await http.AuthenticateAsync();
                if (result is not { Succeeded: true })
                {// Only allow local return URLs to prevent open redirect attacks.
                    //return Results.LocalRedirect(returnUrl);
                    return Results.Redirect(returnUrl);
                }
                // For scenarios where the default sign-out handler configured in the ASP.NET Core
                // authentication options shouldn't be used, a specific scheme can be specified here.
                await http.SignOutAsync();

                //Results.SignOut(properties, new List<string> { OpenIddictClientAspNetCoreDefaults.AuthenticationScheme });

                // Ask the OpenIddict client middleware to redirect the user agent to the identity provider.
                return Results.SignOut(properties, new List<string> { OpenIddictClientAspNetCoreDefaults.AuthenticationScheme });
            })
            .WithName("Logout")
            .WithMetadata(new ValidateAntiForgeryTokenAttribute())
            .WithTags("Auth: Authentication"); ;

            //callbackLogin
            //    .AllowAnonymous()
            //    .WithMetadata(new IgnoreAntiforgeryTokenAttribute());

            // 4) Callback after front-channel logout
            var callbackLogout = endpoints.MapMethods(
                "/callback/logout/{provider}",
                new[] { "GET", "POST" },
                async (HttpContext http) =>
                {
                    // Retrieve the data stored by OpenIddict in the state token created when the logout was triggered.
                    var result = await http.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

                    // In this sample, the local authentication cookie is always removed before the user agent is redirected
                    // to the authorization server. Applications that prefer delaying the removal of the local cookie can
                    // remove the corresponding code from the logout action and remove the authentication cookie in this action.
                    var properties = new AuthenticationProperties(result.Properties.Items)
                    {
                        RedirectUri = string.IsNullOrWhiteSpace(result.Properties.RedirectUri) ? "/" : result.Properties.RedirectUri
                    };

                    return Results.Redirect(properties.RedirectUri!);// Result.Redirect(result!.Properties!.RedirectUri);
                });
            callbackLogout
                .AllowAnonymous()
                .WithMetadata(new IgnoreAntiforgeryTokenAttribute())
                .WithTags("Auth: Authentication"); ;


            // 1) Initiate a login
            endpoints.MapGet("/login", (HttpContext http) =>
            {
                var returnUrl = http.Request.Query["returnUrl"].ToString();
                var redirectUri = Uri.IsWellFormedUriString(returnUrl, UriKind.Relative)
                                  ? returnUrl
                                  : "/";
                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                };
                // wrap the single scheme in an array:
                return Results.Challenge(
                    properties,
                    new[] { OpenIddictClientAspNetCoreDefaults.AuthenticationScheme }
                );
            })
            .WithTags("Auth: Authentication"); ;

            // 2) Callback after external login
            endpoints.MapMethods(
            "/callback/login/{provider}",
            new[] { "GET", "POST" },
            async (HttpContext http) =>
            {
                // 1) Let OpenIddict redeem the callback
                var result = await http.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
                // Multiple strategies exist to handle OAuth 2.0/OpenID Connect callbacks, each with their pros and cons:
                //
                //   * Directly using the tokens to perform the necessary action(s) on behalf of the user, which is suitable
                //     for applications that don't need a long-term access to the user's resources or don't want to store
                //     access/refresh tokens in a database or in an authentication cookie (which has security implications).
                //     It is also suitable for applications that don't need to authenticate users but only need to perform
                //     action(s) on their behalf by making API calls using the access token returned by the remote server.
                //
                //   * Storing the external claims/tokens in a database (and optionally keeping the essential claims in an
                //     authentication cookie so that cookie size limits are not hit). For the applications that use ASP.NET
                //     Core Identity, the UserManager.SetAuthenticationTokenAsync() API can be used to store external tokens.
                //
                //     Note: in this case, it's recommended to use column encryption to protect the tokens in the database.
                //
                //   * Storing the external claims/tokens in an authentication cookie, which doesn't require having
                //     a user database but may be affected by the cookie size limits enforced by most browser vendors
                //     (e.g Safari for macOS and Safari for iOS/iPadOS enforce a per-domain 4KB limit for all cookies).
                //
                //     Note: this is the approach used here, but the external claims are first filtered to only persist
                //     a few claims like the user identifier. The same approach is used to store the access/refresh tokens.

                // Important: if the remote server doesn't support OpenID Connect and doesn't expose a userinfo endpoint,
                // result.Principal.Identity will represent an unauthenticated identity and won't contain any user claim.
                //
                // Such identities cannot be used as-is to build an authentication cookie in ASP.NET Core (as the
                // antiforgery stack requires at least a name claim to bind CSRF cookies to the user's identity) but
                // the access/refresh tokens can be retrieved using result.Properties.GetTokens() to make API calls.
                if (result.Principal is not ClaimsPrincipal { Identity.IsAuthenticated: true })
                {
                    throw new InvalidOperationException("The external authorization data cannot be used for authentication.");
                }
                // Build an identity based on the external claims and that will be used to create the authentication cookie.
                var identity = new ClaimsIdentity(
                    authenticationType: "ExternalLogin",
                    nameType: ClaimTypes.Name,
                    roleType: ClaimTypes.Role);
                // By default, OpenIddict will automatically try to map the email/name and name identifier claims from
                // their standard OpenID Connect or provider-specific equivalent, if available. If needed, additional
                // claims can be resolved from the external identity and copied to the final authentication cookie.
                identity.SetClaim(ClaimTypes.Email, result.Principal.GetClaim(ClaimTypes.Email))
                        .SetClaim(ClaimTypes.Name, result.Principal.GetClaim(ClaimTypes.Name))
                        .SetClaim(ClaimTypes.NameIdentifier, result.Principal.GetClaim(ClaimTypes.NameIdentifier));
                // Preserve the registration details to be able to resolve them later.
                identity.SetClaim(Claims.Private.RegistrationId, result.Principal.GetClaim(Claims.Private.RegistrationId))
                        .SetClaim(Claims.Private.ProviderName, result.Principal.GetClaim(Claims.Private.ProviderName));
                // Important: when using ASP.NET Core Identity and its default UI, the identity created in this action is
                // not directly persisted in the final authentication cookie (called "application cookie" by Identity) but
                // in an intermediate authentication cookie called "external cookie" (the final authentication cookie is
                // later created by Identity's ExternalLogin Razor Page by calling SignInManager.ExternalLoginSignInAsync()).
                //
                // Unfortunately, this process doesn't preserve the claims added here, which prevents flowing claims
                // returned by the external provider down to the final authentication cookie. For scenarios that
                // require that, the claims can be stored in Identity's database by calling UserManager.AddClaimAsync()
                // directly in this action or by scaffolding the ExternalLogin.cshtml page that is part of the default UI:
                // https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/additional-claims#add-and-update-user-claims.
                //
                // Alternatively, if flowing the claims from the "external cookie" to the "application cookie" is preferred,
                // the default ExternalLogin.cshtml page provided by Identity can be scaffolded to replace the call to
                // SignInManager.ExternalLoginSignInAsync() by a manual sign-in operation that will preserve the claims.
                // For scenarios where scaffolding the ExternalLogin.cshtml page is not convenient, a custom SignInManager
                // with an overridden SignInOrTwoFactorAsync() method can also be used to tweak the default Identity logic.
                //
                // For more information, see https://haacked.com/archive/2019/07/16/external-claims/ and
                // https://stackoverflow.com/questions/42660568/asp-net-core-identity-extract-and-save-external-login-tokens-and-add-claims-to-l/42670559#42670559.

                // Build the authentication properties based on the properties that were added when the challenge was triggered.
                var properties = new AuthenticationProperties(result.Properties.Items)
                {
                    RedirectUri = string.IsNullOrWhiteSpace(result.Properties.RedirectUri) ? "/" : result.Properties.RedirectUri
                };
                // If needed, the tokens returned by the authorization server can be stored in the authentication cookie.
                //
                // To make cookies less heavy, tokens that are not used are filtered out before creating the cookie.
                properties.StoreTokens(result.Properties.GetTokens().Where(token => token.Name is
                    // Preserve the access, identity and refresh tokens returned in the token response, if available.
                    //
                    // The expiration date of the access token is also preserved to later determine
                    // whether the access token is expired and proactively refresh tokens if necessary.
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken or
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate or
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken or
                    OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken));
                // Ask the default sign-in handler to return a new cookie and redirect the
                // user agent to the return URL stored in the authentication properties.
                //
                // For scenarios where the default sign-in handler configured in the ASP.NET Core
                // authentication options shouldn't be used, a specific scheme can be specified here.

                // 4) Issue the cookie and return a SignIn result
                //return Results.SignIn(
                //    new ClaimsPrincipal(identity), properties,
                //    CookieAuthenticationDefaults.AuthenticationScheme
                //);
                await http.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    properties
                );
                // now do the redirect
                return Results.Redirect(properties.RedirectUri!);
            })
            .AllowAnonymous()
            .WithMetadata(new IgnoreAntiforgeryTokenAttribute())
            .WithTags("Auth: Authentication"); ;
        }
    }
}
