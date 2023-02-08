using Lemondo.Context;
using Lemondo.UnitofWork.Interface;
using Lemondo.UnitofWork.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using System.Reflection;
using NBitcoin;

var builder = WebApplication.CreateBuilder(args);

//validation
builder.Services.AddControllers()
                .AddFluentValidation(options =>
                {
                    // Validate child properties and root collection elements
                    options.ImplicitlyValidateChildProperties = true;
                    options.ImplicitlyValidateRootCollectionElements = true;

                    // Automatic registration of validators in assembly
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters(); 

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//mvc try
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//oauth2 google
builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/GoogleLogin")
    .AddGoogle(options =>
    {
        options.ClientId = "901946628268-bg72ppla74jnqmdcotv7h8r8sp4e8v8c.apps.googleusercontent.com";
        options.ClientSecret = "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";
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("IsAdmin", "True"));
});
//authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("IsAdmin", "true"));
});

//for documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Lemondo Librarby",
        Description = "An ASP.NET Core Web API for managing Librarby",
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

//mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//unitofwork
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
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



app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseAuthentication();

app.MapRazorPages();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
