using Abp.Json;
using Lemondo.Context;
using Lemondo.OperationFilters;
using Lemondo.UnitofWork.Interface;
using Lemondo.UnitofWork.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NuGet.Protocol;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);
//my try here===>
//var config = builder.Configuration;
//var oktaDomain = config.GetValue<string>("Okta:OktaDomain");
//var clientId = config.GetValue<string>("Okta:ClientId");
//var clientSecret = config.GetValue<string>("Okta:ClientSecret");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//oauth2 2nd try
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name:"v1",new OpenApiInfo { Title="Api", Version="v1" });
    options.AddSecurityDefinition(name: "oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:7084/connect/token"),
                Scopes = new Dictionary<string, string>
            {
             //add key values here:
             //scoepe name==>"api" this should match exagtly what the scope is called on identiti server
             //second one is description "API"
                {"api", "API" }

            }
            }
        }
    });
    options.OperationFilter<AuthorizeOperationFilter>();
});



//oauth2
builder.Services.AddAuthentication(options =>
{
    // If an authentication cookie is present, use it to get authentication information
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    // If authentication is required, and no cookie is present, use Okta (configured below) to sign in
    options.DefaultChallengeScheme = "Okta";
})
.AddCookie() // cookie authentication middleware first
.AddOAuth("Okta", options =>
{
    // Oauth authentication middleware is second
    var oktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain");

    // When a user needs to sign in, they will be redirected to the authorize endpoint
    options.AuthorizationEndpoint = $"{oktaDomain}/oauth2/default/v1/authorize";

    // Okta's OAuth server is OpenID compliant, so request the standard openid
    // scopes when redirecting to the authorization endpoint
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    // After the user signs in, an authorization code will be sent to a callback
    // in this app. The OAuth middleware will intercept it
    options.CallbackPath = new PathString("/authorization-code/callback");

    // The OAuth middleware will send the ClientId, ClientSecret, and the
    // authorization code to the token endpoint, and get an access token in return
    options.ClientId = builder.Configuration.GetValue<string>("Okta:ClientId");
    options.ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret");
    options.TokenEndpoint = $"{oktaDomain}/oauth2/default/v1/token";

    // Below we call the userinfo endpoint to get information about the user
    options.UserInformationEndpoint = $"{oktaDomain}/oauth2/default/v1/userinfo";

    // Describe how to map the user info we receive to user claims
    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "given_name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            // Get user info from the userinfo endpoint and use it to populate user claims
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();
            var userData = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

            context.RunClaimActions(userData);
        }
    };
});

//unitofwork
builder.Services.AddScoped<IUnitofWork, UnitofWorkRepository>();

//dbcontext 
var connectionstring = builder.Configuration.GetConnectionString("LemondoConnection");
builder.Services.AddDbContext<LibraryContext>(options => options.UseSqlServer(connectionstring));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//oauth2
app.UseAuthentication();

app.MapRazorPages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
