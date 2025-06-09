using System.Security.Claims;
using AIRMDataManager.Library.Modules.Authorization.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;
//app\AiclaRM.Server\Extensions\UserApiExtensions.cs

namespace AiclaRM.Server.Extensions
{
    public static class UserApiExtensions
    {
        public static void MapUserApi(this IEndpointRouteBuilder endpoints)
        {
            // GET api/user
            var userRoute = endpoints.MapGet("api/user", (HttpContext http) =>
            {
                var user = http.User;
                if (!user.Identity.IsAuthenticated)
                {
                    return Results.Ok(UserInfo.Anonymous);
                }

                var info = new UserInfo
                {
                    IsAuthenticated = true,
                    NameClaimType = (user.Identity as ClaimsIdentity)?.NameClaimType ?? Claims.Name,
                    RoleClaimType = (user.Identity as ClaimsIdentity)?.RoleClaimType ?? Claims.Role,
                    //Claims = user.FindAll((user.Identity as ClaimsIdentity)?.NameClaimType ?? Claims.Name)
                    //             .Select(c => new ClaimValue(c.Type, c.Value))
                    //             .ToList()
                    Claims = user.Claims
                    .Select(c => new ClaimValue(c.Type, c.Value))
                    .ToList()
                };

                return Results.Ok(info);
            });

            // allow anonymous access
            userRoute
                .AllowAnonymous()
                .WithName("GetCurrentUser")
                .WithTags("Auth: Authorization");
        }
    }
}
