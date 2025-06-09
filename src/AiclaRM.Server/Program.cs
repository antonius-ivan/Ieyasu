using System.Globalization;
using AiclaRM.Server;
using AiclaRM.Server.Extensions;
using AiclaRM.Server.Helpers;
using AiclaRM.Server.Models;
using AiclaRM.Server.Services.ECommerce;
using AiclaRM.Server.Services.Tourney.Employee;
using AiclaRM.Server.Services.Tourney.Person;
using AiclaRM.Server.Services.Tourney.Prize;
using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.DataAccess;
using AIRMDataManager.Library.DataAccess.MsSql;
using AIRMDataManager.Library.Modules.ECommerce.Catalog.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Employee.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Person.DataAccess;
using AIRMDataManager.Library.Modules.Tourney.Prize.DataAccess;
using AIRMDataManager.Library.SystemCoreDataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSwag;
using OpenIddict.Client;
using Quartz;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Client.AspNetCore.OpenIddictClientAspNetCoreConstants;
using static OpenIddict.Client.OpenIddictClientModels;
//\app\AiclaRM.Server\Program.cs
var builder = WebApplication.CreateBuilder(args);

// Configure connection string in appsettings.json and get it here.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddOpenApiDocument(options => {
    options.DocumentName = "MyCustomAPI";
    options.PostProcess = document =>
    {
        document.Info = new OpenApiInfo
        {
            Version = "v1",
            Title = "ToDo API",
            Description = @"Auth   = Authentication/Authorization Module   || 
                    WF   = Weather Module   ||   
                    RM   = Retail Manager Module   ||   
                    TNM   = Tournament Module",
            TermsOfService = "https://example.com/terms",
            Contact = new OpenApiContact
            {
                Name = "Example Contact",
                Url = "https://example.com/contact"
            },
            License = new OpenApiLicense
            {
                Name = "Example License",
                Url = "https://example.com/license"
            }
        };
    };
});

// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("https://localhost:55909")  // ← change to https
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});



// Register the database connection factory.
builder.Services.AddScoped<IDatabaseConnectionFactory>(sp =>
    new MsSqlDatabaseConnectionFactory(connectionString));

builder.Services.AddScoped<IDatabaseConnectionFactory, OLDDatabaseConnectionFactory>();
builder.Services.AddScoped<IPrizeService, PrizeService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


builder.Services.AddScoped<ICatalogRepository, MsSqlCatalogRepository>();
builder.Services.AddScoped<IPrizeRepository, MsSqlPrizeRepository>();
builder.Services.AddScoped<IPersonRepository, MsSqlPersonRepository>();
builder.Services.AddScoped<IEmployeeRepository, MsSqlEmployeeRepository>();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddHttpClient<CatalogService>(o => o.BaseAddress = new Uri("https://localhost:7028"));

//builder.Services.AddScoped<IPrizeRepository, MsSqlPrizeRepository>();
//builder.Services.AddScoped<IPersonRepository, PostgrePersonRepository>();
//builder.Services.AddScoped<IPrizeRepository, PostgrePrizeRepository>();
//builder.Services.AddScoped<IPersonRepository, MsSqlPersonRepository>();

//var dbType = configuration["DatabaseType"]; // e.g., "MsSql" or "Postgres"

//if (dbType == "MsSql")
//{
//    services.AddSingleton<IDatabaseConnectionFactory, MsSqlDatabaseConnectionFactory>();
//    services.AddScoped<IMenuRepository, MsSqlMenuRepository>();
//    services.AddScoped<IPrizeRepository, MsSqlPrizeRepository>();
//    services.AddScoped<ICatalogRepository, MsSqlCatalogRepository>();
//    services.AddScoped<ISuggestionRepository, MsSqlSuggestionRepository>();
//}
//else if (dbType == "Postgres")
//{
//    services.AddSingleton<IDatabaseConnectionFactory, PostgresDatabaseConnectionFactory>();
//    services.AddScoped<IMenuRepository, PostgresMenuRepository>();
//    services.AddScoped<IPrizeRepository, PostgresPrizeRepository>();
//    services.AddScoped<ICatalogRepository, PostgresCatalogRepository>();
//    services.AddScoped<ISuggestionRepository, PostgresSuggestionRepository>();
//}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Configure the context to use sqlite.
    //options.UseSqlite($"Filename={Path.Combine(Path.GetTempPath(), "openiddict-dantooine-webassembly-server.sqlite3")}");
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

// Configure the antiforgery stack to allow extracting
// antiforgery tokens from the X-XSRF-TOKEN header.
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "__Host-X-XSRF-TOKEN";
    options.Cookie.SameSite = SameSiteMode.None;//.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
});

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations from the database) at regular intervals.
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();

        // Developers who prefer using MongoDB can remove the previous lines
        // and configure OpenIddict to use the specified MongoDB database:
        // options.UseMongoDb()
        //        .UseDatabase(new MongoClient().GetDatabase("openiddict"));

        // Enable Quartz.NET integration.
        options.UseQuartz();
    })

    // Register the OpenIddict client components.
    .AddClient(options =>
    {
        // Note: this sample uses the authorization code and refresh token
        // flows, but you can enable the other flows if necessary.
        options.AllowAuthorizationCodeFlow()
               .AllowRefreshTokenFlow();

        // Register the signing and encryption credentials used to protect
        // sensitive data like the state tokens produced by OpenIddict.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
               .EnableStatusCodePagesIntegration()
               .EnableRedirectionEndpointPassthrough()
               .EnablePostLogoutRedirectionEndpointPassthrough();

        // Register the System.Net.Http integration and use the identity of the current
        // assembly as a more specific user agent, which can be useful when dealing with
        // providers that use the user agent as a way to throttle requests (e.g Reddit).
        options.UseSystemNetHttp()
               .SetProductInformation(typeof(Program).Assembly);

        // Add a client registration matching the client application definition in the server project.
        options.AddRegistration(new OpenIddictClientRegistration
        {
            Issuer = new Uri("https://localhost:44319/", UriKind.Absolute),

            ClientId = "reactnetcorecodeflowpkceclient",
            ClientSecret = "codeflow_pkce_client_secret",
            Scopes = { Scopes.OfflineAccess, Scopes.Profile, "api1" },

            // Note: to mitigate mix-up attacks, it's recommended to use a unique redirection endpoint
            // URI per provider, unless all the registered providers support returning a special "iss"
            // parameter containing their URL as part of authorization responses. For more information,
            // see https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4.
            RedirectUri = new Uri("callback/login/local", UriKind.Relative),
            PostLogoutRedirectUri = new Uri("callback/logout/local", UriKind.Relative)
        });
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Create an authorization policy used by YARP when forwarding requests
// from the WASM application to the Dantooine.Api resource server.
builder.Services.AddAuthorization(options => options.AddPolicy("CookieAuthenticationPolicy", builder =>
{
    builder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
    builder.RequireAuthenticatedUser();
}));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builder =>
    {
        builder.AddRequestTransform(async context =>
        {
            // Attach the access token, access token expiration date and refresh token resolved from the authentication
            // cookie to the request options so they can later be resolved from the delegating handler and attached
            // to the request message or used to refresh the tokens if the server returned a 401 error response.
            //
            // Alternatively, the user tokens could be stored in a database or a distributed cache.

            var result = await context.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            context.ProxyRequest.Options.Set(
                key: new(Tokens.BackchannelAccessToken),
                value: result.Properties.GetTokenValue(Tokens.BackchannelAccessToken));

            context.ProxyRequest.Options.Set(
                key: new(Tokens.BackchannelAccessTokenExpirationDate),
                value: result.Properties.GetTokenValue(Tokens.BackchannelAccessTokenExpirationDate));

            context.ProxyRequest.Options.Set(
                key: new(Tokens.RefreshToken),
                value: result.Properties.GetTokenValue(Tokens.RefreshToken));
        });

        builder.AddResponseTransform(async context =>
        {
            // If tokens were refreshed during the request handling (e.g due to the stored access token being
            // expired or a 401 error response being returned by the resource server), extract and attach them
            // to the authentication cookie that will be returned to the browser: doing that is essential as
            // OpenIddict uses rolling refresh tokens: if the refresh token wasn't replaced, future refresh
            // token requests would end up being rejected as they would be treated as replayed requests.

            if (context.ProxyResponse is not TokenRefreshingHttpResponseMessage
                {
                    RefreshTokenAuthenticationResult: RefreshTokenAuthenticationResult
                } response)
            {
                return;
            }

            var result = await context.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Override the tokens using the values returned in the token response.
            var properties = result.Properties.Clone();
            properties.UpdateTokenValue(Tokens.BackchannelAccessToken, response.RefreshTokenAuthenticationResult.AccessToken);

            properties.UpdateTokenValue(Tokens.BackchannelAccessTokenExpirationDate,
                response.RefreshTokenAuthenticationResult.AccessTokenExpirationDate?.ToString(CultureInfo.InvariantCulture));

            // Note: if no refresh token was returned, preserve the refresh token initially returned.
            if (!string.IsNullOrEmpty(response.RefreshTokenAuthenticationResult.RefreshToken))
            {
                properties.UpdateTokenValue(Tokens.RefreshToken, response.RefreshTokenAuthenticationResult.RefreshToken);
            }

            // Remove the redirect URI from the authentication properties
            // to prevent the cookies handler from genering a 302 response.
            properties.RedirectUri = null;

            // Note: this event handler can be called concurrently for the same user if multiple HTTP
            // responses are returned in parallel: in this case, the browser will always store the latest
            // cookie received and the refresh tokens stored in the other cookies will be discarded.
            await context.HttpContext.SignInAsync(result.Ticket.AuthenticationScheme, result.Principal, properties);
        });
    });

// Replace the default HTTP client factory used by YARP by an instance able to inject the HTTP delegating
// handler that will be used to attach the access tokens to HTTP requests or refresh tokens if necessary.
builder.Services.Replace(ServiceDescriptor.Singleton<IForwarderHttpClientFactory, TokenRefreshingForwarderHttpClientFactory>());

// Register the worker responsible for creating the database used to store tokens.
// Note: in a real world application, this step should be part of a setup script.
builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
app.UseOpenApi();


app.UseSwaggerUi();

//app.UseHttpsRedirection();
app.UseHttpsRedirection();
//app.UseReactAspNetCoreFrameworkFiles();
app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapAuthenticationAPI();
app.MapDirectApi();
app.MapUserApi();
app.MapRazorPages();
app.MapControllers();
app.MapReverseProxy();
// Use the extension method
app.MapTourneyAPI();
app.MapCatalogAPI();

app.MapGet("/weatherforecast", () =>
{
var forecast = Enumerable.Range(1, 5).Select(index =>
    new WeatherForecast
    (
        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        Random.Shared.Next(-20, 55),
        summaries[Random.Shared.Next(summaries.Length)]
    ))
    .ToArray();
return forecast;
})
.WithName("GetWeatherForecast")
.WithTags("WF :  Basic");


app.MapFallbackToFile("/index.html");
//app.MapFallbackToPage("/_Host");

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
