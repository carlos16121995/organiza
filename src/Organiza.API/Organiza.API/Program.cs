using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Organiza.API.Configurations.Filters;
using Organiza.API.Configurations.Filters.Middlewares;
using Organiza.API.Configurations.Swagger;
using Organiza.Application;
using Organiza.Domain.Config;
using Organiza.Domain.Entities.Users;
using Organiza.Infrastructure.Persistence;
using System.Globalization;
using System.Text;
using Organiza.Application.Features.Users.Users.Commands.InsertUsers;

var builder = WebApplication.CreateBuilder(args);
Settings.Configure(builder.Configuration);

builder.Services.AddScoped<InterceptorHandlerFilter>();

builder.Services
        .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<Context>()
        .AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddApiVersioning(o =>
{
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
                        {
                            options.SuppressMapClientErrors = true;
                            options.SuppressModelStateInvalidFilter = true;
                        });


builder.Services.AddSwaggerDocumentation(builder.Configuration);

#region JWT

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = false,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = Settings.Token.Issuer,
        ValidAudience = Settings.Token.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Token.SecretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Usuario", policy => policy.RequireClaim("UserPermissao", "Vendedor").RequireClaim("UserPermissao", "Comprador"));
    options.AddPolicy("Adm", policy => policy.RequireClaim("UserPermissao", "Admin"));
});

#endregion

var app = builder.Build();

var supportedCultures = new[] { "pt-BR" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseCors(delegate (CorsPolicyBuilder policyBuilder)
{
    policyBuilder
        .SetIsOriginAllowed((o) => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseDeveloperExceptionPage();

app.UseSwaggerDocumentation();

app.UseRouting();

app.UseForwardedHeaders();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapControllers();

await app.RunAsync();
