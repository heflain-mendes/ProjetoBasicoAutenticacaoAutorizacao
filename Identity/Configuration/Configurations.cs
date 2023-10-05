using Identity.Authorize.PolicyRequired;
using Identity.Interface;
using Identity.Model;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Data;

namespace Identity.Configuration;

/**
 * Revisar e separar as responsabilidades dessa classe
*/
public static class Configurations
{

    //entender o que esse this tá fazendo aí
    public static void ConfigAllIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IGerarTokenService, GeraTokenService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddDbContext<UsuarioDbContext>(x =>
        x.UseNpgsql(configuration.GetConnectionString("NpgsqlConnection")));

        services.AddDefaultIdentity<Usuario>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UsuarioDbContext>()
            .AddDefaultTokenProviders();

        var jwtAppSettingOptions = configuration.GetSection(nameof(JwtOptions));
        var sk = configuration.GetSection("JwtOptions:SecurityKey").Value ?? throw new Exception("Erro ao obter JwtOptions:SecurityKey");
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sk));


        //entender para que serve esse Configuration
        services.Configure<JwtOptions>(
            options => {
                options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
                options.AccessTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.AccessTokenExpiration)] 
                    ?? throw new Exception("Erro ao obter nameof de JwtOptions.AccessTokenExpiration"));

            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,

            ValidateAudience = true,
            ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,

            RequireExpirationTime = true,
            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
        });
    }

    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, HorarioComercialHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.HorarioComercial, policy =>
                policy.Requirements.Add(new HorarioComercialRequirement()));
        });
    }
}
