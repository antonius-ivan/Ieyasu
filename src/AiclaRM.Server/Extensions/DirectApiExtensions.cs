using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiclaRM.Server.Extensions
{
    public static class DirectApiExtensions
    {
        public static void MapDirectApi(this IEndpointRouteBuilder endpoints)
        {
            // GET api/directapi
            var directApi = endpoints.MapGet("api/directapi",
                [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
            [ValidateAntiForgeryToken]
            (HttpContext http) =>
                {
                    // You can return any serializable object; minimal APIs will auto-serialize to JSON
                    return Results.Json(new List<string>
                {
                    "some data",
                    "more data",
                    "loads of data"
                });
                });

            // Optional: name the endpoint and add additional metadata
            directApi
                .WithName("GetDirectData")
                .WithTags("Auth: Authorization");
        }
    }
}
